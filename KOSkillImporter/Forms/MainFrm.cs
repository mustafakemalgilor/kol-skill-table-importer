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
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using KOSkillImporter.Classes;
using KOSkillImporter.Properties;
using System.Threading;

namespace KOSkillImporter.Forms
{
    public partial class MainFrm : Form
    {

        // adjust for v2153 tables
        bool v2153 = true;

        #region UI
        private void lbMagicTablesLoaded_DrawItem(object sender, DrawItemEventArgs e)
        {
            var list = (ListBox)sender;
            if (e.Index > -1)
            {
                var item = list.Items[e.Index];
                e.DrawBackground();
                e.DrawFocusRectangle();
                Brush brush;
                var myFont = new Font(e.Font, FontStyle.Strikeout);
                bool useStrikeOut = false;
                switch (Regex.Split(item.ToString(), "::")[1])
                {
                    case "[OK]":
                        brush = new SolidBrush(Color.Lime);
                        break;
                    case "[L]":
                        brush = new SolidBrush(Color.DeepSkyBlue);
                        break;
                    case "[X]":
                        brush = new SolidBrush(Color.Firebrick);
                        break;
                    case "[I]":
                        brush = new SolidBrush(Color.Gold);
                        break;
                    case "[E]":
                        brush = new SolidBrush(Color.Firebrick);
                        useStrikeOut = true;
                        break;
                    default:
                        brush = new SolidBrush(e.ForeColor);
                        break;
                }
                var size = e.Graphics.MeasureString(item.ToString(), e.Font);
                e.Graphics.DrawString(item.ToString(), useStrikeOut ? myFont : e.Font, brush, x: e.Bounds.Left + (e.Bounds.Width / 2 - size.Width / 2), y: e.Bounds.Top + (e.Bounds.Height / 2 - size.Height / 2));
            }
        }
        #endregion

        internal readonly DataSet skillTableSet = new DataSet();
        private readonly List<byte> IsTableExists = new List<byte>();
        private const string extension = "us";
        private readonly EncryptionKOStandard EncDec = new EncryptionKOStandard();
        private readonly List<string> MAGICQuery = new List<string>();
        private readonly List<List<string>> MAGICExtQueries = new List<List<string>>();
        private readonly ManualResetEvent queryisReady = new ManualResetEvent(false);

        private readonly List<string> FlyingEffectSkillIDs;
        /* 0 = skill magic main, others are ext */


        private byte[] LoadAndDecodeFile(string fileName)
        {
            if (fileName.ToLower().Contains("skill_magic_main_" + extension + ".tbl"))
            {
                lbMagicTablesLoaded.Items[0] = "skill_magic_main_" + extension + ".tbl::[L]";
            }
            for (byte i = 1; i <= 9; i++)
            {
                if (fileName.ToLower().Contains("skill_magic_" + i + ".tbl"))
                {
                    lbMagicTablesLoaded.Items[i] = "skill_magic_" + i + ".tbl::[L]";
                }
            }
            using (var stream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                byte[] buffer = null;
                int offset = 0;
                buffer = new byte[stream.Length];
                while (offset < stream.Length)
                {
                    offset += stream.Read(buffer, offset, ((int)stream.Length) - offset);
                }
                stream.Close();
                EncDec.Decode(ref buffer);
                return buffer;
            }
        }

        private byte index = 0;
        private void LoadByteDataIntoView(byte[] fileData,string Name)
        {
            int startIndex = 0;
            int num2 = BitConverter.ToInt32(fileData, startIndex);
            string tablename = "unknowntable";
            if (Name.ToLower().Contains("skill_magic_main_" + extension + ".tbl"))
            {
                tablename = "skill_magic_main_" + extension + ".tbl";
            }
            for (byte i = 1; i <= 9; i++)
            {
                if (Name.ToLower().Contains("skill_magic_" + i + ".tbl"))
                {
                    tablename = "skill_magic_" + i + ".tbl";
                }
            }
            if ((num2 < 0) || ((num2 * 4) > fileData.Length))
            {
                if (Name.ToLower().Contains("skill_magic_main_" + extension + ".tbl"))
                {
                    lbMagicTablesLoaded.Items[0] = "skill_magic_main_" + extension + ".tbl::[E]";
                }
                for (byte i = 1; i <= 9; i++)
                {
                    if (Name.ToLower().Contains("skill_magic_" + i + ".tbl"))
                    {
                        lbMagicTablesLoaded.Items[i] = "skill_magic_" + i + ".tbl::[E]";
                    }
                }
                return;/*fail*/
            }
            startIndex += 4;
            var numArray = new int[num2];
            var set = new DataSet("tableDataSet");
            var table = new DataTable(tablename);
            for (int i = 0; i < num2; i++)
            {
                DataColumn column;
                int num4 = BitConverter.ToInt32(fileData, startIndex);
                numArray[i] = num4;
                string prefix = i.ToString(CultureInfo.InvariantCulture);
                switch (num4)
                {
                    case 1:
                        column = new DataColumn(prefix + "\n(Signed Byte)", typeof(sbyte))
                        {
                            DefaultValue = (sbyte)0
                        };
                        break;

                    case 2:
                        column = new DataColumn(prefix + "\n(Byte)", typeof(byte))
                        {
                            DefaultValue = (byte)0
                        };
                        break;

                    case 3:
                        column = new DataColumn(prefix + "\n(Int16)", typeof(short))
                        {
                            DefaultValue = (short)0
                        };
                        break;

                    case 5:
                        column = new DataColumn(prefix + "\n(Int32)", typeof(int))
                        {
                            DefaultValue = 0
                        };
                        break;

                    case 6:
                        column = new DataColumn(prefix + "\n(UInt32)", typeof(uint))
                        {
                            DefaultValue = 0
                        };
                        break;

                    case 7:
                        column = new DataColumn(prefix + "\n(String)", typeof(string))
                        {
                            DefaultValue = ""
                        };
                        break;

                    case 8:
                        column = new DataColumn(prefix + "\n(Float)", typeof(float))
                        {
                            DefaultValue = 0f
                        };
                        break;

                    default:
                        column = new DataColumn(prefix + "\n(Unknown) " + num4.ToString(CultureInfo.InvariantCulture))
                        {
                            DefaultValue = 0
                        };
                        break;
                }
                table.Columns.Add(column);
                startIndex += 4;
            }
            
            int num5 = BitConverter.ToInt32(fileData, startIndex);
            startIndex += 4;
            for (int j = 0; (j < num5) && (startIndex < fileData.Length); j++)
            {
                DataRow row = table.NewRow();
                for (int k = 0; (k < num2) && (startIndex < fileData.Length); k++)
                {
                    int num8;
                    switch (numArray[k])
                    {
                        case 1:
                            {
                                row[k] = (fileData[startIndex] > 0x7f)
                                             ? (fileData[startIndex] - 0x100)
                                             : fileData[startIndex];
                                startIndex++;
                                continue;
                            }
                        case 2:
                            {
                                row[k] = fileData[startIndex];
                                startIndex++;
                                continue;
                            }
                        case 3:
                            {
                                row[k] = BitConverter.ToInt16(fileData, startIndex);
                                startIndex += 2;
                                continue;
                            }
                        case 5:
                            {
                                row[k] = BitConverter.ToInt32(fileData, startIndex);
                                startIndex += 4;
                                continue;
                            }
                        case 6:
                            {
                                row[k] = BitConverter.ToUInt32(fileData, startIndex);
                                startIndex += 4;
                                continue;
                            }
                        case 7:
                            {
                                num8 = BitConverter.ToInt32(fileData, startIndex);
                                startIndex += 4;
                                if (num8 > 0)
                                {
                                    break;
                                }
                                continue;
                            }
                        case 8:
                            {
                                row[k] = BitConverter.ToSingle(fileData, startIndex);
                                startIndex += 4;
                                continue;
                            }
                        default:
                            goto Label_03F5;
                    }
                    var chArray = new char[num8];
                    for (int m = 0; m < num8; m++)
                    {
                        chArray[m] = (char)fileData[startIndex];
                        startIndex++;
                    }
                    row[k] = new string(chArray);
                    continue;
                Label_03F5:
                    row[k] = BitConverter.ToInt32(fileData, startIndex);
                    startIndex += 4;
                }
                table.Rows.Add(row);
            }
            if (Name.ToLower().Contains("skill_magic_main_" + extension + ".tbl"))
            {
                lbMagicTablesLoaded.Items[0] = "skill_magic_main_" + extension + ".tbl::[OK]";
            }
            for (byte i = 1; i <= 9; i++)
            {
                if (Name.ToLower().Contains("skill_magic_" + i + ".tbl"))
                {
                    lbMagicTablesLoaded.Items[i] = "skill_magic_" + i + ".tbl::[OK]";
                }
            }
            skillTableSet.Tables.Add(table);
            index++;
        }

