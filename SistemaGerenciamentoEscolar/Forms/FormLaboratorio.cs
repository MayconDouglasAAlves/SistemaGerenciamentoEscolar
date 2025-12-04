using System;
using System.Windows.Forms;
using SistemaGerenciamentoEscolar.Models;
using SistemaGerenciamentoEscolar.Data;

namespace SistemaGerenciamentoEscolar.Forms {
    public class FormLaboratorio : Form {
        private LaboratorioRepository repository = new LaboratorioRepository();
        private EscolaRepository escolaRepo = new EscolaRepository();
        private ComboBox cmbEscola;
        private TextBox txtNome, txtCapacidade, txtLocalizacao, txtEquipamentos;
        private DataGridView dgvLaboratorios;
        private Button btnSalvar, btnNovo, btnExcluir;
        private int laboratorioIdSelecionado = 0;

        public FormLaboratorio() {
            CriarComponentes();
            CarregarEscolas();
            CarregarLaboratorios();
        }

        private void CriarComponentes() {
            this.Text = "Cadastro de Laboratórios";
            this.Size = new System.Drawing.Size(1000, 600);

            int y = 20;
            this.Controls.Add(new Label { Text = "Escola:", Location = new System.Drawing.Point(20, y), AutoSize = true });
            cmbEscola = new ComboBox { Location = new System.Drawing.Point(150, y - 3), Size = new System.Drawing.Size(400, 25), DropDownStyle = ComboBoxStyle.DropDownList };
            this.Controls.Add(cmbEscola);

            y += 35;
            this.Controls.Add(new Label { Text = "Nome:", Location = new System.Drawing.Point(20, y), AutoSize = true });
            txtNome = new TextBox { Location = new System.Drawing.Point(150, y - 3), Size = new System.Drawing.Size(400, 25) };
            this.Controls.Add(txtNome);

            y += 35;
            this.Controls.Add(new Label { Text = "Capacidade:", Location = new System.Drawing.Point(20, y), AutoSize = true });
            txtCapacidade = new TextBox { Location = new System.Drawing.Point(150, y - 3), Size = new System.Drawing.Size(100, 25) };
            this.Controls.Add(txtCapacidade);

            y += 35;
            this.Controls.Add(new Label { Text = "Localização:", Location = new System.Drawing.Point(20, y), AutoSize = true });
            txtLocalizacao = new TextBox { Location = new System.Drawing.Point(150, y - 3), Size = new System.Drawing.Size(400, 25) };
            this.Controls.Add(txtLocalizacao);

            y += 35;
            this.Controls.Add(new Label { Text = "Equipamentos:", Location = new System.Drawing.Point(20, y), AutoSize = true });
            txtEquipamentos = new TextBox { Location = new System.Drawing.Point(150, y - 3), Size = new System.Drawing.Size(600, 50), Multiline = true };
            this.Controls.Add(txtEquipamentos);

            y += 60;
            btnSalvar = new Button { Text = "Salvar", Location = new System.Drawing.Point(150, y), Size = new System.Drawing.Size(100, 35) };
            btnSalvar.Click += (s, e) => SalvarLaboratorio();
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

            y += 50;
            dgvLaboratorios = new DataGridView {
                Location = new System.Drawing.Point(20, y),
                Size = new System.Drawing.Size(950, 200),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                ReadOnly = true,
                AllowUserToAddRows = false
            };
            dgvLaboratorios.CellClick += DgvLaboratorios_CellClick;
            dgvLaboratorios.CellDoubleClick += DgvLaboratorios_CellDoubleClick;
            this.Controls.Add(dgvLaboratorios);
        }

        private void CarregarEscolas() {
            cmbEscola.DataSource = escolaRepo.ListarTodos();
            cmbEscola.DisplayMember = "Nome";
            cmbEscola.ValueMember = "EscolaId";
        }

