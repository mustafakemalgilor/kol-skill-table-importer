namespace KOSkillImporter.Forms
{
    partial class frmViewTables
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
            this.DGVTBL = new System.Windows.Forms.DataGridView();
            this.cbTable = new System.Windows.Forms.ComboBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.DGVTBL)).BeginInit();
            this.SuspendLayout();
            // 
            // DGVTBL
            // 
            this.DGVTBL.AllowUserToAddRows = false;
            this.DGVTBL.AllowUserToDeleteRows = false;
            this.DGVTBL.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGVTBL.Location = new System.Drawing.Point(12, 12);
            this.DGVTBL.Name = "DGVTBL";
            this.DGVTBL.ReadOnly = true;
            this.DGVTBL.Size = new System.Drawing.Size(1115, 519);
            this.DGVTBL.TabIndex = 0;
            // 
            // cbTable
            // 
            this.cbTable.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTable.FormattingEnabled = true;
            this.cbTable.Items.AddRange(new object[] {
            "skill_magic_main_us.tbl",
            "Skill_Magic_1.tbl",
            "Skill_Magic_2.tbl",
            "Skill_Magic_3.tbl",
            "Skill_Magic_4.tbl",
            "Skill_Magic_5.tbl",
            "Skill_Magic_6.tbl",
            "Skill_Magic_7.tbl",
            "Skill_Magic_8.tbl",
            "Skill_Magic_9.tbl"});
            this.cbTable.Location = new System.Drawing.Point(104, 537);
            this.cbTable.Name = "cbTable";
            this.cbTable.Size = new System.Drawing.Size(230, 21);
            this.cbTable.TabIndex = 1;
            this.cbTable.SelectedIndexChanged += new System.EventHandler(this.cbTable_SelectedIndexChanged);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(938, 535);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(189, 23);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Close View";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(38, 540);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Table";
            // 
            // frmViewTables
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1139, 579);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.cbTable);
            this.Controls.Add(this.DGVTBL);
            this.Name = "frmViewTables";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "View Table Content - skill_magic_main_us.tbl";
            this.Load += new System.EventHandler(this.frmViewTables_Load);
            ((System.ComponentModel.ISupportInitialize)(this.DGVTBL)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView DGVTBL;
        private System.Windows.Forms.ComboBox cbTable;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label label1;
    }
}