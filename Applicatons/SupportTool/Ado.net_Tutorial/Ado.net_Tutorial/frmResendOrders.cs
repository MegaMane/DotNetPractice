using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;

namespace Ado.net_Tutorial
{
    public partial class frmResendOrders : Form
    {
        UserDomainEntities uDomain;
        private string posecConnectionString;
        private string opsuiteConnectionString;

        public frmResendOrders(int tenantId)
        {
            InitializeComponent();
            string strConn;
            if (tenantId == 0)
            {
                strConn = ConfigurationManager.ConnectionStrings["UserDomainEntities"].ConnectionString;
            }
            else if (tenantId == 1)
            {
                strConn = ConfigurationManager.ConnectionStrings["UserDomainEntitiesAM"].ConnectionString;
            }

            else if (tenantId == 2)
            {
                strConn = ConfigurationManager.ConnectionStrings["UserDomainEntitiesGS"].ConnectionString;
            }


            else
            {
                strConn = ConfigurationManager.ConnectionStrings["UserDomainEntitiesDemo"].ConnectionString;
            }

            uDomain = new UserDomainEntities(strConn);
            cmbCustomerSelect.DataSource = uDomain.Enterprises.OrderBy(e => e.EnterpriseName).ToList();
            cmbCustomerSelect.DisplayMember = "EnterpriseName";
            cmbCustomerSelect.ValueMember = "EnterpriseId";
            cmbCustomerSelect.SelectedIndex = -1;
            cmbDocSelect.SelectedIndex = 0;
            cmbCustomerSelect.SelectedValueChanged += cmbCustomerSelect_SelectedValueChanged;
            cmbDocSelect.SelectedIndexChanged += cmbDocSelect_SelectedIndexChanged;
            cmbLocation.Items.Add("Choose a Customer");
            txtOrderNumber.ReadOnly = true;
            dtDateRange.Enabled = false;
            dtDateEnd.Enabled = false;
            lblEndDate.Enabled = false;
            lblStartDate.Enabled = false;
            chkHealingAdjustment.Enabled = false;
        }

        private bool checkDates (string startDate, string endDate)
        {
            MessageBox.Show(startDate);

            if (DateTime.Parse(startDate) <= DateTime.Parse(endDate))
                return true;
            else
                return false;
        }

        private bool checkReasonCodes ()
        {
            return false;
        }


        private void cmbCustomerSelect_SelectedValueChanged(object sender, EventArgs e)
        {
            int enterpriseID = (int)cmbCustomerSelect.SelectedValue;
            //MessageBox.Show(enterpriseID);

           // MessageBox.Show(cmbCustomerSelect.Text);

            posecConnectionString = uDomain.ConnectionStrings
                .Where(cs => cs.EnterpriseId == enterpriseID && cs.ConnectionStringTypeId == 1)
                .Select(cs => cs.ConnectionString1)
                .FirstOrDefault();

            opsuiteConnectionString = uDomain.ConnectionStrings
                .Where(cs => cs.EnterpriseId == enterpriseID && cs.ConnectionStringTypeId == 2)
                .Select(cs => cs.ConnectionString1)
                .FirstOrDefault();


            getLocations();

        }


        private void getLocations()
        {
            SqlConnection conn = new SqlConnection(opsuiteConnectionString);
            DataSet dsLocations = new DataSet();
            SqlDataAdapter daLocations = new SqlDataAdapter("SELECT LocationId, Name FROM location WHERE Active = 1 Order By Name", conn);
            try
            {
                daLocations.Fill(dsLocations, "Location");
                cmbLocation.DataSource = dsLocations.Tables["Location"];
                cmbLocation.DisplayMember = "Name";
                cmbLocation.ValueMember = "LocationId";
                cmbLocation.SelectedIndex = 0;
            }

            catch (System.Data.SqlClient.SqlException e)
            {
                var inactive =
                 new[] { "No Active Locations" };
                cmbLocation.DataSource = inactive;
            }


        }



        private void btnGo_Click(object sender, EventArgs e)
        {
            dgvOrderSearch.DataSource = null;
            dgvOrderSearch.Rows.Clear();

            if (cmbCustomerSelect.SelectedIndex > -1 && cmbDocSelect.SelectedIndex > 0)
            {
                string query;

                if (cmbDocSelect.SelectedIndex == 1)
                {
                    query = @"SELECT ICD.InventoryControlDocumentId, 
                                    ICD.OrderNumber, 
	                                ICD.ICDTitle, 
	                                ICD.OriginatorLocationId, 
	                                Loc.Name, 
	                                ICD.DateCreated, 
	                                ICD.LastUpdateDate, 
	                                ICD.IcdStatusId, 
	                                ISTAT.[Status],
	                                ICDM.Itemid AS Itemmasterid,
	                                ICDM.ItemDescription AS [Description],
	                                ICDM.Cost,
	                                ICDM.Price,
	                                ICDM.QuantityOrdered,
	                                ICDM.QuantityReceivedToDate
                                FROM dbo.InventoryControlDocument AS ICD
                        INNER JOIN dbo.IcdStatus AS ISTAT
                                ON ICD.IcdStatusId = ISTAT.ID
                        INNER JOIN Location AS Loc
                                ON Loc.LocationId = ICD.OriginatorLocationId
                        INNER JOIN dbo.ICDMerchandiseLineItem AS ICDM
                                ON ICDM.InventoryControlDocumentId = ICD.InventoryControlDocumentId
                                WHERE ICD.OrderNumber = @orderNumber";
                }
                else
                {
                    query = @"SELECT ICD.InventoryControlDocumentId, 
                                    ICD.OrderNumber, 
	                                ICD.ICDTitle, 
	                                ICD.OriginatorLocationId, 
	                                Loc.Name, 
	                                ICD.DateCreated, 
	                                ICD.LastUpdateDate, 
	                                ICD.IcdStatusId, 
	                                ISTAT.[Status],
	                                RDLI.ItemMasterId AS Itemmasterid,
	                                RDLI.[Description] AS [Description],
	                                RDLI.ReceivedCost AS Cost,
	                                RDLI.Allocation,
	                                RDLI.OrderedQty AS QuantityOrdered,
	                                RDLI.ReceivedQty AS QuantityReceived
                                FROM dbo.InventoryControlDocument AS ICD
                        INNER JOIN dbo.IcdStatus AS ISTAT
                                ON ICD.IcdStatusId = ISTAT.ID
                        INNER JOIN Location AS Loc
                                ON Loc.LocationId = ICD.OriginatorLocationId
                        INNER JOIN dbo.BaseOrderDocument AS BD
                                ON ICD.InventoryControlDocumentId = BD.InventoryControlDocumentId
						INNER JOIN dbo.ReceivingDocumentLineItem AS RDLI
							    ON RDLI.InventoryControlDocumentId = ICD.InventoryControlDocumentId
                                WHERE ICD.OrderNumber = @orderNumber";
                }

                SqlParameter param = new SqlParameter();
                param.ParameterName = "@orderNumber";
                param.Value = txtOrderNumber.Text;



                SqlConnection conn = new SqlConnection(opsuiteConnectionString);
                DataSet dsOrderResults = new DataSet();
                SqlDataAdapter daOrderResults = new SqlDataAdapter(query, conn);
                daOrderResults.SelectCommand.Parameters.Add(param);

                int rowsReturned = daOrderResults.Fill(dsOrderResults, "OrderResults");

                if (rowsReturned > 0)
                {


                    dgvOrderSearch.DataSource = dsOrderResults;
                    dgvOrderSearch.DataMember = "OrderResults";

                    /* Get origin location ID from dataset without going back to db 
                     * create data table so linq can be used to query*/
                    DataTable dt3 = dsOrderResults.Tables["OrderResults"];
                    IEnumerable<DataRow> dquery =
                          from item in dt3.AsEnumerable()
                          select item;

                    int orderLocation = 0;

                    foreach (DataRow p in dquery)
                    {
                        orderLocation = (p.Field<int>("OriginatorLocationId"));
                        break;
                    }

                    cmbLocation.SelectedValue = orderLocation.ToString();

                }

                else
                {
                    MessageBox.Show("No Results Returned");
                } 
            }

            else
            {
                MessageBox.Show("Select a customer before searching for an order and make sure a doc type is selected.");
            }

        
        }

