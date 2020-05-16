using System;

namespace ApiProdutos.ViewModel
{
    public class EnderecoViewModel
    {
        public Guid FornecedorId { get; set; }
        public Guid Id { get; set; }
        public string Logradouro { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Cep { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }

        /* EF Relation */
        // public Fornecedor Fornecedor { get; set; }

    }
}