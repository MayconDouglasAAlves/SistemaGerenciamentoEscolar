using System;
using System.Windows.Forms;
using System.Linq;
using SistemaGerenciamentoEscolar.Models;
using SistemaGerenciamentoEscolar.Data;

namespace SistemaGerenciamentoEscolar.Forms {
    public class FormFeedback : Form {
        private FeedbackRepository feedbackRepo = new FeedbackRepository();
        private AlunoRepository alunoRepo = new AlunoRepository();
        private CursoRepository cursoRepo = new CursoRepository();
        private TurmaRepository turmaRepo = new TurmaRepository();

        private ComboBox cmbAluno, cmbCurso, cmbTurma;
        private NumericUpDown numConteudo, numInstrutor, numInfraestrutura;
        private TextBox txtComentario;
        private CheckBox chkRecomenda;
        private DataGridView dgvFeedbacks;
        private Label lblMedia;
        private Button btnSalvar, btnNovo, btnExcluir, btnAtualizar;
        private string feedbackIdSelecionado = "";

        public FormFeedback() {
            CriarComponentes();
            VerificarConexaoMongo();
            CarregarDados();
        }

        private void VerificarConexaoMongo() {
            if (!ConexaoMongoDB.TestarConexao()) {
                MessageBox.Show("Erro ao conectar com MongoDB! Verifique se está rodando.", "Erro MongoDB", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CriarComponentes() {
            this.Text = "Feedback dos Alunos (NoSQL - MongoDB)";
            this.Size = new System.Drawing.Size(1000, 700);

            int y = 20;
            this.Controls.Add(new Label { Text = "Aluno:", Location = new System.Drawing.Point(20, y), AutoSize = true });
            cmbAluno = new ComboBox { Location = new System.Drawing.Point(150, y - 3), Size = new System.Drawing.Size(350, 25), DropDownStyle = ComboBoxStyle.DropDownList };
            this.Controls.Add(cmbAluno);

            y += 35;
            this.Controls.Add(new Label { Text = "Curso:", Location = new System.Drawing.Point(20, y), AutoSize = true });
            cmbCurso = new ComboBox { Location = new System.Drawing.Point(150, y - 3), Size = new System.Drawing.Size(350, 25), DropDownStyle = ComboBoxStyle.DropDownList };
            cmbCurso.SelectedIndexChanged += (s, e) => CarregarTurmasPorCurso();
            this.Controls.Add(cmbCurso);

            y += 35;
            this.Controls.Add(new Label { Text = "Turma:", Location = new System.Drawing.Point(20, y), AutoSize = true });
            cmbTurma = new ComboBox { Location = new System.Drawing.Point(150, y - 3), Size = new System.Drawing.Size(350, 25), DropDownStyle = ComboBoxStyle.DropDownList };
            this.Controls.Add(cmbTurma);

            y += 40;
            this.Controls.Add(new Label { Text = "Avaliações (1 a 5):", Location = new System.Drawing.Point(20, y), AutoSize = true, Font = new System.Drawing.Font("Segoe UI", 9, System.Drawing.FontStyle.Bold) });

            y += 25;
            this.Controls.Add(new Label { Text = "Conteúdo:", Location = new System.Drawing.Point(30, y), AutoSize = true });
            numConteudo = new NumericUpDown { Location = new System.Drawing.Point(150, y - 3), Size = new System.Drawing.Size(70, 25), Minimum = 1, Maximum = 5, Value = 5 };
            this.Controls.Add(numConteudo);

            this.Controls.Add(new Label { Text = "Instrutor:", Location = new System.Drawing.Point(250, y), AutoSize = true });
            numInstrutor = new NumericUpDown { Location = new System.Drawing.Point(340, y - 3), Size = new System.Drawing.Size(70, 25), Minimum = 1, Maximum = 5, Value = 5 };
            this.Controls.Add(numInstrutor);

            this.Controls.Add(new Label { Text = "Infraestrutura:", Location = new System.Drawing.Point(440, y), AutoSize = true });
            numInfraestrutura = new NumericUpDown { Location = new System.Drawing.Point(560, y - 3), Size = new System.Drawing.Size(70, 25), Minimum = 1, Maximum = 5, Value = 5 };
            this.Controls.Add(numInfraestrutura);

            y += 35;
            this.Controls.Add(new Label { Text = "Comentário:", Location = new System.Drawing.Point(20, y), AutoSize = true });
            txtComentario = new TextBox { Location = new System.Drawing.Point(150, y - 3), Size = new System.Drawing.Size(600, 60), Multiline = true };
            this.Controls.Add(txtComentario);

            y += 70;
            chkRecomenda = new CheckBox { Text = "Recomendo este curso", Location = new System.Drawing.Point(150, y), AutoSize = true, Checked = true };
            this.Controls.Add(chkRecomenda);

            btnSalvar = new Button { Text = "Salvar Feedback", Location = new System.Drawing.Point(320, y - 5), Size = new System.Drawing.Size(130, 35) };
            btnSalvar.Click += (s, e) => SalvarFeedback();
            this.Controls.Add(btnSalvar);

            btnNovo = new Button { Text = "Novo", Location = new System.Drawing.Point(460, y - 5), Size = new System.Drawing.Size(100, 35) };
            btnNovo.Click += (s, e) => LimparCampos();
            this.Controls.Add(btnNovo);

            btnExcluir = new Button {
                Text = "Excluir",
                Location = new System.Drawing.Point(570, y - 5),
                Size = new System.Drawing.Size(100, 35),
                BackColor = System.Drawing.Color.FromArgb(220, 53, 69),
                ForeColor = System.Drawing.Color.White
            };
            btnExcluir.Click += BtnExcluir_Click;
            this.Controls.Add(btnExcluir);

            y += 50;
            lblMedia = new Label { Text = "Média Geral: -", Location = new System.Drawing.Point(20, y), AutoSize = true, Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold) };
            this.Controls.Add(lblMedia);

            btnAtualizar = new Button { Text = "Atualizar", Location = new System.Drawing.Point(200, y - 5), Size = new System.Drawing.Size(100, 30) };
            btnAtualizar.Click += (s, e) => CarregarFeedbacks();
            this.Controls.Add(btnAtualizar);

            y += 40;
            dgvFeedbacks = new DataGridView {
                Location = new System.Drawing.Point(20, y),
                Size = new System.Drawing.Size(950, 250),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                ReadOnly = true,
                AllowUserToAddRows = false
            };
            dgvFeedbacks.CellClick += DgvFeedbacks_CellClick;
            dgvFeedbacks.CellDoubleClick += DgvFeedbacks_CellDoubleClick;
            this.Controls.Add(dgvFeedbacks);
        }

        private void CarregarDados() {
            cmbAluno.DisplayMember = "Nome";
            cmbAluno.ValueMember = "AlunoId";
            cmbAluno.DataSource = alunoRepo.ListarTodos();


            cmbCurso.DisplayMember = "Nome";
            cmbCurso.ValueMember = "CursoId";
            cmbCurso.DataSource = cursoRepo.ListarTodos();

            CarregarFeedbacks();
        }

        private void CarregarTurmasPorCurso() {
            if (cmbCurso.SelectedValue != null) {
                int cursoId = Convert.ToInt32(cmbCurso.SelectedValue);

                var todasTurmas = turmaRepo.ListarTodos();
                var turmasDoCurso = todasTurmas
                    .Where(t => t.CursoId == cursoId)
                    .ToList();

                cmbTurma.DisplayMember = "Codigo";
                cmbTurma.ValueMember = "TurmaId";
                cmbTurma.DataSource = turmasDoCurso;
            }
        }


        private void SalvarFeedback() {
            try {
                var aluno = (Aluno)cmbAluno.SelectedItem;
                var curso = (Curso)cmbCurso.SelectedItem;
                var turma = (Turma)cmbTurma.SelectedItem;

                FeedbackAluno feedback = new FeedbackAluno {
                    AlunoId = aluno.AlunoId,
                    AlunoNome = aluno.Nome,
                    CursoId = curso.CursoId,
                    CursoNome = curso.Nome,
                    TurmaId = turma.TurmaId,
                    TurmaCodigo = turma.Codigo,
                    NotaConteudo = (int)numConteudo.Value,
                    NotaInstrutor = (int)numInstrutor.Value,
                    NotaInfraestrutura = (int)numInfraestrutura.Value,
                    Comentario = txtComentario.Text.Trim(),
                    RecomendaCurso = chkRecomenda.Checked
                };

                if (feedbackRepo.Inserir(feedback)) {
                    MessageBox.Show("Feedback registrado no MongoDB com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimparCampos();
                    CarregarFeedbacks();

                    LogRepository logRepo = new LogRepository();
                    logRepo.Inserir(new LogSistema {
                        Usuario = "Sistema",
                        Acao = "Cadastro",
                        Modulo = "Feedback",
                        Descricao = $"Feedback cadastrado por {aluno.Nome} para o curso {curso.Nome}"
                    });
                }
            }
            catch (Exception ex) {
                MessageBox.Show("Erro: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnExcluir_Click(object sender, EventArgs e) {
            if (string.IsNullOrEmpty(feedbackIdSelecionado)) {
                MessageBox.Show("Selecione um feedback na lista para excluir!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var resultado = MessageBox.Show(
                "Deseja realmente excluir este feedback?\n\nEsta ação não poderá ser desfeita!",
                "Confirmar Exclusão",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (resultado == DialogResult.Yes) {
                try {
                    if (feedbackRepo.Deletar(feedbackIdSelecionado)) {
                        MessageBox.Show("Feedback excluído com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LimparCampos();
                        CarregarFeedbacks();
                    }
                }
                catch (Exception ex) {
                    MessageBox.Show("Erro ao excluir feedback: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void CarregarFeedbacks() {
            try {
                dgvFeedbacks.DataSource = null;
                var feedbacks = feedbackRepo.ListarTodos();
                dgvFeedbacks.DataSource = feedbacks;

                if (dgvFeedbacks.Columns.Contains("Id"))
                    dgvFeedbacks.Columns["Id"].Visible = false;

                if (feedbacks.Count > 0) {
                    double media = feedbacks.Average(f => f.MediaGeral);
                    lblMedia.Text = $"Média Geral: {media:F2} ⭐";
                }
                else {
                    lblMedia.Text = "Média Geral: -";
                }
            }
            catch (Exception ex) {
                MessageBox.Show("Erro: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgvFeedbacks_CellClick(object sender, DataGridViewCellEventArgs e) {
            if (e.RowIndex >= 0) {
                DataGridViewRow row = dgvFeedbacks.Rows[e.RowIndex];
                feedbackIdSelecionado = row.Cells["Id"].Value.ToString();
            }
        }

        private void DgvFeedbacks_CellDoubleClick(object sender, DataGridViewCellEventArgs e) {
            if (e.RowIndex >= 0) {
                DataGridViewRow row = dgvFeedbacks.Rows[e.RowIndex];
                feedbackIdSelecionado = row.Cells["Id"].Value.ToString();

                cmbAluno.SelectedValue = Convert.ToInt32(row.Cells["AlunoId"].Value);
                cmbCurso.SelectedValue = Convert.ToInt32(row.Cells["CursoId"].Value);
                CarregarTurmasPorCurso();
                cmbTurma.SelectedValue = Convert.ToInt32(row.Cells["TurmaId"].Value);
                numConteudo.Value = Convert.ToInt32(row.Cells["NotaConteudo"].Value);
                numInstrutor.Value = Convert.ToInt32(row.Cells["NotaInstrutor"].Value);
                numInfraestrutura.Value = Convert.ToInt32(row.Cells["NotaInfraestrutura"].Value);
                txtComentario.Text = row.Cells["Comentario"].Value?.ToString() ?? "";
                chkRecomenda.Checked = Convert.ToBoolean(row.Cells["RecomendaCurso"].Value);
            }
        }

        private void LimparCampos() {
            feedbackIdSelecionado = "";
            txtComentario.Clear();
            numConteudo.Value = 5;
            numInstrutor.Value = 5;
            numInfraestrutura.Value = 5;
            chkRecomenda.Checked = true;
        }
    }
}