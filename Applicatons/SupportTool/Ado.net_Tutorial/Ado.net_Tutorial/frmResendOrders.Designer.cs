namespace Ado.net_Tutorial
{
    partial class frmResendOrders
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.cmbCustomerSelect = new System.Windows.Forms.ComboBox();
            this.cmbLocation = new System.Windows.Forms.ComboBox();
            this.lblCustSelect = new System.Windows.Forms.Label();
            this.lblLocations = new System.Windows.Forms.Label();
            this.txtOrderNumber = new System.Windows.Forms.TextBox();
            this.lblOrderNumber = new System.Windows.Forms.Label();
            this.btnGo = new System.Windows.Forms.Button();
            this.dgvOrderSearch = new System.Windows.Forms.DataGridView();
            this.rbtSingleOrder = new System.Windows.Forms.RadioButton();
            this.rbtDateRange = new System.Windows.Forms.RadioButton();
            this.dtDateRange = new System.Windows.Forms.DateTimePicker();
            this.btnResend = new System.Windows.Forms.Button();
            this.cmbDocSelect = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chkHealingAdjustment = new System.Windows.Forms.CheckBox();
            this.lblStartDate = new System.Windows.Forms.Label();
            this.dtDateEnd = new System.Windows.Forms.DateTimePicker();
            this.lblEndDate = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrderSearch)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbCustomerSelect
            // 
            this.cmbCustomerSelect.FormattingEnabled = true;
            this.cmbCustomerSelect.Location = new System.Drawing.Point(23, 64);
            this.cmbCustomerSelect.Name = "cmbCustomerSelect";
            this.cmbCustomerSelect.Size = new System.Drawing.Size(220, 21);
            this.cmbCustomerSelect.TabIndex = 0;
            // 
            // cmbLocation
            // 
            this.cmbLocation.FormattingEnabled = true;
            this.cmbLocation.Location = new System.Drawing.Point(329, 64);
            this.cmbLocation.Name = "cmbLocation";
            this.cmbLocation.Size = new System.Drawing.Size(200, 21);
            this.cmbLocation.TabIndex = 2;
            // 
            // lblCustSelect
            // 
            this.lblCustSelect.AutoSize = true;
            this.lblCustSelect.Location = new System.Drawing.Point(23, 42);
            this.lblCustSelect.Name = "lblCustSelect";
            this.lblCustSelect.Size = new System.Drawing.Size(98, 13);
            this.lblCustSelect.TabIndex = 3;
            this.lblCustSelect.Text = "Customer Selection";
            // 
            // lblLocations
            // 
            this.lblLocations.AutoSize = true;
            this.lblLocations.Location = new System.Drawing.Point(329, 42);
            this.lblLocations.Name = "lblLocations";
            this.lblLocations.Size = new System.Drawing.Size(53, 13);
            this.lblLocations.TabIndex = 4;
            this.lblLocations.Text = "Locations";
            // 
            // txtOrderNumber
            // 
            this.txtOrderNumber.Location = new System.Drawing.Point(23, 163);
            this.txtOrderNumber.Name = "txtOrderNumber";
            this.txtOrderNumber.Size = new System.Drawing.Size(230, 20);
            this.txtOrderNumber.TabIndex = 5;
            this.txtOrderNumber.Text = "Choose A Customer";
            this.txtOrderNumber.Enter += new System.EventHandler(this.txtOrderNumber_Enter);
            // 
            // lblOrderNumber
            // 
            this.lblOrderNumber.AutoSize = true;
            this.lblOrderNumber.Location = new System.Drawing.Point(23, 145);
            this.lblOrderNumber.Name = "lblOrderNumber";
            this.lblOrderNumber.Size = new System.Drawing.Size(73, 13);
            this.lblOrderNumber.TabIndex = 6;
            this.lblOrderNumber.Text = "Order Number";
            // 
            // btnGo
            // 
            this.btnGo.Location = new System.Drawing.Point(23, 216);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(142, 23);
            this.btnGo.TabIndex = 7;
            this.btnGo.Text = "Order Search";
            this.btnGo.UseVisualStyleBackColor = true;
            this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // dgvOrderSearch
            // 
            this.dgvOrderSearch.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvOrderSearch.Location = new System.Drawing.Point(23, 262);
            this.dgvOrderSearch.Name = "dgvOrderSearch";
            this.dgvOrderSearch.Size = new System.Drawing.Size(1094, 215);
            this.dgvOrderSearch.TabIndex = 8;
            // 
            // rbtSingleOrder
            // 
            this.rbtSingleOrder.AutoSize = true;
            this.rbtSingleOrder.Checked = true;
            this.rbtSingleOrder.Location = new System.Drawing.Point(332, 169);
            this.rbtSingleOrder.Name = "rbtSingleOrder";
            this.rbtSingleOrder.Size = new System.Drawing.Size(83, 17);
            this.rbtSingleOrder.TabIndex = 9;
            this.rbtSingleOrder.TabStop = true;
            this.rbtSingleOrder.Text = "Single Order";
            this.rbtSingleOrder.UseVisualStyleBackColor = true;
            this.rbtSingleOrder.CheckedChanged += new System.EventHandler(this.rbtSingleOrder_CheckedChanged);
            // 
            // rbtDateRange
            // 
            this.rbtDateRange.AutoSize = true;
            this.rbtDateRange.Location = new System.Drawing.Point(449, 169);
            this.rbtDateRange.Name = "rbtDateRange";
            this.rbtDateRange.Size = new System.Drawing.Size(83, 17);
            this.rbtDateRange.TabIndex = 10;
            this.rbtDateRange.TabStop = true;
            this.rbtDateRange.Text = "Date Range";
            this.rbtDateRange.UseVisualStyleBackColor = true;
            this.rbtDateRange.CheckedChanged += new System.EventHandler(this.rbtDateRange_CheckedChanged);
            // 
            // dtDateRange
            // 
            this.dtDateRange.Location = new System.Drawing.Point(329, 215);
            this.dtDateRange.Name = "dtDateRange";
            this.dtDateRange.Size = new System.Drawing.Size(200, 20);
            this.dtDateRange.TabIndex = 11;
            // 
            // btnResend
            // 
            this.btnResend.Location = new System.Drawing.Point(23, 503);
            this.btnResend.Name = "btnResend";
            this.btnResend.Size = new System.Drawing.Size(123, 23);
            this.btnResend.TabIndex = 12;
            this.btnResend.Text = "Resend Order(s)";
            this.btnResend.UseVisualStyleBackColor = true;
            this.btnResend.Click += new System.EventHandler(this.btnResend_Click);
            // 
            // cmbDocSelect
            // 
            this.cmbDocSelect.FormattingEnabled = true;
            this.cmbDocSelect.Items.AddRange(new object[] {
            "Select A Doc Type",
            "Purchase Order",
            "Voucher",
            "Manual Inventory Adjustment",
            "Physical Inventory Adjustment",
            "Purchase Order To Work Order"});
            this.cmbDocSelect.Location = new System.Drawing.Point(329, 115);
            this.cmbDocSelect.Name = "cmbDocSelect";
            this.cmbDocSelect.Size = new System.Drawing.Size(200, 21);
            this.cmbDocSelect.TabIndex = 13;
   
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(330, 96);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "Document Type";
            // 
            // chkHealingAdjustment
            // 
            this.chkHealingAdjustment.AutoSize = true;
            this.chkHealingAdjustment.Location = new System.Drawing.Point(589, 118);
            this.chkHealingAdjustment.Name = "chkHealingAdjustment";
            this.chkHealingAdjustment.Size = new System.Drawing.Size(200, 17);
            this.chkHealingAdjustment.TabIndex = 15;
            this.chkHealingAdjustment.Text = "Healing Adjustment (Movement Only)";
            this.chkHealingAdjustment.UseVisualStyleBackColor = true;
            // 
            // lblStartDate
            // 
            this.lblStartDate.AutoSize = true;
            this.lblStartDate.Location = new System.Drawing.Point(328, 196);
            this.lblStartDate.Name = "lblStartDate";
            this.lblStartDate.Size = new System.Drawing.Size(55, 13);
            this.lblStartDate.TabIndex = 16;
            this.lblStartDate.Text = "Start Date";
            // 
            // dtDateEnd
            // 
            this.dtDateEnd.Location = new System.Drawing.Point(589, 216);
            this.dtDateEnd.Name = "dtDateEnd";
            this.dtDateEnd.Size = new System.Drawing.Size(200, 20);
            this.dtDateEnd.TabIndex = 17;
            // 
            // lblEndDate
            // 
            this.lblEndDate.AutoSize = true;
            this.lblEndDate.Location = new System.Drawing.Point(589, 196);
            this.lblEndDate.Name = "lblEndDate";
            this.lblEndDate.Size = new System.Drawing.Size(52, 13);
            this.lblEndDate.TabIndex = 18;
            this.lblEndDate.Text = "End Date";
            // 
            // frmResendOrders
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1155, 538);
            this.Controls.Add(this.lblEndDate);
            this.Controls.Add(this.dtDateEnd);
            this.Controls.Add(this.lblStartDate);
            this.Controls.Add(this.chkHealingAdjustment);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbDocSelect);
            this.Controls.Add(this.btnResend);
            this.Controls.Add(this.dtDateRange);
            this.Controls.Add(this.rbtDateRange);
            this.Controls.Add(this.rbtSingleOrder);
            this.Controls.Add(this.dgvOrderSearch);
            this.Controls.Add(this.btnGo);
            this.Controls.Add(this.lblOrderNumber);
            this.Controls.Add(this.txtOrderNumber);
            this.Controls.Add(this.lblLocations);
            this.Controls.Add(this.lblCustSelect);
            this.Controls.Add(this.cmbLocation);
            this.Controls.Add(this.cmbCustomerSelect);
            this.Name = "frmResendOrders";
            this.Text = "OpSuite Support - Voucher/PO";
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrderSearch)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbCustomerSelect;
        private System.Windows.Forms.ComboBox cmbLocation;
        private System.Windows.Forms.Label lblCustSelect;
        private System.Windows.Forms.Label lblLocations;
        private System.Windows.Forms.TextBox txtOrderNumber;
        private System.Windows.Forms.Label lblOrderNumber;
        private System.Windows.Forms.Button btnGo;
        private System.Windows.Forms.DataGridView dgvOrderSearch;
        private System.Windows.Forms.RadioButton rbtSingleOrder;
        private System.Windows.Forms.RadioButton rbtDateRange;
        private System.Windows.Forms.DateTimePicker dtDateRange;
        private System.Windows.Forms.Button btnResend;
        private System.Windows.Forms.ComboBox cmbDocSelect;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkHealingAdjustment;
        private System.Windows.Forms.Label lblStartDate;
        private System.Windows.Forms.DateTimePicker dtDateEnd;
        private System.Windows.Forms.Label lblEndDate;
    }
}

