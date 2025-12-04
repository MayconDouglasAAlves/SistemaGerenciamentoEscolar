using System;
using System.Windows.Forms;
using SistemaGerenciamentoEscolar.Models;
using SistemaGerenciamentoEscolar.Data;

namespace SistemaGerenciamentoEscolar.Forms {
    public partial class FormListaMatriculas : Form {
        private MatriculaRepository repository = new MatriculaRepository();
        private TurmaRepository turmaRepo = new TurmaRepository();
        private ComboBox cmbTurma;
        private DataGridView dgvMatriculas;

        public FormListaMatriculas() {
            CriarComponentes();
        }

        private void CriarComponentes() {
            this.Text = "Lista de Matrículas";
            this.Size = new System.Drawing.Size(900, 500);

            this.Controls.Add(new Label { Text = "Turma:", Location = new System.Drawing.Point(20, 20), AutoSize = true });
            cmbTurma = new ComboBox { Location = new System.Drawing.Point(80, 17), Size = new System.Drawing.Size(400, 25), DropDownStyle = ComboBoxStyle.DropDownList };
            cmbTurma.DataSource = turmaRepo.ListarTodos();
            cmbTurma.DisplayMember = "Codigo";
            cmbTurma.ValueMember = "TurmaId";
            this.Controls.Add(cmbTurma);

            Button btnBuscar = new Button { Text = "Buscar", Location = new System.Drawing.Point(490, 15), Size = new System.Drawing.Size(100, 30) };
            btnBuscar.Click += (s, e) => CarregarMatriculas();
            this.Controls.Add(btnBuscar);

            dgvMatriculas = new DataGridView {
                Location = new System.Drawing.Point(20, 60),
                Size = new System.Drawing.Size(850, 380),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                ReadOnly = true,
                AllowUserToAddRows = false
            };
            this.Controls.Add(dgvMatriculas);
        }

        private void CarregarMatriculas() {
            dgvMatriculas.DataSource = null;
            dgvMatriculas.DataSource = repository.ListarPorTurma((int)cmbTurma.SelectedValue);
            if (dgvMatriculas.Columns.Contains("MatriculaId")) dgvMatriculas.Columns["MatriculaId"].Visible = false;
            if (dgvMatriculas.Columns.Contains("AlunoId")) dgvMatriculas.Columns["AlunoId"].Visible = false;
            if (dgvMatriculas.Columns.Contains("TurmaId")) dgvMatriculas.Columns["TurmaId"].Visible = false;
        }
    }
}