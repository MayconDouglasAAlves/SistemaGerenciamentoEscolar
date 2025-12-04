using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using SistemaGerenciamentoEscolar.Models;

namespace SistemaGerenciamentoEscolar.Data {
    // repository para Curso

    public class CursoRepository {
        public bool Inserir(Curso curso) {
            try {
                using (SqlConnection conn = ConexaoSQL.ObterConexao()) {
                    string query = @"INSERT INTO Curso (Nome, Descricao, CargaHoraria, DataCurso, HorarioInicio, HorarioFim, Ativo) 
                                   VALUES (@Nome, @Descricao, @CargaHoraria, @DataCurso, @HorarioInicio, @HorarioFim, @Ativo)";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Nome", curso.Nome);
                    cmd.Parameters.AddWithValue("@Descricao", curso.Descricao ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@CargaHoraria", curso.CargaHoraria);
                    cmd.Parameters.AddWithValue("@DataCurso", curso.DataCurso);
                    cmd.Parameters.AddWithValue("@HorarioInicio", curso.HorarioInicio);
                    cmd.Parameters.AddWithValue("@HorarioFim", curso.HorarioFim);
                    cmd.Parameters.AddWithValue("@Ativo", curso.Ativo);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex) {
                throw new Exception("Erro ao inserir curso: " + ex.Message);
            }
        }

        public List<Curso> ListarTodos() {
            List<Curso> cursos = new List<Curso>();
            try {
                using (SqlConnection conn = ConexaoSQL.ObterConexao()) {
                    string query = "SELECT * FROM Curso WHERE Ativo = 1 ORDER BY Nome";
                    SqlCommand cmd = new SqlCommand(query, conn);

                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read()) {
                        cursos.Add(new Curso {
                            CursoId = (int)reader["CursoId"],
                            Nome = reader["Nome"].ToString(),
                            Descricao = reader["Descricao"] != DBNull.Value ? reader["Descricao"].ToString() : "",
                            CargaHoraria = (int)reader["CargaHoraria"],
                            DataCurso = (DateTime)reader["DataCurso"],
                            HorarioInicio = (TimeSpan)reader["HorarioInicio"],
                            HorarioFim = (TimeSpan)reader["HorarioFim"],
                            Ativo = (bool)reader["Ativo"]
                        });
                    }
                }
            }
            catch (Exception ex) {
                throw new Exception("Erro ao listar cursos: " + ex.Message);
            }
            return cursos;
        }

        public bool Atualizar(Curso curso) {
            try {
                using (SqlConnection conn = ConexaoSQL.ObterConexao()) {
                    string query = @"UPDATE Curso SET Nome = @Nome, Descricao = @Descricao, 
                               CargaHoraria = @CargaHoraria, DataCurso = @DataCurso, 
                               HorarioInicio = @HorarioInicio, HorarioFim = @HorarioFim, 
                               Ativo = @Ativo WHERE CursoId = @CursoId";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@CursoId", curso.CursoId);
                    cmd.Parameters.AddWithValue("@Nome", curso.Nome);
                    cmd.Parameters.AddWithValue("@Descricao", curso.Descricao ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@CargaHoraria", curso.CargaHoraria);
                    cmd.Parameters.AddWithValue("@DataCurso", curso.DataCurso);
                    cmd.Parameters.AddWithValue("@HorarioInicio", curso.HorarioInicio);
                    cmd.Parameters.AddWithValue("@HorarioFim", curso.HorarioFim);
                    cmd.Parameters.AddWithValue("@Ativo", curso.Ativo);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex) {
                throw new Exception("Erro ao atualizar curso: " + ex.Message);
            }
        }

        public bool Deletar(int cursoId) {
            try {
                using (SqlConnection conn = ConexaoSQL.ObterConexao()) {
                    string query = "UPDATE Curso SET Ativo = 0 WHERE CursoId = @CursoId";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@CursoId", cursoId);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex) {
                throw new Exception("Erro ao deletar curso: " + ex.Message);
            }
        }
    }

    // repository para Laboratório

    public class LaboratorioRepository {
        public bool Inserir(Laboratorio laboratorio) {
            try {
                using (SqlConnection conn = ConexaoSQL.ObterConexao()) {
                    string query = @"INSERT INTO Laboratorio (EscolaId, Nome, Capacidade, Localizacao, Equipamentos, Ativo) 
                                   VALUES (@EscolaId, @Nome, @Capacidade, @Localizacao, @Equipamentos, @Ativo)";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@EscolaId", laboratorio.EscolaId);
                    cmd.Parameters.AddWithValue("@Nome", laboratorio.Nome);
                    cmd.Parameters.AddWithValue("@Capacidade", laboratorio.Capacidade);
                    cmd.Parameters.AddWithValue("@Localizacao", laboratorio.Localizacao ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Equipamentos", laboratorio.Equipamentos ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Ativo", laboratorio.Ativo);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex) {
                throw new Exception("Erro ao inserir laboratório: " + ex.Message);
            }
        }

        public List<Laboratorio> ListarTodos() {
            List<Laboratorio> laboratorios = new List<Laboratorio>();
            try {
                using (SqlConnection conn = ConexaoSQL.ObterConexao()) {
                    string query = @"SELECT L.*, E.Nome AS EscolaNome 
                                   FROM Laboratorio L
                                   INNER JOIN Escola E ON L.EscolaId = E.EscolaId
                                   WHERE L.Ativo = 1
                                   ORDER BY L.Nome";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read()) {
                        laboratorios.Add(new Laboratorio {
                            LaboratorioId = (int)reader["LaboratorioId"],
                            EscolaId = (int)reader["EscolaId"],
                            Nome = reader["Nome"].ToString(),
                            Capacidade = (int)reader["Capacidade"],
                            Localizacao = reader["Localizacao"] != DBNull.Value ? reader["Localizacao"].ToString() : "",
                            Equipamentos = reader["Equipamentos"] != DBNull.Value ? reader["Equipamentos"].ToString() : "",
                            Ativo = (bool)reader["Ativo"],
                            EscolaNome = reader["EscolaNome"].ToString()
                        });
                    }
                }
            }
            catch (Exception ex) {
                throw new Exception("Erro ao listar laboratórios: " + ex.Message);
            }
            return laboratorios;
        }
    }
}