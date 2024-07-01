using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Azure.Core;
using FontAwesome.Sharp;
using Microsoft.Data.SqlClient;

namespace GradingSystem.frm_Collection
{
    public partial class frmMainMenu : Form
    {
        private IconButton currentBtn;
        private Panel leftBorderBtn;
        bool drag = false;
        Point starting_point = new(0, 0);
        string connectionString = "Data Source=JENLAP\\MSSQLSERVERNO;Initial Catalog=GradingSystem;Integrated Security=True;Encrypt=True;Trust Server Certificate=True";



        public frmMainMenu()
        {
            InitializeComponent();
            leftBorderBtn = new();
            leftBorderBtn.Size = new(7, 60);
            MenuPanel.Controls.Add(leftBorderBtn);

            //this.MainPanel.Controls.Clear();
            frmDashboard Home = new()
            {
                Dock = DockStyle.Fill,
                TopLevel = false,
                TopMost = true
            };
            Home.FormBorderStyle = FormBorderStyle.None;
            this.MainPanel.Controls.Add(Home);
            //Home.Show();
            getTests();
        }

        // 255, 204, 112
        // mau nen: 255, 250, 221
        private struct RGBColors
        {
            public static Color color1 = Color.FromArgb(90, 178, 255);
            public static Color color2 = Color.FromArgb(132, 55, 173); // tim sang 
            public static Color color3 = Color.FromArgb(202, 244, 255);
            public static Color color4 = Color.FromArgb(205, 232, 229);
            public static Color color5 = Color.FromArgb(122, 178, 178);
            public static Color color6 = Color.FromArgb(77, 134, 156);
        }

        private void ActiveButton(object sender, Color color)
        {
            if (sender != null)
            {
                DisableButton();
                // button
                currentBtn = (IconButton)sender;
                currentBtn.BackColor = Color.White;
                currentBtn.ForeColor = Color.Black;
                currentBtn.TextAlign = ContentAlignment.MiddleCenter;
                currentBtn.IconColor = Color.Black;
                currentBtn.TextImageRelation = TextImageRelation.TextBeforeImage;
                currentBtn.ImageAlign = ContentAlignment.MiddleRight;
                // left border button
                leftBorderBtn.BackColor = Color.Black;
                leftBorderBtn.Location = new Point(0, currentBtn.Location.Y);
                leftBorderBtn.Visible = true;
                leftBorderBtn.BringToFront();
            }
        }

        private void DisableButton()
        {
            if (currentBtn != null)
            {
                currentBtn.BackColor = Color.FromArgb(34, 102, 141);
                currentBtn.ForeColor = Color.White;
                currentBtn.TextAlign = ContentAlignment.MiddleLeft;
                currentBtn.IconColor = Color.Black;
                currentBtn.TextImageRelation = TextImageRelation.ImageBeforeText;
                currentBtn.ImageAlign = ContentAlignment.MiddleLeft;
            }
        }

        private void DashboardBtn_Click(object sender, EventArgs e)
        {
            ActiveButton(sender, RGBColors.color1);
            this.MainPanel.Controls.Clear();
            frmDashboard frmDashboard = new()
            {
                Dock = DockStyle.Fill,
                TopLevel = false,
                TopMost = true
            };
            frmDashboard.FormBorderStyle = FormBorderStyle.None;
            this.MainPanel.Controls.Add(frmDashboard);
            //frmDashboard.Show();

        }

        private void LogoutBtn_Click(object sender, EventArgs e)
        {
            string message = "Do you want to exit this window?";
            string title = "Close window";
            MessageBoxButtons buttons = MessageBoxButtons.OKCancel;
            DialogResult result = MessageBox.Show(message, title, buttons);

            if (result == DialogResult.OK) { this.Close(); } else { return; }
        }

        private void QuestionsBtn_Click(object sender, EventArgs e)
        {
            ActiveButton(sender, RGBColors.color3);

            this.MainPanel.Controls.Clear();
            FrmQuestion frmQues = new()
            {
                Dock = DockStyle.Fill,
                TopLevel = false,
                TopMost = true
            };
            frmQues.FormBorderStyle = FormBorderStyle.None;
            this.MainPanel.Controls.Add(frmQues);
            frmQues.Show();

        }

        private void ExamsBtn_Click(object sender, EventArgs e)
        {
            ActiveButton(sender, RGBColors.color2);

            this.MainPanel.Controls.Clear();
            FrmExams frmExams = new()
            {
                Dock = DockStyle.Fill,
                TopLevel = false,
                TopMost = true
            };
            frmExams.FormBorderStyle = FormBorderStyle.None;
            this.MainPanel.Controls.Add(frmExams);
            frmExams.Show();
        }

