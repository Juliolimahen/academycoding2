﻿using CalledManagement.EntitiesDAO;
using CalledManagement.EntitiesModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CalledManagement.Views
{
    public partial class FrmLogin : Form
    {
        public FrmLogin()
        {
            InitializeComponent();
        }
        public void Login()
        {
            UserDAO userdao = new UserDAO();

            if (userdao.VerificaLogin(txtUser, txtPassword)==true)
            {
                this.Visible = false;
                FrmMain frmMain = new FrmMain();
                frmMain.ShowDialog();
                this.Dispose();
            }
            else
            {
                MessageBox.Show("Usuário/Senha incorreto!");
            }
        }
        private void btnEntrar_Click(object sender, EventArgs e)
        {
            Login();
        }

    }
}
