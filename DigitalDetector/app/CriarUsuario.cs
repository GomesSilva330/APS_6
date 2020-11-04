using DigitalDetector.infra.service;
using DigitalDetector.models;
using DigitalDetector.shared;
using DigitalDetector.shared.combo;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace DigitalDetector.app
{
    public partial class CriarUsuario : Form
    {
        private string base64 = "";
        public CriarUsuario()
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
            if (string.IsNullOrWhiteSpace(cmbNivel.SelectedItem.ToString()))
            {
                MessageBox.Show("Selecione um nível de acesso!", "Problemas...", MessageBoxButtons.OK);
                valid = false;

            }
            if (string.IsNullOrWhiteSpace(txtNome.Text))
            {
                MessageBox.Show("Digite um Nome para o usuário!", "Problemas...", MessageBoxButtons.OK);
                valid = false;
            }
            return valid;
        }
        private void BtnEnviar_click(object sender, EventArgs e)
        {

            if (!Validar())
                return;

            Usuario_Service _repo = new Usuario_Service();
            var ok = _repo.CriarUsuario(new Usuario()
            {
                Id = Guid.NewGuid(),
                Nome = txtNome.Text,
                Digital = base64,
                Nivel = (ENivelUsuario)((ComboBoxItem)cmbNivel.SelectedItem).Value
            });
            
            if (!ok)
            {
                MessageBox.Show("Problema ao incluir, tente novamente...", "Problemas...", MessageBoxButtons.OK);
                return;
            }

            MessageBox.Show("Usuário cadastrado com sucesso!", "Sucesso!", MessageBoxButtons.OK);

            txtNome.Text = "";
            pbDigital.ImageLocation = "";
        }

        private void btnCarregarDigital_Click(object sender, EventArgs e)
        {
            base64 = Tools.CarregarDigital(pbDigital);
        }

        private void CriarUsuario_Load(object sender, EventArgs e)
        {
            base64 = "";
            txtNome.Text = "";
            pbDigital.ImageLocation = "";
            Tools.CarregarComboNivel(cmbNivel);
        }

        private void BtnAtualizar_click(object sender, EventArgs e)
        {
            if (!Validar())
                return;

            Usuario_Service _repo = new Usuario_Service();

            var usuario = _repo.CarregarUsuario(base64, (ENivelUsuario)((ComboBoxItem)cmbNivel.SelectedItem).Value);

            if (usuario == null)
            {
                MessageBox.Show("Digital não encontrada, cadastre-a primeiro!", "Problemas...", MessageBoxButtons.OK);
                return;
            }

            var ok = _repo.AlterarUsuario(usuario);

            if (!ok)
            {
                MessageBox.Show("Problema ao atualizar, tente novamente...", "Problemas...", MessageBoxButtons.OK);
                return;
            }

            MessageBox.Show("Usuário alterado com sucesso!", "Sucesso!", MessageBoxButtons.OK);

            txtNome.Text = "";
            pbDigital.ImageLocation = "";
        }
    }
}
