using System;
using System.Windows.Forms;
using SistemaGerenciamentoEscolar.Models;
using SistemaGerenciamentoEscolar.Data;

namespace SistemaGerenciamentoEscolar.Forms {
    public class FormEscola : Form {
        private EscolaRepository repository = new EscolaRepository();
        private TextBox txtNome, txtCNPJ, txtEndereco, txtTelefone, txtEmail;
        private DataGridView dgvEscolas;
        private Button btnSalvar, btnNovo, btnAtualizar, btnExcluir;
        private int escolaIdSelecionado = 0;

        public FormEscola() {
            CriarComponentes();
            CarregarEscolas();
        }

        private void CriarComponentes() {
            this.Text = "Cadastro de Escolas";
            this.Size = new System.Drawing.Size(1000, 600);

            Label lblNome = new Label { Text = "Nome:", Location = new System.Drawing.Point(20, 20), AutoSize = true };
            txtNome = new TextBox { Location = new System.Drawing.Point(120, 17), Size = new System.Drawing.Size(350, 25) };

            Label lblCNPJ = new Label { Text = "CNPJ:", Location = new System.Drawing.Point(20, 55), AutoSize = true };
            txtCNPJ = new TextBox { Location = new System.Drawing.Point(120, 52), Size = new System.Drawing.Size(200, 25) };

            Label lblEndereco = new Label { Text = "Endereço:", Location = new System.Drawing.Point(20, 90), AutoSize = true };
            txtEndereco = new TextBox { Location = new System.Drawing.Point(120, 87), Size = new System.Drawing.Size(450, 25) };

            Label lblTelefone = new Label { Text = "Telefone:", Location = new System.Drawing.Point(20, 125), AutoSize = true };
            txtTelefone = new TextBox { Location = new System.Drawing.Point(120, 122), Size = new System.Drawing.Size(200, 25) };

            Label lblEmail = new Label { Text = "Email:", Location = new System.Drawing.Point(20, 160), AutoSize = true };
            txtEmail = new TextBox { Location = new System.Drawing.Point(120, 157), Size = new System.Drawing.Size(350, 25) };

            btnSalvar = new Button { Text = "Salvar", Location = new System.Drawing.Point(120, 200), Size = new System.Drawing.Size(100, 35) };
            btnSalvar.Click += BtnSalvar_Click;

            btnNovo = new Button { Text = "Novo", Location = new System.Drawing.Point(230, 200), Size = new System.Drawing.Size(100, 35) };
            btnNovo.Click += (s, e) => LimparCampos();

            btnExcluir = new Button {
                Text = "Excluir",
                Location = new System.Drawing.Point(340, 200),
                Size = new System.Drawing.Size(100, 35),
                BackColor = System.Drawing.Color.FromArgb(220, 53, 69),
                ForeColor = System.Drawing.Color.White
            };
            btnExcluir.Click += BtnExcluir_Click;

            btnAtualizar = new Button { Text = "Atualizar Lista", Location = new System.Drawing.Point(450, 200), Size = new System.Drawing.Size(120, 35) };
            btnAtualizar.Click += (s, e) => CarregarEscolas();

            dgvEscolas = new DataGridView {
                Location = new System.Drawing.Point(20, 250),
                Size = new System.Drawing.Size(950, 300),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                ReadOnly = true,
                AllowUserToAddRows = false
            };
            dgvEscolas.CellClick += DgvEscolas_CellClick;
            dgvEscolas.CellDoubleClick += DgvEscolas_CellDoubleClick;

            this.Controls.AddRange(new Control[] {
                lblNome, txtNome, lblCNPJ, txtCNPJ, lblEndereco, txtEndereco,
                lblTelefone, txtTelefone, lblEmail, txtEmail,
                btnSalvar, btnNovo, btnExcluir, btnAtualizar, dgvEscolas
            });
        }

