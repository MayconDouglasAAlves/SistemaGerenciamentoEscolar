using System;
using System.Windows.Forms;
using SistemaGerenciamentoEscolar.Models;
using SistemaGerenciamentoEscolar.Data;

namespace SistemaGerenciamentoEscolar.Forms {
    public class FormTurma : Form {
        private TurmaRepository repository = new TurmaRepository();
        private CursoRepository cursoRepo = new CursoRepository();
        private LaboratorioRepository labRepo = new LaboratorioRepository();
        private ComboBox cmbCurso, cmbLaboratorio;
        private TextBox txtCodigo, txtVagas, txtObservacoes;
        private DateTimePicker dtpDataAula;
        private MaskedTextBox mtbHoraInicio, mtbHoraFim;
        private DataGridView dgvTurmas;
        private Button btnSalvar, btnNovo, btnExcluir;
        private int turmaIdSelecionado = 0;

        public FormTurma() {
            CriarComponentes();
            CarregarDados();
        }

        private void CriarComponentes() {
            this.Text = "Cadastro de Turmas";
            this.Size = new System.Drawing.Size(1000, 600);

            int y = 20;
            this.Controls.Add(new Label { Text = "Curso:", Location = new System.Drawing.Point(20, y), AutoSize = true });
            cmbCurso = new ComboBox { Location = new System.Drawing.Point(150, y - 3), Size = new System.Drawing.Size(400, 25), DropDownStyle = ComboBoxStyle.DropDownList };
            this.Controls.Add(cmbCurso);

            y += 35;
            this.Controls.Add(new Label { Text = "Laboratório:", Location = new System.Drawing.Point(20, y), AutoSize = true });
            cmbLaboratorio = new ComboBox { Location = new System.Drawing.Point(150, y - 3), Size = new System.Drawing.Size(400, 25), DropDownStyle = ComboBoxStyle.DropDownList };
            this.Controls.Add(cmbLaboratorio);

            y += 35;
            this.Controls.Add(new Label { Text = "Código:", Location = new System.Drawing.Point(20, y), AutoSize = true });
            txtCodigo = new TextBox { Location = new System.Drawing.Point(150, y - 3), Size = new System.Drawing.Size(200, 25) };
            this.Controls.Add(txtCodigo);

            y += 35;
            this.Controls.Add(new Label { Text = "Data da Aula:", Location = new System.Drawing.Point(20, y), AutoSize = true });
            dtpDataAula = new DateTimePicker { Location = new System.Drawing.Point(150, y - 3), Size = new System.Drawing.Size(200, 25), Format = DateTimePickerFormat.Short };
            this.Controls.Add(dtpDataAula);

            y += 35;
            this.Controls.Add(new Label { Text = "Horário Início:", Location = new System.Drawing.Point(20, y), AutoSize = true });
            mtbHoraInicio = new MaskedTextBox { Location = new System.Drawing.Point(150, y - 3), Size = new System.Drawing.Size(80, 25), Mask = "00:00" };
            this.Controls.Add(mtbHoraInicio);

            this.Controls.Add(new Label { Text = "Fim:", Location = new System.Drawing.Point(250, y), AutoSize = true });
            mtbHoraFim = new MaskedTextBox { Location = new System.Drawing.Point(300, y - 3), Size = new System.Drawing.Size(80, 25), Mask = "00:00" };
            this.Controls.Add(mtbHoraFim);

            y += 35;
            this.Controls.Add(new Label { Text = "Vagas:", Location = new System.Drawing.Point(20, y), AutoSize = true });
            txtVagas = new TextBox { Location = new System.Drawing.Point(150, y - 3), Size = new System.Drawing.Size(100, 25) };
            this.Controls.Add(txtVagas);

            y += 35;
            this.Controls.Add(new Label { Text = "Observações:", Location = new System.Drawing.Point(20, y), AutoSize = true });
            txtObservacoes = new TextBox { Location = new System.Drawing.Point(150, y - 3), Size = new System.Drawing.Size(500, 40), Multiline = true };
            this.Controls.Add(txtObservacoes);

            y += 50;
            btnSalvar = new Button { Text = "Salvar", Location = new System.Drawing.Point(150, y), Size = new System.Drawing.Size(100, 35) };
            btnSalvar.Click += (s, e) => SalvarTurma();
            this.Controls.Add(btnSalvar);

            btnNovo = new Button { Text = "Novo", Location = new System.Drawing.Point(260, y), Size = new System.Drawing.Size(100, 35) };
            btnNovo.Click += (s, e) => LimparCampos();
            this.Controls.Add(btnNovo);

            btnExcluir = new Button {
                Text = "Excluir",
                Location = new System.Drawing.Point(370, y),
                Size = new System.Drawing.Size(100, 35),
                BackColor = System.Drawing.Color.FromArgb(220, 53, 69),
                ForeColor = System.Drawing.Color.White
            };
            btnExcluir.Click += BtnExcluir_Click;
            this.Controls.Add(btnExcluir);

            y += 45;
            dgvTurmas = new DataGridView {
                Location = new System.Drawing.Point(20, y),
                Size = new System.Drawing.Size(950, 150),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                ReadOnly = true,
                AllowUserToAddRows = false
            };
            dgvTurmas.CellClick += DgvTurmas_CellClick;
            dgvTurmas.CellDoubleClick += DgvTurmas_CellDoubleClick;
            this.Controls.Add(dgvTurmas);
            CarregarTurmas();
        }

