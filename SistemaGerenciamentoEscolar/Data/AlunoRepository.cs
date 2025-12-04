using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using SistemaGerenciamentoEscolar.Models;

namespace SistemaGerenciamentoEscolar.Data {
    public class AlunoRepository {
        public bool Inserir(Aluno aluno) {
            try {
                using (SqlConnection conn = ConexaoSQL.ObterConexao()) {
                    string query = @"INSERT INTO Aluno (Nome, CPF, DataNascimento, Telefone, Email, Endereco, DataCadastro, Ativo) 
                                   VALUES (@Nome, @CPF, @DataNascimento, @Telefone, @Email, @Endereco, @DataCadastro, @Ativo)";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Nome", aluno.Nome);
                    cmd.Parameters.AddWithValue("@CPF", aluno.CPF);
                    cmd.Parameters.AddWithValue("@DataNascimento", aluno.DataNascimento);
                    cmd.Parameters.AddWithValue("@Telefone", aluno.Telefone ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Email", aluno.Email ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Endereco", aluno.Endereco ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@DataCadastro", aluno.DataCadastro);
                    cmd.Parameters.AddWithValue("@Ativo", aluno.Ativo);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex) {
                throw new Exception("Erro ao inserir aluno: " + ex.Message);
            }
        }

        public bool Atualizar(Aluno aluno) {
            try {
                using (SqlConnection conn = ConexaoSQL.ObterConexao()) {
                    string query = @"UPDATE Aluno SET Nome = @Nome, CPF = @CPF, DataNascimento = @DataNascimento, 
                                   Telefone = @Telefone, Email = @Email, Endereco = @Endereco, Ativo = @Ativo 
                                   WHERE AlunoId = @AlunoId";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@AlunoId", aluno.AlunoId);
                    cmd.Parameters.AddWithValue("@Nome", aluno.Nome);
                    cmd.Parameters.AddWithValue("@CPF", aluno.CPF);
                    cmd.Parameters.AddWithValue("@DataNascimento", aluno.DataNascimento);
                    cmd.Parameters.AddWithValue("@Telefone", aluno.Telefone ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Email", aluno.Email ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Endereco", aluno.Endereco ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Ativo", aluno.Ativo);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex) {
                throw new Exception("Erro ao atualizar aluno: " + ex.Message);
            }
        }

        public bool Deletar(int alunoId) {
            try {
                using (SqlConnection conn = ConexaoSQL.ObterConexao()) {
                    string query = "UPDATE Aluno SET Ativo = 0 WHERE AlunoId = @AlunoId";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@AlunoId", alunoId);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex) {
                throw new Exception("Erro ao deletar aluno: " + ex.Message);
            }
        }

        public List<Aluno> ListarTodos() {
            List<Aluno> alunos = new List<Aluno>();
            try {
                using (SqlConnection conn = ConexaoSQL.ObterConexao()) {
                    string query = "SELECT * FROM Aluno WHERE Ativo = 1 ORDER BY Nome";
                    SqlCommand cmd = new SqlCommand(query, conn);

                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read()) {
                        alunos.Add(new Aluno {
                            AlunoId = (int)reader["AlunoId"],
                            Nome = reader["Nome"].ToString(),
                            CPF = reader["CPF"].ToString(),
                            DataNascimento = (DateTime)reader["DataNascimento"],
                            Telefone = reader["Telefone"] != DBNull.Value ? reader["Telefone"].ToString() : "",
                            Email = reader["Email"] != DBNull.Value ? reader["Email"].ToString() : "",
                            Endereco = reader["Endereco"] != DBNull.Value ? reader["Endereco"].ToString() : "",
                            DataCadastro = (DateTime)reader["DataCadastro"],
                            Ativo = (bool)reader["Ativo"]
                        });
                    }
                }
            }
            catch (Exception ex) {
                throw new Exception("Erro ao listar alunos: " + ex.Message);
            }
            return alunos;
        }

        public Aluno BuscarPorId(int alunoId) {
            try {
                using (SqlConnection conn = ConexaoSQL.ObterConexao()) {
                    string query = "SELECT * FROM Aluno WHERE AlunoId = @AlunoId";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@AlunoId", alunoId);

                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read()) {
                        return new Aluno {
                            AlunoId = (int)reader["AlunoId"],
                            Nome = reader["Nome"].ToString(),
                            CPF = reader["CPF"].ToString(),
                            DataNascimento = (DateTime)reader["DataNascimento"],
                            Telefone = reader["Telefone"] != DBNull.Value ? reader["Telefone"].ToString() : "",
                            Email = reader["Email"] != DBNull.Value ? reader["Email"].ToString() : "",
                            Endereco = reader["Endereco"] != DBNull.Value ? reader["Endereco"].ToString() : "",
                            DataCadastro = (DateTime)reader["DataCadastro"],
                            Ativo = (bool)reader["Ativo"]
                        };
                    }
                }
            }
            catch (Exception ex) {
                throw new Exception("Erro ao buscar aluno: " + ex.Message);
            }
            return null;
        }
    }
}