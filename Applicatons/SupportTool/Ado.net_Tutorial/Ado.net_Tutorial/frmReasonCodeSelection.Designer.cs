namespace Ado.net_Tutorial
{
    partial class frmReasonCodeSelection
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
            this.cmbReasonCodes = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblReasonCodeAdjust = new System.Windows.Forms.Label();
            this.btnReasonCodeUpdate = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cmbReasonCodes
            // 
            this.cmbReasonCodes.FormattingEnabled = true;
            this.cmbReasonCodes.Location = new System.Drawing.Point(51, 85);
            this.cmbReasonCodes.Name = "cmbReasonCodes";
            this.cmbReasonCodes.Size = new System.Drawing.Size(186, 21);
            this.cmbReasonCodes.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(58, 56);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(172, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Select a new default Reason Code";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblReasonCodeAdjust
            // 
            this.lblReasonCodeAdjust.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblReasonCodeAdjust.Location = new System.Drawing.Point(0, 0);
            this.lblReasonCodeAdjust.Name = "lblReasonCodeAdjust";
            this.lblReasonCodeAdjust.Size = new System.Drawing.Size(296, 23);
            this.lblReasonCodeAdjust.TabIndex = 2;
            this.lblReasonCodeAdjust.Text = "Company Name Inventory Adjustments";
            this.lblReasonCodeAdjust.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnReasonCodeUpdate
            // 
            this.btnReasonCodeUpdate.Location = new System.Drawing.Point(102, 135);
            this.btnReasonCodeUpdate.Name = "btnReasonCodeUpdate";
            this.btnReasonCodeUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnReasonCodeUpdate.TabIndex = 3;
            this.btnReasonCodeUpdate.Text = "Update";
            this.btnReasonCodeUpdate.UseVisualStyleBackColor = true;
            this.btnReasonCodeUpdate.Click += new System.EventHandler(this.btnReasonCodeUpdate_Click);
            // 
            // frmReasonCodeSelection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(296, 182);
            this.Controls.Add(this.btnReasonCodeUpdate);
            this.Controls.Add(this.lblReasonCodeAdjust);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbReasonCodes);
            this.Name = "frmReasonCodeSelection";
            this.Text = "Reason Code Selection";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbReasonCodes;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblReasonCodeAdjust;
        private System.Windows.Forms.Button btnReasonCodeUpdate;
    }
}