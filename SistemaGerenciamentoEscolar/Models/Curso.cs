using System;

namespace SistemaGerenciamentoEscolar.Models {
    public class Curso {
        public int CursoId { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public int CargaHoraria { get; set; }
        public DateTime DataCurso { get; set; }
        public TimeSpan HorarioInicio { get; set; }
        public TimeSpan HorarioFim { get; set; }
        public bool Ativo { get; set; }

        public Curso() {
            Ativo = true;
            DataCurso = DateTime.Now.Date;
        }
    }
}