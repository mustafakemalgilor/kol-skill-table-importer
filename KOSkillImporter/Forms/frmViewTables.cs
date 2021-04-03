/**
 * ______________________________________________________
 * This file is part of ko-skill-table-importer project.
 * 
 * @author       Mustafa Kemal Gılor <mustafagilor@gmail.com> (2016)
 * .
 * SPDX-License-Identifier:	MIT
 * ______________________________________________________
 */

using System;
using System.Data;
using System.Windows.Forms;

namespace KOSkillImporter.Forms
{
    public partial class frmViewTables : Form
    {
        public frmViewTables(DataSet mySet)
        {
            InitializeComponent();
            DGVTBL.DataSource = mySet;
        }

        private void frmViewTables_Load(object sender, EventArgs e)
        {
            cbTable.SelectedIndex = 0;
        }

        private void cbTable_SelectedIndexChanged(object sender, EventArgs e)
        {

            DGVTBL.SuspendLayout();
          /* DGVTBL.DataMember = "skillTable"+cbTable.SelectedIndex;
            DG
            DGVTBL.DataMember = cbTable.Text;*/
            DGVTBL.ResumeLayout();
            Text = "Viewing table content - " + cbTable.Text;
        }
    }
}
