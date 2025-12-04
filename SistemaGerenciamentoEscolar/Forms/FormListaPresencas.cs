using System;
using System.Windows.Forms;
using SistemaGerenciamentoEscolar.Models;
using SistemaGerenciamentoEscolar.Data;

namespace SistemaGerenciamentoEscolar.Forms {
    public class FormListaPresencas : Form {
        private PresencaRepository presencaRepo = new PresencaRepository();
        private TurmaRepository turmaRepo = new TurmaRepository();
        private ComboBox cmbTurma, cmbFiltroStatus;
        private DateTimePicker dtpDataInicio, dtpDataFim;
        private DataGridView dgvPresencas;
        private Button btnBuscarTurma, btnBuscarData, btnFiltrar, btnExportar, btnLimpar;
        private Label lblTotal, lblPresentes, lblAusentes;

        public FormListaPresencas() {
            CriarComponentes();
        }

        private void CriarComponentes() {
            this.Text = "Lista de Presenças Registradas";
            this.Size = new System.Drawing.Size(1150, 700);

            GroupBox gbFiltros = new GroupBox {
                Text = "Filtros de Busca",
                Location = new System.Drawing.Point(20, 12),
                Size = new System.Drawing.Size(1100, 150)
            };

            int y = 30;
            gbFiltros.Controls.Add(new Label { Text = "Buscar por Turma:", Location = new System.Drawing.Point(20, y), AutoSize = true, Font = new System.Drawing.Font("Segoe UI", 9, System.Drawing.FontStyle.Bold) });

            y += 25;
            gbFiltros.Controls.Add(new Label { Text = "Turma:", Location = new System.Drawing.Point(30, y), AutoSize = true });
            cmbTurma = new ComboBox { Location = new System.Drawing.Point(90, y - 3), Size = new System.Drawing.Size(300, 25), DropDownStyle = ComboBoxStyle.DropDownList };
            cmbTurma.DataSource = turmaRepo.ListarTodos();
            cmbTurma.DisplayMember = "Codigo";
            cmbTurma.ValueMember = "TurmaId";
            gbFiltros.Controls.Add(cmbTurma);

            btnBuscarTurma = new Button {
                Text = "🔍 Buscar por Turma",
                Location = new System.Drawing.Point(400, y - 5),
                Size = new System.Drawing.Size(150, 30),
                BackColor = System.Drawing.Color.FromArgb(0, 123, 255),
                ForeColor = System.Drawing.Color.White
            };
            btnBuscarTurma.Click += BtnBuscarTurma_Click;
            gbFiltros.Controls.Add(btnBuscarTurma);

            y += 40;
            gbFiltros.Controls.Add(new Label { Text = "Buscar por Período:", Location = new System.Drawing.Point(20, y), AutoSize = true, Font = new System.Drawing.Font("Segoe UI", 9, System.Drawing.FontStyle.Bold) });

            y += 25;
            gbFiltros.Controls.Add(new Label { Text = "De:", Location = new System.Drawing.Point(30, y), AutoSize = true });
            dtpDataInicio = new DateTimePicker { Location = new System.Drawing.Point(70, y - 3), Size = new System.Drawing.Size(150, 25), Format = DateTimePickerFormat.Short };
            dtpDataInicio.Value = DateTime.Now.AddDays(-7);
            gbFiltros.Controls.Add(dtpDataInicio);

            gbFiltros.Controls.Add(new Label { Text = "Até:", Location = new System.Drawing.Point(240, y), AutoSize = true });
            dtpDataFim = new DateTimePicker { Location = new System.Drawing.Point(280, y - 3), Size = new System.Drawing.Size(150, 25), Format = DateTimePickerFormat.Short };
            gbFiltros.Controls.Add(dtpDataFim);

            btnBuscarData = new Button {
                Text = "📅 Buscar por Data",
                Location = new System.Drawing.Point(450, y - 5),
                Size = new System.Drawing.Size(150, 30),
                BackColor = System.Drawing.Color.FromArgb(40, 167, 69),
                ForeColor = System.Drawing.Color.White
            };
            btnBuscarData.Click += BtnBuscarData_Click;
            gbFiltros.Controls.Add(btnBuscarData);

            gbFiltros.Controls.Add(new Label { Text = "Status:", Location = new System.Drawing.Point(620, y), AutoSize = true });
            cmbFiltroStatus = new ComboBox { Location = new System.Drawing.Point(680, y - 3), Size = new System.Drawing.Size(120, 25), DropDownStyle = ComboBoxStyle.DropDownList };
            cmbFiltroStatus.Items.AddRange(new string[] { "Todos", "Presente", "Ausente", "Justificado" });
            cmbFiltroStatus.SelectedIndex = 0;
            gbFiltros.Controls.Add(cmbFiltroStatus);

            btnFiltrar = new Button { Text = "Filtrar", Location = new System.Drawing.Point(810, y - 5), Size = new System.Drawing.Size(80, 30) };
            btnFiltrar.Click += BtnFiltrar_Click;
            gbFiltros.Controls.Add(btnFiltrar);

            btnLimpar = new Button {
                Text = "🗑️ Limpar",
                Location = new System.Drawing.Point(900, y - 5),
                Size = new System.Drawing.Size(80, 30),
                BackColor = System.Drawing.Color.FromArgb(108, 117, 125),
                ForeColor = System.Drawing.Color.White
            };
            btnLimpar.Click += BtnLimpar_Click;
            gbFiltros.Controls.Add(btnLimpar);

            this.Controls.Add(gbFiltros);

            GroupBox gbEstatisticas = new GroupBox {
                Text = "Estatísticas",
                Location = new System.Drawing.Point(20, 160),
                Size = new System.Drawing.Size(1100, 70)
            };

            lblTotal = new Label {
                Text = "Total: 0",
                Location = new System.Drawing.Point(20, 30),
                AutoSize = true,
                Font = new System.Drawing.Font("Segoe UI", 11, System.Drawing.FontStyle.Bold)
            };
            gbEstatisticas.Controls.Add(lblTotal);

            lblPresentes = new Label {
                Text = "Presentes: 0 (0%)",
                Location = new System.Drawing.Point(250, 30),
                AutoSize = true,
                ForeColor = System.Drawing.Color.Green,
                Font = new System.Drawing.Font("Segoe UI", 11, System.Drawing.FontStyle.Bold)
            };
            gbEstatisticas.Controls.Add(lblPresentes);

            lblAusentes = new Label {
                Text = "Ausentes: 0 (0%)",
                Location = new System.Drawing.Point(550, 30),
                AutoSize = true,
                ForeColor = System.Drawing.Color.Red,
                Font = new System.Drawing.Font("Segoe UI", 11, System.Drawing.FontStyle.Bold)
            };
            gbEstatisticas.Controls.Add(lblAusentes);

            this.Controls.Add(gbEstatisticas);

            dgvPresencas = new DataGridView {
                Location = new System.Drawing.Point(20, 240),
                Size = new System.Drawing.Size(1100, 410),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle { BackColor = System.Drawing.Color.FromArgb(240, 240, 240) }
            };
            this.Controls.Add(dgvPresencas);
        }

