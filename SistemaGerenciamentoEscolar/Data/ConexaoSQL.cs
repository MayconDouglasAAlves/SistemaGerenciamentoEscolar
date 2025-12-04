using System.Data.SqlClient;

namespace SistemaGerenciamentoEscolar.Data {
    public static class ConexaoSQL {
        // string de conexão com o SQL Server
        private static readonly string connectionString =
            @"Server=MAYCON\SQLEXPRESS;Database=GerenciamentoEscolar;User Id=sa;Password=123;";

        // obter uma nova conexão
        public static SqlConnection ObterConexao() {
            return new SqlConnection(connectionString);
        }

        // testar a conexão
        public static bool TestarConexao() {
            try {
                using (SqlConnection conn = ObterConexao()) {
                    conn.Open();
                    return true;
                }
            }
            catch {
                return false;
            }
        }
    }
}