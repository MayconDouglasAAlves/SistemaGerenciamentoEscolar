using System;

namespace SistemaGerenciamentoEscolar.Models {
    public class Escola {
        public int EscolaId { get; set; }
        public string Nome { get; set; }
        public string CNPJ { get; set; }
        public string Endereco { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public DateTime DataCadastro { get; set; }
        public bool Ativo { get; set; }

        public Escola() {
            DataCadastro = DateTime.Now;
            Ativo = true;
        }
    }
}