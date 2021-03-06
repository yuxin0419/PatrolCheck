﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace WorkStation
{
    public partial class frmAddRoutName : Form
    {
        public Boolean isEdit=false;
        public object routeID;
        public string routeName, routeAlias, routeArea;
        public TreeView tView;
        public frmAddRoutName()
        {
            InitializeComponent();
        }

        private void frmAddRoutName_Load(object sender, EventArgs e)
        {
            this.cbo_init();
            if (isEdit)
            {
                this.btnTrue.Text = "修改";
                this.Text = "修改巡检路线";
                SqlDataReader dr = SqlHelper.ExecuteReader("Select Site_ID,Name,Alias,Sequence From CheckRoute Where ID="+routeID.ToString());
                if (dr == null) return;
                while (dr.Read())
                {
                    this.cboSiteArea.SelectedValue = dr["Site_ID"];
                    this.tbRouteName.Text = dr["Name"].ToString();
                    this.tbRouteAlias.Text = dr["Alias"].ToString();
                    this.cboInOrder.SelectedValue = dr["Sequence"].ToString();
                }
                dr.Dispose();
            }
           
        }

        private void btnTrue_Click(object sender, EventArgs e)
        {
            int _ret=(int)SqlHelper.ExecuteScalar("Select Count(1) From CheckRoute Where Name='" + this.tbRouteName.Text.Trim() + "' and Site_ID=" + cboSiteArea.SelectedValue.ToString());
            if ( isEdit==false&&_ret!= 0)
            {
                MessageBox.Show("请确保路线名称的唯一性");
                return;
            }
            string strsql = "";
            SqlParameter[] pars = new SqlParameter[] { 
               new SqlParameter("@id",SqlDbType.BigInt),
               new SqlParameter("@name",this.tbRouteName.Text.Trim().ToString()),
               new SqlParameter("@alias",this.tbRouteAlias.Text.Trim().ToString()),
               new SqlParameter("@routeid",SqlDbType.BigInt),
               new SqlParameter("@sequence",SqlDbType.Int)
            };
            if (isEdit)
            {
                strsql = "Update CheckRoute Set Site_ID=@id,[Name]=@name,Alias=@alias,Sequence=@sequence Where ID=@routeid";                
            }
            else
            {
                strsql = "Insert Into CheckRoute(Site_ID,[Name],Alias,Sequence) Values(@id,@name,@alias,@sequence)";
                
            }           
            pars[0].Value = cboSiteArea.SelectedValue.ToString();
            pars[3].Value = routeID;
            pars[4].Value = this.cboInOrder.SelectedValue;
            SqlHelper.ExecuteNonQuery(strsql, pars);
            frmAddRoute.tvRouteInit(tView);
            tView.ExpandAll();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cbo_init()
        {
            DataSet ds = SqlHelper.ExecuteDataset("Select Code,Meaning From Codes where purpose='CheckSequence' ");
            this.cboInOrder.DataSource = ds.Tables[0];
            this.cboInOrder.DisplayMember = "Meaning";
            this.cboInOrder.ValueMember = "Code";
            this.cboInOrder.SelectedIndex = this.cboInOrder.Items.Count > 0 ? 0 : -1;
            ds.Dispose();

            ds = SqlHelper.ExecuteDataset("Select Id,Name From Site");
            cboSiteArea.DataSource = ds.Tables[0];
            cboSiteArea.DisplayMember = "Name";
            cboSiteArea.ValueMember = "ID";
            this.cboSiteArea.SelectedIndex = this.cboSiteArea.Items.Count > 0 ? 0 : -1;
            ds.Dispose();
        }
    }
}