        private readonly DatabaseManager DBManager;

        public MainFrm()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            DBManager = new DatabaseManager();
            DBManager.OnException += db_onexception;
            try
            {
                FlyingEffectSkillIDs = new List<string>(File.ReadAllLines("flyingeffect.txt"));
            }
            catch (Exception)
            {

                MessageBox.Show("flyingeffect.txt does not exists!");
                Application.Exit();
            }
           
        }

        private void db_onexception(Exception ex)
        {
            richTextBox1.AppendText((ex.Message));
        }


        private void MainFrm_Load(object sender, EventArgs e)
        {
            
        }


        private bool CheckConnectionParameters(bool isLocal)
        {
            if (string.IsNullOrEmpty(tbServer.Text))
            {
                MessageBox.Show("Database server must be specified in order to connect database.");
                return false;
            }
            if (string.IsNullOrEmpty(tbDBName.Text))
            {
                MessageBox.Show("Database name must be specified in order to connect database.");
                return false;
            }
            if (!isLocal)
            {
                if (string.IsNullOrEmpty(tbUserID.Text))
                {
                    MessageBox.Show("Username must be specified in order to connect database.");
                    return false;
                }
                if (string.IsNullOrEmpty(tbPassword.Text))
                {
                    MessageBox.Show("Password must be specified in order to connect database.");
                    return false;
                }
            }
            return true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
         if(tbServer.Text != "(local)")/*if we are not connecting to a local server*/
           if(!CheckConnectionParameters(false)) return;
         else/*we're on local**/
             if (!CheckConnectionParameters(true)) return;

            SqlConnectionStringBuilder sqlConn = new SqlConnectionStringBuilder()
                                                     {
                                                         InitialCatalog = tbDBName.Text,
                                                         UserID = tbUserID.Text,
                                                         Password = tbPassword.Text,
                                                         DataSource = tbServer.Text,
                                                         IntegratedSecurity = true
                                                     };
            if (DBManager.TestConnection(sqlConn))
            {
                DBManager.Connect(sqlConn,sqlInfoOut);
                lblConnStatus.ForeColor = Color.ForestGreen;
                lblConnStatus.Text = "Connected.";
                gbDatabase.Enabled = false;
                gbStatus.Enabled = true;
            }
            else
            {
                lblConnStatus.ForeColor = Color.IndianRed;
                lblConnStatus.Text = "Connection attempt fail!";  
            }
                
        }

        private void sqlInfoOut(object sender, SqlInfoMessageEventArgs e)
        {
            richTextBox1.AppendText(e.Message);
        }

        class InsertIntoQuery
        {
            private StringBuilder _storage = new StringBuilder();
            private string tblName;
            public InsertIntoQuery(string TableName)
            {
                _storage.Append(string.Format("INSERT INTO {0} VALUES(", TableName));
                tblName = TableName;
            }
            public void AppendValue(string value)
            {
                _storage.Append(string.Format("{0},", value));
            }
            public void AppendString(string value)
            {
                _storage.Append(string.Format("'{0}',", value));  
            }
            public void AppendEndValue(string value)
            {
                _storage.Append(string.Format("{0}", value));  
            }

            public string GetQuery()
            {
                return _storage.ToString() + ")";
            }
            public void Clear()
            {
                _storage = new StringBuilder();
                _storage.Append(string.Format("INSERT INTO {0} VALUES(", tblName));
            }

        }

        private string GetNameFromMAIN(int skillid)
        {
            foreach (DataRow r in skillTableSet.Tables["skill_magic_main_" + extension + ".tbl"].Rows)
            {
                Int32 SkillID = Convert.ToInt32(r[0]);
                if (SkillID == skillid) return r[1].ToString().Replace("'","");
            }
            return "No Name Specified";
        }
        private string GetDescFromMAIN(int skillid)
        {
            foreach (DataRow r in skillTableSet.Tables["skill_magic_main_" + extension + ".tbl"].Rows)
            {
                Int32 SkillID = Convert.ToInt32(r[0]);
                if (SkillID == skillid) return r[2].ToString().Replace("'","");
            }
            return "No Description Specified";
        }

