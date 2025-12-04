using System;

namespace SistemaGerenciamentoEscolar.Models {
    public class Aluno {
        public int AlunoId { get; set; }
        public string Nome { get; set; }
        public string CPF { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public string Endereco { get; set; }
        public DateTime DataCadastro { get; set; }
        public bool Ativo { get; set; }

        public Aluno() {
            DataCadastro = DateTime.Now;
            Ativo = true;
        }

        public int Idade {
            get {
                int idade = DateTime.Now.Year - DataNascimento.Year;
                if (DateTime.Now.DayOfYear < DataNascimento.DayOfYear)
                    idade--;
                return idade;
            }
        }
    }
}