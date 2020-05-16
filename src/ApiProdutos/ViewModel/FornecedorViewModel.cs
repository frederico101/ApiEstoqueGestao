using System;
using System.Collections.Generic;

namespace ApiProdutos.ViewModel
{
    public class FornecedorViewModel
    {
        public Guid Id { get; set; }           
        public string Nome { get; set; }
        public string Documento { get; set; }
       // public TipoFornecedor TipoFornecedor { get; set; }
        public EnderecoViewModel Endereco { get; set; }
        public bool Ativo { get; set; }

        /* EF Relations */
       public IEnumerable<ProdutoViewModel> Produtos { get; set; }
    }
}