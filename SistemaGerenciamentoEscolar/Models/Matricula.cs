using System;

namespace SistemaGerenciamentoEscolar.Models {
    public class Matricula {
        public int MatriculaId { get; set; }
        public int AlunoId { get; set; }
        public int TurmaId { get; set; }
        public string NumeroMatricula { get; set; }
        public DateTime DataMatricula { get; set; }
        public string Status { get; set; }
        public string Observacoes { get; set; }

        public string AlunoNome { get; set; }
        public string TurmaCodigo { get; set; }
        public string CursoNome { get; set; }

        public Matricula() {
            DataMatricula = DateTime.Now;
            Status = "Ativa";
        }
    }
}