        private void ScoreBtn_Click(object sender, EventArgs e)
        {
            ActiveButton(sender, RGBColors.color5);
            this.MainPanel.Controls.Clear();
            FrmExams frmExams = new()
            {
                Dock = DockStyle.Fill,
                TopLevel = false,
                TopMost = true
            };
            frmExams.FormBorderStyle = FormBorderStyle.None;
            this.MainPanel.Controls.Add(frmExams);
            frmExams.Show();
        }

        private void ContactBtn_Click(object sender, EventArgs e)
        {
            ActiveButton(sender, RGBColors.color4);
            this.MainPanel.Controls.Clear();
            FrmExams frmExams = new()
            {
                Dock = DockStyle.Fill,
                TopLevel = false,
                TopMost = true
            };
            frmExams.FormBorderStyle = FormBorderStyle.None;
            this.MainPanel.Controls.Add(frmExams);
            frmExams.Show();
        }

        private void Reset()
        {
            DisableButton();
            leftBorderBtn.Visible = false;
        }

        // make taskbar 
        private void PanelMove_MouseDown(object sender, MouseEventArgs e)
        {
            drag = true;
            starting_point = new Point(e.X, e.Y);
        }

        private void PanelMove_MouseMove(object sender, MouseEventArgs e)
        {
            if (drag)
            {
                Point p = PointToScreen(e.Location);
                this.Location = new Point(p.X - starting_point.X, p.Y - starting_point.Y);
            }
        }

        private void PanelMove_MouseUp(object sender, MouseEventArgs e)
        {
            drag = false;
        }

        private void getTests()
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand();
            SqlConnection cn = new SqlConnection();
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            cmd.Connection = cn;
            cmd.CommandType = CommandType.Text;
           // cn.ConnectionString = con.connections1;
            cmd.CommandText = "Select  dtransdate, ntotal_pass, nincomeday, ndiesel_exp,  nstartkm, nendkm  from ROUTE2 where ccontrol_no = '" + txtCN.Text + "'";
            da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            ds = new DataSet();
            da.Fill(ds, "Data");
            //dgroute.DataSource = ds;
            //dgroute.DataMember = "Data";
            //dgroute.Columns[0].HeaderText = "Date";
            //dgroute.Columns[1].HeaderText = "Total Passenger";
            //dgroute.Columns[2].HeaderText = "Income Day";
            //dgroute.Columns[3].HeaderText = "Diesel (w/ reciept)";
            //dgroute.Columns[4].HeaderText = "Start";
            //dgroute.Columns[5].HeaderText = "End";

        }

        


        private void LabelFromSQL(string command, List<string> columns, List<Guna.UI2.WinForms.Guna2GroupBox> boxes)
        {
           // string command = "SELECT Top 3 * from Exams";
            //List<string> columns =  new List<string>();
            //columns.Add("start_time");
            //columns.Add("exam_name");
            //columns.Add("teacher_id");
            //List<Guna.UI2.WinForms.Guna2GroupBox> boxes = new List<Guna.UI2.WinForms.Guna2GroupBox>();
            //boxes.Add(guna2GroupBox1);
            //boxes.Add(guna2GroupBox2);
            //boxes.Add(guna2GroupBox3);
            //LabelFromSQL(command, columns, boxes);

            //command = command + "where exam_id =";
            //LabelFromSQL(command, "exam_name", guna2GroupBox2);

            //string command = "SELECT * from Exams";
            //LabelFromSQL(command, "exam_name", guna2GroupBox3);
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand(command, con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            con.Open();
            DataGridView dt = new DataGridView();
            //BindingSource bind = new BindingSource();
           // da.Fill(dt.DataSource);
            dataGridView1.DataSource = dt;
            //return dt;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand query = new SqlCommand(command, connection);
                SqlDataReader oReader = query.ExecuteReader();


                while (oReader.Read()) 
                {
                    for (int i = 0; i < 3; i++)
                    {
                        foreach (Label ctrl in boxes[i].Controls.OfType<Label>())
                        {
                            for (int j = 0; j < columns.Count; j++)
                            {
                                ctrl.Text = oReader[columns[j]].ToString();
                            }
                        }
                    }    

                }

                //while (oReader.Read())
                //{
                //    for (int i = 0; i < 3; i++)
                //    {

                    

                //        for (int j = columns.Count; j < 3; j++)
                //        { foreach (Label ctrl in boxes[i].Controls.OfType<Label>())
                //            {
                //                label2.Text = columns[j] + i.ToString();
                //              ctrl.Text = oReader[columns[j]].ToString(); }
                //        }
                       
                //    }

                //}           



                connection.Close();

            }
        }


    }
}
