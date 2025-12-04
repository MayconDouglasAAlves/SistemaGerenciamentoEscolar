using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using SistemaGerenciamentoEscolar.Models;

namespace SistemaGerenciamentoEscolar.Data {
    public class PresencaRepository {
        public bool RegistrarPresenca(Presenca presenca) {
            try {
                using (SqlConnection conn = ConexaoSQL.ObterConexao()) {
                    string query = @"INSERT INTO Presenca (MatriculaId, DataPresenca, Status, Observacoes) 
                                   VALUES (@MatriculaId, @DataPresenca, @Status, @Observacoes)";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@MatriculaId", presenca.MatriculaId);
                    cmd.Parameters.AddWithValue("@DataPresenca", presenca.DataPresenca);
                    cmd.Parameters.AddWithValue("@Status", presenca.Status);
                    cmd.Parameters.AddWithValue("@Observacoes", presenca.Observacoes ?? (object)DBNull.Value);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex) {
                throw new Exception("Erro ao registrar presença: " + ex.Message);
            }
        }

        public bool AtualizarPresenca(Presenca presenca) {
            try {
                using (SqlConnection conn = ConexaoSQL.ObterConexao()) {
                    string query = @"UPDATE Presenca SET Status = @Status, Observacoes = @Observacoes 
                                   WHERE PresencaId = @PresencaId";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@PresencaId", presenca.PresencaId);
                    cmd.Parameters.AddWithValue("@Status", presenca.Status);
                    cmd.Parameters.AddWithValue("@Observacoes", presenca.Observacoes ?? (object)DBNull.Value);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex) {
                throw new Exception("Erro ao atualizar presença: " + ex.Message);
            }
        }

        public List<Presenca> ListarPorTurma(int turmaId) {
            List<Presenca> presencas = new List<Presenca>();
            try {
                using (SqlConnection conn = ConexaoSQL.ObterConexao()) {
                    string query = @"SELECT P.*, A.Nome AS AlunoNome, M.NumeroMatricula
                                   FROM Presenca P
                                   INNER JOIN Matricula M ON P.MatriculaId = M.MatriculaId
                                   INNER JOIN Aluno A ON M.AlunoId = A.AlunoId
                                   WHERE M.TurmaId = @TurmaId
                                   ORDER BY A.Nome";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@TurmaId", turmaId);

                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read()) {
                        presencas.Add(new Presenca {
                            PresencaId = (int)reader["PresencaId"],
                            MatriculaId = (int)reader["MatriculaId"],
                            DataPresenca = (DateTime)reader["DataPresenca"],
                            Status = reader["Status"].ToString(),
                            Observacoes = reader["Observacoes"] != DBNull.Value ? reader["Observacoes"].ToString() : "",
                            AlunoNome = reader["AlunoNome"].ToString(),
                            NumeroMatricula = reader["NumeroMatricula"].ToString()
                        });
                    }
                }
            }
            catch (Exception ex) {
                throw new Exception("Erro ao listar presenças: " + ex.Message);
            }
            return presencas;
        }

        public bool VerificarPresencaExistente(int matriculaId, DateTime dataPresenca) {
            try {
                using (SqlConnection conn = ConexaoSQL.ObterConexao()) {
                    string query = @"SELECT COUNT(*) FROM Presenca 
                                   WHERE MatriculaId = @MatriculaId 
                                   AND CAST(DataPresenca AS DATE) = CAST(@DataPresenca AS DATE)";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@MatriculaId", matriculaId);
                    cmd.Parameters.AddWithValue("@DataPresenca", dataPresenca);

                    conn.Open();
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
            catch (Exception ex) {
                throw new Exception("Erro ao verificar presença: " + ex.Message);
            }
        }
    }
}