﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalledManagement.DAO
{
    //Classe responsável pela conexão com o banco 
    public class ToConnection
    {
        SqlConnection conn = new SqlConnection();

        public ToConnection()
        {
            conn.ConnectionString = @"Data Source=DEVPC\SQLEXPRESS;Initial Catalog=academycoding2;Integrated Security=True";
            //@"server=TI-NET-PC\SQLEXPRESS;DataBase=academycoding2";
        }
        //Método responsável pela abertura de conexão com o banco
        public SqlConnection ToConnect()
        {
            if (conn.State == System.Data.ConnectionState.Closed)
            {
                conn.Open();
            }

            return conn;
        }
        //Método responsavel pelo fechamento de conexão com o banco 
        public void ToDisconnect()
        {

            if (conn.State == System.Data.ConnectionState.Open)
            {
                conn.Close();
            }

        }
    }
}
