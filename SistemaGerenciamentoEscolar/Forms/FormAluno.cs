using System;
using System.Windows.Forms;
using SistemaGerenciamentoEscolar.Models;
using SistemaGerenciamentoEscolar.Data;

namespace SistemaGerenciamentoEscolar.Forms {
    public class FormAluno : Form {
        private AlunoRepository repository = new AlunoRepository();
        private TextBox txtNome, txtCPF, txtTelefone, txtEmail, txtEndereco;
        private DateTimePicker dtpNascimento;
        private DataGridView dgvAlunos;
        private Button btnSalvar, btnNovo, btnAtualizar, btnExcluir;
        private int alunoIdSelecionado = 0;

        public FormAluno() {
            CriarComponentes();
            CarregarAlunos();
        }

        private void CriarComponentes() {
            this.Text = "Cadastro de Alunos";
            this.Size = new System.Drawing.Size(1000, 600);

            Label lblNome = new Label { Text = "Nome:", Location = new System.Drawing.Point(20, 20), AutoSize = true };
            txtNome = new TextBox { Location = new System.Drawing.Point(150, 17), Size = new System.Drawing.Size(400, 25) };

            Label lblCPF = new Label { Text = "CPF:", Location = new System.Drawing.Point(20, 55), AutoSize = true };
            txtCPF = new TextBox { Location = new System.Drawing.Point(150, 52), Size = new System.Drawing.Size(200, 25) };

            Label lblNascimento = new Label { Text = "Data Nascimento:", Location = new System.Drawing.Point(20, 90), AutoSize = true };
            dtpNascimento = new DateTimePicker { Location = new System.Drawing.Point(150, 87), Size = new System.Drawing.Size(200, 25), Format = DateTimePickerFormat.Short };

            Label lblTelefone = new Label { Text = "Telefone:", Location = new System.Drawing.Point(20, 125), AutoSize = true };
            txtTelefone = new TextBox { Location = new System.Drawing.Point(150, 122), Size = new System.Drawing.Size(200, 25) };

            Label lblEmail = new Label { Text = "Email:", Location = new System.Drawing.Point(20, 160), AutoSize = true };
            txtEmail = new TextBox { Location = new System.Drawing.Point(150, 157), Size = new System.Drawing.Size(350, 25) };

            Label lblEndereco = new Label { Text = "Endereço:", Location = new System.Drawing.Point(20, 195), AutoSize = true };
            txtEndereco = new TextBox { Location = new System.Drawing.Point(150, 192), Size = new System.Drawing.Size(500, 25) };

            btnSalvar = new Button { Text = "Salvar", Location = new System.Drawing.Point(150, 240), Size = new System.Drawing.Size(100, 35) };
            btnSalvar.Click += BtnSalvar_Click;

            btnNovo = new Button { Text = "Novo", Location = new System.Drawing.Point(260, 240), Size = new System.Drawing.Size(100, 35) };
            btnNovo.Click += (s, e) => LimparCampos();

            btnExcluir = new Button {
                Text = "Excluir",
                Location = new System.Drawing.Point(370, 240),
                Size = new System.Drawing.Size(100, 35),
                BackColor = System.Drawing.Color.FromArgb(220, 53, 69),
                ForeColor = System.Drawing.Color.White
            };
            btnExcluir.Click += BtnExcluir_Click;

            btnAtualizar = new Button { Text = "Atualizar Lista", Location = new System.Drawing.Point(480, 240), Size = new System.Drawing.Size(120, 35) };
            btnAtualizar.Click += (s, e) => CarregarAlunos();

            dgvAlunos = new DataGridView {
                Location = new System.Drawing.Point(20, 290),
                Size = new System.Drawing.Size(950, 280),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                ReadOnly = true,
                AllowUserToAddRows = false
            };
            dgvAlunos.CellClick += DgvAlunos_CellClick;
            dgvAlunos.CellDoubleClick += DgvAlunos_CellDoubleClick;

            this.Controls.AddRange(new Control[] {
                lblNome, txtNome, lblCPF, txtCPF, lblNascimento, dtpNascimento,
                lblTelefone, txtTelefone, lblEmail, txtEmail, lblEndereco, txtEndereco,
                btnSalvar, btnNovo, btnExcluir, btnAtualizar, dgvAlunos
            });
        }

