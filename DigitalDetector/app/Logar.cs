using DigitalDetector.infra.service;
using DigitalDetector.shared;
using DigitalDetector.shared.combo;
using System;
using System.Linq;
using System.Windows.Forms;

namespace DigitalDetector.app
{
    public partial class Logar : Form
    {
        private string base64 = "";
        public Logar()
        {
            InitializeComponent();
        }
        private bool Validar()
        {
            var valid = true;
            if (string.IsNullOrWhiteSpace(base64))
            {
                MessageBox.Show("Carregue a digital primeiro!", "Problemas...", MessageBoxButtons.OK);
                valid = false;

            }
            if (string.IsNullOrWhiteSpace(txtNome.Text))
            {
                MessageBox.Show("Digite o seu nome de usuário", "Problemas...", MessageBoxButtons.OK);
                txtNome.Focus();
                valid = false;

            }
            return valid;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            CriarUsuario criarUsuario = new CriarUsuario();
            criarUsuario.Show();
        }

        private void btnLogar_Click(object sender, EventArgs e)
        {
            if (!Validar())
                return;
            var comboitem = (ComboBoxItem)cmbNivel.SelectedItem;
            Usuario_Service _repo = new Usuario_Service();

            var usuarios = _repo.CarregarTodosUsuario();

            var usuario = Tools.CompararDigital(txtNome.Text, base64, usuarios.Select(x => x.Digital).ToList());
            if (usuario == null) { 
                MessageBox.Show($"Digital não reconhecida com a base de dados!", "Problemas...!", MessageBoxButtons.OK);
                return;
            }

            if ((int)usuario.Nivel >= comboitem.Value)
                MessageBox.Show($"Usuário Logado no nível {comboitem.Text} com sucesso!", "Sucesso!!", MessageBoxButtons.OK);
            else
                MessageBox.Show($"Usuário não tem permissão para acessar o {comboitem.Text}", "Problemas...!", MessageBoxButtons.OK);
        }

        private void btnCarregarDigital_Click(object sender, EventArgs e)
        {
            base64 = Tools.CarregarDigital(pcDigital);
        }

        private void Logar_Load(object sender, EventArgs e)
        {
            Tools.CarregarComboNivel(cmbNivel);
        }
    }
}
