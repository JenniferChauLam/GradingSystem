using GradingSystem.frm_Collection;
using Guna.UI2.WinForms;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Net.Mail;

namespace GradingSystem
{
    public partial class FrmLogin : Form
    {
        // make movable form 
        bool drag = false;
        Point starting_point = new(0, 0);

        public FrmLogin()
        {
            InitializeComponent();
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Panel1_MouseDown(object sender, MouseEventArgs e)
        {
            drag = true;
            starting_point = new Point(e.X, e.Y);
        }

        private void Panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (drag)
            {
                Point p = PointToScreen(e.Location);
                this.Location = new Point(p.X - starting_point.X, p.Y - starting_point.Y);
            }
        }

        private void Panel1_MouseUp(object sender, MouseEventArgs e)
        {
            drag = false;
        }

        private void CaLbl_Click(object sender, EventArgs e)
        {
            this.Hide();
            FrmSignup frmForgotPassword = new();
            frmForgotPassword.ShowDialog();
            this.Close();
        }

        private static void ChangeBorder(Guna2TextBox textBox)
        {
            textBox.BorderColor = Color.FromArgb(243, 36, 36);
        }

        private static bool IsTextboxEmpty(Guna2TextBox textBox)
        {
            return string.IsNullOrEmpty(textBox.Text);
        }

        private bool CheckandChange(Guna2TextBox textBox)
        {
            if (IsTextboxEmpty(textBox))
            {
                ChangeBorder(textBox);
                return true;
            }
            return false;
        }

        private static string GetRole(string username, string password)
        {
            string role = "";

            using (SqlConnection connection = new("Data Source=TRANTRUONG;Initial Catalog=GradingSystem;Integrated Security=True;Trust Server Certificate=True"))
            {
                connection.Open();
                string query = "select username, password, role from Users where username = @username and password = @password ";
                using (SqlCommand command = new (query, connection))
                {
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@password", password);
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        role = reader["role"].ToString();
                    }
                    reader.Close();
                }
            }

            return role;
        }

        private void LoginBtn_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;
            string role = GetRole(username, password);
            bool isAnyTextboxEmpty = false;

            // Check each textbox using the new method
            isAnyTextboxEmpty |= CheckandChange(txtUsername);
            isAnyTextboxEmpty |= CheckandChange(txtPassword);

            if (isAnyTextboxEmpty)
            {
                return;
            }
            else
            {
                if (role == "teacher")
                {
                    FrmQuestion question = new();
                    question.Show();
                }
                else if (role == "student")
                {
                    FrmExams exams = new();
                    exams.Show();
                }
                else { MessageBox.Show("Invalid username, password"); }
            }
        }

        private void ForgotPw_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmForgotPw frmForgotPw = new ();
            frmForgotPw.ShowDialog();
            this.Close();
        }
    }
}

/* references:
 * https://youtu.be/nh-fleqcds4?si=2erllywErUqHfpi9 ---- cai nay dung de check username & pw trong LoginBtn nhung dung GPT de fix loi "Sequence contains more than one element" 
 * https://stackoverflow.com/questions/72324586/c-sharp-ide0090-new-expression-can-be-simplified-visual-studio-2022

 */