using System;
using System.Windows.Forms;
using SistemaGerenciamentoEscolar.Data;
using SistemaGerenciamentoEscolar.Forms;

namespace SistemaGerenciamentoEscolar {
    public partial class FormPrincipal : Form {
        public FormPrincipal() {
            InitializeComponent();
            ConfigurarMenu();
            TestarConexao();
        }

        protected override void OnResize(EventArgs e) {
            base.OnResize(e);

            if (tituloSistema != null && painelSuperior != null) {
                tituloSistema.Left = (painelSuperior.Width - tituloSistema.Width) / 2;
                tituloSistema.Top = (painelSuperior.Height - tituloSistema.Height) / 2;
            }
        }

        private void ConfigurarMenu() {
            this.Text = "Sistema de Gerenciamento Escolar";
            this.Size = new System.Drawing.Size(1200, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.WindowState = FormWindowState.Maximized;

            MenuStrip menuPrincipal = new MenuStrip();

            ToolStripMenuItem menuCadastros = new ToolStripMenuItem("Cadastros");
            menuCadastros.DropDownItems.Add("Escolas", null, (s, e) => AbrirForm(new FormEscola()));
            menuCadastros.DropDownItems.Add("Laboratórios", null, (s, e) => AbrirForm(new FormLaboratorio()));
            menuCadastros.DropDownItems.Add("Cursos", null, (s, e) => AbrirForm(new FormCurso()));
            menuCadastros.DropDownItems.Add("Alunos", null, (s, e) => AbrirForm(new FormAluno()));
            menuCadastros.DropDownItems.Add("Turmas", null, (s, e) => AbrirForm(new FormTurma()));

            ToolStripMenuItem menuMatriculas = new ToolStripMenuItem("Matrículas");
            menuMatriculas.DropDownItems.Add("Nova Matrícula", null, (s, e) => AbrirForm(new FormMatricula()));
            menuMatriculas.DropDownItems.Add("Listar Matrículas", null, (s, e) => AbrirForm(new FormListaMatriculas()));

            ToolStripMenuItem menuPresenca = new ToolStripMenuItem("Controle de Presença");
            menuPresenca.DropDownItems.Add("Registrar Presença", null, (s, e) => AbrirForm(new FormPresenca()));
            menuPresenca.DropDownItems.Add("Listar Presenças", null, (s, e) => AbrirForm(new FormListaPresencas()));

            ToolStripMenuItem menuNoSQL = new ToolStripMenuItem("Feedbacks e Logs");
            menuNoSQL.DropDownItems.Add("Feedback dos Alunos", null, (s, e) => AbrirForm(new FormFeedback()));
            menuNoSQL.DropDownItems.Add("Logs do Sistema", null, (s, e) => AbrirForm(new FormLogs()));

            ToolStripMenuItem menuSair = new ToolStripMenuItem("Sair");
            menuSair.Click += (s, e) => Application.Exit();

            menuPrincipal.Items.Add(menuCadastros);
            menuPrincipal.Items.Add(menuMatriculas);
            menuPrincipal.Items.Add(menuPresenca);
            menuPrincipal.Items.Add(menuNoSQL);
            menuPrincipal.Items.Add(menuSair);

            this.MainMenuStrip = menuPrincipal;
            this.Controls.Add(menuPrincipal);

            Panel painelBemVindo = new Panel {
                Dock = DockStyle.Fill,
                BackColor = System.Drawing.Color.FromArgb(240, 240, 240)
            };

            Label lblTitulo = new Label {
                Text = "Sistema de Gerenciamento Escolar",
                Font = new System.Drawing.Font("Segoe UI", 24, System.Drawing.FontStyle.Bold),
                AutoSize = true,
                Location = new System.Drawing.Point(50, 100)
            };

            Label lblSubtitulo = new Label {
                Text = "Bem-vindo! Utilize o menu acima para navegar pelas funcionalidades do sistema.",
                Font = new System.Drawing.Font("Segoe UI", 12),
                AutoSize = true,
                Location = new System.Drawing.Point(50, 160),
                ForeColor = System.Drawing.Color.FromArgb(100, 100, 100)
            };

            painelBemVindo.Controls.Add(lblTitulo);
            painelBemVindo.Controls.Add(lblSubtitulo);
            this.Controls.Add(painelBemVindo);
        }

        private void AbrirForm(Form form) {
            painelConteudo.Controls.Clear();

            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;

            painelConteudo.Controls.Add(form);
            form.Show();
        }


        private void TestarConexao() {
            if (!ConexaoSQL.TestarConexao()) {
                MessageBox.Show("Erro ao conectar com o banco de dados SQL Server!", "Erro de Conexão", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
