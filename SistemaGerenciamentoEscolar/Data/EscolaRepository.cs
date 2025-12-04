using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using SistemaGerenciamentoEscolar.Models;

namespace SistemaGerenciamentoEscolar.Data {
    public class EscolaRepository {
        public bool Inserir(Escola escola) {
            try {
                using (SqlConnection conn = ConexaoSQL.ObterConexao()) {
                    string query = @"INSERT INTO Escola (Nome, CNPJ, Endereco, Telefone, Email, DataCadastro, Ativo) 
                                   VALUES (@Nome, @CNPJ, @Endereco, @Telefone, @Email, @DataCadastro, @Ativo)";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Nome", escola.Nome);
                    cmd.Parameters.AddWithValue("@CNPJ", escola.CNPJ);
                    cmd.Parameters.AddWithValue("@Endereco", escola.Endereco ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Telefone", escola.Telefone ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Email", escola.Email ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@DataCadastro", escola.DataCadastro);
                    cmd.Parameters.AddWithValue("@Ativo", escola.Ativo);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex) {
                throw new Exception("Erro ao inserir escola: " + ex.Message);
            }
        }

        public bool Atualizar(Escola escola) {
            try {
                using (SqlConnection conn = ConexaoSQL.ObterConexao()) {
                    string query = @"UPDATE Escola SET Nome = @Nome, CNPJ = @CNPJ, Endereco = @Endereco, 
                                   Telefone = @Telefone, Email = @Email, Ativo = @Ativo 
                                   WHERE EscolaId = @EscolaId";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@EscolaId", escola.EscolaId);
                    cmd.Parameters.AddWithValue("@Nome", escola.Nome);
                    cmd.Parameters.AddWithValue("@CNPJ", escola.CNPJ);
                    cmd.Parameters.AddWithValue("@Endereco", escola.Endereco ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Telefone", escola.Telefone ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Email", escola.Email ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Ativo", escola.Ativo);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex) {
                throw new Exception("Erro ao atualizar escola: " + ex.Message);
            }
        }

        public bool Deletar(int escolaId) {
            try {
                using (SqlConnection conn = ConexaoSQL.ObterConexao()) {
                    string query = "UPDATE Escola SET Ativo = 0 WHERE EscolaId = @EscolaId";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@EscolaId", escolaId);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex) {
                throw new Exception("Erro ao deletar escola: " + ex.Message);
            }
        }

        public List<Escola> ListarTodos() {
            List<Escola> escolas = new List<Escola>();
            try {
                using (SqlConnection conn = ConexaoSQL.ObterConexao()) {
                    string query = "SELECT * FROM Escola WHERE Ativo = 1 ORDER BY Nome";
                    SqlCommand cmd = new SqlCommand(query, conn);

                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read()) {
                        escolas.Add(new Escola {
                            EscolaId = (int)reader["EscolaId"],
                            Nome = reader["Nome"].ToString(),
                            CNPJ = reader["CNPJ"].ToString(),
                            Endereco = reader["Endereco"] != DBNull.Value ? reader["Endereco"].ToString() : "",
                            Telefone = reader["Telefone"] != DBNull.Value ? reader["Telefone"].ToString() : "",
                            Email = reader["Email"] != DBNull.Value ? reader["Email"].ToString() : "",
                            DataCadastro = (DateTime)reader["DataCadastro"],
                            Ativo = (bool)reader["Ativo"]
                        });
                    }
                }
            }
            catch (Exception ex) {
                throw new Exception("Erro ao listar escolas: " + ex.Message);
            }
            return escolas;
        }

        public Escola BuscarPorId(int escolaId) {
            try {
                using (SqlConnection conn = ConexaoSQL.ObterConexao()) {
                    string query = "SELECT * FROM Escola WHERE EscolaId = @EscolaId";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@EscolaId", escolaId);

                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read()) {
                        return new Escola {
                            EscolaId = (int)reader["EscolaId"],
                            Nome = reader["Nome"].ToString(),
                            CNPJ = reader["CNPJ"].ToString(),
                            Endereco = reader["Endereco"] != DBNull.Value ? reader["Endereco"].ToString() : "",
                            Telefone = reader["Telefone"] != DBNull.Value ? reader["Telefone"].ToString() : "",
                            Email = reader["Email"] != DBNull.Value ? reader["Email"].ToString() : "",
                            DataCadastro = (DateTime)reader["DataCadastro"],
                            Ativo = (bool)reader["Ativo"]
                        };
                    }
                }
            }
            catch (Exception ex) {
                throw new Exception("Erro ao buscar escola: " + ex.Message);
            }
            return null;
        }
    }
}