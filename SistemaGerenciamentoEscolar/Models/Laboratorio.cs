namespace SistemaGerenciamentoEscolar.Models {
    public class Laboratorio {
        public int LaboratorioId { get; set; }
        public int EscolaId { get; set; }
        public string Nome { get; set; }
        public int Capacidade { get; set; }
        public string Localizacao { get; set; }
        public string Equipamentos { get; set; }
        public bool Ativo { get; set; }

        public string EscolaNome { get; set; }

        public Laboratorio() {
            Ativo = true;
        }
    }
}