        private void BtnSalvar_Click(object sender, EventArgs e) {
            if (string.IsNullOrWhiteSpace(txtNome.Text) || string.IsNullOrWhiteSpace(txtCNPJ.Text)) {
                MessageBox.Show("Nome e CNPJ são obrigatórios!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try {
                if (escolaIdSelecionado > 0) {
                    Escola escola = new Escola {
                        EscolaId = escolaIdSelecionado,
                        Nome = txtNome.Text.Trim(),
                        CNPJ = txtCNPJ.Text.Trim(),
                        Endereco = txtEndereco.Text.Trim(),
                        Telefone = txtTelefone.Text.Trim(),
                        Email = txtEmail.Text.Trim(),
                        Ativo = true
                    };

                    if (repository.Atualizar(escola)) {
                        MessageBox.Show("Escola atualizada com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LimparCampos();
                        CarregarEscolas();
                    }
                }
                else {
                    Escola escola = new Escola {
                        Nome = txtNome.Text.Trim(),
                        CNPJ = txtCNPJ.Text.Trim(),
                        Endereco = txtEndereco.Text.Trim(),
                        Telefone = txtTelefone.Text.Trim(),
                        Email = txtEmail.Text.Trim()
                    };

                    if (repository.Inserir(escola)) {
                        MessageBox.Show("Escola cadastrada com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LimparCampos();
                        CarregarEscolas();
                    }
                }
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnExcluir_Click(object sender, EventArgs e) {
            if (escolaIdSelecionado == 0) {
                MessageBox.Show("Selecione uma escola na lista para excluir!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var resultado = MessageBox.Show(
                $"Deseja realmente excluir a escola '{txtNome.Text}'?\n\nEsta ação não poderá ser desfeita!",
                "Confirmar Exclusão",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (resultado == DialogResult.Yes) {
                try {
                    if (repository.Deletar(escolaIdSelecionado)) {
                        MessageBox.Show("Escola excluída com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LimparCampos();
                        CarregarEscolas();
                    }
                }
                catch (Exception ex) {
                    MessageBox.Show("Erro ao excluir escola: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void CarregarEscolas() {
            try {
                dgvEscolas.DataSource = null;
                dgvEscolas.DataSource = repository.ListarTodos();

                if (dgvEscolas.Columns.Contains("EscolaId"))
                    dgvEscolas.Columns["EscolaId"].Visible = false;
                if (dgvEscolas.Columns.Contains("DataCadastro"))
                    dgvEscolas.Columns["DataCadastro"].Visible = false;
                if (dgvEscolas.Columns.Contains("Ativo"))
                    dgvEscolas.Columns["Ativo"].Visible = false;
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgvEscolas_CellClick(object sender, DataGridViewCellEventArgs e) {
            if (e.RowIndex >= 0) {
                DataGridViewRow row = dgvEscolas.Rows[e.RowIndex];
                escolaIdSelecionado = Convert.ToInt32(row.Cells["EscolaId"].Value);
            }
        }

        private void DgvEscolas_CellDoubleClick(object sender, DataGridViewCellEventArgs e) {
            if (e.RowIndex >= 0) {
                DataGridViewRow row = dgvEscolas.Rows[e.RowIndex];
                escolaIdSelecionado = Convert.ToInt32(row.Cells["EscolaId"].Value);
                txtNome.Text = row.Cells["Nome"].Value.ToString();
                txtCNPJ.Text = row.Cells["CNPJ"].Value.ToString();
                txtEndereco.Text = row.Cells["Endereco"].Value?.ToString() ?? "";
                txtTelefone.Text = row.Cells["Telefone"].Value?.ToString() ?? "";
                txtEmail.Text = row.Cells["Email"].Value?.ToString() ?? "";

                btnSalvar.Text = "Atualizar";
            }
        }

        private void LimparCampos() {
            escolaIdSelecionado = 0;
            txtNome.Clear();
            txtCNPJ.Clear();
            txtEndereco.Clear();
            txtTelefone.Clear();
            txtEmail.Clear();
            txtNome.Focus();
            btnSalvar.Text = "Salvar";
        }
    }
}