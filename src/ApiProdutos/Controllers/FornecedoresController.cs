using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApiProdutos.ViewModel;
using AutoMapper;
using DevIO.Business.Intefaces;
using DevIO.Business.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApiProdutos.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class FornecedoresController : MainController
    {

        private readonly IFornecedorRepository _fornecedorRepositorio;
        private readonly IFornecedorService _fornecedorService;
        private readonly IMapper _mapper;
        private readonly IEnderecoRepository _enderecoRepository;

        public FornecedoresController(INotificador notificador, IFornecedorRepository fornecedorRepositorio,
         IFornecedorService fornecedorService, IMapper imaper, IEnderecoRepository enderecoRepository)
        : base(notificador)
        {
            _fornecedorRepositorio = fornecedorRepositorio;
            _fornecedorService = fornecedorService;
            _mapper = imaper;
            _enderecoRepository = enderecoRepository;
        }


        [HttpGet]
        public async Task<IEnumerable<FornecedorViewModel>> ObterTodos()
        {

            var repositorio = _mapper.Map<IEnumerable<FornecedorViewModel>>(await _fornecedorRepositorio.ObterTodos());

            return repositorio;
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<FornecedorViewModel>> ObterPorId(Guid id)
        {

            var repositorio = await ObterFornecedorProdutoEndereco(id);
            if (repositorio == null) return NotFound();

            return repositorio;
        }
        public async Task<FornecedorViewModel> ObterFornecedorProdutoEndereco(Guid id) =>
         _mapper.Map<FornecedorViewModel>(await _fornecedorRepositorio.ObterFornecedorProdutosEndereco(id));


        [HttpGet("obter-endereco/{id:guid}")]
        public async Task<EnderecoViewModel> ObterEnderecoPorId(Guid id) =>
        _mapper.Map<EnderecoViewModel>(await _enderecoRepository.ObterPorId(id));

        [HttpPut("atualizar-endereco/{id:guid}")]
        public async Task<ActionResult> AtualizaEndereco(Guid id, EnderecoViewModel enderecoViewModel)
        {
            if (id != enderecoViewModel.Id)
            {
                  NotificarError("O id informado é nao é o mesmo que foi passado na query");
                  return CustomResponse(enderecoViewModel);
            }
            if (!ModelState.IsValid) return CustomResponse(ModelState);
            await _fornecedorService.AtualizarEndereco(_mapper.Map<Endereco>(enderecoViewModel));
            
            return CustomResponse(enderecoViewModel);
        }


        [HttpPost]
        public async Task<ActionResult<FornecedorViewModel>> Adicionar(FornecedorViewModel fornecedorViewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _fornecedorRepositorio.Adicionar(_mapper.Map<Fornecedor>(fornecedorViewModel));
            return CustomResponse(ModelState);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<FornecedorViewModel>> Atualizar(Guid id, FornecedorViewModel fornecedorViewModel)
        {
            if (!ModelState.IsValid || id != fornecedorViewModel.Id)
            {
                NotificarError("O id informado é nao é o mesmo que foi passado na query");
                return CustomResponse(fornecedorViewModel);
            }

            await _fornecedorRepositorio.Atualizar(_mapper.Map<Fornecedor>(fornecedorViewModel));
            return CustomResponse(fornecedorViewModel);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<FornecedorViewModel>> Excluir(Guid id)
        {

            var repositorioViewModel = await ObterFornecedorEndereco(id);
            if (repositorioViewModel == null) return NotFound();

            await _fornecedorService.Remover(id);

            return CustomResponse(repositorioViewModel);
        }
        public async Task<FornecedorViewModel> ObterFornecedorEndereco(Guid id) =>
         _mapper.Map<FornecedorViewModel>(await _fornecedorRepositorio.ObterFornecedorEndereco(id));
    }
}