        private void txtOrderNumber_Enter(object sender, EventArgs e)
        {
            if (cmbCustomerSelect.SelectedIndex > -1 && txtOrderNumber.ReadOnly && rbtSingleOrder.Checked)
            {
                txtOrderNumber.ReadOnly = false;
                txtOrderNumber.Text = "";
            }
        }

        private void rbtSingleOrder_CheckedChanged(object sender, EventArgs e)
        {
            dtDateRange.Enabled = false;
            dtDateEnd.Enabled = false;
            lblStartDate.Enabled = false;
            lblEndDate.Enabled = false;

            if (cmbCustomerSelect.SelectedIndex > -1)
                txtOrderNumber.Text = "Enter Order Number";
            else
                txtOrderNumber.Text = "Choose A Customer";
        }

        private void rbtDateRange_CheckedChanged(object sender, EventArgs e)
        {
            dtDateRange.Enabled = true;
            dtDateEnd.Enabled = true;
            lblStartDate.Enabled = true;
            lblEndDate.Enabled = true;

            if (cmbCustomerSelect.SelectedIndex > -1)
                txtOrderNumber.Text = "Date range";
            else
                txtOrderNumber.Text = "Choose A Customer";
            txtOrderNumber.ReadOnly = true;
        }

        private void cmbDocSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            //MessageBox.Show(cmbDocSelect.SelectedItem.ToString());


            if (cmbDocSelect.SelectedIndex == 1 || cmbDocSelect.SelectedIndex == 2)
            {
                rbtSingleOrder.Enabled = true;
                rbtDateRange.Enabled = true;
                chkHealingAdjustment.Enabled = false;
            }



            else if (cmbDocSelect.SelectedIndex == 3 || cmbDocSelect.SelectedIndex == 4)
            {
                rbtSingleOrder.Enabled = false;
                rbtDateRange.Enabled = true;
                rbtDateRange.Checked = true;
                chkHealingAdjustment.Enabled = true;
            }

            else if (cmbDocSelect.SelectedIndex == 5)
            {
                rbtSingleOrder.Enabled = true;
                rbtDateRange.Enabled = false;
                rbtSingleOrder.Checked = true;
                chkHealingAdjustment.Enabled = false;
            }


        }

        private void btnResend_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(posecConnectionString);

