namespace KOSkillImporter.Forms
{
    partial class MainFrm
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gbStatus = new System.Windows.Forms.GroupBox();
            this.btnView = new System.Windows.Forms.Button();
            this.btnImport = new System.Windows.Forms.Button();
            this.lbImportList = new System.Windows.Forms.ListBox();
            this.label9 = new System.Windows.Forms.Label();
            this.btnLoad = new System.Windows.Forms.Button();
            this.lbMagicTablesLoaded = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnConnect = new System.Windows.Forms.Button();
            this.lblConnStatus = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.tbServer = new System.Windows.Forms.TextBox();
            this.tbDBName = new System.Windows.Forms.TextBox();
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.tbUserID = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.gbDatabase = new System.Windows.Forms.GroupBox();
            this.gbIProcess = new System.Windows.Forms.GroupBox();
            this.pbOverall = new KOSkillImporter.Forms.CustomProgressBar();
            this.pbCurrent = new KOSkillImporter.Forms.CustomProgressBar();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.SelectFolderDlg = new System.Windows.Forms.FolderBrowserDialog();
            this.label8 = new System.Windows.Forms.Label();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.menuStrip1.SuspendLayout();
            this.gbStatus.SuspendLayout();
            this.gbDatabase.SuspendLayout();
            this.gbIProcess.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(620, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            this.menuStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.menuStrip1_ItemClicked);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // gbStatus
            // 
            this.gbStatus.Controls.Add(this.btnView);
            this.gbStatus.Controls.Add(this.btnImport);
            this.gbStatus.Controls.Add(this.lbImportList);
            this.gbStatus.Controls.Add(this.label9);
            this.gbStatus.Controls.Add(this.btnLoad);
            this.gbStatus.Controls.Add(this.lbMagicTablesLoaded);
            this.gbStatus.Controls.Add(this.label1);
            this.gbStatus.Enabled = false;
            this.gbStatus.Location = new System.Drawing.Point(269, 27);
            this.gbStatus.Name = "gbStatus";
            this.gbStatus.Size = new System.Drawing.Size(344, 234);
            this.gbStatus.TabIndex = 1;
            this.gbStatus.TabStop = false;
            this.gbStatus.Text = "Status";
            // 
            // btnView
            // 
            this.btnView.Enabled = false;
            this.btnView.Location = new System.Drawing.Point(149, 194);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(40, 23);
            this.btnView.TabIndex = 29;
            this.btnView.Text = "View";
            this.btnView.UseVisualStyleBackColor = true;
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // btnImport
            // 
            this.btnImport.Enabled = false;
            this.btnImport.Location = new System.Drawing.Point(195, 194);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(135, 23);
            this.btnImport.TabIndex = 27;
            this.btnImport.Text = "Load tables first!";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // lbImportList
            // 
            this.lbImportList.BackColor = System.Drawing.Color.Black;
            this.lbImportList.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.lbImportList.ForeColor = System.Drawing.Color.White;
            this.lbImportList.FormattingEnabled = true;
            this.lbImportList.Items.AddRange(new object[] {
            "MAGIC::[X]",
            "MAGIC_TYPE1::[X]",
            "MAGIC_TYPE2::[X]",
            "MAGIC_TYPE3::[X]",
            "MAGIC_TYPE4::[X]",
            "MAGIC_TYPE5::[X]",
            "MAGIC_TYPE6::[X]",
            "MAGIC_TYPE7::[X]",
            "MAGIC_TYPE8::[X]",
            "MAGIC_TYPE9::[X]"});
            this.lbImportList.Location = new System.Drawing.Point(195, 42);
            this.lbImportList.Name = "lbImportList";
            this.lbImportList.Size = new System.Drawing.Size(135, 147);
            this.lbImportList.TabIndex = 28;
            this.lbImportList.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lbMagicTablesLoaded_DrawItem);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(229, 21);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(69, 13);
            this.label9.TabIndex = 27;
            this.label9.Text = "Import Status";
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(12, 194);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(131, 23);
            this.btnLoad.TabIndex = 26;
            this.btnLoad.Text = "Load Magic Tables";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.button3_Click);
            // 
            // lbMagicTablesLoaded
            // 
            this.lbMagicTablesLoaded.BackColor = System.Drawing.Color.Black;
            this.lbMagicTablesLoaded.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.lbMagicTablesLoaded.ForeColor = System.Drawing.SystemColors.Info;
            this.lbMagicTablesLoaded.FormattingEnabled = true;
            this.lbMagicTablesLoaded.Items.AddRange(new object[] {
            "skill_magic_main_us.tbl::[X]",
            "skill_magic_1.tbl::[X]",
            "skill_magic_2.tbl::[X]",
            "skill_magic_3.tbl::[X]",
            "skill_magic_4.tbl::[X]",
            "skill_magic_5.tbl::[X]",
            "skill_magic_6.tbl::[X]",
            "skill_magic_7.tbl::[X]",
            "skill_magic_8.tbl::[X]",
            "skill_magic_9.tbl::[X]"});
            this.lbMagicTablesLoaded.Location = new System.Drawing.Point(9, 42);
            this.lbMagicTablesLoaded.Name = "lbMagicTablesLoaded";
            this.lbMagicTablesLoaded.Size = new System.Drawing.Size(180, 147);
            this.lbMagicTablesLoaded.TabIndex = 1;
            this.lbMagicTablesLoaded.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lbMagicTablesLoaded_DrawItem);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(9, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(180, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "Magic tables loaded";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(16, 165);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(220, 52);
            this.btnConnect.TabIndex = 25;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.button2_Click);
            // 
            // lblConnStatus
            // 
            this.lblConnStatus.ForeColor = System.Drawing.Color.Maroon;
            this.lblConnStatus.Location = new System.Drawing.Point(85, 133);
            this.lblConnStatus.Name = "lblConnStatus";
            this.lblConnStatus.Size = new System.Drawing.Size(146, 29);
            this.lblConnStatus.TabIndex = 24;
            this.lblConnStatus.Text = "Not connected yet";
            this.lblConnStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(6, 133);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(85, 29);
            this.label7.TabIndex = 23;
            this.label7.Text = "Connection Status";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tbServer
            // 
            this.tbServer.Location = new System.Drawing.Point(93, 110);
            this.tbServer.Name = "tbServer";
            this.tbServer.Size = new System.Drawing.Size(143, 20);
            this.tbServer.TabIndex = 22;
            this.tbServer.Text = "(local)";
            this.tbServer.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tbDBName
            // 
            this.tbDBName.Location = new System.Drawing.Point(93, 87);
            this.tbDBName.Name = "tbDBName";
            this.tbDBName.Size = new System.Drawing.Size(143, 20);
            this.tbDBName.TabIndex = 21;
            this.tbDBName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tbPassword
            // 
            this.tbPassword.Location = new System.Drawing.Point(93, 64);
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.Size = new System.Drawing.Size(143, 20);
            this.tbPassword.TabIndex = 20;
            this.tbPassword.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tbUserID
            // 
            this.tbUserID.Location = new System.Drawing.Point(93, 41);
            this.tbUserID.Name = "tbUserID";
            this.tbUserID.Size = new System.Drawing.Size(143, 20);
            this.tbUserID.TabIndex = 14;
            this.tbUserID.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(6, 108);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(85, 23);
            this.label6.TabIndex = 19;
            this.label6.Text = "Server";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(6, 85);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(85, 23);
            this.label5.TabIndex = 18;
            this.label5.Text = "DB Name";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(6, 62);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(85, 23);
            this.label4.TabIndex = 17;
            this.label4.Text = "Password";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(6, 39);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(85, 23);
            this.label3.TabIndex = 16;
            this.label3.Text = "User ID";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(6, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(225, 23);
            this.label2.TabIndex = 15;
            this.label2.Text = "Database";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // gbDatabase
            // 
            this.gbDatabase.Controls.Add(this.btnConnect);
            this.gbDatabase.Controls.Add(this.label2);
            this.gbDatabase.Controls.Add(this.label3);
            this.gbDatabase.Controls.Add(this.lblConnStatus);
            this.gbDatabase.Controls.Add(this.label4);
            this.gbDatabase.Controls.Add(this.label7);
            this.gbDatabase.Controls.Add(this.label5);
            this.gbDatabase.Controls.Add(this.tbServer);
            this.gbDatabase.Controls.Add(this.label6);
            this.gbDatabase.Controls.Add(this.tbDBName);
            this.gbDatabase.Controls.Add(this.tbUserID);
            this.gbDatabase.Controls.Add(this.tbPassword);
            this.gbDatabase.Location = new System.Drawing.Point(6, 27);
            this.gbDatabase.Name = "gbDatabase";
            this.gbDatabase.Size = new System.Drawing.Size(257, 234);
            this.gbDatabase.TabIndex = 26;
            this.gbDatabase.TabStop = false;
            this.gbDatabase.Text = "Database";
            // 
            // gbIProcess
            // 
            this.gbIProcess.Controls.Add(this.richTextBox1);
            this.gbIProcess.Controls.Add(this.label8);
            this.gbIProcess.Controls.Add(this.pbOverall);
            this.gbIProcess.Controls.Add(this.pbCurrent);
            this.gbIProcess.Controls.Add(this.btnCancel);
            this.gbIProcess.Controls.Add(this.label11);
            this.gbIProcess.Controls.Add(this.label10);
            this.gbIProcess.Location = new System.Drawing.Point(6, 267);
            this.gbIProcess.Name = "gbIProcess";
            this.gbIProcess.Size = new System.Drawing.Size(607, 285);
            this.gbIProcess.TabIndex = 27;
            this.gbIProcess.TabStop = false;
            this.gbIProcess.Text = "Import Progress";
            this.gbIProcess.Visible = false;
            // 
            // pbOverall
            // 
            this.pbOverall.CustomText = "7 table imported out of 11 ";
            this.pbOverall.DisplayStyle = KOSkillImporter.Forms.ProgressBarDisplayText.CustomText;
            this.pbOverall.Location = new System.Drawing.Point(6, 65);
            this.pbOverall.Name = "pbOverall";
            this.pbOverall.Size = new System.Drawing.Size(595, 15);
            this.pbOverall.Step = 1;
            this.pbOverall.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.pbOverall.TabIndex = 37;
            // 
            // pbCurrent
            // 
            this.pbCurrent.CustomText = "Importing 3252 entries into MAGIC_TYPE1";
            this.pbCurrent.DisplayStyle = KOSkillImporter.Forms.ProgressBarDisplayText.CustomText;
            this.pbCurrent.Location = new System.Drawing.Point(6, 32);
            this.pbCurrent.Name = "pbCurrent";
            this.pbCurrent.Size = new System.Drawing.Size(595, 15);
            this.pbCurrent.Step = 1;
            this.pbCurrent.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.pbCurrent.TabIndex = 36;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(198, 256);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(208, 23);
            this.btnCancel.TabIndex = 26;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(6, 49);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(595, 14);
            this.label11.TabIndex = 34;
            this.label11.Text = "Overall progress";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(6, 16);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(595, 14);
            this.label10.TabIndex = 33;
            this.label10.Text = "Current stage progress";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SelectFolderDlg
            // 
            this.SelectFolderDlg.Description = "Select Data folder that includes your skill tables";
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(6, 84);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(595, 14);
            this.label8.TabIndex = 39;
            this.label8.Text = "SQL Message Output";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(6, 101);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(595, 149);
            this.richTextBox1.TabIndex = 40;
            this.richTextBox1.Text = "";
            // 
            // MainFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(620, 560);
            this.Controls.Add(this.gbIProcess);
            this.Controls.Add(this.gbDatabase);
            this.Controls.Add(this.gbStatus);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainFrm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Knight OnLine Skill Importer :: Written by PENTAGRAM";
            this.Load += new System.EventHandler(this.MainFrm_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.gbStatus.ResumeLayout(false);
            this.gbStatus.PerformLayout();
            this.gbDatabase.ResumeLayout(false);
            this.gbDatabase.PerformLayout();
            this.gbIProcess.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.GroupBox gbStatus;
        private System.Windows.Forms.ListBox lbMagicTablesLoaded;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox lbImportList;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Label lblConnStatus;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tbServer;
        private System.Windows.Forms.TextBox tbDBName;
        private System.Windows.Forms.TextBox tbPassword;
        private System.Windows.Forms.TextBox tbUserID;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox gbDatabase;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        public System.Windows.Forms.GroupBox gbIProcess;
        private CustomProgressBar pbCurrent;
        private CustomProgressBar pbOverall;
        private System.Windows.Forms.FolderBrowserDialog SelectFolderDlg;
        private System.Windows.Forms.Button btnView;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.RichTextBox richTextBox1;

    }
}