        private void GenerateQueries(object x)
        {
            var iiQuery = new InsertIntoQuery("MAGIC_NEW");
            foreach (DataRow r in skillTableSet.Tables["skill_magic_main_"+extension+".tbl"].Rows)
            {/*5*/
                /*Normal = 207,
                    Master = 208*/
                /*
                  0 - MagicNum
                  1 -  KrName
                  2 - EnName
                  3 - Desc
                  4 - SelfAni1
                  5 - BeforeAction
                  6 - TargetAction
                  7 - SelfEffect1
                  8 - SelfPart1
                  9 - SelftEffect2
                  10 - SelfPart2
                  11 - FlyingEffect
                  12 - TargetEffect
                  13 - TargetPart
                  14 - Moral
                  15 - SkillLevel
                  16 - Skill
                  17 - Msp
                  18 - Hp
                  19 - ItemGroup
                  20 - UseItem
                  21 - CastTime
                  22 - RecastTime
                  23 - TempMemory1
                  24 - TempMemory2
                  25 - SuccessRate
                  26 - Type1
                  27 - Type2
                  28 - Range
                  29 - Etc
                  30 - Event
                */
                Int32 SkillID = Convert.ToInt32(r[0]);
                int before = Convert.ToInt32(r[5]);
                int target = Convert.ToInt32(r[6]);
                int self = Convert.ToInt32(r[7]);
                int casttime = Convert.ToInt32(r[21]);
                int recasttime = Convert.ToInt32(r[22]);
                iiQuery.AppendValue(string.IsNullOrEmpty(r[0].ToString()) ? 0.ToString(CultureInfo.InvariantCulture) : SkillID.ToString());/*magicnum*/
                iiQuery.AppendString(string.IsNullOrEmpty(r[1].ToString()) ? "No Name Specified" : r[1].ToString().Replace("'",""));/*enname*/
                iiQuery.AppendString(string.IsNullOrEmpty(r[2].ToString()) ? "No Name Specified" : r[2].ToString().Replace("'", ""));/*krname*/
                iiQuery.AppendString(string.IsNullOrEmpty(r[3].ToString()) ? "No Description Specified" : r[3].ToString().Replace("'", ""));/*desc*/
                iiQuery.AppendValue(string.IsNullOrEmpty(r[4].ToString()) ? 0.ToString(CultureInfo.InvariantCulture) : (before > 255 || before < 0 ? 0.ToString():before.ToString()));/*beforeact*/
                iiQuery.AppendValue(string.IsNullOrEmpty(r[5].ToString()) ? 0.ToString(CultureInfo.InvariantCulture) : (target > 255 || target < 0 ? 0.ToString() : target.ToString()));/*targetact*/
                iiQuery.AppendValue(string.IsNullOrEmpty(r[6].ToString()) ? 0.ToString(CultureInfo.InvariantCulture) : (self > 255 || self < 0 ? 0.ToString() : self.ToString()));/*selfeffect*/
                /* flying effect check */
             /*   if(SkillID > 207499 && SkillID < 207584 || SkillID > 208499 && SkillID < 208584 ||SkillID > 107499 && SkillID < 107584 || SkillID > 108499 && SkillID < 108584 )
                    iiQuery.AppendValue("1");
                else
                {
                    
                    foreach(string s in FlyingEffectSkillIDs)
                    {
                        if (s == SkillID.ToString())
                        {
                            iiQuery.AppendValue("1");
                            goto continuefromhere;
                        }
                    }
                    iiQuery.AppendValue("0");
                }*/
                iiQuery.AppendValue(string.IsNullOrEmpty(r[11].ToString()) ? 0.ToString() : r[11].ToString());
            continuefromhere:
                ;
                iiQuery.AppendValue(string.IsNullOrEmpty(r[12].ToString()) ? 0.ToString(CultureInfo.InvariantCulture) : r[12].ToString());/*target effect*/
                iiQuery.AppendValue(string.IsNullOrEmpty(r[14].ToString()) ? 0.ToString(CultureInfo.InvariantCulture) : r[14].ToString());/*moral*/
                iiQuery.AppendValue(string.IsNullOrEmpty(r[15].ToString()) ? 0.ToString(CultureInfo.InvariantCulture) : r[15].ToString());/*skill level*/
                iiQuery.AppendValue(string.IsNullOrEmpty(r[16].ToString()) ? 0.ToString(CultureInfo.InvariantCulture) : r[16].ToString());/*skill*/
                iiQuery.AppendValue(string.IsNullOrEmpty(r[17].ToString()) ? 0.ToString(CultureInfo.InvariantCulture) : r[17].ToString());/*msp*/
                iiQuery.AppendValue(string.IsNullOrEmpty(r[18].ToString()) ? 0.ToString(CultureInfo.InvariantCulture) : r[18].ToString());/*hp*/
                iiQuery.AppendValue(string.IsNullOrEmpty(r[19].ToString()) ? 0.ToString(CultureInfo.InvariantCulture) : r[19].ToString());/*itemgroup*/
                iiQuery.AppendValue(string.IsNullOrEmpty(r[20].ToString()) ? 0.ToString(CultureInfo.InvariantCulture) : r[20].ToString());/*useitem*/
                iiQuery.AppendValue(string.IsNullOrEmpty(r[21].ToString()) ? 0.ToString(CultureInfo.InvariantCulture) : (casttime > 255 ? 255.ToString() : casttime.ToString()));/*casttime*/
                iiQuery.AppendValue(string.IsNullOrEmpty(r[22].ToString()) ? 0.ToString(CultureInfo.InvariantCulture) : (recasttime > 255 ? 255.ToString() : recasttime.ToString()));/*recasttime*/
                iiQuery.AppendValue(string.IsNullOrEmpty(r[25].ToString()) ? 0.ToString(CultureInfo.InvariantCulture) : r[25].ToString());/*successrate*/
                iiQuery.AppendValue(string.IsNullOrEmpty(r[26].ToString()) ? 0.ToString(CultureInfo.InvariantCulture) : r[26].ToString());/*Type1*/
                iiQuery.AppendValue(string.IsNullOrEmpty(r[27].ToString()) ? 0.ToString(CultureInfo.InvariantCulture) : r[27].ToString());/*Type2*/
                iiQuery.AppendValue(string.IsNullOrEmpty(r[28].ToString()) ? 0.ToString(CultureInfo.InvariantCulture) : r[28].ToString());/*Range*/
                iiQuery.AppendValue(string.IsNullOrEmpty(r[29].ToString()) ? 0.ToString(CultureInfo.InvariantCulture) : r[29].ToString());/*Etc*/
                iiQuery.AppendEndValue("0");/*event*/
                MAGICQuery.Add(iiQuery.GetQuery());
                iiQuery.Clear();
               
            }
            lbImportList.Items[0] = "MAGIC::[I]";
            List<string> mql;
            for(int i = 1; i <= 9; i++)
            {
                switch (i)
                {
                    case 1:
                        iiQuery = new InsertIntoQuery("MAGIC_TYPE1_NEW");
                         mql = new List<string>();
                        mql.Clear();
                        foreach (DataRow r in skillTableSet.Tables["skill_magic_1.tbl"].Rows)
                        {
                            Int32 SkillID = Convert.ToInt32(r[0]);
                            iiQuery.AppendValue(string.IsNullOrEmpty(r[0].ToString()) ? 0.ToString(CultureInfo.InvariantCulture) : SkillID.ToString());/*magicnum*/
                            iiQuery.AppendString(GetNameFromMAIN(SkillID));
                            iiQuery.AppendString(GetDescFromMAIN(SkillID));
                            iiQuery.AppendValue(string.IsNullOrEmpty(r[1].ToString()) ? 0.ToString(CultureInfo.InvariantCulture) : r[1].ToString());/*type*/
                            iiQuery.AppendValue(string.IsNullOrEmpty(r[2].ToString()) ? 0.ToString(CultureInfo.InvariantCulture) : r[2].ToString());/*hitrate*/
                            iiQuery.AppendValue(string.IsNullOrEmpty(r[3].ToString()) ? 0.ToString(CultureInfo.InvariantCulture) : r[3].ToString());/*hit*/
                            iiQuery.AppendValue(0.ToString());                                                                                      /*adddamage*/
                            iiQuery.AppendValue(string.IsNullOrEmpty(r[4].ToString()) ? 0.ToString(CultureInfo.InvariantCulture) : r[4].ToString());/*delay*/
                            iiQuery.AppendValue(string.IsNullOrEmpty(r[5].ToString()) ? 0.ToString(CultureInfo.InvariantCulture) : r[5].ToString());/*combotype*/
                            iiQuery.AppendValue(string.IsNullOrEmpty(r[6].ToString()) ? 0.ToString(CultureInfo.InvariantCulture) : r[6].ToString());/*combocount*/
                            iiQuery.AppendValue(string.IsNullOrEmpty(r[7].ToString()) ? 0.ToString(CultureInfo.InvariantCulture) : r[7].ToString());/*combodamage*/
                            iiQuery.AppendEndValue(string.IsNullOrEmpty(r[8].ToString()) ? 0.ToString(CultureInfo.InvariantCulture) : r[8].ToString());/*range*/
                            mql.Add(iiQuery.GetQuery());
                            iiQuery.Clear();
                        }
                        MAGICExtQueries.Add(mql);
                        lbImportList.Items[1] = "MAGIC_TYPE1::[I]";
                        break;
                    case 2:
                        iiQuery = new InsertIntoQuery("MAGIC_TYPE2_NEW");
                        mql = new List<string>();
                        mql.Clear();
                        foreach (DataRow r in skillTableSet.Tables["skill_magic_2.tbl"].Rows)
                        {
                            Int32 SkillID = Convert.ToInt32(r[0]);
                            iiQuery.AppendValue(string.IsNullOrEmpty(r[0].ToString()) ? 0.ToString(CultureInfo.InvariantCulture) : SkillID.ToString());/*magicnum*/
                            iiQuery.AppendString(GetNameFromMAIN(SkillID));
                            iiQuery.AppendString(GetDescFromMAIN(SkillID));
                            iiQuery.AppendValue(string.IsNullOrEmpty(r[1].ToString()) ? 0.ToString(CultureInfo.InvariantCulture) : r[1].ToString());/*hittype*/
                            iiQuery.AppendValue(string.IsNullOrEmpty(r[2].ToString()) ? 0.ToString(CultureInfo.InvariantCulture) : r[2].ToString());/*hitrate*/
                            iiQuery.AppendValue(string.IsNullOrEmpty(r[3].ToString()) ? 0.ToString(CultureInfo.InvariantCulture) : r[3].ToString());/*adddamage*/                                                                                 /*adddamage*/
                            iiQuery.AppendValue(string.IsNullOrEmpty(r[4].ToString()) ? 0.ToString(CultureInfo.InvariantCulture) : r[4].ToString());/*addrange*/
                            iiQuery.AppendValue(string.IsNullOrEmpty(r[5].ToString()) ? 0.ToString(CultureInfo.InvariantCulture) : r[5].ToString());/*needarrow*/
                            iiQuery.AppendEndValue(string.IsNullOrEmpty(r[5].ToString()) ? 0.ToString(CultureInfo.InvariantCulture) : r[5].ToString());/*adddamageplus*/
                            mql.Add(iiQuery.GetQuery());
                            iiQuery.Clear();
                        }
                        MAGICExtQueries.Add(mql);
                        lbImportList.Items[2] = "MAGIC_TYPE2::[I]";
                        break;
                    case 3:
                        iiQuery = new InsertIntoQuery("MAGIC_TYPE3_NEW");
                        mql = new List<string>();
                        mql.Clear();
                        foreach (DataRow r in skillTableSet.Tables["skill_magic_3.tbl"].Rows)
                        {
                            Int32 SkillID = Convert.ToInt32(r[0]);
                            iiQuery.AppendValue(string.IsNullOrEmpty(r[0].ToString()) ? 0.ToString(CultureInfo.InvariantCulture) : SkillID.ToString());/*magicnum*/
                            iiQuery.AppendString(GetNameFromMAIN(SkillID));
                            iiQuery.AppendString(GetDescFromMAIN(SkillID));
                            iiQuery.AppendValue(string.IsNullOrEmpty(r[1].ToString()) ? 0.ToString(CultureInfo.InvariantCulture) : r[1].ToString());/*radius*/
                            iiQuery.AppendValue(0.ToString());/*angle*/
                            iiQuery.AppendValue(string.IsNullOrEmpty(r[2].ToString()) ? 0.ToString(CultureInfo.InvariantCulture) : r[2].ToString());/*directtype*/                                                                                 /*adddamage*/
                            iiQuery.AppendValue(string.IsNullOrEmpty(r[3].ToString()) ? 0.ToString(CultureInfo.InvariantCulture) : r[3].ToString());/*firstdamage*/
                            iiQuery.AppendValue(string.IsNullOrEmpty(r[3].ToString()) ? 0.ToString(CultureInfo.InvariantCulture) : (Convert.ToInt32(r[3]) > 1 ? (Convert.ToInt32(r[3])/2).ToString():0.ToString()));/*enddamage*/
                            iiQuery.AppendValue(string.IsNullOrEmpty(r[4].ToString()) ? 0.ToString(CultureInfo.InvariantCulture) : r[4].ToString());/*timedamage*/
                            iiQuery.AppendValue(string.IsNullOrEmpty(r[5].ToString()) ? 0.ToString(CultureInfo.InvariantCulture) : r[5].ToString());/*duration*/
                            iiQuery.AppendEndValue(string.IsNullOrEmpty(r[6].ToString()) ? 0.ToString(CultureInfo.InvariantCulture) : r[6].ToString());/*attribute*/
                            mql.Add(iiQuery.GetQuery());
                            iiQuery.Clear();
                        }
                        MAGICExtQueries.Add(mql);
                        lbImportList.Items[3] = "MAGIC_TYPE3::[I]";
                        break;
                    case 4:
                        iiQuery = new InsertIntoQuery("MAGIC_TYPE4_NEW");
                        mql = new List<string>();
                        mql.Clear();
                        foreach (DataRow r in skillTableSet.Tables["skill_magic_4.tbl"].Rows)
                        {
                            Int32 SkillID = Convert.ToInt32(r[0]);
                            iiQuery.AppendValue(string.IsNullOrEmpty(r[0].ToString()) ? 0.ToString(CultureInfo.InvariantCulture) : SkillID.ToString());/*magicnum*/
                            iiQuery.AppendString(GetNameFromMAIN(SkillID));
                            iiQuery.AppendString(GetDescFromMAIN(SkillID));
                            iiQuery.AppendValue(string.IsNullOrEmpty(r[1].ToString()) ? 0.ToString(CultureInfo.InvariantCulture) : r[1].ToString());/*bufftype*/
                            iiQuery.AppendValue(string.IsNullOrEmpty(r[2].ToString()) ? 0.ToString(CultureInfo.InvariantCulture) : r[2].ToString());/*radius*/
                            iiQuery.AppendValue(string.IsNullOrEmpty(r[3].ToString()) ? 0.ToString(CultureInfo.InvariantCulture) : r[3].ToString());/*duration*/                                                                               
                            iiQuery.AppendValue(string.IsNullOrEmpty(r[4].ToString()) ? 0.ToString(CultureInfo.InvariantCulture) : r[4].ToString());/*attackspeed*/
                            iiQuery.AppendValue(string.IsNullOrEmpty(r[5].ToString()) ? 0.ToString(CultureInfo.InvariantCulture) : r[5].ToString());/*speed*/
                            iiQuery.AppendValue(string.IsNullOrEmpty(r[6].ToString()) ? 0.ToString(CultureInfo.InvariantCulture) : r[6].ToString());/*ac*/
                            iiQuery.AppendValue(string.IsNullOrEmpty(r[7].ToString()) ? 0.ToString(CultureInfo.InvariantCulture) : r[7].ToString());/*acpct*/
                            iiQuery.AppendValue(string.IsNullOrEmpty(r[8].ToString()) ? 0.ToString(CultureInfo.InvariantCulture) : r[8].ToString());/*attack*/
                            iiQuery.AppendValue(string.IsNullOrEmpty(r[9].ToString()) ? 0.ToString(CultureInfo.InvariantCulture) : r[9].ToString());/*magicattack*/
                            iiQuery.AppendValue(string.IsNullOrEmpty(r[10].ToString()) ? 0.ToString(CultureInfo.InvariantCulture) : r[10].ToString());/*maxhp*/
                            iiQuery.AppendValue(string.IsNullOrEmpty(r[11].ToString()) ? 0.ToString(CultureInfo.InvariantCulture) : r[11].ToString());/*maxhppct*/
                            iiQuery.AppendValue(string.IsNullOrEmpty(r[12].ToString()) ? 0.ToString(CultureInfo.InvariantCulture) : r[12].ToString());/*maxmp*/
                            iiQuery.AppendValue(string.IsNullOrEmpty(r[13].ToString()) ? 0.ToString(CultureInfo.InvariantCulture) : r[13].ToString());/*maxmppct*/
                            iiQuery.AppendValue(100.ToString());/*hitrate*/
                            iiQuery.AppendValue(100.ToString());/*avoidrate*/
                            iiQuery.AppendValue(string.IsNullOrEmpty(r[14].ToString()) ? 0.ToString(CultureInfo.InvariantCulture) : r[14].ToString());/*str*/
                            iiQuery.AppendValue(string.IsNullOrEmpty(r[15].ToString()) ? 0.ToString(CultureInfo.InvariantCulture) : r[15].ToString());/*sta*/
                            iiQuery.AppendValue(string.IsNullOrEmpty(r[16].ToString()) ? 0.ToString(CultureInfo.InvariantCulture) : r[16].ToString());/*dex*/
                            iiQuery.AppendValue(string.IsNullOrEmpty(r[17].ToString()) ? 0.ToString(CultureInfo.InvariantCulture) : r[17].ToString());/*intel*/
                            iiQuery.AppendValue(string.IsNullOrEmpty(r[18].ToString()) ? 0.ToString(CultureInfo.InvariantCulture) : r[18].ToString());/*cha*/
                            iiQuery.AppendValue(string.IsNullOrEmpty(r[19].ToString()) ? 0.ToString(CultureInfo.InvariantCulture) : r[19].ToString());/*fr*/
                            iiQuery.AppendValue(string.IsNullOrEmpty(r[20].ToString()) ? 0.ToString(CultureInfo.InvariantCulture) : r[20].ToString());/*gr*/
                            iiQuery.AppendValue(string.IsNullOrEmpty(r[21].ToString()) ? 0.ToString(CultureInfo.InvariantCulture) : r[21].ToString());/*lr*/
                            iiQuery.AppendValue(string.IsNullOrEmpty(r[22].ToString()) ? 0.ToString(CultureInfo.InvariantCulture) : r[22].ToString());/*mr*/
                            iiQuery.AppendValue(string.IsNullOrEmpty(r[23].ToString()) ? 0.ToString(CultureInfo.InvariantCulture) : r[23].ToString());/*dr*/
                            iiQuery.AppendValue(string.IsNullOrEmpty(r[24].ToString()) ? 0.ToString(CultureInfo.InvariantCulture) : r[24].ToString());/*pr*/
                            iiQuery.AppendValue(string.IsNullOrEmpty(r[25].ToString()) ? 0.ToString(CultureInfo.InvariantCulture) : r[25].ToString());/*exppct*/
                            iiQuery.AppendValue(0.ToString(CultureInfo.InvariantCulture) );/*loyaltypct*/
                            iiQuery.AppendEndValue(0.ToString(CultureInfo.InvariantCulture));/*specialamount*/
                            mql.Add(iiQuery.GetQuery());
                            iiQuery.Clear();
                        }
                        MAGICExtQueries.Add(mql);
                        lbImportList.Items[4] = "MAGIC_TYPE4::[I]";
                        break;
                    case 5:
                        iiQuery = new InsertIntoQuery("MAGIC_TYPE5_NEW");
                        mql = new List<string>();
                        mql.Clear();
                        
                        foreach (DataRow r in skillTableSet.Tables["skill_magic_5.tbl"].Rows)
                        {
                            Int32 SkillID = Convert.ToInt32(r[0]);
                            iiQuery.AppendValue(string.IsNullOrEmpty(r[0].ToString()) ? 0.ToString(CultureInfo.InvariantCulture) : SkillID.ToString());/*magicnum*/
                            iiQuery.AppendString(GetNameFromMAIN(SkillID));
                            iiQuery.AppendString(GetDescFromMAIN(SkillID));
                            if (v2153)
                            {
                                iiQuery.AppendValue(string.IsNullOrEmpty(r[1].ToString()) ? 0.ToString(CultureInfo.InvariantCulture) : r[1].ToString());/*type*/
                                iiQuery.AppendValue(string.IsNullOrEmpty(r[2].ToString()) ? 0.ToString(CultureInfo.InvariantCulture) : r[2].ToString());/*exprecover*/
                                iiQuery.AppendEndValue(string.IsNullOrEmpty(r[3].ToString()) ? 0.ToString(CultureInfo.InvariantCulture) : r[3].ToString());/*needstone*/
                            }
                            else
                            {
                                iiQuery.AppendValue(string.IsNullOrEmpty(r[2].ToString()) ? 0.ToString(CultureInfo.InvariantCulture) : r[2].ToString());/*type*/
                                iiQuery.AppendValue(string.IsNullOrEmpty(r[3].ToString()) ? 0.ToString(CultureInfo.InvariantCulture) : r[3].ToString());/*exprecover*/
                                iiQuery.AppendEndValue(string.IsNullOrEmpty(r[4].ToString()) ? 0.ToString(CultureInfo.InvariantCulture) : r[4].ToString());/*needstone*/
                            }
                            mql.Add(iiQuery.GetQuery());
                            iiQuery.Clear();
                        }
                        MAGICExtQueries.Add(mql);
                        lbImportList.Items[5] = "MAGIC_TYPE5::[I]";
                        break;
                    case 6:
                        iiQuery = new InsertIntoQuery("MAGIC_TYPE6_NEW");
                        mql = new List<string>();
                        mql.Clear();
                        foreach (DataRow r in skillTableSet.Tables["skill_magic_6.tbl"].Rows)
                        {
                            Int32 SkillID = Convert.ToInt32(r[0]);
                            iiQuery.AppendValue(string.IsNullOrEmpty(r[0].ToString()) ? 0.ToString(CultureInfo.InvariantCulture) : SkillID.ToString());/*magicnum*/
                            iiQuery.AppendString(GetNameFromMAIN(SkillID));
                            iiQuery.AppendString(GetDescFromMAIN(SkillID));
                            iiQuery.AppendValue(string.IsNullOrEmpty(r[3].ToString()) ? 0.ToString(CultureInfo.InvariantCulture) : r[3].ToString());/*size*/
                            iiQuery.AppendValue(string.IsNullOrEmpty(r[4].ToString()) ? 0.ToString(CultureInfo.InvariantCulture) : r[4].ToString());/*transformid*/
                            iiQuery.AppendValue(string.IsNullOrEmpty(r[5].ToString()) ? 0.ToString(CultureInfo.InvariantCulture) : r[5].ToString());/*duration*/                                                                               
                            iiQuery.AppendValue(string.IsNullOrEmpty(r[6].ToString()) ? 0.ToString(CultureInfo.InvariantCulture) : r[6].ToString());/*maxhp*/
                            iiQuery.AppendValue(string.IsNullOrEmpty(r[7].ToString()) ? 0.ToString(CultureInfo.InvariantCulture) : r[7].ToString());/*maxmp*/
                            iiQuery.AppendValue(string.IsNullOrEmpty(r[8].ToString()) ? 0.ToString(CultureInfo.InvariantCulture) : r[8].ToString());/*speed*/
                            iiQuery.AppendValue(string.IsNullOrEmpty(r[9].ToString()) ? 0.ToString(CultureInfo.InvariantCulture) : r[9].ToString());/*attackspeed*/
                            iiQuery.AppendValue(string.IsNullOrEmpty(r[10].ToString()) ? 0.ToString(CultureInfo.InvariantCulture) : r[10].ToString());/*totalhit*/
                            iiQuery.AppendValue(string.IsNullOrEmpty(r[11].ToString()) ? 0.ToString(CultureInfo.InvariantCulture) : r[11].ToString());/*totalac*/
                            iiQuery.AppendValue(string.IsNullOrEmpty(r[12].ToString()) ? 0.ToString(CultureInfo.InvariantCulture) : r[12].ToString());/*totalhitrate*/
                            iiQuery.AppendValue(string.IsNullOrEmpty(r[13].ToString()) ? 0.ToString(CultureInfo.InvariantCulture) : r[13].ToString());/*totalevasionrate*/
                            iiQuery.AppendValue(string.IsNullOrEmpty(r[14].ToString()) ? 0.ToString(CultureInfo.InvariantCulture) : r[14].ToString());/*totalfr*/
                            iiQuery.AppendValue(string.IsNullOrEmpty(r[15].ToString()) ? 0.ToString(CultureInfo.InvariantCulture) : r[15].ToString());/*totalgr*/
                            iiQuery.AppendValue(string.IsNullOrEmpty(r[16].ToString()) ? 0.ToString(CultureInfo.InvariantCulture) : r[16].ToString());/*totallr*/
                            iiQuery.AppendValue(string.IsNullOrEmpty(r[17].ToString()) ? 0.ToString(CultureInfo.InvariantCulture) : r[17].ToString());/*totalmr*/
                            iiQuery.AppendValue(string.IsNullOrEmpty(r[18].ToString()) ? 0.ToString(CultureInfo.InvariantCulture) : r[18].ToString());/*totaldr*/
                            iiQuery.AppendValue(string.IsNullOrEmpty(r[19].ToString()) ? 0.ToString(CultureInfo.InvariantCulture) : r[19].ToString());/*totalpr*/
                            iiQuery.AppendValue(string.IsNullOrEmpty(r[21].ToString()) ? 0.ToString(CultureInfo.InvariantCulture) : r[21].ToString());/*class*/
                            iiQuery.AppendValue(string.IsNullOrEmpty(r[22].ToString()) ? 0.ToString(CultureInfo.InvariantCulture) : r[22].ToString());/*userskilluse*/
                            iiQuery.AppendValue(string.IsNullOrEmpty(r[23].ToString()) ? 0.ToString(CultureInfo.InvariantCulture) : r[23].ToString());/*needitem*/
                            iiQuery.AppendValue(string.IsNullOrEmpty(r[24].ToString()) ? 0.ToString(CultureInfo.InvariantCulture) : r[24].ToString());/*skillsuccessrate*/
                            iiQuery.AppendEndValue(string.IsNullOrEmpty(r[25].ToString()) ? 0.ToString(CultureInfo.InvariantCulture) : r[25].ToString());/*monsterfriendly*/
                            mql.Add(iiQuery.GetQuery());
                            iiQuery.Clear();
                        }
                        MAGICExtQueries.Add(mql);
                        lbImportList.Items[6] = "MAGIC_TYPE6::[I]";
                        break;
                    case 7:
                    case 8:
                        break;
                    case 9:
                        iiQuery = new InsertIntoQuery("MAGIC_TYPE9_NEW");
                        mql = new List<string>();
                        mql.Clear();
                        foreach (DataRow r in skillTableSet.Tables["skill_magic_9.tbl"].Rows)
                        {
                            Int32 SkillID = Convert.ToInt32(r[0]);
                            iiQuery.AppendValue(string.IsNullOrEmpty(r[0].ToString()) ? 0.ToString(CultureInfo.InvariantCulture) : SkillID.ToString());/*magicnum*/
                            iiQuery.AppendString(GetNameFromMAIN(SkillID));
                            iiQuery.AppendString(GetDescFromMAIN(SkillID));
                            iiQuery.AppendValue(string.IsNullOrEmpty(r[3].ToString()) ? 0.ToString(CultureInfo.InvariantCulture) : r[3].ToString());/*validgroup*/
                            iiQuery.AppendValue(string.IsNullOrEmpty(r[4].ToString()) ? 0.ToString(CultureInfo.InvariantCulture) : r[4].ToString());/*nationchange*/
                            iiQuery.AppendValue(string.IsNullOrEmpty(r[5].ToString()) ? 0.ToString(CultureInfo.InvariantCulture) : r[5].ToString());/*monsternum*/                                                                               
                            iiQuery.AppendValue(string.IsNullOrEmpty(r[6].ToString()) ? 0.ToString(CultureInfo.InvariantCulture) : r[6].ToString());/*targetchange*/
                            iiQuery.AppendValue(string.IsNullOrEmpty(r[7].ToString()) ? 0.ToString(CultureInfo.InvariantCulture) : r[7].ToString());/*statechange*/
                            iiQuery.AppendValue(string.IsNullOrEmpty(r[8].ToString()) ? 0.ToString(CultureInfo.InvariantCulture) : r[8].ToString());/*radius*/
                            iiQuery.AppendValue(string.IsNullOrEmpty(r[9].ToString()) ? 0.ToString(CultureInfo.InvariantCulture) : r[9].ToString());/*hitrate*/
                            iiQuery.AppendValue(string.IsNullOrEmpty(r[10].ToString()) ? 0.ToString(CultureInfo.InvariantCulture) : r[10].ToString());/*duration*/
                            iiQuery.AppendValue(string.IsNullOrEmpty(r[11].ToString()) ? 0.ToString(CultureInfo.InvariantCulture) : r[11].ToString());/*adddamage*/
                            iiQuery.AppendValue(string.IsNullOrEmpty(r[12].ToString()) ? 0.ToString(CultureInfo.InvariantCulture) : r[12].ToString());/*vision*/
                            iiQuery.AppendEndValue(string.IsNullOrEmpty(r[13].ToString()) ? 0.ToString(CultureInfo.InvariantCulture) : r[13].ToString());/*needitem*/

                            mql.Add(iiQuery.GetQuery());
                            iiQuery.Clear();
                        }
                        MAGICExtQueries.Add(mql);

                        lbImportList.Items[9] = "MAGIC_TYPE9::[I]";
                        break;
                }
            }
            queryisReady.Set();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SelectFolderDlg.ShowDialog();
            if(!string.IsNullOrEmpty(SelectFolderDlg.SelectedPath))
            {
                string[] tblList = Directory.GetFiles(SelectFolderDlg.SelectedPath);
                List<string> skillTableList = new List<string>();
                foreach (string s in tblList)
                {
                 if(s.ToLower().Contains("skill_magic_main_"+extension+".tbl"))
                 {
                     IsTableExists.Add(0);
                     skillTableList.Add(s.ToLower());
                     continue;/* save a few cpu cycles*/
                 }
                    for(byte i = 1; i<=9;i++)
                    {
                        if (s.ToLower().Contains("skill_magic_" + i + ".tbl"))
                        {
                            IsTableExists.Add(i);
                            skillTableList.Add(s.ToLower());
                            continue;
                        }
                    }
                }
                if (IsTableExists.Count == 10)
                {
                    foreach (string s in skillTableList)
                        LoadByteDataIntoView(LoadAndDecodeFile(s),s);
                    btnImport.Text = "Begin Import!";
                    btnImport.Enabled = true;
                    btnView.Enabled = true;
                }
                else
                {
                    var missingfiles = new List<string>();
                    if (!(IsTableExists.Count > 0))
                    {
                        missingfiles.Add("skill_magic_main_" + extension + ".tbl");
                        for (int s = 1; s <= 9; s++)
                            missingfiles.Add("skill_magic_" + s + ".tbl");
                    }
                    else
                    {
                        for (byte i = 0; i <= 9; i++)
                        {
                            if (IsTableExists.Contains(i)) continue;

                            if (i == 0) missingfiles.Add("skill_magic_main_" + extension + ".tbl");
                            else missingfiles.Add("skill_magic_" + i + ".tbl");
                        }

                    }

                    var msg = new StringBuilder();
                    msg.Append("Some skill tables are missing!" + Environment.NewLine);
                    foreach(string s in missingfiles)
                        msg.Append(s + Environment.NewLine);

                    MessageBox.Show(msg.ToString());
                }
            }
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            using(var fv = new frmViewTables(skillTableSet))
            {
                fv.ShowDialog();
            }
        }

