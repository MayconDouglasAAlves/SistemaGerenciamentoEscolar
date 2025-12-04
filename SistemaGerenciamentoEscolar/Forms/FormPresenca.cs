using System;
using System.Windows.Forms;
using SistemaGerenciamentoEscolar.Models;
using SistemaGerenciamentoEscolar.Data;

namespace SistemaGerenciamentoEscolar.Forms {
    public partial class FormPresenca : Form {
        private PresencaRepository repository = new PresencaRepository();
        private MatriculaRepository matriculaRepo = new MatriculaRepository();
        private TurmaRepository turmaRepo = new TurmaRepository();
        private ComboBox cmbTurma;
        private DataGridView dgvPresenca;

        public FormPresenca() {
            CriarComponentes();
        }

        private void CriarComponentes() {
            this.Text = "Controle de Presença";
            this.Size = new System.Drawing.Size(900, 550);

            this.Controls.Add(new Label { Text = "Turma:", Location = new System.Drawing.Point(20, 20), AutoSize = true });
            cmbTurma = new ComboBox { Location = new System.Drawing.Point(80, 17), Size = new System.Drawing.Size(400, 25), DropDownStyle = ComboBoxStyle.DropDownList };
            cmbTurma.DataSource = turmaRepo.ListarTodos();
            cmbTurma.DisplayMember = "Codigo";
            cmbTurma.ValueMember = "TurmaId";
            this.Controls.Add(cmbTurma);

            Button btnCarregar = new Button { Text = "Carregar Alunos", Location = new System.Drawing.Point(490, 15), Size = new System.Drawing.Size(130, 30) };
            btnCarregar.Click += (s, e) => CarregarAlunos();
            this.Controls.Add(btnCarregar);

            dgvPresenca = new DataGridView {
                Location = new System.Drawing.Point(20, 60),
                Size = new System.Drawing.Size(1500, 600),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AllowUserToAddRows = false
            };
            this.Controls.Add(dgvPresenca);

            Button btnRegistrar = new Button { Text = "Registrar Presenças", Location = new System.Drawing.Point(650, 15), Size = new System.Drawing.Size(130, 30) };
            btnRegistrar.Click += (s, e) => RegistrarPresencas();
            this.Controls.Add(btnRegistrar);
        }

        private void CarregarAlunos() {
            var matriculas = matriculaRepo.ListarPorTurma((int)cmbTurma.SelectedValue);
            dgvPresenca.DataSource = null;
            dgvPresenca.DataSource = matriculas;

            if (!dgvPresenca.Columns.Contains("Presente")) {
                DataGridViewCheckBoxColumn colPresente = new DataGridViewCheckBoxColumn {
                    HeaderText = "Presente",
                    Name = "Presente",
                    Width = 80
                };
                dgvPresenca.Columns.Add(colPresente);
            }
        }

        private void RegistrarPresencas() {
            try {
                int registrados = 0;
                foreach (DataGridViewRow row in dgvPresenca.Rows) {
                    bool presente = row.Cells["Presente"].Value != null && (bool)row.Cells["Presente"].Value;
                    int matriculaId = (int)row.Cells["MatriculaId"].Value;

                    if (!repository.VerificarPresencaExistente(matriculaId, DateTime.Now)) {
                        Presenca presenca = new Presenca {
                            MatriculaId = matriculaId,
                            Status = presente ? "Presente" : "Ausente"
                        };
                        repository.RegistrarPresenca(presenca);
                        registrados++;
                    }
                }
                MessageBox.Show($"{registrados} presenças registradas!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) {
                MessageBox.Show("Erro: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}