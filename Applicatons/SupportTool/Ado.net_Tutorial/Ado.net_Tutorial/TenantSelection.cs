using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ado.net_Tutorial
{
    public partial class TenantSelection : Form
    {
        public TenantSelection()
        {
            InitializeComponent();
            cmbTenantSelect.SelectedIndex = 0;
        }

        private int vpnTest()
        {
            int result = 0;

            // 1. Instantiate the connection
            SqlConnection conn = new SqlConnection(
            "Data Source=172.30.0.68;Initial Catalog=Monitor;persist security info=True;user id=sa;password=1black3#;MultipleActiveResultSets=True;");
            try
            {
                // 2. Open the connection
                conn.Open();

                // 3. Pass the connection to a command object
                SqlCommand cmd = new SqlCommand("select 1 from ssis_Configurations", conn);

                //
                // 4. Use the connection
                SqlDataReader rdr = cmd.ExecuteReader();

            }
            catch (SqlException sqlEx)
            {
                //MessageBox.Show("An exception occured " + sqlEx.ToString());
                switch (sqlEx.Number)
                {
                    case 53:   // Client Timeout
                        result = 53;
                        break;
                    case 701:  // Out of Memory
                    case 1204: // Lock Issue 
                    case 1205: // >>> Deadlock Victim
                    case 1222: // Lock Request Timeout
                    case 2627: // Primary Key Violation
                    case 8645: // Timeout waiting for memory resource 
                    case 8651: // Low memory condition 
                        break;
            
                }
            }
            finally
            {

                // 5. Close the connection
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return result;
        }


        private void btnGo_Click(object sender, EventArgs e)
        {
            int connected = vpnTest();

            if (connected == 0)
            {

                if (cmbTenantSelect.SelectedIndex > -1)
                {
                    int tenantId = cmbTenantSelect.SelectedIndex;
                    var form = new frmResendOrders(tenantId);
                    form.ShowDialog();

                }

                else
                {
                    MessageBox.Show("Select A Tenant");
                }
            }
            else{
                MessageBox.Show("Sql Connection timed out: Make sure you are connected to the VPN");
            }

        }
    }
}
