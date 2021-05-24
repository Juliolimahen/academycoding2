﻿using CalledManagement.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CalledManagement.DAO
{
    //Classe responsavel pela comunicação da entidade HourWorked com o banco de dados 
    class HourWorkedDAO
    {
        public bool Insert(HourWorked hourworked)
        {
            //string strConn = @"server=TI-NET-PC\SQLEXPRESS; DataBase=academycoding2; Trusted_Connection = True";

            // Cria objeto cmd da classe SqlCommand passando os comandos SQL e a conexão com o Banco de Dados
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "insert into HOURWORKED (CalledId, DateInserted, DateStarted, EndDate) values (@CalledId, @DateInserted, @DateStarted, @EndDate)";

            {
                try // Verifica se a operação com o banco irá ocorre irá ocorresem erros
                {
                    ToConnection toconnection = new ToConnection();

                    cmd.Parameters.AddWithValue("@CalledId", hourworked.CalledId.Id);
                    cmd.Parameters.AddWithValue("@DateInserted", hourworked.DateInserted);
                    cmd.Parameters.AddWithValue("@DateStarted", hourworked.DateStarted);
                    cmd.Parameters.AddWithValue("@EndDate", hourworked.EndDate);
                    //cmd.Parameters.AddWithValue("@Manual", hourworked.Manual);
                    //cmd.Parameters.AddWithValue("@DateChange", hourworked.DateChange);
                    
                    cmd.Connection = toconnection.ToConnect();
                    // O objetro cmd recebe os parâmetros com os valores dos campos

                    cmd.ExecuteNonQuery();

                    //teste...
                    MessageBox.Show("Cadastro salvo com sucesso!");

                    // Retorna o comando SQL de INSERT no banco de dados. 
                    return true;
                    // Retorna true (verdadeiro) caso a inserção do registro seja realizado corretamente.
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao salvar registro: " + ex.Message);
                    return false;
                    // Caso ocorrra algum erro nos comandos abaixo do try será executado o catch(), disparado uma mensagem de erro para

                }
                // O finally é sempre executado,
                finally
                {
                    ToConnection toconection = new ToConnection();
                    // fechando a conexão com o banco de dados.
                    toconection.ToDisconnect();
                }
            }
        }

        public bool Change(HourWorked hourworked)
        {
            //string strConn = @"server=TI-NET-PC\SQLEXPRESS; DataBase=academycoding2; Trusted_Connection = True";

            //SqlConnection conn = new SqlConnection(strConn);

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "update HOURWORKED set DateStarted = @DateStarted, EndDate = @EndDate, DateChange = @DateChange, Manual = @Manual  where CalledId = @CalledId";
            {
                try // Verifica se a operação com o banco irá ocorre irá ocorresem erros
                {
                    //SqlCommand cmd = new SqlCommand(strConn);

                    //conn.Open(); // Abre a conexão com o banco de dados.

                    ToConnection toconnection = new ToConnection();
                    toconnection.ToConnect();

                    // Esse objeto é responsável em executar os comandos SQL
                    cmd.Parameters.AddWithValue("@CalledId", hourworked.CalledId);
                    //cmd.Parameters.AddWithValue("@DateInserted", hourworked.DateInserted);
                    cmd.Parameters.AddWithValue("@DateStarted", hourworked.DateStarted);
                    cmd.Parameters.AddWithValue("@EndDate", hourworked.EndDate);
                    cmd.Parameters.AddWithValue("@DateChange", hourworked.DateChange);
                    cmd.Parameters.AddWithValue("@Manual", hourworked.Manual);

                    cmd.Connection = toconnection.ToConnect();
                    // O objetro cmd recebe os parâmetros com os valores dos campos Ex.: @nome, @logradouro, @numero, etc.
                    cmd.ExecuteNonQuery();

                    //teste
                    MessageBox.Show("Cadastro alterado com sucesso!");

                    // Retorna o comando SQL de INSERT no banco de dados. (Sem Retorno de dados)
                    return true;
                    // Retorna true (verdadeiro) caso a inserção do registro seja realizado corretamente.
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao salvar registro: " + ex.Message);
                    return false;
                    // Caso ocorrra algum erro nos comandos abaixo do try será executado o catch(), disparado uma mensagem de erro para
                    // Informando "Erro ao salvar registro" + o erro recebido do banco de dados
                }
                // O finally é sempre executado,
                finally
                {
                    // fechando a conexão com o banco de dados.
                    ToConnection toconection = new ToConnection();
                    toconection.ToDisconnect();
                }
            }
        }
        public bool Delete(int ID)
        {
            {
                try
                {
                    //abre conexão
                    SqlCommand cmd = new SqlCommand();
                    ToConnection toconnection = new ToConnection();
                    cmd.Connection = toconnection.ToConnect();


                    cmd.CommandText = "delete from HOURWORKED where calledId = @Id";
                    // Esse objeto é responsável em executar os comandos SQL
                    cmd.Parameters.AddWithValue("@Id", ID);
                    cmd.ExecuteNonQuery();
                    //teste
                    MessageBox.Show("Cadastro Excluido com sucesso!");


                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao exluir registro: " + ex.Message);
                    return false;
                }
                finally
                {
                    ToConnection toconection = new ToConnection();
                    toconection.ToDisconnect();
                }
            }
        }
        public void ListarGrid(DataGridView dgvSec, string name)
        {
            SqlCommand cmd = new SqlCommand();
            ToConnection toconnection = new ToConnection();

            try
            {
                cmd.Connection = toconnection.ToConnect();
                cmd.CommandText = "SELECT CalledId, DateInserted, DateStarted, EndDate, DateChange, Manual FROM HOURWORKED ORDER BY DateInserted DESC";

                if (name.Length > 0)
                {
                    cmd.CommandText = "SELECT CalledId, DateInserted, DateStarted, EndDate, DateChange, Manual FROM HOURWORKED WHERE Name LIKE @Name";
                    cmd.Parameters.AddWithValue("@Name", "%" + name + "%");

                    cmd.ExecuteNonQuery();
                }
                // cmd.Parameters.AddWithValue("@Name", "%" + Name + "%");

                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataTable db = new DataTable();
                adp.Fill(db);
                dgvSec.DataSource = db;
            }

            catch (Exception ex)
            {
                MessageBox.Show("Erro ao Listas registros: " + ex.Message);
            }
        }
    }
}