            //Purchase Orders
            if (cmbDocSelect.SelectedIndex == 1 && cmbCustomerSelect.SelectedIndex > -1 )
            {
                if (rbtSingleOrder.Checked)
                {

                    int InventoryControlDocumentID = -1;

                    try
                    {
                        // Open the connection
                        conn.Open();

                        // 1. Instantiate a new command
                        SqlCommand cmd = new SqlCommand(
                            @"SELECT InventoryControlDocumentID
                                FROM dbo.synInventoryControlDocument 
                               WHERE OrderNumber = @ordernumber
                                 AND ICDTransferTypeId = 0", conn);


                        SqlParameter param = new SqlParameter();
                        param.ParameterName = "@orderNumber";
                        param.Value = txtOrderNumber.Text;

                        // 2. add new parameter to command object
                        cmd.Parameters.Add(param);

                        // 3. Call ExecuteScalar to send command
                        //Returns object with null reference if empty result set
                        //use convert to int 32 to convert null to 0
                        InventoryControlDocumentID = Convert.ToInt32(cmd.ExecuteScalar());

                        if (InventoryControlDocumentID > 0)
                        {
                            
                            cmd = new SqlCommand(
                                @"	BEGIN;
				                    INSERT INTO dbo.posecIdb_ReferredChangeLog
				                            ( ChangeID ,
				                              ChangeType ,
				                              RecordID ,
				                              MessageType ,
				                              MessageTypeVersion ,
				                              AppliesTo ,
				                              Priority ,
				                              InsertedDT ,
				                              Actioned ,
				                              GroupingId
				                            )
                                    SELECT  NEWID() ,
                                            'Send' ,
                                            ICD.InventoryControlDocumentId ,
                                            'PurchaseOrder' ,
                                            '1.0' ,
                                            RTI.IPAddressOrDomainName ,
                                            1 ,
                                            GETDATE() ,
                                            0 ,
                                            0
                                    FROM    dbo.synInventoryControlDocument ICD
                                            LEFT JOIN dbo.synRtiStoreAllocation RTI ON ICD.OriginatorLocationId = RTI.LocationId
						                    WHERE ICD.InventoryControlDocumentId = @InventoryControlDocumentID
						                    AND RTI.LocationId IS NOT NULL
				                    END;", conn);

                            param = new SqlParameter();
                            param.ParameterName = "@InventoryControlDocumentID";
                            param.Value = InventoryControlDocumentID;

                            cmd.Parameters.Add(param);

                            cmd.ExecuteNonQuery();
                            MessageBox.Show("The PO " + txtOrderNumber + " exists and was sent to the store.");

                        }
                        else
                        {
                            MessageBox.Show("The PO " + txtOrderNumber + " was not found.");
                        }
                    }
                    finally
                    {
                        // Close the connection
                        if (conn != null)
                        {
                            conn.Close();
                        }
                    }
                }

                //PO From Date Range
                else
                {
                    //dtDateRange.Format = DateTimePickerFormat.Custom;
                    //dtDateRange.CustomFormat = "yyyyMMdd hh:mm:ss";
                    MessageBox.Show(dtDateRange.Value.ToString("yyyyMMdd 00:00:00"));
                    MessageBox.Show(dtDateEnd.Value.ToString("yyyyMMdd 23:59:59"));
                    if (checkDates(dtDateRange.Value.ToString("MM/dd/yyyy"), dtDateEnd.Value.ToString("MM/dd/yyyy")))
                    {

                        DialogResult result = MessageBox.Show("Do you want to send all Purchase Orders for "
                                                                                                    + cmbLocation.Text + " from "
                                                                                                    + dtDateRange.Value.ToString("MM/dd/yyyy")
                                                                                                    + " to "
                                                                                                    + dtDateEnd.Value.ToString("MM/dd/yyyy")
                                                                                                    + " ?", "Info", MessageBoxButtons.YesNo);

                        if (result == DialogResult.Yes)
                        {

                            try
                            {
                                // Open the connection
                                conn.Open();

                                // 1. Instantiate a new command
                                SqlCommand cmd = new SqlCommand(
                                    @"	BEGIN;
				                        INSERT INTO dbo.posecIdb_ReferredChangeLog
				                                ( ChangeID ,
				                                  ChangeType ,
				                                  RecordID ,
				                                  MessageType ,
				                                  MessageTypeVersion ,
				                                  AppliesTo ,
				                                  Priority ,
				                                  InsertedDT ,
				                                  Actioned ,
				                                  GroupingId
				                                )
                                        SELECT  NEWID() ,
                                                'Send' ,
                                                ICD.InventoryControlDocumentId ,
                                                'PurchaseOrder' ,
                                                '1.0' ,
                                                RTI.IPAddressOrDomainName ,
                                                1 ,
                                                GETDATE() ,
                                                0 ,
                                                0
                                        FROM    dbo.synInventoryControlDocument ICD
                                                LEFT JOIN dbo.synRtiStoreAllocation RTI ON ICD.OriginatorLocationId = RTI.LocationId
						                        WHERE ICD.OriginatorLocationId = @locationID 
						                        AND ICD.LastUpdateDate Between @date AND @enddate
						                        AND ICD.isplaced = 1 
						                        AND ICD.IcdTransferTypeId = 0
						                        AND RTI.LocationId IS NOT NULL
		                            END;", conn);


                                SqlParameter date = new SqlParameter();
                                date.ParameterName = "@date";
                                date.Value = dtDateRange.Value.ToString("yyyyMMdd");

                                SqlParameter enddate = new SqlParameter();
                                enddate.ParameterName = "@enddate";
                                enddate.Value = dtDateEnd.Value.ToString("yyyyMMdd");

                                SqlParameter location = new SqlParameter();
                                location.ParameterName = "@locationID ";
                                location.Value = cmbLocation.SelectedValue;

                                // 2. add new parameter to command object
                                cmd.Parameters.Add(date);
                                cmd.Parameters.Add(enddate);
                                cmd.Parameters.Add(location);

                                //Execute the query
                                cmd.ExecuteNonQuery();
                                MessageBox.Show("Purchase Orders Sent");

                            }

                            finally
                            {
                                // Close the connection
                                if (conn != null)
                                {
                                    conn.Close();
                                }
                            }

                        }
                        else
                        {
                            MessageBox.Show("Send Canceled");
                        }

                    }

                    else
                    {
                        MessageBox.Show("End Date must be >= to Start Date");
                    }
                }
            }

            //Vouchers
            else if (cmbDocSelect.SelectedIndex == 2 && cmbCustomerSelect.SelectedIndex > -1)
            {
                //single voucher
                if (rbtSingleOrder.Checked)
                {

                    int InventoryControlDocumentID = -1;

                    try
                    {
                        // Open the connection
                        conn.Open();

                        // 1. Instantiate a new command
                        SqlCommand cmd = new SqlCommand(
                            @"SELECT InventoryControlDocumentID
                                FROM dbo.synInventoryControlDocument 
                               WHERE OrderNumber = @ordernumber
                                 AND ICDTransferTypeId = 6", conn);


                        SqlParameter param = new SqlParameter();
                        param.ParameterName = "@orderNumber";
                        param.Value = txtOrderNumber.Text;

                        // 2. add new parameter to command object
                        cmd.Parameters.Add(param);

                        // 3. Call ExecuteScalar to send command
                        //Returns object with null reference if empty result set
                        //use convert to int 32 to convert null to 0
                        InventoryControlDocumentID = Convert.ToInt32(cmd.ExecuteScalar());

                        if (InventoryControlDocumentID > 0)
                        {
                            
                            cmd = new SqlCommand(
                                @"	BEGIN;
				                    INSERT INTO dbo.posecIdb_ReferredChangeLog
				                            ( ChangeID ,
				                              ChangeType ,
				                              RecordID ,
				                              MessageType ,
				                              MessageTypeVersion ,
				                              AppliesTo ,
				                              Priority ,
				                              InsertedDT ,
				                              Actioned ,
				                              GroupingId
				                            )
                                    SELECT  NEWID() ,
                                            'Send' ,
                                            ICD.InventoryControlDocumentId ,
                                            'Voucher' ,
                                            '1.0' ,
                                            RTI.IPAddressOrDomainName ,
                                            1 ,
                                            GETDATE() ,
                                            0 ,
                                            0
                                    FROM    dbo.synInventoryControlDocument ICD
                                            LEFT JOIN dbo.synRtiStoreAllocation RTI ON ICD.OriginatorLocationId = RTI.LocationId
						                    WHERE ICD.InventoryControlDocumentId = @InventoryControlDocumentID
						                    AND RTI.LocationId IS NOT NULL
				                    END;", conn);

                            param = new SqlParameter();
                            param.ParameterName = "@InventoryControlDocumentID";
                            param.Value = InventoryControlDocumentID;

                            cmd.Parameters.Add(param);

                            cmd.ExecuteNonQuery();
                            MessageBox.Show("The voucher " + txtOrderNumber.Text + " exists and was resent to the store.");

                        }

                        else
                        {
                            MessageBox.Show("The voucher " + txtOrderNumber + " was not found.");
                        }
                    }
                    finally
                    {
                        // Close the connection
                        if (conn != null)
                        {
                            conn.Close();
                        }
                    }

                }

                //Date Range Voucher
                else
                {
                    if (checkDates(dtDateRange.Value.ToString("MM/dd/yyyy"), dtDateEnd.Value.ToString("MM/dd/yyyy")))
                    {

                        DialogResult result = MessageBox.Show("Do you want to send all Vouchers for "
                                                            + cmbLocation.Text + " from "
                                                            + dtDateRange.Value.ToString("MM/dd/yyyy")
                                                            + " to "
                                                            + dtDateEnd.Value.ToString("MM/dd/yyyy")
                                                            + " ?", "Info", MessageBoxButtons.YesNo);

                        if (result == DialogResult.Yes)
                        {

                            try
                            {
                                // Open the connection
                                conn.Open();

                                // 1. Instantiate a new command
                                SqlCommand cmd = new SqlCommand(
                                    @"	BEGIN;
				                        INSERT INTO dbo.posecIdb_ReferredChangeLog
				                                ( ChangeID ,
				                                  ChangeType ,
				                                  RecordID ,
				                                  MessageType ,
				                                  MessageTypeVersion ,
				                                  AppliesTo ,
				                                  Priority ,
				                                  InsertedDT ,
				                                  Actioned ,
				                                  GroupingId
				                                )
                                        SELECT  NEWID() ,
                                                'Send' ,
                                                ICD.InventoryControlDocumentId ,
                                                'Voucher' ,
                                                '1.0' ,
                                                RTI.IPAddressOrDomainName ,
                                                1 ,
                                                GETDATE() ,
                                                0 ,
                                                0
                                        FROM    dbo.synInventoryControlDocument ICD
                                                LEFT JOIN dbo.synRtiStoreAllocation RTI ON ICD.OriginatorLocationId = RTI.LocationId
						                        WHERE ICD.OriginatorLocationId = @locationID 
						                        AND ICD.LastUpdateDate BETWEEN @date AND @enddate
						                        AND ICD.isplaced = 1 
						                        AND ICD.IcdTransferTypeId = 6
						                        AND RTI.LocationId IS NOT NULL
		                            END;", conn);


                                SqlParameter date = new SqlParameter();
                                date.ParameterName = "@date";
                                date.Value = dtDateRange.Value.ToString("yyyyMMdd");

                                SqlParameter enddate = new SqlParameter();
                                enddate.ParameterName = "@enddate";
                                enddate.Value = dtDateEnd.Value.ToString("yyyyMMdd");

                                SqlParameter location = new SqlParameter();
                                location.ParameterName = "@locationID ";
                                location.Value = cmbLocation.SelectedValue;

                                // 2. add new parameter to command object
                                cmd.Parameters.Add(date);
                                cmd.Parameters.Add(enddate);
                                cmd.Parameters.Add(location);

                                //Execute the query
                                cmd.ExecuteNonQuery();
                                MessageBox.Show("Vouchers Sent");

                            }

                            finally
                            {
                                // Close the connection
                                if (conn != null)
                                {
                                    conn.Close();
                                }
                            }

                        }
                        else
                        {
                            MessageBox.Show("Send Canceled");
                        }
                    }

                    else
                    {
                        MessageBox.Show("End Date must be >= to Start Date");
                    }
                }
            }


            
            //Manual Inventory Adjustment
            else if (cmbDocSelect.SelectedIndex == 3 && cmbCustomerSelect.SelectedIndex > -1)
            {
                bool invalidState = false;

                if (checkDates(dtDateRange.Value.ToString("MM/dd/yyyy"), dtDateEnd.Value.ToString("MM/dd/yyyy")))
                {
                    SqlConnection opConn = new SqlConnection(opsuiteConnectionString);
                    try
                    {
                        //Check for any reason codes that are null in the inventory change log within the date range

                        SqlDataReader rdr = null;
                        
                        // create a command object
                        SqlCommand cmd = new SqlCommand(@" SELECT COUNT(InventoryChangeLogId) AS recordCount
                                                             FROM dbo.InventoryChangeLog
                                                            WHERE InventoryChangeTypeId = 7
                                                              AND InventoryControlDocumentId IS NOT NULL
                                                              AND ReasonCodeId IS NULL
                                                              AND LocationID = @locationID
                                                              AND ChangeDate BETWEEN @date AND @enddate", opConn);

                        SqlParameter date = new SqlParameter();
                        date.ParameterName = "@date";
                        date.Value = dtDateRange.Value.ToString("yyyyMMdd");

                        SqlParameter enddate = new SqlParameter();
                        enddate.ParameterName = "@enddate";
                        enddate.Value = dtDateEnd.Value.ToString("yyyyMMdd");

                        SqlParameter location = new SqlParameter();
                        location.ParameterName = "@locationID ";
                        location.Value = cmbLocation.SelectedValue;

                        // 2. add new parameter to command object
                        cmd.Parameters.Add(date);
                        cmd.Parameters.Add(enddate);
                        cmd.Parameters.Add(location);


                        opConn.Open();

                        // get an instance of the SqlDataReader
                        rdr = cmd.ExecuteReader();

                        rdr.Read();

                        int recordCount = (int)rdr["recordCount"];

                        //MessageBox.Show("The record count is " + recordCount);

                        if (rdr != null){rdr.Close();}
                        cmd.Parameters.Clear();

                        //handle null reason codes in inventory change log
                        if (recordCount > 0)
                        {
                            

                            //check for default reason code and use to update null records if it exists
                            SqlCommand chkDefaultReasonCode = new SqlCommand("select ISNULL(DefaultInventoryTransferReasoncodeID,0) AS  defaultCode from Company", opConn);

                            // get an instance of the SqlDataReader
                            rdr = chkDefaultReasonCode.ExecuteReader();

                            rdr.Read();
                            

                            int defaultReasonCode = (int)rdr["defaultCode"]; 

                            //MessageBox.Show("The default reason code is " + defaultReasonCode);

                            if (rdr != null) { rdr.Close(); }

                            if (defaultReasonCode != 0)
                            {
                                //update null records to use default reason code
                                SqlCommand updateReasonCodes = new SqlCommand(@" UPDATE dbo.InventoryChangeLog
                                                                                  SET ReasonCodeId = @defaultReasonCode
                                                                                WHERE InventoryChangeTypeId = 7
                                                                                  AND InventoryControlDocumentId IS NOT NULL
                                                                                  AND ReasonCodeId IS NULL
                                                                                  AND LocationID = @locationID
                                                                                  AND ChangeDate BETWEEN @date AND @enddate", opConn);


                                SqlParameter reasonCode = new SqlParameter();
                                reasonCode.ParameterName = "@defaultReasonCode";
                                reasonCode.Value = defaultReasonCode;

                                // 2. add new parameter to command object
                                updateReasonCodes.Parameters.Add(date);
                                updateReasonCodes.Parameters.Add(enddate);
                                updateReasonCodes.Parameters.Add(location);
                                updateReasonCodes.Parameters.Add(reasonCode);

                                updateReasonCodes.ExecuteNonQuery();

                                MessageBox.Show("Null values found for ReasonCode column. Updated to use default reason code");
                            }

                            else
                            {
                                //MessageBox.Show("No default reason code");
                                //check for any valid reason codes that can be used to populate the default

                                //NOTE * *type is 2(Inventory Main Manual Adjust)
                                SqlCommand chkValidReasonCode = new SqlCommand("select COUNT(ReasonCodeId) AS validCodes from reasoncode where reasoncodetypeID = 2", opConn);

                                rdr = chkValidReasonCode.ExecuteReader();

                                rdr.Read();


                                int validReasonCodes = (int)rdr["validCodes"];

                                if (rdr != null) { rdr.Close(); }

                                if (validReasonCodes > 0)
                                {
                                    //select new default reason code
                                    MessageBox.Show("There were Null values in the reason code columns " +
                                                    "for the records in the date range selected, " +
                                                    "but no default reason code was found for the customer. " +
                                                    "Please select a reason code to use as the new default to continue.", "Info", MessageBoxButtons.OK);

                                    var form = new frmReasonCodeSelection(opsuiteConnectionString, cmbCustomerSelect.Text);
                                    form.ShowDialog();
                                    //default reason code selected but rows not updated to use reason code yet need to click resend button again
                                    invalidState = true;

                                }

                                else
                                {
                                    //no valid reason codes found one must be created in Opsuite and then the update should be attempted again

                                    //select new default reason code
                                    MessageBox.Show("There were no valid reason codes found that could be used as the default. " 
                                                    +"Login to Opsuite and create one then try to resend again.", "Info", MessageBoxButtons.OK);

                                    invalidState = true;
                                }


                            }

                        }



                        if (chkHealingAdjustment.Checked == true && !invalidState)
                        {
                            DialogResult result = MessageBox.Show("Healing Adjustment Box Checked."
                                                                + " Only the item movement will be sent down."
                                                                + " Item quantities will not be updated. Proceed?", "Info", MessageBoxButtons.YesNo);

                            if (result == DialogResult.Yes)
                            {
                                try
                                {
                                    // Open the connection
                                    conn.Open();

                                    //clear previous parameters associated with this command
                                    cmd.Parameters.Clear();

                                    cmd = new SqlCommand(
                                        @"	BEGIN;
	                                            ;WITH cte_CHANGELOG AS 
	                                            (
	                                            SELECT DISTINCT 'Send' AS ChangeType ,
					                                             InventoryControlDocumentId AS RecordID,
					                                            'HealQuantityAdjustment' AS MessageType,
					                                            '1.0' AS MessageTypeVersion,
					                                             1 AS [Priority],
					                                             LocationID
			                                               FROM dbo.synInventoryChangeLog
			                                              WHERE InventoryChangeTypeId = 7
				                                            AND InventoryControlDocumentId IS NOT NULL
				                                            AND ReasonCodeId IS NOT NULL
				                                            AND LocationID = @locationID
				                                            AND ChangeDate BETWEEN @date AND @enddate
	                                            ) 
										
	                                            INSERT INTO dbo.posecIdb_ReferredChangeLog
			                                            ( ChangeID ,
				                                            ChangeType ,
				                                            RecordID ,
				                                            MessageType ,
				                                            MessageTypeVersion ,
				                                            AppliesTo ,
				                                            Priority ,
				                                            InsertedDT ,
				                                            Actioned ,
				                                            GroupingId
			                                            )


	                                            SELECT  NEWID() AS ChangeID,
			                                            ICL.ChangeType ,
			                                            ICL.RecordID ,
			                                            ICL.MessageType,
			                                            ICL.MessageTypeVersion ,
			                                            RTI.IPAddressOrDomainName AS AppliesTo ,
			                                            ICL.[Priority] ,
			                                            GETDATE() AS InsertedDT,
			                                            0 AS Actioned,
			                                            0 AS GroupingId
	                                            FROM    cte_CHANGELOG AS  ICL
			                                            INNER JOIN dbo.synRtiStoreAllocation RTI ON ICL.LocationId = RTI.LocationId 
												

                                            END;", conn);


                                    date = new SqlParameter();
                                    date.ParameterName = "@date";
                                    date.Value = dtDateRange.Value.ToString("yyyyMMdd");

                                    enddate = new SqlParameter();
                                    enddate.ParameterName = "@enddate";
                                    enddate.Value = dtDateEnd.Value.ToString("yyyyMMdd");

                                    location = new SqlParameter();
                                    location.ParameterName = "@locationID ";
                                    location.Value = cmbLocation.SelectedValue;

                                    // 2. add new parameter to command object
                                    cmd.Parameters.Add(date);
                                    cmd.Parameters.Add(enddate);
                                    cmd.Parameters.Add(location);

                                    //Execute the query
                                    cmd.ExecuteNonQuery();

                                }

                                finally
                                {
                                    // Close the connection
                                    if (conn != null)
                                    {
                                        conn.Close();
                                    }
                                }

                                //Processing for manual inventory adjustment with healing message for referred changelog
                                MessageBox.Show("Healing Adjustment Sent.");
                            }

                            else
                            {
                                MessageBox.Show("Healing Adjustment Canceled.");
                            }

                        }

                        else if (chkHealingAdjustment.Checked == false && !invalidState)
                        {
                            DialogResult result = MessageBox.Show(" Manual inventory adjustment will be sent down to the store."
                                    + " Proceed?", "Info", MessageBoxButtons.YesNo);

                            if (result == DialogResult.Yes)
                            {
                                try
                                {
                                    // Open the connection
                                    conn.Open();

                                    //clear previous parameters associated with this command
                                    cmd.Parameters.Clear();

                                    cmd = new SqlCommand(
                                       @"	BEGIN;
	                                            ;WITH cte_CHANGELOG AS 
	                                            (
	                                            SELECT DISTINCT 'Send' AS ChangeType ,
					                                             InventoryControlDocumentId AS RecordID,
					                                            'QuantityAdjustment' AS MessageType,
					                                            '1.0' AS MessageTypeVersion,
					                                             1 AS [Priority],
					                                             LocationID
			                                               FROM dbo.synInventoryChangeLog
			                                              WHERE InventoryChangeTypeId = 7
				                                            AND InventoryControlDocumentId IS NOT NULL
				                                            AND ReasonCodeId IS NOT NULL
				                                            AND LocationID = @locationID
				                                            AND ChangeDate BETWEEN @date AND @enddate
	                                            ) 
										
	                                            INSERT INTO dbo.posecIdb_ReferredChangeLog
			                                            ( ChangeID ,
				                                            ChangeType ,
				                                            RecordID ,
				                                            MessageType ,
				                                            MessageTypeVersion ,
				                                            AppliesTo ,
				                                            Priority ,
				                                            InsertedDT ,
				                                            Actioned ,
				                                            GroupingId
			                                            )


	                                            SELECT  NEWID() AS ChangeID,
			                                            ICL.ChangeType ,
			                                            ICL.RecordID ,
			                                            ICL.MessageType,
			                                            ICL.MessageTypeVersion ,
			                                            RTI.IPAddressOrDomainName AS AppliesTo ,
			                                            ICL.[Priority] ,
			                                            GETDATE() AS InsertedDT,
			                                            0 AS Actioned,
			                                            0 AS GroupingId
	                                            FROM    cte_CHANGELOG AS  ICL
			                                            INNER JOIN dbo.synRtiStoreAllocation RTI ON ICL.LocationId = RTI.LocationId 
												

                                            END;", conn);

                                    date = new SqlParameter();
                                    date.ParameterName = "@date";
                                    date.Value = dtDateRange.Value.ToString("yyyyMMdd");

                                    enddate = new SqlParameter();
                                    enddate.ParameterName = "@enddate";
                                    enddate.Value = dtDateEnd.Value.ToString("yyyyMMdd");

                                    location = new SqlParameter();
                                    location.ParameterName = "@locationID ";
                                    location.Value = cmbLocation.SelectedValue;

                                    // 2. add new parameter to command object
                                    cmd.Parameters.Add(date);
                                    cmd.Parameters.Add(enddate);
                                    cmd.Parameters.Add(location);

                                    //Execute the query
                                    cmd.ExecuteNonQuery();

                                }

                                finally
                                {
                                    // Close the connection
                                    if (conn != null)
                                    {
                                        conn.Close();
                                    }
                                }

                                //Normal processing for manual inventory adjustment
                                MessageBox.Show("Manual inventory adjustment sent.");
                            }

                            else
                            {
                                //Normal processing for manual inventory adjustment
                                MessageBox.Show("Manual inventory adjustment send canceled.");
                            }

                        }
                    }

                    finally
                    {
                        // Close the connection
                        if (conn != null)
                        {
                            opConn.Close();
                        }
                    }

                }

                else
                {
                    MessageBox.Show("End Date must be >= to Start Date");
                }

            }


            //Physical Inventory Adjustment
            else if (cmbDocSelect.SelectedIndex == 4 && cmbCustomerSelect.SelectedIndex > -1)
            {
                bool invalidState = false;

                if (checkDates(dtDateRange.Value.ToString("MM/dd/yyyy"), dtDateEnd.Value.ToString("MM/dd/yyyy")))
                {
                    SqlConnection opConn = new SqlConnection(opsuiteConnectionString);
                    try
                    {
                        //Check for any reason codes that are null in the inventory change log within the date range

                        SqlDataReader rdr = null;

                        // create a command object
                        SqlCommand cmd = new SqlCommand(@" SELECT COUNT(InventoryChangeLogId) AS recordCount
                                                             FROM dbo.InventoryChangeLog
                                                            WHERE InventoryChangeTypeId = 9
                                                              AND InventoryControlDocumentId IS NOT NULL
                                                              AND ReasonCodeId IS NULL
                                                              AND LocationID = @locationID
                                                              AND ChangeDate BETWEEN @date AND @enddate", opConn);

                        SqlParameter date = new SqlParameter();
                        date.ParameterName = "@date";
                        date.Value = dtDateRange.Value.ToString("yyyyMMdd");

                        SqlParameter enddate = new SqlParameter();
                        enddate.ParameterName = "@enddate";
                        enddate.Value = dtDateEnd.Value.ToString("yyyyMMdd");

                        SqlParameter location = new SqlParameter();
                        location.ParameterName = "@locationID ";
                        location.Value = cmbLocation.SelectedValue;

                        // 2. add new parameter to command object
                        cmd.Parameters.Add(date);
                        cmd.Parameters.Add(enddate);
                        cmd.Parameters.Add(location);


                        opConn.Open();

                        // get an instance of the SqlDataReader
                        rdr = cmd.ExecuteReader();

                        rdr.Read();

                        int recordCount = (int)rdr["recordCount"];

                        //MessageBox.Show("The record count is " + recordCount);

                        if (rdr != null) { rdr.Close(); }
                        cmd.Parameters.Clear();

                        //handle null reason codes in inventory change log
                        if (recordCount > 0)
                        {


                            //check for default reason code and use to update null records if it exists
                            SqlCommand chkDefaultReasonCode = new SqlCommand("select ISNULL(DefaultInventoryTransferReasoncodeID,0) AS  defaultCode from Company", opConn);

                            // get an instance of the SqlDataReader
                            rdr = chkDefaultReasonCode.ExecuteReader();

                            rdr.Read();


                            int defaultReasonCode = (int)rdr["defaultCode"];

                            //MessageBox.Show("The default reason code is " + defaultReasonCode);

                            if (rdr != null) { rdr.Close(); }

                            if (defaultReasonCode != 0)
                            {
                                //update null records to use default reason code
                                SqlCommand updateReasonCodes = new SqlCommand(@" UPDATE dbo.InventoryChangeLog
                                                                                  SET ReasonCodeId = @defaultReasonCode
                                                                                WHERE InventoryChangeTypeId = 9
                                                                                  AND InventoryControlDocumentId IS NOT NULL
                                                                                  AND ReasonCodeId IS NULL
                                                                                  AND LocationID = @locationID
                                                                                  AND ChangeDate BETWEEN @date AND @enddate", opConn);


                                SqlParameter reasonCode = new SqlParameter();
                                reasonCode.ParameterName = "@defaultReasonCode";
                                reasonCode.Value = defaultReasonCode;

                                // 2. add new parameter to command object
                                updateReasonCodes.Parameters.Add(date);
                                updateReasonCodes.Parameters.Add(enddate);
                                updateReasonCodes.Parameters.Add(location);
                                updateReasonCodes.Parameters.Add(reasonCode);

                                updateReasonCodes.ExecuteNonQuery();

                                MessageBox.Show("Null values found for ReasonCode column. Updated to use default reason code");
                            }

                            else
                            {
                                //MessageBox.Show("No default reason code");
                                //check for any valid reason codes that can be used to populate the default

                                //NOTE * *type is 2(Inventory Main Manual Adjust)
                                SqlCommand chkValidReasonCode = new SqlCommand("select COUNT(ReasonCodeId) AS validCodes from reasoncode where reasoncodetypeID = 2", opConn);

                                rdr = chkValidReasonCode.ExecuteReader();

                                rdr.Read();


                                int validReasonCodes = (int)rdr["validCodes"];

                                if (rdr != null) { rdr.Close(); }

                                if (validReasonCodes > 0)
                                {
                                    //select new default reason code
                                    MessageBox.Show("There were Null values in the reason code columns " +
                                                    "for the records in the date range selected, " +
                                                    "but no default reason code was found for the customer. " +
                                                    "Please select a reason code to use as the new default to continue.", "Info", MessageBoxButtons.OK);

                                    var form = new frmReasonCodeSelection(opsuiteConnectionString, cmbCustomerSelect.Text);
                                    form.ShowDialog();
                                    //default reason code selected but rows not updated to use reason code yet need to click resend button again
                                    invalidState = true;

                                }

                                else
                                {
                                    //no valid reason codes found one must be created in Opsuite and then the update should be attempted again

                                    //select new default reason code
                                    MessageBox.Show("There were no valid reason codes found that could be used as the default. "
                                                    + "Login to Opsuite and create one then try to resend again.", "Info", MessageBoxButtons.OK);

                                    invalidState = true;
                                }


                            }

                        }



                        if (chkHealingAdjustment.Checked == true && !invalidState)
                        {
                            DialogResult result = MessageBox.Show("Healing Adjustment Box Checked."
                                                                + " Only the item movement will be sent down."
                                                                + " Item quantities will not be updated. Proceed?", "Info", MessageBoxButtons.YesNo);

                            if (result == DialogResult.Yes)
                            {
                                try
                                {
                                    // Open the connection
                                    conn.Open();

                                    //clear previous parameters associated with this command
                                    cmd.Parameters.Clear();

                                    cmd = new SqlCommand(
                                        @"	BEGIN;
	                                            ;WITH cte_CHANGELOG AS 
	                                            (
	                                            SELECT DISTINCT 'Send' AS ChangeType ,
					                                             InventoryControlDocumentId AS RecordID,
					                                            'HealQuantityAdjustment' AS MessageType,
					                                            '1.0' AS MessageTypeVersion,
					                                             1 AS [Priority],
					                                             LocationID
			                                               FROM dbo.synInventoryChangeLog
			                                              WHERE InventoryChangeTypeId = 9
				                                            AND InventoryControlDocumentId IS NOT NULL
				                                            AND ReasonCodeId IS NOT NULL
				                                            AND LocationID = @locationID
				                                            AND ChangeDate BETWEEN @date AND @enddate
	                                            ) 
										
	                                            INSERT INTO dbo.posecIdb_ReferredChangeLog
			                                            ( ChangeID ,
				                                            ChangeType ,
				                                            RecordID ,
				                                            MessageType ,
				                                            MessageTypeVersion ,
				                                            AppliesTo ,
				                                            Priority ,
				                                            InsertedDT ,
				                                            Actioned ,
				                                            GroupingId
			                                            )


	                                            SELECT  NEWID() AS ChangeID,
			                                            ICL.ChangeType ,
			                                            ICL.RecordID ,
			                                            ICL.MessageType,
			                                            ICL.MessageTypeVersion ,
			                                            RTI.IPAddressOrDomainName AS AppliesTo ,
			                                            ICL.[Priority] ,
			                                            GETDATE() AS InsertedDT,
			                                            0 AS Actioned,
			                                            0 AS GroupingId
	                                            FROM    cte_CHANGELOG AS  ICL
			                                            INNER JOIN dbo.synRtiStoreAllocation RTI ON ICL.LocationId = RTI.LocationId 
												

                                            END;", conn);


                                    date = new SqlParameter();
                                    date.ParameterName = "@date";
                                    date.Value = dtDateRange.Value.ToString("yyyyMMdd");

                                    enddate = new SqlParameter();
                                    enddate.ParameterName = "@enddate";
                                    enddate.Value = dtDateEnd.Value.ToString("yyyyMMdd");

                                    location = new SqlParameter();
                                    location.ParameterName = "@locationID ";
                                    location.Value = cmbLocation.SelectedValue;

                                    // 2. add new parameter to command object
                                    cmd.Parameters.Add(date);
                                    cmd.Parameters.Add(enddate);
                                    cmd.Parameters.Add(location);

                                    //Execute the query
                                    cmd.ExecuteNonQuery();

                                }

                                finally
                                {
                                    // Close the connection
                                    if (conn != null)
                                    {
                                        conn.Close();
                                    }
                                }

                                //Processing for manual inventory adjustment with healing message for referred changelog
                                MessageBox.Show("Healing Adjustment Sent.");
                            }

                            else
                            {
                                MessageBox.Show("Healing Adjustment Canceled.");
                            }

                        }

                        else if (chkHealingAdjustment.Checked == false && !invalidState)
                        {
                            DialogResult result = MessageBox.Show(" Physical inventory adjustment will be sent down to the store."
                                    + " Proceed?", "Info", MessageBoxButtons.YesNo);

                            if (result == DialogResult.Yes)
                            {
                                try
                                {
                                    // Open the connection
                                    conn.Open();

                                    //clear previous parameters associated with this command
                                    cmd.Parameters.Clear();

                                    cmd = new SqlCommand(
                                       @"	BEGIN;
	                                            ;WITH cte_CHANGELOG AS 
	                                            (
	                                            SELECT DISTINCT 'Send' AS ChangeType ,
					                                             InventoryControlDocumentId AS RecordID,
					                                            'QuantityAdjustment' AS MessageType,
					                                            '1.0' AS MessageTypeVersion,
					                                             1 AS [Priority],
					                                             LocationID
			                                               FROM dbo.synInventoryChangeLog
			                                              WHERE InventoryChangeTypeId = 9
				                                            AND InventoryControlDocumentId IS NOT NULL
				                                            AND ReasonCodeId IS NOT NULL
				                                            AND LocationID = @locationID
				                                            AND ChangeDate BETWEEN @date AND @enddate
	                                            ) 
										
	                                            INSERT INTO dbo.posecIdb_ReferredChangeLog
			                                            ( ChangeID ,
				                                            ChangeType ,
				                                            RecordID ,
				                                            MessageType ,
				                                            MessageTypeVersion ,
				                                            AppliesTo ,
				                                            Priority ,
				                                            InsertedDT ,
				                                            Actioned ,
				                                            GroupingId
			                                            )


	                                            SELECT  NEWID() AS ChangeID,
			                                            ICL.ChangeType ,
			                                            ICL.RecordID ,
			                                            ICL.MessageType,
			                                            ICL.MessageTypeVersion ,
			                                            RTI.IPAddressOrDomainName AS AppliesTo ,
			                                            ICL.[Priority] ,
			                                            GETDATE() AS InsertedDT,
			                                            0 AS Actioned,
			                                            0 AS GroupingId
	                                            FROM    cte_CHANGELOG AS  ICL
			                                            INNER JOIN dbo.synRtiStoreAllocation RTI ON ICL.LocationId = RTI.LocationId 
												

                                            END;", conn);

                                    date = new SqlParameter();
                                    date.ParameterName = "@date";
                                    date.Value = dtDateRange.Value.ToString("yyyyMMdd");

                                    enddate = new SqlParameter();
                                    enddate.ParameterName = "@enddate";
                                    enddate.Value = dtDateEnd.Value.ToString("yyyyMMdd");

                                    location = new SqlParameter();
                                    location.ParameterName = "@locationID ";
                                    location.Value = cmbLocation.SelectedValue;

                                    // 2. add new parameter to command object
                                    cmd.Parameters.Add(date);
                                    cmd.Parameters.Add(enddate);
                                    cmd.Parameters.Add(location);

                                    //Execute the query
                                    cmd.ExecuteNonQuery();

                                }

                                finally
                                {
                                    // Close the connection
                                    if (conn != null)
                                    {
                                        conn.Close();
                                    }
                                }

                                //Normal processing for manual inventory adjustment
                                MessageBox.Show("Physical inventory adjustment sent.");
                            }

                            else
                            {
                                //Normal processing for manual inventory adjustment
                                MessageBox.Show("Physical inventory adjustment send canceled.");
                            }

                        }
                    }

                    finally
                    {
                        // Close the connection
                        if (conn != null)
                        {
                            opConn.Close();
                        }
                    }

                }

                else
                {
                    MessageBox.Show("End Date must be >= to Start Date");
                }

            }

            //Purchase Order to Work Order
            else if (cmbDocSelect.SelectedIndex == 5 && cmbCustomerSelect.SelectedIndex > -1)
            {

                int InventoryControlDocumentID = -1;

                try
                {
                    // Open the connection
                    conn.Open();

                    // 1. Instantiate a new command
                    SqlCommand cmd = new SqlCommand(
                        @"SELECT InventoryControlDocumentID
							FROM dbo.synInventoryControlDocument 
						   WHERE OrderNumber = @ordernumber
							 AND ICDTransferTypeId = 0", conn);


                    SqlParameter param = new SqlParameter();
                    param.ParameterName = "@orderNumber";
                    param.Value = txtOrderNumber.Text;

                    // 2. add new parameter to command object
                    cmd.Parameters.Add(param);

                    // 3. Call ExecuteScalar to send command
                    //Returns object with null reference if empty result set
                    //use convert to int 32 to convert null to 0
                    InventoryControlDocumentID = Convert.ToInt32(cmd.ExecuteScalar());

                    if (InventoryControlDocumentID > 0)
                    {

                        cmd = new SqlCommand(
                            @"	BEGIN;
								INSERT INTO dbo.posecIdb_ReferredChangeLog
										( ChangeID ,
										  ChangeType ,
										  RecordID ,
										  MessageType ,
										  MessageTypeVersion ,
										  AppliesTo ,
										  Priority ,
										  InsertedDT ,
										  Actioned ,
										  GroupingId
										)
								SELECT  NEWID() ,
										'Send' ,
										ICD.InventoryControlDocumentId ,
										'PurchaseOrderToWorkOrder' ,
										'1.0' ,
										RTI.IPAddressOrDomainName ,
										1 ,
										GETDATE() ,
										0 ,
										0
								FROM    dbo.synInventoryControlDocument ICD
										LEFT JOIN dbo.synRtiStoreAllocation RTI ON ICD.OriginatorLocationId = RTI.LocationId
										WHERE ICD.InventoryControlDocumentId = @InventoryControlDocumentID
										AND RTI.LocationId IS NOT NULL
								END;", conn);

                        param = new SqlParameter();
                        param.ParameterName = "@InventoryControlDocumentID";
                        param.Value = InventoryControlDocumentID;

                        cmd.Parameters.Add(param);

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("The PO " + txtOrderNumber + " exists and was sent to the store as a work order.");

                    }
                    else
                    {
                        MessageBox.Show("The PO " + txtOrderNumber + " was not found.");
                    }
                }
                finally
                {
                    // Close the connection
                    if (conn != null)
                    {
                        conn.Close();
                    }
                }

            }


            //No customer or doc type selected
            else
            {
                MessageBox.Show("Please select a customer, location, and document type before resending an order.");
            }
        }

      
    }
}
