﻿using CalledManagement.DAO;
using CalledManagement.Entities;
using CalledManagement.Utils;
using CalledManagement.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CalledManagement
{

    //Classe resposavel pelo formulário de registro. herdada da classe Form
    public partial class FrmRegisterCalled : Form
    {

        // variavel global resposanvel por armazanar qual operação seguir na troca de botoes 
        string operation;

        //Construtor responsavel por inicializar o componente; 
        public FrmRegisterCalled()
        {
            InitializeComponent();
        }

        private void FrmRegisterCalled_Load(object sender, EventArgs e)
        {
            Function.EnableFields(this, false);
            Function.EnableButtons(this, "Load");
            LoadComboxs();
            LoadGrids();
            
            if (rbRegCalledHoursSystem.Checked == true)
            {
                dtpRegDate.Enabled = false;
            }
            else
            {
                dtpRegDate.Enabled = true;
            }
            txtSecSearchHours.Enabled = true;
            txtSecSearchCalled.Enabled = true;
            Function.Clean(this);
        }
        //Botão para Finalizar chamado
        private void btnRegFinish_Click(object sender, EventArgs e)
        {
            Dispose();
        }
        //Botao para iniciar chamado 
        private void btnRegInit_Click(object sender, EventArgs e)
        {
            Function.EnableFields(this, true);
            Function.Clean(this);
            Function.EnableButtons(this, "Save");
            cbxRegID.Enabled = false;
            txtRegName.Focus();
            operation = "Init";
            txtSecSearchCalled.Enabled = true;
        }
        //botão para salvar registros 
        private void btnRegSave_Click(object sender, EventArgs e)
        {
            if (ValidateDataCalled() == true)
            {
                Called called = new Called();
                HourWorked hourworked = new HourWorked();
                called.Name = txtRegName.Text;
                if (rbRegCalledHoursSystem.Checked == true)
                {
                    dtpRegDate.Enabled = false;
                    called.Date = DateTime.Now;
                }
                else
                {
                    dtpRegDate.Enabled = true;
                    called.Date = Convert.ToDateTime(dtpRegDate.Text);
                }
                called.Descripition = txtRegDescripition.Text;

                if (rbRegStatusFinished.Checked == true)
                {
                    called.Finished = "S";
                }
                else
                {
                    called.Finished = "N";
                }
                //Associação
                Priority priority = new Priority();
                called.PriorityId = priority;
                called.PriorityId.Id = Convert.ToInt32(cbxRegPriority.SelectedValue.ToString());
                if (operation == "Init")
                {
                    CalledDAO calleddao = new CalledDAO();
                    if (calleddao.Insert(called) == false)
                    {
                        txtRegName.Focus();
                        return;
                    }
                }
                else if (operation == "Change")
                {
                    CalledDAO calleddao = new CalledDAO();
                    called.Id = Convert.ToInt32(cbxRegID.SelectedValue.ToString());
                    if (calleddao.Change(called) == false)
                    {
                        txtRegName.Focus();
                        return;
                    }
                }
            }
            LoadComboxs();
            LoadGrids();
            Function.EnableFields(this, false);
            Function.Clean(this);
            Function.EnableButtons(this, "Init");
            cbxRegID.Enabled = true;
            cbxRegID.Focus();
            operation = "";
            txtSecSearchCalled.Enabled = true;
        }
        //botão para editar registros 
        private void btnRegChange_Click(object sender, EventArgs e)
        {

            if (cbxRegID.SelectedIndex < 0)
            {
                MessageBox.Show("Selecione um codigo identificador para alterar um registro e clique novamente em alterar!", "Atenção!!!");
                cbxRegID.Focus();
                Function.EnableFields(this, true);
                Function.EnableButtons(this, "Change");
            }

            else if (cbxRegID.SelectedIndex >= 0)
            {
                Function.EnableFields(this, true);
                Function.EnableButtons(this, "Save");
                BuscarRegistroCalled();
                cbxRegID.Enabled = true;
                txtRegName.Focus();
            }

            operation = "Change";
            txtSecSearchCalled.Enabled = true;
        }
        //botão responsável por cancelar cadastro 
        private void btnRegCancel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Deseja realmente cancelar a edição do registro ?", "Atenção",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Function.EnableFields(this, false);
                Function.Clean(this);
                Function.EnableButtons(this, "Load");
                operation = "";
                txtSecSearchCalled.Enabled = true;
            }
        }
        private Boolean ValidateDataHours()
        {

            if (!mstbRegDateTimeInit.MaskCompleted)
            {
                MessageBox.Show("O campo data/hora inicio é obrigatório!", "Atenção");
                mstbRegDateTimeInit.Focus();
                return false;
            }
            
            // MessageBox.Show(result.ToString());
            if(mstbRegDateTimeInit.MaskCompleted && mstbRegDateTimeFinished.MaskCompleted)
            {
                if (Convert.ToDateTime(mstbRegDateTimeInit.Text) > Convert.ToDateTime(mstbRegDateTimeFinished.Text) || Convert.ToDateTime(mstbRegDateTimeInit.Text) > DateTime.Now)
                {
                    MessageBox.Show("A Data de inicio não pode ser mais recente que a data de termino!");
                    mstbRegDateTimeInit.Focus();
                    return false;
                }
            }

            return true;
        }
        //botão resposavel por deletar registros dos chamados 
        private void btnRegDelete_Click(object sender, EventArgs e)
        {
            //verifica se campo nome esta vazio 

            if (cbxRegID.SelectedIndex >= 0)
            {
                if (MessageBox.Show("Confirma a exclusão do registro?", "Atenção",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {

                    CalledDAO calleddao = new CalledDAO();
                    if (calleddao.Delete(Convert.ToInt32(cbxRegID.SelectedValue.ToString())) == false)
                    {
                        cbxRegID.Focus();
                    }
                    else
                    {
                        Function.Clean(this);
                        Function.EnableButtons(this, "Init");
                        cbxRegID.Focus();
                        LoadGrids();
                        LoadComboxs();
                    }
                }
            }
            else
            {
                MessageBox.Show("Digite um codigo identificador para excluir um registro!");
                Function.EnableFields(this, true);
            }
            txtSecSearchCalled.Enabled = true;
        }
        //Botão pesquisa na grid view 
        private void btnSecSearch_Click(object sender, EventArgs e)
        {
            //nova instancia 
            CalledDAO calleddao = new CalledDAO();
            HourWorkedDAO hourworkeddao = new HourWorkedDAO();

            string name = txtSecSearchCalled.Text;
            calleddao.ListarGrid(dgvSecCalled, name);

            string SecSearchHours;
            SecSearchHours = txtSecSearchHours.Text;
            hourworkeddao.ListarGrid(dgvSecHours, SecSearchHours);

        }
        //botão responsável por iniciar o cadastro de horas 
        private void btnRegInitHours_Click(object sender, EventArgs e)
        {
            Function.EnableFields(this, true);
            Function.Clean(this);
            Function.EnableButtons(this, "Save");
            cbxRegIDHours.Enabled = false;
            cbxRegHours.Focus();
            operation = "Init";
            txtSecSearchCalled.Enabled = true;
        }
        //botão responsável por por aterar cadastro horas
        private void btnRegChangeHours_Click(object sender, EventArgs e)
        {

            if (cbxRegIDHours.SelectedIndex < 0)
            {
                MessageBox.Show("Selecione um codigo identificador para alterar o registro!");
                cbxRegIDHours.Focus();
                Function.EnableFields(this, true);
                Function.EnableButtons(this, "Change");
                cbxRegHours.Enabled = false;


            }

            else if (cbxRegIDHours.SelectedIndex > -1)
            {
                Function.EnableFields(this, true);
                Function.EnableButtons(this, "Save");
                BuscarRegistroHours();
                cbxRegHours.Enabled = false;
                //txtRegName.Focus();
            }

            operation = "Change";
            txtSecSearchCalled.Enabled = true;
        }
        //botão responsável por deletar cadastro de horas 
        private void btnRegDeleteHours_Click(object sender, EventArgs e)
        {
            if (cbxRegID.SelectedIndex >= 0)
            {
                if (MessageBox.Show("Confirma a exclusão do registro?", "Atenção",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {

                    HourWorkedDAO hourworkeddao = new HourWorkedDAO();
                    if (hourworkeddao.Delete(Convert.ToInt32(cbxRegHours.SelectedValue.ToString())) == false)
                    {
                        cbxRegHours.Focus();
                    }
                    else
                    {
                        Function.Clean(this);
                        Function.EnableButtons(this, "Init");
                        cbxRegHours.Focus();
                        LoadComboxs();
                        LoadGrids();
                    }
                }
            }
            else
            {
                MessageBox.Show("Digite um codigo identificador para excluir um registro!");
                Function.EnableFields(this, true);
            }
            txtSecSearchCalled.Enabled = true;
        }
        //botao responsavel por salvar resgistro de horas 
        private void btnRegSaveHours_Click(object sender, EventArgs e)
        {
            HourWorked hourworked = new HourWorked();
            HourWorkedDAO hourworkeddao = new HourWorkedDAO();
            Called called = new Called();
            //teste
            //MessageBox.Show(text: cbxRegHours.SelectedValue.ToString());
            hourworked.CalledId = called;
            hourworked.CalledId.Id = Convert.ToInt32(cbxRegHours.SelectedValue.ToString());

            if (ValidateDataHours() == true)
            {
                hourworked.DateStarted = Convert.ToDateTime(mstbRegDateTimeInit.Text);
                hourworked.EndDate = Convert.ToDateTime(mstbRegDateTimeFinished.Text);
                
                if (operation == "Init")
                {
                    hourworked.DateInserted = DateTime.Now;
                    hourworkeddao.Insert(hourworked);
                }
                else if (operation == "Change")
                {
                    hourworked.Id = Convert.ToInt32(cbxRegIDHours.SelectedValue.ToString());
                    hourworked.DateChange = DateTime.Now;
                    if (hourworkeddao.Change(hourworked) == false)
                    {
                        txtRegName.Focus();
                        return;
                    }
                    else
                    {
                        MessageBox.Show("Cadastro salvo com sucesso!");
                    }
                }
            }

            LoadComboxs();
            LoadGrids();
            Function.EnableFields(this, false);
            Function.Clean(this);
            Function.EnableButtons(this, "Init");
            operation = "";
            txtSecSearchCalled.Enabled = true;
        }
        //botao responsável por disabilitar a tela de cadastro de horas 
        private void btnRegFinishedHours_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Deseja realmente finalizar o cadastro?", "Atenção",
              MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                gbxRegCalled.Enabled = true;
                gbxRegHours.Enabled = false;
            }
            txtSecSearchCalled.Enabled = true;
        }
        //botao responsável por disabilitar a tela de cadastro de chamados
        private void btnRegFinishedCalled_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Deseja realmente finalizar o cadastro?", "Atenção",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                gbxRegHours.Enabled = true;
                gbxRegCalled.Enabled = false;
            }
            txtSecSearchCalled.Enabled = true;
        }
        //metodo resposavel pela validação dos dados de chamados
        private Boolean ValidateDataCalled()
        {
            if (txtRegName.Text == string.Empty)
            {
                MessageBox.Show("O campo nome é obrigatório!", "Atenção");
                txtRegName.Focus();
                return false;
            }
            return true;
        }
        //metodo resposavel pela validação dos dados de horas 
       
        //metodo resposavel por buscar os registros dos chamados para alteração
        private void BuscarRegistroCalled()
        {
            int id = int.Parse(cbxRegID.SelectedValue.ToString());
            Function.Clean(this);
            CalledDAO calleddao = new CalledDAO();
            Called called = new Called();

            called = calleddao.SearchID(id);

            if (called.Id > 0)
            {
                cbxRegID.SelectedValue = int.Parse(called.Id.ToString());
                txtRegName.Text = called.Name;
                txtRegDescripition.Text = called.Descripition;
                dtpRegDate.Text = called.Date.ToString();
                cbxRegPriority.SelectedValue = int.Parse(called.PriorityId.Id.ToString());
                if (called.Finished == "N")
                {
                    rbRegStatusProgress.Checked = true;
                }
                else
                {
                    rbRegStatusFinished.Checked = true;
                }

                Function.EnableButtons(this, "Change");
            }
            else MessageBox.Show("Codigo do chamado não encontrado!", "Atenção");
        }
        //metodo resposavel por buscar os registros das horas para alteração
        private void BuscarRegistroHours()
        {
            int id = int.Parse(cbxRegIDHours.SelectedValue.ToString());
            Function.Clean(this);
            HourWorkedDAO hourworkeddao = new HourWorkedDAO();
            HourWorked hourworked = new HourWorked();

            hourworked = hourworkeddao.SearchID(id);

            if (hourworked.Id > 0)
            {
                cbxRegIDHours.SelectedValue = int.Parse(hourworked.Id.ToString());
                cbxRegHours.SelectedValue = int.Parse(hourworked.CalledId.Id.ToString());
                mstbRegDateTimeInit.Text = hourworked.DateStarted.ToString();
                mstbRegDateTimeFinished.Text = hourworked.EndDate.ToString();

                Function.EnableButtons(this, "Change");
            }
            else MessageBox.Show("Nenhuma hora cadastrada para este chamado!", "Atenção");
        }
        //botão responsável por pesquisar cadastro horas
        private void btnSecSearchHours_Click(object sender, EventArgs e)
        {
            //nova instancia 
            /*HourWorkedDAO hourworkeddao = new HourWorkedDAO();

            string name = txtSecSearchCalled.Text;
            hourworkeddao.ListarGrid(dgvSecHours, name);
            string SecSearchHours;
            SecSearchHours = txtSecSearchHours.Text;
            hourworkeddao.ListarGrid(dgvSecHours, SecSearchHours);*/
            LoadGrids();

        }
        //metodo responsavel por carregar DataGridView
        public void LoadGrids()
        {
            CalledDAO calleddao = new CalledDAO();
            HourWorkedDAO hourworkeddao = new HourWorkedDAO();
            PriorityDAO prioritydao = new PriorityDAO();

            string name = txtSecSearchCalled.Text;
            calleddao.ListarGrid(dgvSecCalled, name);

            string SecSearchHours;
            SecSearchHours = txtSecSearchHours.Text;
            hourworkeddao.ListarGrid(dgvSecHours, SecSearchHours);
            
            //textBoxPesquisa
            txtSecSearchCalled.Enabled = true;
            txtSecSearchHours.Enabled = true;
        }
        //metodo responsavel por carregar ComboBox
        public void LoadComboxs()
        {
            CalledDAO calleddao = new CalledDAO();
            HourWorkedDAO hourworkeddao = new HourWorkedDAO();
            PriorityDAO prioritydao = new PriorityDAO();

            calleddao.ListarComboBox(cbxRegHours);
            prioritydao.ListarComboBox(cbxRegPriority);
            calleddao.ListarComBoxID(cbxRegID);
            hourworkeddao.ListarComBoxID(cbxRegIDHours);
            txtSecSearchCalled.Enabled = true;
        }
    }
}