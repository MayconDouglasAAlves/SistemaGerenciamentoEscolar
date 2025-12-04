using System.Drawing;
using System.Windows.Forms;

namespace SistemaGerenciamentoEscolar {
    partial class FormPrincipal {
        private System.ComponentModel.IContainer components = null;

        private MenuStrip menuPrincipal;
        private Panel painelSuperior;
        private Panel painelConteudo;
        private Label tituloSistema;

        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent() {
            this.menuPrincipal = new MenuStrip();
            this.painelSuperior = new Panel();
            this.painelConteudo = new Panel();
            this.tituloSistema = new Label();
            this.SuspendLayout();

            // 
            // menuPrincipal
            // 
            this.menuPrincipal.Location = new Point(0, 0);
            this.menuPrincipal.Name = "menuPrincipal";
            this.menuPrincipal.Size = new Size(1200, 28);

            // 
            // painelSuperior
            // 
            this.painelSuperior.Dock = DockStyle.Top;
            this.painelSuperior.Height = 80;
            this.painelSuperior.BackColor = Color.LightGray;
            this.painelSuperior.Controls.Add(this.tituloSistema);

            // 
            // tituloSistema
            // 
            this.tituloSistema.Text = "Sistema de Gerenciamento Escolar LMMO's";
            this.tituloSistema.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            this.tituloSistema.AutoSize = true;
            this.tituloSistema.Location = new Point(20, 20);
            this.tituloSistema.Anchor = AnchorStyles.None;

            // 
            // painelConteudo
            // 
            this.painelConteudo.Dock = DockStyle.Fill;
            this.painelConteudo.BackColor = Color.WhiteSmoke;

            // 
            // FormPrincipal
            // 
            this.ClientSize = new Size(1200, 700);
            this.Controls.Add(this.painelConteudo);
            this.Controls.Add(this.painelSuperior);
            this.Controls.Add(this.menuPrincipal);
            this.MainMenuStrip = this.menuPrincipal;
            this.Name = "FormPrincipal";
            this.Text = "Sistema de Gerenciamento Escolar";

            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
