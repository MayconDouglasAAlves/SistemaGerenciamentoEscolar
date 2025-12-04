using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace SistemaGerenciamentoEscolar.Data {
    // conexão MongoDB

    public static class ConexaoMongoDB {
        private static readonly string connectionString = "mongodb://localhost:27017";
        private static readonly string databaseName = "GerenciamentoEscolarNoSQL";
        private static IMongoDatabase database;

        public static IMongoDatabase ObterDatabase() {
            if (database == null) {
                var client = new MongoClient(connectionString);
                database = client.GetDatabase(databaseName);
            }
            return database;
        }

        public static bool TestarConexao() {
            try {
                var db = ObterDatabase();
                db.ListCollectionNames().FirstOrDefault();
                return true;
            }
            catch {
                return false;
            }
        }
    }
}

namespace SistemaGerenciamentoEscolar.Models {
    // feedbacks dos alunos

    public class FeedbackAluno {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("alunoId")]
        public int AlunoId { get; set; }

        [BsonElement("alunoNome")]
        public string AlunoNome { get; set; }

        [BsonElement("cursoId")]
        public int CursoId { get; set; }

        [BsonElement("cursoNome")]
        public string CursoNome { get; set; }

        [BsonElement("turmaId")]
        public int TurmaId { get; set; }

        [BsonElement("turmaCodigo")]
        public string TurmaCodigo { get; set; }

        [BsonElement("notaConteudo")]
        public int NotaConteudo { get; set; }

        [BsonElement("notaInstrutor")]
        public int NotaInstrutor { get; set; }

        [BsonElement("notaInfraestrutura")]
        public int NotaInfraestrutura { get; set; }

        [BsonElement("comentario")]
        public string Comentario { get; set; }

        [BsonElement("dataFeedback")]
        public DateTime DataFeedback { get; set; }

        [BsonElement("recomendaCurso")]
        public bool RecomendaCurso { get; set; }

        public FeedbackAluno() {
            DataFeedback = DateTime.Now;
        }

        [BsonIgnore]
        public double MediaGeral {
            get { return (NotaConteudo + NotaInstrutor + NotaInfraestrutura) / 3.0; }
        }
    }

    // logs do sistema

    public class LogSistema {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("dataHora")]
        public DateTime DataHora { get; set; }

        [BsonElement("usuario")]
        public string Usuario { get; set; }

        [BsonElement("acao")]
        public string Acao { get; set; }

        [BsonElement("modulo")]
        public string Modulo { get; set; }

        [BsonElement("descricao")]
        public string Descricao { get; set; }

        [BsonElement("nivel")]
        public string Nivel { get; set; }

        [BsonElement("ipAddress")]
        public string IpAddress { get; set; }

        [BsonElement("detalhes")]
        public BsonDocument Detalhes { get; set; }

        public LogSistema() {
            DataHora = DateTime.Now;
            Nivel = "Info";
        }
    }
}