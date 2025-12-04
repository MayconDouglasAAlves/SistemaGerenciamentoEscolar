using MongoDB.Driver;
using System;
using System.Collections.Generic;
using SistemaGerenciamentoEscolar.Models;

namespace SistemaGerenciamentoEscolar.Data {
    // repository feedbacks dos alunos NoSQL

    public class FeedbackRepository {
        private readonly IMongoCollection<FeedbackAluno> collection;

        public FeedbackRepository() {
            var database = ConexaoMongoDB.ObterDatabase();
            collection = database.GetCollection<FeedbackAluno>("feedbacks");
        }

        public bool Inserir(FeedbackAluno feedback) {
            try {
                collection.InsertOne(feedback);

                LogRepository log = new LogRepository();
                log.Inserir(new LogSistema {
                    Usuario = "Sistema",
                    Modulo = "Feedback",
                    Acao = "Criar",
                    Nivel = "Info",
                    Descricao = "Feedback criado para o aluno " + feedback.AlunoId,
                    DataHora = DateTime.Now
                });

                return true;
            }
            catch (Exception ex) {
                LogRepository log = new LogRepository();
                log.Inserir(new LogSistema {
                    Usuario = "Sistema",
                    Modulo = "Feedback",
                    Acao = "Erro",
                    Nivel = "Error",
                    Descricao = ex.Message,
                    DataHora = DateTime.Now
                });

                throw new Exception("Erro ao inserir feedback: " + ex.Message);
            }
        }


        public List<FeedbackAluno> ListarTodos() {
            try {
                return collection.Find(_ => true)
                    .SortByDescending(f => f.DataFeedback)
                    .ToList();
            }
            catch (Exception ex) {
                throw new Exception("Erro ao listar feedbacks: " + ex.Message);
            }
        }

        public List<FeedbackAluno> ListarPorCurso(int cursoId) {
            try {
                return collection.Find(f => f.CursoId == cursoId)
                    .SortByDescending(f => f.DataFeedback)
                    .ToList();
            }
            catch (Exception ex) {
                throw new Exception("Erro ao listar feedbacks por curso: " + ex.Message);
            }
        }

        public List<FeedbackAluno> ListarPorTurma(int turmaId) {
            try {
                return collection.Find(f => f.TurmaId == turmaId)
                    .SortByDescending(f => f.DataFeedback)
                    .ToList();
            }
            catch (Exception ex) {
                throw new Exception("Erro ao listar feedbacks por turma: " + ex.Message);
            }
        }

        public List<FeedbackAluno> ListarPorAluno(int alunoId) {
            try {
                return collection.Find(f => f.AlunoId == alunoId)
                    .SortByDescending(f => f.DataFeedback)
                    .ToList();
            }
            catch (Exception ex) {
                throw new Exception("Erro ao listar feedbacks por aluno: " + ex.Message);
            }
        }

        public double ObterMediaPorCurso(int cursoId) {
            try {
                var feedbacks = ListarPorCurso(cursoId);
                if (feedbacks.Count == 0) return 0;

                double soma = 0;
                foreach (var f in feedbacks) {
                    soma += f.MediaGeral;
                }
                return soma / feedbacks.Count;
            }
            catch {
                return 0;
            }
        }

        public bool Deletar(string id) {
            try {
                var result = collection.DeleteOne(f => f.Id == id);
                return result.DeletedCount > 0;
            }
            catch (Exception ex) {
                throw new Exception("Erro ao deletar feedback: " + ex.Message);
            }
        }
    }

    // repository logs do sistema NoSQL

    public class LogRepository {
        private readonly IMongoCollection<LogSistema> collection;

        public LogRepository() {
            var database = ConexaoMongoDB.ObterDatabase();
            collection = database.GetCollection<LogSistema>("logs");
        }

        public bool Inserir(LogSistema log) {
            try {
                collection.InsertOne(log);
                return true;
            }
            catch (Exception ex) {
                throw new Exception("Erro ao inserir log: " + ex.Message);
            }
        }

        public List<LogSistema> ListarTodos(int limite = 1000) {
            try {
                return collection.Find(_ => true)
                    .SortByDescending(l => l.DataHora)
                    .Limit(limite)
                    .ToList();
            }
            catch (Exception ex) {
                throw new Exception("Erro ao listar logs: " + ex.Message);
            }
        }

        public List<LogSistema> ListarPorModulo(string modulo, int limite = 500) {
            try {
                return collection.Find(l => l.Modulo == modulo)
                    .SortByDescending(l => l.DataHora)
                    .Limit(limite)
                    .ToList();
            }
            catch (Exception ex) {
                throw new Exception("Erro ao listar logs por módulo: " + ex.Message);
            }
        }

        public List<LogSistema> ListarPorNivel(string nivel, int limite = 500) {
            try {
                return collection.Find(l => l.Nivel == nivel)
                    .SortByDescending(l => l.DataHora)
                    .Limit(limite)
                    .ToList();
            }
            catch (Exception ex) {
                throw new Exception("Erro ao listar logs por nível: " + ex.Message);
            }
        }

        public List<LogSistema> ListarPorPeriodo(DateTime dataInicio, DateTime dataFim) {
            try {
                return collection.Find(l => l.DataHora >= dataInicio && l.DataHora <= dataFim)
                    .SortByDescending(l => l.DataHora)
                    .ToList();
            }
            catch (Exception ex) {
                throw new Exception("Erro ao listar logs por período: " + ex.Message);
            }
        }

        public List<LogSistema> ListarPorUsuario(string usuario, int limite = 500) {
            try {
                return collection.Find(l => l.Usuario == usuario)
                    .SortByDescending(l => l.DataHora)
                    .Limit(limite)
                    .ToList();
            }
            catch (Exception ex) {
                throw new Exception("Erro ao listar logs por usuário: " + ex.Message);
            }
        }

        public long ContarPorNivel(string nivel) {
            try {
                return collection.CountDocuments(l => l.Nivel == nivel);
            }
            catch {
                return 0;
            }
        }

        public bool LimparLogsAntigos(int diasParaManter) {
            try {
                var dataLimite = DateTime.Now.AddDays(-diasParaManter);
                var result = collection.DeleteMany(l => l.DataHora < dataLimite);
                return result.DeletedCount > 0;
            }
            catch (Exception ex) {
                throw new Exception("Erro ao limpar logs antigos: " + ex.Message);
            }
        }

        public bool Deletar(string id) {
            try {
                var result = collection.DeleteOne(l => l.Id == id);
                return result.DeletedCount > 0;
            }
            catch (Exception ex) {
                throw new Exception("Erro ao deletar log: " + ex.Message);
            }
        }
    }
}