        private void BtnSalvar_Click(object sender, EventArgs e) {
            if (string.IsNullOrWhiteSpace(txtNome.Text) || string.IsNullOrWhiteSpace(txtCPF.Text)) {
                MessageBox.Show("Nome e CPF são obrigatórios!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try {
                if (alunoIdSelecionado > 0) {
                    Aluno aluno = new Aluno {
                        AlunoId = alunoIdSelecionado,
                        Nome = txtNome.Text.Trim(),
                        CPF = txtCPF.Text.Trim(),
                        DataNascimento = dtpNascimento.Value.Date,
                        Telefone = txtTelefone.Text.Trim(),
                        Email = txtEmail.Text.Trim(),
                        Endereco = txtEndereco.Text.Trim(),
                        Ativo = true
                    };

                    if (repository.Atualizar(aluno)) {
                        MessageBox.Show("Aluno atualizado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LimparCampos();
                        CarregarAlunos();
                    }
                }
                else {
                    Aluno aluno = new Aluno {
                        Nome = txtNome.Text.Trim(),
                        CPF = txtCPF.Text.Trim(),
                        DataNascimento = dtpNascimento.Value.Date,
                        Telefone = txtTelefone.Text.Trim(),
                        Email = txtEmail.Text.Trim(),
                        Endereco = txtEndereco.Text.Trim()
                    };

                    if (repository.Inserir(aluno)) {
                        MessageBox.Show("Aluno cadastrado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LimparCampos();
                        CarregarAlunos();
                    }
                }
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnExcluir_Click(object sender, EventArgs e) {
            if (alunoIdSelecionado == 0) {
                MessageBox.Show("Selecione um aluno na lista para excluir!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var resultado = MessageBox.Show(
                $"Deseja realmente excluir o aluno '{txtNome.Text}'?\n\nEsta ação não poderá ser desfeita!",
                "Confirmar Exclusão",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (resultado == DialogResult.Yes) {
                try {
                    if (repository.Deletar(alunoIdSelecionado)) {
                        MessageBox.Show("Aluno excluído com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LimparCampos();
                        CarregarAlunos();
                    }
                }
                catch (Exception ex) {
                    MessageBox.Show("Erro ao excluir aluno: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void CarregarAlunos() {
            try {
                dgvAlunos.DataSource = null;
                dgvAlunos.DataSource = repository.ListarTodos();

                if (dgvAlunos.Columns.Contains("AlunoId"))
                    dgvAlunos.Columns["AlunoId"].Visible = false;
                if (dgvAlunos.Columns.Contains("DataCadastro"))
                    dgvAlunos.Columns["DataCadastro"].Visible = false;
                if (dgvAlunos.Columns.Contains("Ativo"))
                    dgvAlunos.Columns["Ativo"].Visible = false;
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgvAlunos_CellClick(object sender, DataGridViewCellEventArgs e) {
            if (e.RowIndex >= 0) {
                DataGridViewRow row = dgvAlunos.Rows[e.RowIndex];
                alunoIdSelecionado = Convert.ToInt32(row.Cells["AlunoId"].Value);
            }
        }

        private void DgvAlunos_CellDoubleClick(object sender, DataGridViewCellEventArgs e) {
            if (e.RowIndex >= 0) {
                DataGridViewRow row = dgvAlunos.Rows[e.RowIndex];
                alunoIdSelecionado = Convert.ToInt32(row.Cells["AlunoId"].Value);
                txtNome.Text = row.Cells["Nome"].Value.ToString();
                txtCPF.Text = row.Cells["CPF"].Value.ToString();
                dtpNascimento.Value = Convert.ToDateTime(row.Cells["DataNascimento"].Value);
                txtTelefone.Text = row.Cells["Telefone"].Value?.ToString() ?? "";
                txtEmail.Text = row.Cells["Email"].Value?.ToString() ?? "";
                txtEndereco.Text = row.Cells["Endereco"].Value?.ToString() ?? "";

                btnSalvar.Text = "Atualizar";
            }
        }

        private void LimparCampos() {
            alunoIdSelecionado = 0;
            txtNome.Clear();
            txtCPF.Clear();
            txtTelefone.Clear();
            txtEmail.Clear();
            txtEndereco.Clear();
            dtpNascimento.Value = DateTime.Now;
            txtNome.Focus();
            btnSalvar.Text = "Salvar";
        }
    }
}