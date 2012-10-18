﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using Model;
using WorkStation;
using System.Configuration;
namespace WorkStation
{
    public partial class frmAddCard : Form
    {
        public frmAddCard()
        {
            InitializeComponent();
        }
        private static string sqlConnectionStr = "Data Source=192.168.1.221;Initial Catalog=Patrol;User ID=sa;Password=sa123";         
        /// <summary>
        /// datagridview的显示
        /// </summary>
        public void Bind()
        {
            string sql2 = "select ID,Name,Alias,RFID,PurposeName from Rfid,RfidPurpose where Rfid.Purpose = RfidPurpose.PurposeCode";
            DataSet ds = SqlHelper.ExecuteDataset(sqlConnectionStr, CommandType.Text, sql2);               
            dataGridView1.DataSource = ds.Tables[0];              
        }    
        private void btnAdd_Click(object sender, EventArgs e)
        {
             string sql = "insert into Rfid([Name],Alias,RFID,Purpose) values(@name,@alias,@rFID,@RfidPurpose)";         
                   SqlParameter[] pars = new SqlParameter[] { 
                   new SqlParameter("@name", SqlDbType.NVarChar),
                   new SqlParameter("@alias", SqlDbType.NVarChar),
                   new SqlParameter("@rFID", SqlDbType.NVarChar),  
                   new SqlParameter("@RfidPurpose", SqlDbType.Int)
            };     
            pars[0].Value =this.txtName.Text.Trim();
            pars[1].Value =this.txtAlias.Text.Trim();
            pars[2].Value =this.txtCard.Text.Trim();
            pars[3].Value = this.comboBox1.SelectedValue;
            int n = SqlHelper.ExecuteNonQuery(sql, pars);
            if (n > 0)
            {
                MessageBox.Show("保存成功！");
            }
            else
            {
                MessageBox.Show("保存失败！");
            }              
            Bind();

        }

       
        private void frmAddCard_Load(object sender, EventArgs e)
        {
            string sql2 = "select * from RfidPurpose ";
            DataSet ds = SqlHelper.ExecuteDataset(sqlConnectionStr, CommandType.Text, sql2);
            comboBox1.DataSource = ds.Tables[0];
            comboBox1.DisplayMember = "PurposeName";
            comboBox1.ValueMember = "PurposeCode";      
            Bind();
        }



    }
}
