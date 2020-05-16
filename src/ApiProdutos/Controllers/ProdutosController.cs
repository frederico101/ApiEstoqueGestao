using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ApiProdutos.ViewModel;
using AutoMapper;
using DevIO.Business.Intefaces;
using DevIO.Business.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApiProdutos.Controllers
{
    [Route("/api/produtos")]
    public class ProdutosController : MainController
    {
        private readonly IProdutoRepository _produtoRepositorio;
        private readonly IProdutoService _produtoService;
        private readonly IMapper _mapper;
        private readonly IEnderecoRepository _enderecoRepository;

        public ProdutosController(INotificador notificador, IProdutoRepository produtoRepositorio,
         IProdutoService produtoService, IMapper imaper, IEnderecoRepository enderecoRepository)
        : base(notificador)
        {
            _produtoRepositorio = produtoRepositorio;
            _produtoService = produtoService;
            _mapper = imaper;
            _enderecoRepository = enderecoRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<ProdutoViewModel>> ObterTodos()
        {

            var produtoViewModel = _mapper.Map<IEnumerable<ProdutoViewModel>>(await _produtoRepositorio.ObterTodos());

            return produtoViewModel;
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ProdutoViewModel>> ObterPorId(Guid id)
        {

            var produtoViewModel = await ObterProduto(id);
            if (produtoViewModel == null) return NotFound();

            return produtoViewModel;
        }
        public async Task<ProdutoViewModel> ObterProduto(Guid id) =>
         _mapper.Map<ProdutoViewModel>(await _produtoRepositorio.ObterProdutoFornecedor(id));


        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<FornecedorViewModel>> Excluir(Guid id)
        {

            var produtoViewModel = await ObterProduto(id);
            if (produtoViewModel == null) return NotFound();

            await _produtoService.Remover(id);

            return CustomResponse(produtoViewModel);
        }


        [HttpPost]
        public async Task<ActionResult<ProdutoViewModel>> Adicionar(ProdutoViewModel produtoViewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _produtoRepositorio.Adicionar(_mapper.Map<Produto>(produtoViewModel));
            return CustomResponse(ModelState);
        }

        private bool UploadArquivo(string arquivo, string imgNome)
        {
            var imageDataByteArray = Convert.FromBase64String(arquivo);
            if (string.IsNullOrEmpty(arquivo ))
            {
                ModelState.AddModelError(string.Empty, "Forneça uma imagem para este produto!");
                return false;
            }
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imagens", imgNome);
            if (System.IO.File.Exists(filePath))
            {
                ModelState.AddModelError(string.Empty, "Já existe um arquivo com este nome");
                return false;
            }

            System.IO.File.WriteAllBytes(filePath, imageDataByteArray);
            return true;
        }

    }
}