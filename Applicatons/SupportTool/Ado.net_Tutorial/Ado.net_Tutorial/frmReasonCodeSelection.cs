using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Ado.net_Tutorial
{
    public partial class frmReasonCodeSelection : Form
    {
        private string opsuiteConnectionString;

        public frmReasonCodeSelection(string opsuiteConnectionString, string customer)
        {

            InitializeComponent();
            this.opsuiteConnectionString = opsuiteConnectionString;
            lblReasonCodeAdjust.Text =  customer + " Inventory Adjustments";
            getReasonCodes();

        }

        private void getReasonCodes()
        {
            SqlConnection conn = new SqlConnection(this.opsuiteConnectionString);
            DataSet dsReasonCodes = new DataSet();
            SqlDataAdapter daReasonCodes = new SqlDataAdapter("SELECT ReasonCodeId,[Description] From ReasonCode where reasoncodetypeID = 2", conn);
            try
            {
                daReasonCodes.Fill(dsReasonCodes, "ReasonCode");
                cmbReasonCodes.DataSource = dsReasonCodes.Tables["ReasonCode"];
                cmbReasonCodes.DisplayMember = "Description";
                cmbReasonCodes.ValueMember = "ReasonCodeId";
                cmbReasonCodes.SelectedIndex = 0;
            }

            catch (System.Data.SqlClient.SqlException e)
            {
                var inactive =
                 new[] { "No Reason Codes Available" };
                cmbReasonCodes.DataSource = inactive;
            }
        }

        private void btnReasonCodeUpdate_Click(object sender, EventArgs e)
        {
            if (cmbReasonCodes.SelectedIndex > -1)
            {
                SqlConnection opConn = new SqlConnection(this.opsuiteConnectionString);

                try
                {
                    opConn.Open();

                    //update null records to use default reason code
                    SqlCommand updateReasonCode = new SqlCommand(@"UPDATE dbo.Company
                                                           SET DefaultInventoryTransferReasonCodeId = @defaultReasonCode
                                                           WHERE DefaultInventoryTransferReasonCodeId IS NULL ", opConn);


                    SqlParameter reasonCode = new SqlParameter();
                    reasonCode.ParameterName = "@defaultReasonCode";
                    reasonCode.Value = cmbReasonCodes.SelectedValue;

                    // 2. add new parameter to command object
                    updateReasonCode.Parameters.Add(reasonCode);

                    updateReasonCode.ExecuteNonQuery();
                }

                finally
                {
                    // Close the connection
                    if (opConn != null)
                    {
                        opConn.Close();
                    }
                }

                MessageBox.Show("Default Reason Code Updated to " + cmbReasonCodes.Text + ". Try resending the orders again.", "Info", MessageBoxButtons.OK);
            }

            else
            {
                MessageBox.Show("Please select a reason code from the list before clicking the update button.", "Info", MessageBoxButtons.OK);
            }

        }
    }
}