        private void ImportMagic(object x)
        {
            var createQuery = new SqlQuery { CommandText = Resources.Queries_CreateMagicNew };
            DBManager.ExecuteQuery(createQuery);
            pbCurrent.Maximum = MAGICQuery.Count;
            int pcount = 0;
            foreach (string s in MAGICQuery)
            {
                pbCurrent.CustomText = string.Format("Importing MAGIC table, {0} out of {1} queries processed.", pcount, MAGICQuery.Count);
                var tempQuery = new SqlQuery { CommandText = s };
                DBManager.ExecuteQuery(tempQuery);
                pbCurrent.PerformStep();
                pcount++;
            }
            pbOverall.CustomText = string.Format("{0} table imported out of 8", ++impcount);
            pbOverall.PerformStep();
            processLock.Set();
        }

        private void ImportType(object x)
        {
            int type = Convert.ToInt32(x);
            SqlQuery createQuery;
            
            switch(type)
            {
                case 0:
                    createQuery = new SqlQuery { CommandText = Resources.Queries_CreateMagic1New };
                    DBManager.ExecuteQuery(createQuery);
                    break;
                case 1:
                    createQuery = new SqlQuery { CommandText = Resources.Queries_CreateMagic2New };
                    DBManager.ExecuteQuery(createQuery);
                    break;
                case 2:
                    createQuery = new SqlQuery { CommandText = Resources.Queries_CreateMagic3New };
                    DBManager.ExecuteQuery(createQuery);
                    break;
                case 3:
                    createQuery = new SqlQuery { CommandText = Resources.Queries_CreateMagic4New };
                    DBManager.ExecuteQuery(createQuery);
                    break;
                case 4:
                    createQuery = new SqlQuery { CommandText = Resources.Queries_CreateMagic5New };
                    DBManager.ExecuteQuery(createQuery);
                    break;
                case 5:
                    createQuery = new SqlQuery { CommandText = Resources.Queries_CreateMagic6New };
                    DBManager.ExecuteQuery(createQuery);
                    break;
                case 6:
                    createQuery = new SqlQuery { CommandText = Resources.Queries_CreateMagic9New };
                    DBManager.ExecuteQuery(createQuery);
                    break;
            }
           
            if (type == 6) type = 9;
            int pcount = 0;
            pbCurrent.Maximum = MAGICExtQueries[type == 9 ? 6 : type].Count;
            pbCurrent.Value = 0;
            foreach (string s in MAGICExtQueries[type== 9 ? 6:type])
            {
                pbCurrent.CustomText = string.Format("Importing MAGIC_TYPE{2} table, {0} out of {1} queries processed.", pcount, MAGICExtQueries[type == 9 ? 6 : type].Count, type == 9 ? 9 : type + 1);
                var tempQuery = new SqlQuery() { CommandText = s };
                DBManager.ExecuteQuery(tempQuery);
                pbCurrent.PerformStep();
                pcount++;
            }
            
            pbOverall.CustomText = string.Format("{0} table imported out of 8", ++impcount);
            pbOverall.PerformStep();
            processLock.Set();
        }