        private void BtnBuscarTurma_Click(object sender, EventArgs e) {
            try {
                if (cmbTurma.SelectedValue == null) {
                    MessageBox.Show("Selecione uma turma!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int turmaId = (int)cmbTurma.SelectedValue;
                var presencas = presencaRepo.ListarPorTurma(turmaId);

                ExibirPresencas(presencas);

                if (presencas.Count == 0) {
                    MessageBox.Show("Nenhuma presença encontrada para esta turma.", "Informação", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex) {
                MessageBox.Show("Erro: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnBuscarData_Click(object sender, EventArgs e) {
            try {
                DateTime dataInicio = dtpDataInicio.Value.Date;
                DateTime dataFim = dtpDataFim.Value.Date.AddDays(1).AddSeconds(-1);

                if (dataInicio > dataFim) {
                    MessageBox.Show("A data inicial não pode ser maior que a data final!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var presencas = BuscarPresencasPorPeriodo(dataInicio, dataFim);

                ExibirPresencas(presencas);

                if (presencas.Count == 0) {
                    MessageBox.Show($"Nenhuma presença encontrada entre {dtpDataInicio.Value:dd/MM/yyyy} e {dtpDataFim.Value:dd/MM/yyyy}.",
                        "Informação", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex) {
                MessageBox.Show("Erro: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private System.Collections.Generic.List<Presenca> BuscarPresencasPorPeriodo(DateTime inicio, DateTime fim) {
            var todasPresencas = new System.Collections.Generic.List<Presenca>();
            var turmas = turmaRepo.ListarTodos();

            foreach (var turma in turmas) {
                var presencasTurma = presencaRepo.ListarPorTurma(turma.TurmaId);
                todasPresencas.AddRange(presencasTurma);
            }

            return todasPresencas.FindAll(p => p.DataPresenca >= inicio && p.DataPresenca <= fim);
        }

        private void BtnFiltrar_Click(object sender, EventArgs e) {
            try {
                if (dgvPresencas.DataSource == null) {
                    MessageBox.Show("Faça uma busca primeiro!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var presencasOriginais = (System.Collections.Generic.List<Presenca>)dgvPresencas.DataSource;

                if (cmbFiltroStatus.SelectedItem.ToString() != "Todos") {
                    string statusFiltro = cmbFiltroStatus.SelectedItem.ToString();
                    var presencasFiltradas = presencasOriginais.FindAll(p => p.Status == statusFiltro);
                    ExibirPresencas(presencasFiltradas);
                }
                else {
                    ExibirPresencas(presencasOriginais);
                }
            }
            catch (Exception ex) {
                MessageBox.Show("Erro: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnLimpar_Click(object sender, EventArgs e) {
            dgvPresencas.DataSource = null;
            cmbFiltroStatus.SelectedIndex = 0;
            dtpDataInicio.Value = DateTime.Now.AddDays(-7);
            dtpDataFim.Value = DateTime.Now;
            lblTotal.Text = "Total: 0";
            lblPresentes.Text = "Presentes: 0 (0%)";
            lblAusentes.Text = "Ausentes: 0 (0%)";
        }

        private void BtnExportar_Click(object sender, EventArgs e) {
            try {
                if (dgvPresencas.Rows.Count == 0) {
                    MessageBox.Show("Não há dados para exportar!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Filter = "CSV file (*.csv)|*.csv";
                saveDialog.FileName = $"Presencas_{DateTime.Now:yyyyMMdd_HHmmss}.csv";

                if (saveDialog.ShowDialog() == DialogResult.OK) {
                    using (System.IO.StreamWriter writer = new System.IO.StreamWriter(saveDialog.FileName, false, System.Text.Encoding.UTF8)) {
                        writer.WriteLine("Aluno;Matricula;Data;Hora;Status;Observacoes");

                        foreach (DataGridViewRow row in dgvPresencas.Rows) {
                            DateTime data = Convert.ToDateTime(row.Cells["DataPresenca"].Value);
                            writer.WriteLine($"{row.Cells["AlunoNome"].Value};{row.Cells["NumeroMatricula"].Value};{data:dd/MM/yyyy};{data:HH:mm};{row.Cells["Status"].Value};{row.Cells["Observacoes"].Value}");
                        }
                    }

                    MessageBox.Show("Arquivo exportado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    System.Diagnostics.Process.Start("explorer.exe", $"/select,\"{saveDialog.FileName}\"");
                }
            }
            catch (Exception ex) {
                MessageBox.Show("Erro ao exportar: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExibirPresencas(System.Collections.Generic.List<Presenca> presencas) {
            dgvPresencas.DataSource = null;
            dgvPresencas.DataSource = presencas;

            if (dgvPresencas.Columns.Contains("PresencaId"))
                dgvPresencas.Columns["PresencaId"].Visible = false;
            if (dgvPresencas.Columns.Contains("MatriculaId"))
                dgvPresencas.Columns["MatriculaId"].Visible = false;

            if (dgvPresencas.Columns.Contains("DataPresenca")) {
                dgvPresencas.Columns["DataPresenca"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
                dgvPresencas.Columns["DataPresenca"].HeaderText = "Data/Hora";
            }

            if (dgvPresencas.Columns.Contains("AlunoNome"))
                dgvPresencas.Columns["AlunoNome"].HeaderText = "Aluno";

            if (dgvPresencas.Columns.Contains("NumeroMatricula"))
                dgvPresencas.Columns["NumeroMatricula"].HeaderText = "Matrícula";

            if (dgvPresencas.Columns.Contains("Observacoes"))
                dgvPresencas.Columns["Observacoes"].HeaderText = "Observações";

            foreach (DataGridViewRow row in dgvPresencas.Rows) {
                string status = row.Cells["Status"].Value.ToString();
                if (status == "Presente")
                    row.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(220, 255, 220);
                else if (status == "Ausente")
                    row.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(255, 220, 220);
                else if (status == "Justificado")
                    row.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(255, 250, 205);
            }

            AtualizarEstatisticas(presencas);
        }

        private void AtualizarEstatisticas(System.Collections.Generic.List<Presenca> presencas) {
            int total = presencas.Count;
            int presentes = presencas.FindAll(p => p.Status == "Presente").Count;
            int ausentes = presencas.FindAll(p => p.Status == "Ausente").Count;
            int justificados = presencas.FindAll(p => p.Status == "Justificado").Count;

            lblTotal.Text = $"Total: {total} registros";
            lblPresentes.Text = $"✅ Presentes: {presentes} ({(total > 0 ? (presentes * 100.0 / total).ToString("F1") : "0")}%)";
            lblAusentes.Text = $"❌ Ausentes: {ausentes} ({(total > 0 ? (ausentes * 100.0 / total).ToString("F1") : "0")}%) | ⚠️ Justificados: {justificados}";
        }
    }
}