        private void SalvarLaboratorio() {
            try {
                if (string.IsNullOrWhiteSpace(txtNome.Text) || string.IsNullOrWhiteSpace(txtCapacidade.Text)) {
                    MessageBox.Show("Nome e Capacidade são obrigatórios!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (laboratorioIdSelecionado > 0) {
                    Laboratorio lab = new Laboratorio {
                        LaboratorioId = laboratorioIdSelecionado,
                        EscolaId = (int)cmbEscola.SelectedValue,
                        Nome = txtNome.Text.Trim(),
                        Capacidade = int.Parse(txtCapacidade.Text),
                        Localizacao = txtLocalizacao.Text.Trim(),
                        Equipamentos = txtEquipamentos.Text.Trim(),
                        Ativo = true
                    };

                    MessageBox.Show("Laboratório atualizado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimparCampos();
                    CarregarLaboratorios();
                }
                else {
                    Laboratorio lab = new Laboratorio {
                        EscolaId = (int)cmbEscola.SelectedValue,
                        Nome = txtNome.Text.Trim(),
                        Capacidade = int.Parse(txtCapacidade.Text),
                        Localizacao = txtLocalizacao.Text.Trim(),
                        Equipamentos = txtEquipamentos.Text.Trim()
                    };

                    if (repository.Inserir(lab)) {
                        MessageBox.Show("Laboratório cadastrado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LimparCampos();
                        CarregarLaboratorios();
                    }
                }
            }
            catch (Exception ex) {
                MessageBox.Show("Erro: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnExcluir_Click(object sender, EventArgs e) {
            if (laboratorioIdSelecionado == 0) {
                MessageBox.Show("Selecione um laboratório na lista para excluir!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var resultado = MessageBox.Show(
                $"Deseja realmente excluir o laboratório '{txtNome.Text}'?\n\nEsta ação não poderá ser desfeita!",
                "Confirmar Exclusão",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (resultado == DialogResult.Yes) {
                try {
                    if (DeletarLaboratorio(laboratorioIdSelecionado)) {
                        MessageBox.Show("Laboratório excluído com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LimparCampos();
                        CarregarLaboratorios();
                    }
                }
                catch (Exception ex) {
                    MessageBox.Show("Erro ao excluir laboratório: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private bool DeletarLaboratorio(int laboratorioId) {
            try {
                using (System.Data.SqlClient.SqlConnection conn = ConexaoSQL.ObterConexao()) {
                    string query = "UPDATE Laboratorio SET Ativo = 0 WHERE LaboratorioId = @LaboratorioId";
                    System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@LaboratorioId", laboratorioId);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex) {
                throw new Exception("Erro ao deletar laboratório: " + ex.Message);
            }
        }

        private void CarregarLaboratorios() {
            dgvLaboratorios.DataSource = null;
            dgvLaboratorios.DataSource = repository.ListarTodos();
            if (dgvLaboratorios.Columns.Contains("LaboratorioId")) dgvLaboratorios.Columns["LaboratorioId"].Visible = false;
            if (dgvLaboratorios.Columns.Contains("EscolaId")) dgvLaboratorios.Columns["EscolaId"].Visible = false;
            if (dgvLaboratorios.Columns.Contains("Ativo")) dgvLaboratorios.Columns["Ativo"].Visible = false;
        }

        private void DgvLaboratorios_CellClick(object sender, DataGridViewCellEventArgs e) {
            if (e.RowIndex >= 0) {
                DataGridViewRow row = dgvLaboratorios.Rows[e.RowIndex];
                laboratorioIdSelecionado = Convert.ToInt32(row.Cells["LaboratorioId"].Value);
            }
        }

        private void DgvLaboratorios_CellDoubleClick(object sender, DataGridViewCellEventArgs e) {
            if (e.RowIndex >= 0) {
                DataGridViewRow row = dgvLaboratorios.Rows[e.RowIndex];
                laboratorioIdSelecionado = Convert.ToInt32(row.Cells["LaboratorioId"].Value);
                txtNome.Text = row.Cells["Nome"].Value.ToString();
                txtCapacidade.Text = row.Cells["Capacidade"].Value.ToString();
                txtLocalizacao.Text = row.Cells["Localizacao"].Value?.ToString() ?? "";
                txtEquipamentos.Text = row.Cells["Equipamentos"].Value?.ToString() ?? "";

                cmbEscola.SelectedValue = Convert.ToInt32(row.Cells["EscolaId"].Value);
                btnSalvar.Text = "Atualizar";
            }
        }

        private void LimparCampos() {
            laboratorioIdSelecionado = 0;
            txtNome.Clear();
            txtCapacidade.Clear();
            txtLocalizacao.Clear();
            txtEquipamentos.Clear();
            btnSalvar.Text = "Salvar";
        }
    }
}