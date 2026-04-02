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

namespace ASM_Database
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
        
        

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        private bool ValidateData(string username, string password)
        {
            if (string.IsNullOrEmpty(username))
            {
                MessageBox.Show("Username cannot be blank", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUserName.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Password cannot be blank", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassWord.Focus();
                return false;
            }

            return true;
        }
        private void ClearData()
        {
            txtUserName.Text = string.Empty;
            txtPassWord.Text = string.Empty;
           
            txtUserName.Focus();
        }

        private void iconButton1_Click(object sender, EventArgs e)
        {

            string username = txtUserName.Text;
            string password = txtPassWord.Text;
            bool isValid = ValidateData(username, password);
            if (!isValid) return;

            using (SqlConnection connection = DatabaseConnection.GetConnection())
            {
                if (connection == null)
                {
                    MessageBox.Show("Cannot connect to database.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                connection.Open();


                string query = @"SELECT Employee_ID, Authority FROM EMPLOYEE 
                 WHERE Username COLLATE SQL_Latin1_General_CP1_CS_AS = @Username 
                 AND Pass_word COLLATE SQL_Latin1_General_CP1_CS_AS = @Password
                 AND IsActive = 1";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", password);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        int employeeId = 0;
                        string authority = "";

                        if (reader.Read())
                        {
                            employeeId = reader.GetInt32(reader.GetOrdinal("Employee_ID"));
                            authority = reader["Authority"].ToString();
                        }

                        if (employeeId > 0)
                        {
                            MessageBox.Show("Login successful!", "Success",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);

                            Form1 mainForm = new Form1(employeeId, authority);
                            mainForm.Show();
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("Invalid username or password. Please try again.",
                                "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            ClearData();
                        }
                    }
                }
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (btnShowPassword.Checked)
            {
                txtPassWord.PasswordChar = '\0'; 
            }
            else
            {
                txtPassWord.PasswordChar = '●'; 
            }
        }
    }

}
