using System;
using System.Linq;
using DevIO.Business.Intefaces;
using DevIO.Business.Notificacoes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ApiProdutos.Controllers
{
    [ApiController]
    public abstract class MainController : Controller
    {
        private readonly INotificador _notificador;
        public MainController(INotificador notificador)
        {
            _notificador = notificador;
        }
        protected bool OperacaoValida()=> !_notificador.TemNotificacao();
         protected ActionResult CustomResponse(object result = null)
         { 
             if(OperacaoValida())
             {
                 return Ok( new
                     { 
                         success = true,
                         data = result
                      });
              }
              return BadRequest(
                new 
                {success = false,
                errors = _notificador.ObterNotificacoes().Select(n => n.Mensagem)
                });
         }
        protected ActionResult CustomResponse(ModelStateDictionary modelState)
        {
             if(!ModelState.IsValid) NotificarErroModelInvalida(modelState);
             return CustomResponse();
        }
        protected void NotificarErroModelInvalida(ModelStateDictionary modelState)
        {
            var erros = modelState.Values.SelectMany(e => e.Errors);
            foreach (var erro in erros)
            {
                var errorMsg = erro.Exception == null ? erro.ErrorMessage : erro.Exception.Message;
                NotificarError(errorMsg);
            }
        }

        protected void NotificarError(string mensagem)
        {
            _notificador.Handle(new Notificacao(mensagem));
        }
    }
}    