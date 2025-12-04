using System;

namespace SistemaGerenciamentoEscolar.Models {
    public class Presenca {
        public int PresencaId { get; set; }
        public int MatriculaId { get; set; }
        public DateTime DataPresenca { get; set; }
        public string Status { get; set; }
        public string Observacoes { get; set; }

        public string AlunoNome { get; set; }
        public string NumeroMatricula { get; set; }

        public Presenca() {
            DataPresenca = DateTime.Now;
            Status = "Ausente";
        }
    }
}