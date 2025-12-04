using System;
using System.Windows.Forms;
using SistemaGerenciamentoEscolar.Models;
using SistemaGerenciamentoEscolar.Data;

namespace SistemaGerenciamentoEscolar.Forms {
    public class FormMatricula : Form {
        private MatriculaRepository repository = new MatriculaRepository();
        private AlunoRepository alunoRepo = new AlunoRepository();
        private TurmaRepository turmaRepo = new TurmaRepository();
        private ComboBox cmbAluno, cmbTurma, cmbTurmaFiltro;
        private TextBox txtNumeroMatricula;
        private DataGridView dgvMatriculas;
        private Button btnMatricular, btnGerar, btnExcluir, btnBuscar, btnNovo;
        private int matriculaIdSelecionado = 0;

        public FormMatricula() {
            CriarComponentes();
            CarregarDados();
        }

        private void CriarComponentes() {
            this.Text = "Gerenciar Matrículas";
            this.Size = new System.Drawing.Size(1000, 650);

            GroupBox gbNovaMatricula = new GroupBox {
                Text = "Nova Matrícula",
                Location = new System.Drawing.Point(20, 20),
                Size = new System.Drawing.Size(950, 200)
            };

            int y = 35;
            gbNovaMatricula.Controls.Add(new Label { Text = "Aluno:", Location = new System.Drawing.Point(20, y), AutoSize = true });
            cmbAluno = new ComboBox { Location = new System.Drawing.Point(150, y - 3), Size = new System.Drawing.Size(350, 25), DropDownStyle = ComboBoxStyle.DropDownList };
            gbNovaMatricula.Controls.Add(cmbAluno);

            y += 40;
            gbNovaMatricula.Controls.Add(new Label { Text = "Turma:", Location = new System.Drawing.Point(20, y), AutoSize = true });
            cmbTurma = new ComboBox { Location = new System.Drawing.Point(150, y - 3), Size = new System.Drawing.Size(350, 25), DropDownStyle = ComboBoxStyle.DropDownList };
            gbNovaMatricula.Controls.Add(cmbTurma);

            y += 40;
            gbNovaMatricula.Controls.Add(new Label { Text = "Número Matrícula:", Location = new System.Drawing.Point(20, y), AutoSize = true });
            txtNumeroMatricula = new TextBox { Location = new System.Drawing.Point(150, y - 3), Size = new System.Drawing.Size(200, 25), ReadOnly = true };
            gbNovaMatricula.Controls.Add(txtNumeroMatricula);

            btnGerar = new Button { Text = "Gerar Novo", Location = new System.Drawing.Point(360, y - 5), Size = new System.Drawing.Size(100, 30) };
            btnGerar.Click += (s, e) => txtNumeroMatricula.Text = repository.GerarNumeroMatricula();
            gbNovaMatricula.Controls.Add(btnGerar);

            y += 50;
            btnMatricular = new Button {
                Text = "Matricular",
                Location = new System.Drawing.Point(150, y),
                Size = new System.Drawing.Size(120, 40),
                BackColor = System.Drawing.Color.FromArgb(40, 167, 69),
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnMatricular.Click += (s, e) => RealizarMatricula();
            gbNovaMatricula.Controls.Add(btnMatricular);

            btnNovo = new Button { Text = "Limpar", Location = new System.Drawing.Point(280, y), Size = new System.Drawing.Size(100, 40) };
            btnNovo.Click += (s, e) => LimparCampos();
            gbNovaMatricula.Controls.Add(btnNovo);

            this.Controls.Add(gbNovaMatricula);

            GroupBox gbConsulta = new GroupBox {
                Text = "Matrículas Cadastradas",
                Location = new System.Drawing.Point(20, 230),
                Size = new System.Drawing.Size(950, 370)
            };

            y = 35;
            gbConsulta.Controls.Add(new Label { Text = "Filtrar por Turma:", Location = new System.Drawing.Point(20, y), AutoSize = true });
            cmbTurmaFiltro = new ComboBox { Location = new System.Drawing.Point(130, y - 3), Size = new System.Drawing.Size(350, 25), DropDownStyle = ComboBoxStyle.DropDownList };
            gbConsulta.Controls.Add(cmbTurmaFiltro);

            btnBuscar = new Button { Text = "Buscar", Location = new System.Drawing.Point(490, y - 5), Size = new System.Drawing.Size(100, 30) };
            btnBuscar.Click += (s, e) => CarregarMatriculas();
            gbConsulta.Controls.Add(btnBuscar);

            btnExcluir = new Button {
                Text = "Excluir Selecionada",
                Location = new System.Drawing.Point(600, y - 5),
                Size = new System.Drawing.Size(150, 30),
                BackColor = System.Drawing.Color.FromArgb(220, 53, 69),
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnExcluir.Click += BtnExcluir_Click;
            gbConsulta.Controls.Add(btnExcluir);

            y += 50;
            dgvMatriculas = new DataGridView {
                Location = new System.Drawing.Point(20, y),
                Size = new System.Drawing.Size(910, 285),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                ReadOnly = true,
                AllowUserToAddRows = false
            };
            dgvMatriculas.CellClick += DgvMatriculas_CellClick;
            gbConsulta.Controls.Add(dgvMatriculas);

            this.Controls.Add(gbConsulta);
        }

        private void CarregarDados() {
            cmbAluno.DataSource = alunoRepo.ListarTodos();
            cmbAluno.DisplayMember = "Nome";
            cmbAluno.ValueMember = "AlunoId";

            var turmas = turmaRepo.ListarTodos();

            cmbTurma.DataSource = turmas;
            cmbTurma.DisplayMember = "Codigo";
            cmbTurma.ValueMember = "TurmaId";

            var turmasFiltro = turmaRepo.ListarTodos();
            cmbTurmaFiltro.DataSource = turmasFiltro;
            cmbTurmaFiltro.DisplayMember = "Codigo";
            cmbTurmaFiltro.ValueMember = "TurmaId";

            txtNumeroMatricula.Text = repository.GerarNumeroMatricula();

            if (turmas.Count > 0) {
                CarregarMatriculas();
            }
        }

        private void RealizarMatricula() {
            try {
                if (cmbAluno.SelectedValue == null || cmbTurma.SelectedValue == null) {
                    MessageBox.Show("Selecione um aluno e uma turma!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Matricula matricula = new Matricula {
                    AlunoId = (int)cmbAluno.SelectedValue,
                    TurmaId = (int)cmbTurma.SelectedValue,
                    NumeroMatricula = txtNumeroMatricula.Text
                };

                if (repository.Inserir(matricula)) {
                    MessageBox.Show("Matrícula realizada com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtNumeroMatricula.Text = repository.GerarNumeroMatricula();
                    CarregarMatriculas();

                    try {
                        LogRepository logRepo = new LogRepository();
                        logRepo.Inserir(new LogSistema {
                            Usuario = "Sistema",
                            Acao = "Cadastro",
                            Modulo = "Matrícula",
                            Descricao = $"Matrícula {matricula.NumeroMatricula} realizada"
                        });
                    }
                    catch { }
                }
            }
            catch (Exception ex) {
                MessageBox.Show("Erro ao matricular: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnExcluir_Click(object sender, EventArgs e) {
            if (matriculaIdSelecionado == 0) {
                MessageBox.Show("Selecione uma matrícula na lista para excluir!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var resultado = MessageBox.Show(
                "Deseja realmente excluir esta matrícula?\n\nEsta ação não poderá ser desfeita!",
                "Confirmar Exclusão",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (resultado == DialogResult.Yes) {
                try {
                    MessageBox.Show("Matrícula excluída com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    matriculaIdSelecionado = 0;
                    CarregarMatriculas();
                }
                catch (Exception ex) {
                    MessageBox.Show("Erro ao excluir matrícula: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void CarregarMatriculas() {
            try {
                if (cmbTurmaFiltro.SelectedValue != null) {
                    dgvMatriculas.DataSource = null;
                    dgvMatriculas.DataSource = repository.ListarPorTurma((int)cmbTurmaFiltro.SelectedValue);

                    if (dgvMatriculas.Columns.Contains("MatriculaId"))
                        dgvMatriculas.Columns["MatriculaId"].Visible = false;
                    if (dgvMatriculas.Columns.Contains("AlunoId"))
                        dgvMatriculas.Columns["AlunoId"].Visible = false;
                    if (dgvMatriculas.Columns.Contains("TurmaId"))
                        dgvMatriculas.Columns["TurmaId"].Visible = false;
                    if (dgvMatriculas.Columns.Contains("Observacoes"))
                        dgvMatriculas.Columns["Observacoes"].Visible = false;
                }
            }
            catch (Exception ex) {
                MessageBox.Show("Erro ao carregar matrículas: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgvMatriculas_CellClick(object sender, DataGridViewCellEventArgs e) {
            if (e.RowIndex >= 0) {
                DataGridViewRow row = dgvMatriculas.Rows[e.RowIndex];
                matriculaIdSelecionado = Convert.ToInt32(row.Cells["MatriculaId"].Value);
            }
        }

        private void LimparCampos() {
            matriculaIdSelecionado = 0;
            txtNumeroMatricula.Text = repository.GerarNumeroMatricula();
        }
    }
}