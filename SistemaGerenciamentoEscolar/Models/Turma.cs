using System;

namespace SistemaGerenciamentoEscolar.Models {
    public class Turma {
        public int TurmaId { get; set; }
        public int CursoId { get; set; }
        public int LaboratorioId { get; set; }
        public string Codigo { get; set; }
        public DateTime DataAula { get; set; }
        public TimeSpan HorarioInicio { get; set; }
        public TimeSpan HorarioFim { get; set; }
        public int VagasDisponiveis { get; set; }
        public string Observacoes { get; set; }
        public bool Ativo { get; set; }

        public string CursoNome { get; set; }
        public string LaboratorioNome { get; set; }

        public Turma() {
            Ativo = true;
            DataAula = DateTime.Now.Date;
        }
    }
}