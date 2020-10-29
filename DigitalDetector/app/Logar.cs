using DigitalDetector.infra.service;
using DigitalDetector.shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
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
                MessageBox.Show("Digite um Nome para o usuário!", "Problemas...", MessageBoxButtons.OK);
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

            Usuario_Service _repo = new Usuario_Service();

            var usuario = _repo.CarregarUsuario(txtNome.Text, base64);

            if (usuario == null)
            {
                MessageBox.Show("Login incorreto, tente novamente!", "Problemas...", MessageBoxButtons.OK);
                return;
            }

            MessageBox.Show("Usuário Logado com sucesso!", "Problemas...", MessageBoxButtons.OK);
        }

        private void btnCarregarDigital_Click(object sender, EventArgs e)
        {
            base64 = Tools.CarregarDigital(pcDigital);
        }
    }
}
