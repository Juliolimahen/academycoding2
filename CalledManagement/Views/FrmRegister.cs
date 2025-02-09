﻿using CalledManagement.EntitiesDAO;
using CalledManagement.Utils;
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
            txtSecSearchHours.Enabled = true;
            txtSecSearchCalled.Enabled = true;
            Function.Clean(this);
            dtpRegTimeInit.Format = DateTimePickerFormat.Time;
            dtpRegTimeInit.ShowUpDown = true;
            dtpRegTimeFinished.Format = DateTimePickerFormat.Time;
            dtpRegTimeFinished.ShowUpDown = true;
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
            txtRegDate.Enabled = false;
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
                called.Date = DateTime.Now;

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
                    txtRegDate.Enabled = false;
                }
            }

            MessageBox.Show("Cadastro salvo com sucesso!");
            LoadComboxs();
            LoadGrids();
            Function.EnableFields(this, false);
            Function.Clean(this);
            Function.EnableButtons(this, "Init");
            cbxRegID.Enabled = true;
            cbxRegID.Focus();
            operation = "";
            txtSecSearchHours.Enabled = true;
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
            txtSecSearchHours.Enabled = true;
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
                txtSecSearchHours.Enabled = true;
                txtSecSearchCalled.Enabled = true;
            }
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
                        MessageBox.Show("Cadastro Excluido com sucesso!");
                    }
                }
            }
            else
            {
                MessageBox.Show("Digite um codigo identificador para excluir um registro!");
                Function.EnableFields(this, true);
            }
            txtSecSearchCalled.Enabled = true;
            txtSecSearchCalled.Enabled = true;
        }
        //Botão pesquisa na grid view 
        private void btnSecSearch_Click(object sender, EventArgs e)
        {
            LoadGrids();
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
            txtSecSearchHours.Enabled = true;
            txtSecSearchCalled.Enabled = true;
        }
        //botão responsável por por aterar cadastro horas
        private void btnRegChangeHours_Click(object sender, EventArgs e)
        {

            if (cbxRegIDHours.SelectedIndex < 0)
            {
                MessageBox.Show("Selecione um codigo identificador para alterar o registro e clique novamente em alterar para recuperar os dados!");
                cbxRegIDHours.Focus();
                Function.EnableFields(this, true);
                Function.EnableButtons(this, "Change");
                cbxRegHours.Enabled = false;
            }

            else if (cbxRegIDHours.SelectedIndex >= 0)
            {
                Function.EnableFields(this, true);
                Function.EnableButtons(this, "Save");
                SearchHourRecords();
                cbxRegHours.Enabled = false;
            }

            operation = "Change";
            txtSecSearchHours.Enabled = true;
            txtSecSearchCalled.Enabled = true;
        }
        //botão responsável por deletar cadastro de horas 
        private void btnRegDeleteHours_Click(object sender, EventArgs e)
        {
            //verifica se campo nome esta vazio 

            if (cbxRegIDHours.SelectedIndex >= 0)
            {
                if (MessageBox.Show("Confirma a exclusão do registro?", "Atenção",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    HourWorkedDAO HourWorkeddao = new HourWorkedDAO();
                    if (HourWorkeddao.Delete(Convert.ToInt32(cbxRegIDHours.SelectedValue.ToString())) == false)
                    {
                        cbxRegIDHours.Focus();
                    }
                    else
                    {
                        Function.Clean(this);
                        Function.EnableButtons(this, "Init");
                        cbxRegIDHours.Focus();
                        LoadGrids();
                        LoadComboxs();
                        MessageBox.Show("Cadastro Excluido com sucesso!");
                    }
                }
            }
            else
            {
                MessageBox.Show("Digite um codigo identificador para excluir um registro!");
                Function.EnableFields(this, true);
                cbxRegHours.Enabled = false;
            }
            txtSecSearchCalled.Enabled = true;
            txtSecSearchCalled.Enabled = true;
        }
        //botao responsavel por salvar resgistro de horas 
        private void btnRegSaveHours_Click(object sender, EventArgs e)
        {
            //novas instacias 
            HourWorked hourworked = new HourWorked();
            HourWorkedDAO hourworkeddao = new HourWorkedDAO();
            Called called = new Called();
            hourworked.CalledId = called;
            hourworked.CalledId.Id = Convert.ToInt32(cbxRegHours.SelectedValue.ToString());

            if (ValidateDataHours() == true)
            {
                hourworked.DateStarted = Convert.ToDateTime(dtpRegDateInit.Value.Date + dtpRegTimeInit.Value.TimeOfDay);
                hourworked.EndDate = Convert.ToDateTime(dtpRegDateFinished.Value.Date + dtpRegTimeFinished.Value.TimeOfDay);

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
            txtSecSearchHours.Enabled = true;
            txtSecSearchCalled.Enabled = true;
        }
        //botao responsável por disabilitar a tela de cadastro de horas 
        private void btnRegFinishedHours_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Deseja realmente ir para o cadastra de chamados?", "Atenção",
              MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                gbxRegCalled.Enabled = true;
                gbxRegHours.Enabled = false;
            }
            txtSecSearchHours.Enabled = true;
            txtSecSearchCalled.Enabled = true;
        }
        //botao responsável por disabilitar a tela de cadastro de chamados
        private void btnRegFinishedCalled_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Deseja realmente ir ao cadastro de horas?", "Atenção",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                gbxRegHours.Enabled = true;
                gbxRegCalled.Enabled = false;
            }
            txtSecSearchHours.Enabled = true;
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
            if (txtRegDescripition.Text == string.Empty)
            {
                MessageBox.Show("O campo Descrição é obrigatório!", "Atenção");
                txtRegName.Focus();
                return false;
            }
            return true;
        }
        //metodo resposavel pela validação dos dados de horas 
        private Boolean ValidateDataHours()
        {
            /*if (!mstbRegDateTimeInit.MaskCompleted)
            {
                MessageBox.Show("O campo data/hora inicio é obrigatório!", "Atenção");
                mstbRegDateTimeInit.Focus();
                return false;

            }
            if (!mstbRegDateTimeInit.MaskCompleted)
            {
                MessageBox.Show("O campo data/hora Termino é obrigatório!", "Atenção");
                mstbRegDateTimeInit.Focus();
                return false;
            }*/
            /*
            if (mstbRegDateTimeInit.MaskCompleted && mstbRegDateTimeFinished.MaskCompleted)
            {
                if (Convert.ToDateTime(mstbRegDateTimeInit.Text) > Convert.ToDateTime(mstbRegDateTimeFinished.Text) || Convert.ToDateTime(mstbRegDateTimeInit.Text) > DateTime.Now)
                {
                    MessageBox.Show("A Data de inicio não pode ser mais recente que a data de termino!");
                    mstbRegDateTimeInit.Focus();
                    return false;
                }
            }*/
            return true;
        }
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
                txtRegDate.Text = called.Date.ToString();
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
        private void SearchHourRecords()
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
                dtpRegDateInit.Text = hourworked.DateStarted.Date.ToString();
                dtpRegTimeInit.Text = hourworked.DateStarted.TimeOfDay.ToString();
                dtpRegDateFinished.Text = hourworked.EndDate.Date.ToString();
                dtpRegTimeFinished.Text = hourworked.EndDate.TimeOfDay.ToString();

                Function.EnableButtons(this, "Change");
            }
            else MessageBox.Show("Nenhuma hora cadastrada para este chamado!", "Atenção");
        }
        //botão responsável por pesquisar cadastro horas
        private void btnSecSearchHours_Click(object sender, EventArgs e)
        {
            LoadGrids();
        }
        //metodo responsavel por carregar DataGridViews
        public void LoadGrids()
        {
            CalledDAO calleddao = new CalledDAO();
            HourWorkedDAO hourworkeddao = new HourWorkedDAO();
            PriorityDAO prioritydao = new PriorityDAO();

            string name = txtSecSearchCalled.Text;

            dgvSecCalled.DataSource= calleddao.ToListGrid(name);

            string SecSearchHours;
            SecSearchHours = txtSecSearchHours.Text;
            dgvSecHours.DataSource = hourworkeddao.ToListGrid(SecSearchHours);

            //textBoxPesquisa
            txtSecSearchCalled.Enabled = true;
            txtSecSearchHours.Enabled = true;
        }
        //metodo responsavel por carregar ComboBoxs
        public void LoadComboxs()
        {
            CalledDAO calleddao = new CalledDAO();
            HourWorkedDAO hourworkeddao = new HourWorkedDAO();
            PriorityDAO prioritydao = new PriorityDAO();
            cbxRegPriority.DisplayMember = "Name";
            cbxRegPriority.ValueMember = "Id";
            cbxRegPriority.DataSource = prioritydao.ToListComboBox();
            cbxRegHours.Text = "Selecione um chamado";
            cbxRegHours.DisplayMember = "Name";
            cbxRegHours.ValueMember = "Id";
            cbxRegHours.DataSource = calleddao.ToListComboBox();
            cbxRegID.Text = "Selecione um id";
            cbxRegID.DisplayMember = "Id";
            cbxRegID.ValueMember = "Id";
            cbxRegID.DataSource = calleddao.ToListComBoxID();
            hourworkeddao.ToListComBox(cbxRegIDHours);
            txtSecSearchCalled.Enabled = true;
        }

    }
}