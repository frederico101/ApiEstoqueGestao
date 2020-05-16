using System;

namespace ApiProdutos.ViewModel
{
    public class ProdutoViewModel
    {
         public Guid FornecedorId { get; set; }

        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string ImagemUpload { get; set; }
        public string Imagem { get; set; }
        public decimal Valor { get; set; }
        public DateTime DataCadastro { get; set; }
        public bool Ativo { get; set; }

         /* EF Relations */
      public string NomeFornecedor { get; set; }
    }
}