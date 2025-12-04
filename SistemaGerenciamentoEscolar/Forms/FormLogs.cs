using System;
using System.Windows.Forms;
using SistemaGerenciamentoEscolar.Models;
using SistemaGerenciamentoEscolar.Data;

namespace SistemaGerenciamentoEscolar.Forms {
    public class FormLogs : Form {
        private LogRepository logRepo = new LogRepository();
        private DataGridView dgvLogs;
        private ComboBox cmbNivel, cmbModulo;
        private DateTimePicker dtpInicio, dtpFim;
        private Label lblTotalLogs, lblInfo, lblWarning, lblError;
        private Button btnFiltrar, btnAtualizar, btnLimparAntigos, btnExcluir;
        private string logIdSelecionado = "";

        public FormLogs() {
            CriarComponentes();
            VerificarConexaoMongo();
            CarregarLogs();
            AtualizarEstatisticas();
        }

        private void VerificarConexaoMongo() {
            if (!ConexaoMongoDB.TestarConexao()) {
                MessageBox.Show("Erro ao conectar com MongoDB!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CriarComponentes() {
            this.Text = "Logs do Sistema (NoSQL - MongoDB)";
            this.Size = new System.Drawing.Size(1200, 700);

            GroupBox gbFiltros = new GroupBox {
                Text = "Filtros",
                Location = new System.Drawing.Point(20, 20),
                Size = new System.Drawing.Size(1150, 100)
            };

            int y = 30;
            gbFiltros.Controls.Add(new Label { Text = "Nível:", Location = new System.Drawing.Point(20, y), AutoSize = true });
            cmbNivel = new ComboBox { Location = new System.Drawing.Point(80, y - 3), Size = new System.Drawing.Size(120, 25), DropDownStyle = ComboBoxStyle.DropDownList };
            cmbNivel.Items.AddRange(new string[] { "Todos", "Info", "Warning", "Error", "Critical" });
            cmbNivel.SelectedIndex = 0;
            gbFiltros.Controls.Add(cmbNivel);

            gbFiltros.Controls.Add(new Label { Text = "Módulo:", Location = new System.Drawing.Point(220, y), AutoSize = true });
            cmbModulo = new ComboBox { Location = new System.Drawing.Point(290, y - 3), Size = new System.Drawing.Size(150, 25), DropDownStyle = ComboBoxStyle.DropDownList };
            cmbModulo.Items.AddRange(new string[] { "Todos", "Aluno", "Curso", "Turma", "Matrícula", "Presença", "Feedback" });
            cmbModulo.SelectedIndex = 0;
            gbFiltros.Controls.Add(cmbModulo);

            y += 35;
            gbFiltros.Controls.Add(new Label { Text = "Período:", Location = new System.Drawing.Point(20, y), AutoSize = true });
            dtpInicio = new DateTimePicker { Location = new System.Drawing.Point(80, y - 3), Size = new System.Drawing.Size(150, 25), Format = DateTimePickerFormat.Short };
            dtpInicio.Value = DateTime.Now.AddDays(-7);
            gbFiltros.Controls.Add(dtpInicio);

            gbFiltros.Controls.Add(new Label { Text = "até", Location = new System.Drawing.Point(240, y), AutoSize = true });
            dtpFim = new DateTimePicker { Location = new System.Drawing.Point(270, y - 3), Size = new System.Drawing.Size(150, 25), Format = DateTimePickerFormat.Short };
            gbFiltros.Controls.Add(dtpFim);

            btnFiltrar = new Button { Text = "Filtrar", Location = new System.Drawing.Point(440, y - 5), Size = new System.Drawing.Size(100, 30) };
            btnFiltrar.Click += (s, e) => FiltrarLogs();
            gbFiltros.Controls.Add(btnFiltrar);

            btnLimparAntigos = new Button { Text = "Limpar Antigos", Location = new System.Drawing.Point(550, y - 5), Size = new System.Drawing.Size(120, 30) };
            btnLimparAntigos.Click += (s, e) => LimparLogsAntigos();
            gbFiltros.Controls.Add(btnLimparAntigos);

            btnExcluir = new Button {
                Text = "Excluir Selecionado",
                Location = new System.Drawing.Point(680, y - 5),
                Size = new System.Drawing.Size(140, 30),
                BackColor = System.Drawing.Color.FromArgb(220, 53, 69),
                ForeColor = System.Drawing.Color.White
            };
            btnExcluir.Click += BtnExcluir_Click;
            gbFiltros.Controls.Add(btnExcluir);

            btnAtualizar = new Button { Text = "Atualizar", Location = new System.Drawing.Point(830, y - 5), Size = new System.Drawing.Size(100, 30) };
            btnAtualizar.Click += (s, e) => { CarregarLogs(); AtualizarEstatisticas(); };
            gbFiltros.Controls.Add(btnAtualizar);

            this.Controls.Add(gbFiltros);

            GroupBox gbEstatisticas = new GroupBox {
                Text = "Estatísticas",
                Location = new System.Drawing.Point(20, 130),
                Size = new System.Drawing.Size(1150, 70)
            };

            lblTotalLogs = new Label { Text = "Total: 0", Location = new System.Drawing.Point(20, 30), AutoSize = true, Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold) };
            gbEstatisticas.Controls.Add(lblTotalLogs);

            lblInfo = new Label { Text = "Info: 0", Location = new System.Drawing.Point(200, 30), AutoSize = true, ForeColor = System.Drawing.Color.Green, Font = new System.Drawing.Font("Segoe UI", 10) };
            gbEstatisticas.Controls.Add(lblInfo);

            lblWarning = new Label { Text = "Warning: 0", Location = new System.Drawing.Point(350, 30), AutoSize = true, ForeColor = System.Drawing.Color.Orange, Font = new System.Drawing.Font("Segoe UI", 10) };
            gbEstatisticas.Controls.Add(lblWarning);

            lblError = new Label { Text = "Error: 0", Location = new System.Drawing.Point(500, 30), AutoSize = true, ForeColor = System.Drawing.Color.Red, Font = new System.Drawing.Font("Segoe UI", 10) };
            gbEstatisticas.Controls.Add(lblError);

            this.Controls.Add(gbEstatisticas);

            dgvLogs = new DataGridView {
                Location = new System.Drawing.Point(20, 210),
                Size = new System.Drawing.Size(1150, 450),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                ReadOnly = true,
                AllowUserToAddRows = false
            };
            dgvLogs.CellClick += DgvLogs_CellClick;
            this.Controls.Add(dgvLogs);
        }

        private void CarregarLogs() {
            try {
                dgvLogs.DataSource = null;
                dgvLogs.DataSource = logRepo.ListarTodos(500);

                if (dgvLogs.Columns.Contains("Id"))
                    dgvLogs.Columns["Id"].Visible = false;
                if (dgvLogs.Columns.Contains("Detalhes"))
                    dgvLogs.Columns["Detalhes"].Visible = false;
                if (dgvLogs.Columns.Contains("IpAddress"))
                    dgvLogs.Columns["IpAddress"].Visible = false;
            }
            catch (Exception ex) {
                MessageBox.Show("Erro: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FiltrarLogs() {
            try {
                var logs = logRepo.ListarPorPeriodo(dtpInicio.Value.Date, dtpFim.Value.Date.AddDays(1).AddSeconds(-1));
                dgvLogs.DataSource = null;
                dgvLogs.DataSource = logs;
            }
            catch (Exception ex) {
                MessageBox.Show("Erro: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AtualizarEstatisticas() {
            try {
                var total = logRepo.ListarTodos(5000).Count;
                var info = logRepo.ContarPorNivel("Info");
                var warning = logRepo.ContarPorNivel("Warning");
                var error = logRepo.ContarPorNivel("Error");

                lblTotalLogs.Text = $"Total: {total}";
                lblInfo.Text = $"Info: {info}";
                lblWarning.Text = $"Warning: {warning}";
                lblError.Text = $"Error: {error}";
            }
            catch { }
        }

        private void LimparLogsAntigos() {
            var resultado = MessageBox.Show(
                "Deseja limpar logs com mais de 30 dias?",
                "Confirmar",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (resultado == DialogResult.Yes) {
                try {
                    logRepo.LimparLogsAntigos(30);
                    MessageBox.Show("Logs antigos removidos com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CarregarLogs();
                    AtualizarEstatisticas();
                }
                catch (Exception ex) {
                    MessageBox.Show("Erro: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnExcluir_Click(object sender, EventArgs e) {
            if (string.IsNullOrEmpty(logIdSelecionado)) {
                MessageBox.Show("Selecione um log na lista para excluir!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var resultado = MessageBox.Show(
                "Deseja realmente excluir este log?",
                "Confirmar Exclusão",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (resultado == DialogResult.Yes) {
                try {
                    MessageBox.Show("Log excluído com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    logIdSelecionado = "";
                    CarregarLogs();
                    AtualizarEstatisticas();
                }
                catch (Exception ex) {
                    MessageBox.Show("Erro ao excluir log: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void DgvLogs_CellClick(object sender, DataGridViewCellEventArgs e) {
            if (e.RowIndex >= 0) {
                DataGridViewRow row = dgvLogs.Rows[e.RowIndex];
                logIdSelecionado = row.Cells["Id"].Value.ToString();
            }
        }
    }
}