        private void CarregarDados() {
            cmbCurso.DataSource = cursoRepo.ListarTodos();
            cmbCurso.DisplayMember = "Nome";
            cmbCurso.ValueMember = "CursoId";

            cmbLaboratorio.DataSource = labRepo.ListarTodos();
            cmbLaboratorio.DisplayMember = "Nome";
            cmbLaboratorio.ValueMember = "LaboratorioId";
        }

        private void SalvarTurma() {
            try {
                Turma turma = new Turma {
                    TurmaId = turmaIdSelecionado,
                    CursoId = (int)cmbCurso.SelectedValue,
                    LaboratorioId = (int)cmbLaboratorio.SelectedValue,
                    Codigo = txtCodigo.Text.Trim(),
                    DataAula = dtpDataAula.Value.Date,
                    HorarioInicio = TimeSpan.Parse(mtbHoraInicio.Text),
                    HorarioFim = TimeSpan.Parse(mtbHoraFim.Text),
                    VagasDisponiveis = int.Parse(txtVagas.Text),
                    Observacoes = txtObservacoes.Text.Trim()
                };

                if (turmaIdSelecionado > 0) {
                    MessageBox.Show("Turma atualizada!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else {
                    if (repository.Inserir(turma)) {
                        MessageBox.Show("Turma cadastrada!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

                LimparCampos();
                CarregarTurmas();
            }
            catch (Exception ex) {
                MessageBox.Show("Erro: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnExcluir_Click(object sender, EventArgs e) {
            if (turmaIdSelecionado == 0) {
                MessageBox.Show("Selecione uma turma para excluir!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var resultado = MessageBox.Show(
                $"Deseja realmente excluir a turma '{txtCodigo.Text}'?",
                "Confirmar Exclusão",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (resultado == DialogResult.Yes) {
                try {
                    MessageBox.Show("Turma excluída!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimparCampos();
                    CarregarTurmas();
                }
                catch (Exception ex) {
                    MessageBox.Show("Erro: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void CarregarTurmas() {
            dgvTurmas.DataSource = null;
            dgvTurmas.DataSource = repository.ListarTodos();
            if (dgvTurmas.Columns.Contains("TurmaId")) dgvTurmas.Columns["TurmaId"].Visible = false;
            if (dgvTurmas.Columns.Contains("CursoId")) dgvTurmas.Columns["CursoId"].Visible = false;
            if (dgvTurmas.Columns.Contains("LaboratorioId")) dgvTurmas.Columns["LaboratorioId"].Visible = false;
            if (dgvTurmas.Columns.Contains("Ativo")) dgvTurmas.Columns["Ativo"].Visible = false;
        }

        private void DgvTurmas_CellClick(object sender, DataGridViewCellEventArgs e) {
            if (e.RowIndex >= 0) {
                turmaIdSelecionado = Convert.ToInt32(dgvTurmas.Rows[e.RowIndex].Cells["TurmaId"].Value);
            }
        }

        private void DgvTurmas_CellDoubleClick(object sender, DataGridViewCellEventArgs e) {
            if (e.RowIndex >= 0) {
                var row = dgvTurmas.Rows[e.RowIndex];
                turmaIdSelecionado = Convert.ToInt32(row.Cells["TurmaId"].Value);
                cmbCurso.SelectedValue = Convert.ToInt32(row.Cells["CursoId"].Value);
                cmbLaboratorio.SelectedValue = Convert.ToInt32(row.Cells["LaboratorioId"].Value);
                txtCodigo.Text = row.Cells["Codigo"].Value.ToString();
                dtpDataAula.Value = Convert.ToDateTime(row.Cells["DataAula"].Value);
                mtbHoraInicio.Text = ((TimeSpan)row.Cells["HorarioInicio"].Value).ToString(@"hh\:mm");
                mtbHoraFim.Text = ((TimeSpan)row.Cells["HorarioFim"].Value).ToString(@"hh\:mm");
                txtVagas.Text = row.Cells["VagasDisponiveis"].Value.ToString();
                txtObservacoes.Text = row.Cells["Observacoes"].Value?.ToString() ?? "";
                btnSalvar.Text = "Atualizar";
            }
        }

        private void LimparCampos() {
            turmaIdSelecionado = 0;
            txtCodigo.Clear();
            txtVagas.Clear();
            txtObservacoes.Clear();
            mtbHoraInicio.Clear();
            mtbHoraFim.Clear();
            btnSalvar.Text = "Salvar";
        }
    }
}