        private int impcount = 0;
        private ManualResetEvent processLock = new ManualResetEvent(false);

        private void RenameTables(object x)
        {
            pbCurrent.Value = 0;
            pbCurrent.Maximum = 8;
            pbOverall.CustomText = "Renaming tables and backing up old tables..";
            var tempQuery = new SqlQuery() {CommandText = Resources.Queries_RenameTable,Parameters = new List<SqlParameter>()};
            tempQuery.Parameters.Add(new SqlParameter("OldName", "MAGIC"));
            tempQuery.Parameters.Add(new SqlParameter("NewName", "MAGIC_BK"));
            DBManager.ExecuteQuery(tempQuery);
            pbCurrent.CustomText = "1 table backed up out of 8";
            pbCurrent.PerformStep();
            tempQuery.Parameters.Clear();
            tempQuery.Parameters.Add(new SqlParameter("OldName", "MAGIC_TYPE1"));
            tempQuery.Parameters.Add(new SqlParameter("NewName", "MAGIC_TYPE1_BK"));
            pbCurrent.CustomText = "2 table backed up out of 8";
            pbCurrent.PerformStep();
            DBManager.ExecuteQuery(tempQuery);
            tempQuery.Parameters.Clear();
            tempQuery.Parameters.Add(new SqlParameter("OldName", "MAGIC_TYPE2"));
            tempQuery.Parameters.Add(new SqlParameter("NewName", "MAGIC_TYPE2_BK"));
            pbCurrent.CustomText = "3 table backed up out of 8";
            pbCurrent.PerformStep();
            DBManager.ExecuteQuery(tempQuery);
            tempQuery.Parameters.Clear();
            tempQuery.Parameters.Add(new SqlParameter("OldName", "MAGIC_TYPE3"));
            tempQuery.Parameters.Add(new SqlParameter("NewName", "MAGIC_TYPE3_BK"));
            pbCurrent.CustomText = "4 table backed up out of 8";
            pbCurrent.PerformStep();
            DBManager.ExecuteQuery(tempQuery);
            tempQuery.Parameters.Clear();
            tempQuery.Parameters.Add(new SqlParameter("OldName", "MAGIC_TYPE4"));
            tempQuery.Parameters.Add(new SqlParameter("NewName", "MAGIC_TYPE4_BK"));
            pbCurrent.CustomText = "5 table backed up out of 8";
            pbCurrent.PerformStep();
            DBManager.ExecuteQuery(tempQuery);
            tempQuery.Parameters.Clear();
            tempQuery.Parameters.Add(new SqlParameter("OldName", "MAGIC_TYPE5"));
            tempQuery.Parameters.Add(new SqlParameter("NewName", "MAGIC_TYPE5_BK"));
                pbCurrent.CustomText = "6 table backed up out of 8";
            pbCurrent.PerformStep();
            DBManager.ExecuteQuery(tempQuery);
            tempQuery.Parameters.Clear();
            tempQuery.Parameters.Add(new SqlParameter("OldName", "MAGIC_TYPE6"));
            tempQuery.Parameters.Add(new SqlParameter("NewName", "MAGIC_TYPE6_BK"));
            pbCurrent.CustomText = "7 table backed up out of 8";
            pbCurrent.PerformStep();
            DBManager.ExecuteQuery(tempQuery);
            tempQuery.Parameters.Clear();
            tempQuery.Parameters.Add(new SqlParameter("OldName", "MAGIC_TYPE9"));
            tempQuery.Parameters.Add(new SqlParameter("NewName", "MAGIC_TYPE9_BK"));
            pbCurrent.CustomText = "8 table backed up out of 8";
            pbCurrent.PerformStep();
            pbCurrent.Value = 0;
            DBManager.ExecuteQuery(tempQuery);
            tempQuery.Parameters.Clear();
            tempQuery.Parameters.Add(new SqlParameter("OldName", "MAGIC_NEW"));
            tempQuery.Parameters.Add(new SqlParameter("NewName", "MAGIC"));
            pbCurrent.CustomText = "1 table renamed out of 8";
            pbCurrent.PerformStep();
            DBManager.ExecuteQuery(tempQuery);
            tempQuery.Parameters.Clear();
            tempQuery.Parameters.Add(new SqlParameter("OldName", "MAGIC_TYPE1_NEW"));
            tempQuery.Parameters.Add(new SqlParameter("NewName", "MAGIC_TYPE1"));
            pbCurrent.CustomText = "2 table renamed out of 8";
            pbCurrent.PerformStep();
            DBManager.ExecuteQuery(tempQuery);
            tempQuery.Parameters.Clear();
            tempQuery.Parameters.Add(new SqlParameter("OldName", "MAGIC_TYPE2_NEW"));
            tempQuery.Parameters.Add(new SqlParameter("NewName", "MAGIC_TYPE2"));
            pbCurrent.CustomText = "3 table renamed out of 8";
            pbCurrent.PerformStep();
            DBManager.ExecuteQuery(tempQuery);
            tempQuery.Parameters.Clear();
            tempQuery.Parameters.Add(new SqlParameter("OldName", "MAGIC_TYPE3_NEW"));
            tempQuery.Parameters.Add(new SqlParameter("NewName", "MAGIC_TYPE3"));
            pbCurrent.CustomText = "4 table renamed out of 8";
            pbCurrent.PerformStep();
            DBManager.ExecuteQuery(tempQuery);
            tempQuery.Parameters.Clear();
            tempQuery.Parameters.Add(new SqlParameter("OldName", "MAGIC_TYPE4_NEW"));
            tempQuery.Parameters.Add(new SqlParameter("NewName", "MAGIC_TYPE4"));
            pbCurrent.CustomText = "5 table renamed out of 8";
            pbCurrent.PerformStep();
            DBManager.ExecuteQuery(tempQuery);
            tempQuery.Parameters.Clear();
            tempQuery.Parameters.Add(new SqlParameter("OldName", "MAGIC_TYPE5_NEW"));
            tempQuery.Parameters.Add(new SqlParameter("NewName", "MAGIC_TYPE5"));
            pbCurrent.CustomText = "6 table renamed out of 8";
            pbCurrent.PerformStep();
            DBManager.ExecuteQuery(tempQuery);
            tempQuery.Parameters.Clear();
            tempQuery.Parameters.Add(new SqlParameter("OldName", "MAGIC_TYPE6_NEW"));
            tempQuery.Parameters.Add(new SqlParameter("NewName", "MAGIC_TYPE6"));
            pbCurrent.CustomText = "7 table renamed out of 8";
            pbCurrent.PerformStep();
            DBManager.ExecuteQuery(tempQuery);
            tempQuery.Parameters.Clear();
            tempQuery.Parameters.Add(new SqlParameter("OldName", "MAGIC_TYPE9_NEW"));
            tempQuery.Parameters.Add(new SqlParameter("NewName", "MAGIC_TYPE9"));
            pbCurrent.CustomText = "8 table renamed out of 8";
            pbCurrent.PerformStep();
            DBManager.ExecuteQuery(tempQuery);
            tempQuery.Parameters.Clear();
            pbCurrent.CustomText = "Finished.";
            processLock.Set();
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            btnImport.Enabled = false;
            btnImport.Text = "Generating SQL Queries..";
            queryisReady.Reset();
            ThreadPool.QueueUserWorkItem(GenerateQueries,new object());
            queryisReady.WaitOne();
            btnImport.Text = "Starting Import..";
            Thread.Sleep(2000);
            gbDatabase.Visible = false;
            gbStatus.Visible = false;
            gbIProcess.Location = new Point(gbIProcess.Location.X, 0);
            gbIProcess.Visible = true;
            pbOverall.Maximum = 8;
            pbOverall.CustomText = "Cleaning up extising _NEW tables..";
            pbOverall.CustomText = "Checking prerequisites.";
            var clearQuery = new SqlQuery() {CommandText = Resources.Queries_DropAllNew};
            DBManager.ExecuteQuery(clearQuery);
            pbOverall.CustomText = "Import starting..";
            pbOverall.CustomText = "0 table imported out of 8";
            ThreadPool.QueueUserWorkItem(ImportMagic, new object());
            processLock.WaitOne();
            for(int i = 0; i < 7; i++)
            {
                processLock.Reset();
                ThreadPool.QueueUserWorkItem(ImportType, i);
                processLock.WaitOne();
            }
            processLock.Reset();
            ThreadPool.QueueUserWorkItem(RenameTables, new object());
            processLock.WaitOne();
            MessageBox.Show("Import finished!\n Greetings. Now you can enjoy your new skills :P \n\n This program has been made by PENTAGRAM. \n All rights preserved(P).");
            Close();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This program has been made by PENTAGRAM. \n All rights preserved(P).");
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
    }

}
