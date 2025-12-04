using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using SistemaGerenciamentoEscolar.Models;

namespace SistemaGerenciamentoEscolar.Data {
    // repository para Turma
    public class TurmaRepository {
        public bool Inserir(Turma turma) {
            try {
                using (SqlConnection conn = ConexaoSQL.ObterConexao()) {
                    string query = @"INSERT INTO Turma (CursoId, LaboratorioId, Codigo, DataAula, HorarioInicio, HorarioFim, VagasDisponiveis, Observacoes, Ativo) 
                                   VALUES (@CursoId, @LaboratorioId, @Codigo, @DataAula, @HorarioInicio, @HorarioFim, @VagasDisponiveis, @Observacoes, @Ativo)";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@CursoId", turma.CursoId);
                    cmd.Parameters.AddWithValue("@LaboratorioId", turma.LaboratorioId);
                    cmd.Parameters.AddWithValue("@Codigo", turma.Codigo);
                    cmd.Parameters.AddWithValue("@DataAula", turma.DataAula);
                    cmd.Parameters.AddWithValue("@HorarioInicio", turma.HorarioInicio);
                    cmd.Parameters.AddWithValue("@HorarioFim", turma.HorarioFim);
                    cmd.Parameters.AddWithValue("@VagasDisponiveis", turma.VagasDisponiveis);
                    cmd.Parameters.AddWithValue("@Observacoes", turma.Observacoes ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Ativo", turma.Ativo);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex) {
                throw new Exception("Erro ao inserir turma: " + ex.Message);
            }
        }

        public bool Deletar(int matriculaId) {
            try {
                using (SqlConnection conn = ConexaoSQL.ObterConexao()) {
                    string query = "DELETE FROM Matricula WHERE MatriculaId = @MatriculaId";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@MatriculaId", matriculaId);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex) {
                throw new Exception("Erro ao deletar matrícula: " + ex.Message);
            }
        }

        public List<Turma> ListarTodos() {
            List<Turma> turmas = new List<Turma>();
            try {
                using (SqlConnection conn = ConexaoSQL.ObterConexao()) {
                    string query = @"SELECT T.*, C.Nome AS CursoNome, L.Nome AS LaboratorioNome
                                   FROM Turma T
                                   INNER JOIN Curso C ON T.CursoId = C.CursoId
                                   INNER JOIN Laboratorio L ON T.LaboratorioId = L.LaboratorioId
                                   WHERE T.Ativo = 1
                                   ORDER BY T.DataAula DESC";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read()) {
                        turmas.Add(new Turma {
                            TurmaId = (int)reader["TurmaId"],
                            CursoId = (int)reader["CursoId"],
                            LaboratorioId = (int)reader["LaboratorioId"],
                            Codigo = reader["Codigo"].ToString(),
                            DataAula = (DateTime)reader["DataAula"],
                            HorarioInicio = (TimeSpan)reader["HorarioInicio"],
                            HorarioFim = (TimeSpan)reader["HorarioFim"],
                            VagasDisponiveis = (int)reader["VagasDisponiveis"],
                            Observacoes = reader["Observacoes"] != DBNull.Value ? reader["Observacoes"].ToString() : "",
                            Ativo = (bool)reader["Ativo"],
                            CursoNome = reader["CursoNome"].ToString(),
                            LaboratorioNome = reader["LaboratorioNome"].ToString()
                        });
                    }
                }
            }
            catch (Exception ex) {
                throw new Exception("Erro ao listar turmas: " + ex.Message);
            }
            return turmas;
        }
    }

    // repository para Matrícula
    public class MatriculaRepository {
        public bool Inserir(Matricula matricula) {
            try {
                using (SqlConnection conn = ConexaoSQL.ObterConexao()) {
                    string query = @"INSERT INTO Matricula (AlunoId, TurmaId, NumeroMatricula, DataMatricula, Status, Observacoes) 
                                   VALUES (@AlunoId, @TurmaId, @NumeroMatricula, @DataMatricula, @Status, @Observacoes)";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@AlunoId", matricula.AlunoId);
                    cmd.Parameters.AddWithValue("@TurmaId", matricula.TurmaId);
                    cmd.Parameters.AddWithValue("@NumeroMatricula", matricula.NumeroMatricula);
                    cmd.Parameters.AddWithValue("@DataMatricula", matricula.DataMatricula);
                    cmd.Parameters.AddWithValue("@Status", matricula.Status);
                    cmd.Parameters.AddWithValue("@Observacoes", matricula.Observacoes ?? (object)DBNull.Value);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex) {
                throw new Exception("Erro ao inserir matrícula: " + ex.Message);
            }
        }

        public List<Matricula> ListarPorTurma(int turmaId) {
            List<Matricula> matriculas = new List<Matricula>();
            try {
                using (SqlConnection conn = ConexaoSQL.ObterConexao()) {
                    string query = @"SELECT M.*, A.Nome AS AlunoNome, T.Codigo AS TurmaCodigo, C.Nome AS CursoNome
                                   FROM Matricula M
                                   INNER JOIN Aluno A ON M.AlunoId = A.AlunoId
                                   INNER JOIN Turma T ON M.TurmaId = T.TurmaId
                                   INNER JOIN Curso C ON T.CursoId = C.CursoId
                                   WHERE M.TurmaId = @TurmaId
                                   ORDER BY A.Nome";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@TurmaId", turmaId);

                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read()) {
                        matriculas.Add(new Matricula {
                            MatriculaId = (int)reader["MatriculaId"],
                            AlunoId = (int)reader["AlunoId"],
                            TurmaId = (int)reader["TurmaId"],
                            NumeroMatricula = reader["NumeroMatricula"].ToString(),
                            DataMatricula = (DateTime)reader["DataMatricula"],
                            Status = reader["Status"].ToString(),
                            Observacoes = reader["Observacoes"] != DBNull.Value ? reader["Observacoes"].ToString() : "",
                            AlunoNome = reader["AlunoNome"].ToString(),
                            TurmaCodigo = reader["TurmaCodigo"].ToString(),
                            CursoNome = reader["CursoNome"].ToString()
                        });
                    }
                }
            }
            catch (Exception ex) {
                throw new Exception("Erro ao listar matrículas: " + ex.Message);
            }
            return matriculas;
        }

        public string GerarNumeroMatricula() {
            return DateTime.Now.Year.ToString() + DateTime.Now.Ticks.ToString().Substring(8);
        }
    }
}