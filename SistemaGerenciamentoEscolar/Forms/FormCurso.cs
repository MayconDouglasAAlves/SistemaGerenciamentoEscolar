using System;
using System.Windows.Forms;
using SistemaGerenciamentoEscolar.Models;
using SistemaGerenciamentoEscolar.Data;

namespace SistemaGerenciamentoEscolar.Forms {
    public class FormCurso : Form {
        private CursoRepository repository = new CursoRepository();
        private TextBox txtNome, txtDescricao, txtCargaHoraria;
        private DateTimePicker dtpDataCurso;
        private MaskedTextBox mtbHoraInicio, mtbHoraFim;
        private DataGridView dgvCursos;
        private Button btnSalvar, btnNovo, btnExcluir;
        private int cursoIdSelecionado = 0;

        public FormCurso() {
            CriarComponentes();
            CarregarCursos();
        }

        private void CriarComponentes() {
            this.Text = "Cadastro de Cursos";
            this.Size = new System.Drawing.Size(1000, 600);

            int y = 20;
            this.Controls.Add(new Label { Text = "Nome:", Location = new System.Drawing.Point(20, y), AutoSize = true });
            txtNome = new TextBox { Location = new System.Drawing.Point(150, y - 3), Size = new System.Drawing.Size(400, 25) };
            this.Controls.Add(txtNome);

            y += 35;
            this.Controls.Add(new Label { Text = "Descrição:", Location = new System.Drawing.Point(20, y), AutoSize = true });
            txtDescricao = new TextBox { Location = new System.Drawing.Point(150, y - 3), Size = new System.Drawing.Size(600, 50), Multiline = true };
            this.Controls.Add(txtDescricao);

            y += 60;
            this.Controls.Add(new Label { Text = "Carga Horária:", Location = new System.Drawing.Point(20, y), AutoSize = true });
            txtCargaHoraria = new TextBox { Location = new System.Drawing.Point(150, y - 3), Size = new System.Drawing.Size(100, 25) };
            this.Controls.Add(txtCargaHoraria);

            y += 35;
            this.Controls.Add(new Label { Text = "Data do Curso:", Location = new System.Drawing.Point(20, y), AutoSize = true });
            dtpDataCurso = new DateTimePicker { Location = new System.Drawing.Point(150, y - 3), Size = new System.Drawing.Size(200, 25), Format = DateTimePickerFormat.Short };
            this.Controls.Add(dtpDataCurso);

            y += 35;
            this.Controls.Add(new Label { Text = "Horário Início:", Location = new System.Drawing.Point(20, y), AutoSize = true });
            mtbHoraInicio = new MaskedTextBox { Location = new System.Drawing.Point(150, y - 3), Size = new System.Drawing.Size(80, 25), Mask = "00:00" };
            this.Controls.Add(mtbHoraInicio);

            this.Controls.Add(new Label { Text = "Horário Fim:", Location = new System.Drawing.Point(280, y), AutoSize = true });
            mtbHoraFim = new MaskedTextBox { Location = new System.Drawing.Point(390, y - 3), Size = new System.Drawing.Size(80, 25), Mask = "00:00" };
            this.Controls.Add(mtbHoraFim);

            y += 45;
            btnSalvar = new Button { Text = "Salvar", Location = new System.Drawing.Point(150, y), Size = new System.Drawing.Size(100, 35) };
            btnSalvar.Click += (s, e) => SalvarCurso();
            this.Controls.Add(btnSalvar);

            btnNovo = new Button { Text = "Novo", Location = new System.Drawing.Point(260, y), Size = new System.Drawing.Size(100, 35) };
            btnNovo.Click += (s, e) => LimparCampos();
            this.Controls.Add(btnNovo);

            btnExcluir = new Button {
                Text = "Excluir",
                Location = new System.Drawing.Point(370, y),
                Size = new System.Drawing.Size(100, 35),
                BackColor = System.Drawing.Color.FromArgb(220, 53, 69),
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnExcluir.Click += BtnExcluir_Click;
            this.Controls.Add(btnExcluir);

            y += 50;
            dgvCursos = new DataGridView {
                Location = new System.Drawing.Point(20, y),
                Size = new System.Drawing.Size(950, 250),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                ReadOnly = true,
                AllowUserToAddRows = false
            };
            dgvCursos.CellClick += DgvCursos_CellClick;
            dgvCursos.CellDoubleClick += DgvCursos_CellDoubleClick;
            this.Controls.Add(dgvCursos);
        }

        private void SalvarCurso() {
            try {
                if (string.IsNullOrWhiteSpace(txtNome.Text)) {
                    MessageBox.Show("Nome do curso é obrigatório!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Curso curso = new Curso {
                    CursoId = cursoIdSelecionado,
                    Nome = txtNome.Text.Trim(),
                    Descricao = txtDescricao.Text.Trim(),
                    CargaHoraria = int.Parse(txtCargaHoraria.Text),
                    DataCurso = dtpDataCurso.Value.Date,
                    HorarioInicio = TimeSpan.Parse(mtbHoraInicio.Text),
                    HorarioFim = TimeSpan.Parse(mtbHoraFim.Text)
                };

                if (cursoIdSelecionado > 0) {
                    curso.Ativo = true;
                    if (repository.Atualizar(curso)) {
                        MessageBox.Show("Curso atualizado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LimparCampos();
                        CarregarCursos();
                    }
                }
                else {
                    if (repository.Inserir(curso)) {
                        MessageBox.Show("Curso cadastrado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LimparCampos();
                        CarregarCursos();
                    }
                }
            }
            catch (Exception ex) {
                MessageBox.Show("Erro: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnExcluir_Click(object sender, EventArgs e) {
            if (cursoIdSelecionado == 0) {
                MessageBox.Show("Selecione um curso na lista para excluir!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var resultado = MessageBox.Show(
                $"Deseja realmente excluir o curso '{txtNome.Text}'?\n\nEsta ação não poderá ser desfeita!",
                "Confirmar Exclusão",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (resultado == DialogResult.Yes) {
                try {
                    if (repository.Deletar(cursoIdSelecionado)) {
                        MessageBox.Show("Curso excluído com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LimparCampos();
                        CarregarCursos();
                    }
                }
                catch (Exception ex) {
                    MessageBox.Show("Erro ao excluir curso: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void CarregarCursos() {
            try {
                dgvCursos.DataSource = null;
                dgvCursos.DataSource = repository.ListarTodos();

                if (dgvCursos.Columns.Contains("CursoId"))
                    dgvCursos.Columns["CursoId"].Visible = false;
                if (dgvCursos.Columns.Contains("Ativo"))
                    dgvCursos.Columns["Ativo"].Visible = false;
            }
            catch (Exception ex) {
                MessageBox.Show("Erro ao carregar cursos: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgvCursos_CellClick(object sender, DataGridViewCellEventArgs e) {
            if (e.RowIndex >= 0) {
                cursoIdSelecionado = Convert.ToInt32(dgvCursos.Rows[e.RowIndex].Cells["CursoId"].Value);
            }
        }

        private void DgvCursos_CellDoubleClick(object sender, DataGridViewCellEventArgs e) {
            if (e.RowIndex >= 0) {
                var row = dgvCursos.Rows[e.RowIndex];
                cursoIdSelecionado = Convert.ToInt32(row.Cells["CursoId"].Value);
                txtNome.Text = row.Cells["Nome"].Value.ToString();
                txtDescricao.Text = row.Cells["Descricao"].Value?.ToString() ?? "";
                txtCargaHoraria.Text = row.Cells["CargaHoraria"].Value.ToString();
                dtpDataCurso.Value = Convert.ToDateTime(row.Cells["DataCurso"].Value);
                mtbHoraInicio.Text = ((TimeSpan)row.Cells["HorarioInicio"].Value).ToString(@"hh\:mm");
                mtbHoraFim.Text = ((TimeSpan)row.Cells["HorarioFim"].Value).ToString(@"hh\:mm");
                btnSalvar.Text = "Atualizar";
            }
        }

        private void LimparCampos() {
            cursoIdSelecionado = 0;
            txtNome.Clear();
            txtDescricao.Clear();
            txtCargaHoraria.Clear();
            mtbHoraInicio.Clear();
            mtbHoraFim.Clear();
            dtpDataCurso.Value = DateTime.Now;
            btnSalvar.Text = "Salvar";
        }
    }
}