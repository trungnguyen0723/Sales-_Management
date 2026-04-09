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

namespace ASM_Database.Forms
{
    public partial class FormEmployee : Form
    {
        public FormEmployee()
        {
            InitializeComponent();
            LoadTheme();
            LoadEmployeeData();
        }
        private int _selectedEmployeeId = 0;

        private void ClearData()
        {
            _selectedEmployeeId = 0;
            tbFullname.Text = string.Empty;
            tbPosition.Text = string.Empty;
            cbbAuthority.SelectedIndex = 0;
            tbUsername.Text = string.Empty;
            tbPassword.Text = string.Empty;
            tbPhone.Text = string.Empty;
            tbEmail.Text = string.Empty;
        }
        private bool ValidateData(string employeeName, string position, string authority,
                          string username, string password, string phone, string email)
        {
            if (string.IsNullOrEmpty(employeeName))
            {
                MessageBox.Show("Employee Name cannot be blank", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tbFullname.Focus(); return false;
            }
            if (string.IsNullOrEmpty(position))
            {
                MessageBox.Show("Position cannot be blank", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tbPosition.Focus(); return false;
            }
            if (string.IsNullOrEmpty(authority))
            {
                MessageBox.Show("Authority cannot be blank", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cbbAuthority.Focus(); return false;
            }
            if (string.IsNullOrEmpty(username))
            {
                MessageBox.Show("Username cannot be blank", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tbUsername.Focus(); return false;
            }
            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Password cannot be blank", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tbPassword.Focus(); return false;
            }

            if (string.IsNullOrEmpty(phone))
            {
                MessageBox.Show("Phone Number cannot be blank", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tbPhone.Focus(); return false;
            }
            if (!System.Text.RegularExpressions.Regex.IsMatch(phone, @"^(0[3|5|7|8|9])[0-9]{8}$"))
            {
                MessageBox.Show("Invalid phone number.\nMust start with 03, 05, 07, 08, 09 and have 10 digits.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tbPhone.Focus(); return false;
            }

            if (string.IsNullOrEmpty(email))
            {
                MessageBox.Show("Email cannot be blank", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tbEmail.Focus(); return false;
            }
            if (!System.Text.RegularExpressions.Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                MessageBox.Show("Invalid email format.\nExample: example@gmail.com",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tbEmail.Focus(); return false;
            }

            return true;
        }
        private void LoadEmployeeData()
        {
            using (SqlConnection connection = DatabaseConnection.GetConnection())
            {
                if (connection == null) return;
                connection.Open();
                string sql = "SELECT Employee_ID, Employee_Name, Position, Authority, Username, Pass_word, Phone_Number, Email FROM EMPLOYEE WHERE IsActive = 1";
                SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
                DataTable table = new DataTable();
                adapter.Fill(table);
                dtgEmployee.DataSource = table;
            }
        }

        private void LoadTheme() 
        { 
            foreach (Control btns in panelMenu.Controls)
            {
                if (btns is FontAwesome.Sharp.IconButton)
                {
                    Button btn = (Button)btns;  
                    btn.BackColor = ThemeColor.PrimaryColor;
                    btn.ForeColor = Color.White;
                    btn.FlatAppearance.BorderColor = ThemeColor.SecondaryColor;
                }
            }
            label10.ForeColor = ThemeColor.SecondaryColor;
            label1.ForeColor = ThemeColor.PrimaryColor;
        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void dtgEmployee_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = e.RowIndex;
            if (index < 0) return;

            _selectedEmployeeId = Convert.ToInt32(dtgEmployee.Rows[index].Cells["Employee_ID"].Value);
            tbFullname.Text = dtgEmployee.Rows[index].Cells["Employee_Name"].Value.ToString();
            tbPosition.Text = dtgEmployee.Rows[index].Cells["Position"].Value.ToString();
            cbbAuthority.Text = dtgEmployee.Rows[index].Cells["Authority"].Value.ToString();
            tbUsername.Text = dtgEmployee.Rows[index].Cells["Username"].Value.ToString();
            tbPassword.Text = dtgEmployee.Rows[index].Cells["Pass_word"].Value.ToString();
            tbPhone.Text = dtgEmployee.Rows[index].Cells["Phone_Number"].Value.ToString();
            tbEmail.Text = dtgEmployee.Rows[index].Cells["Email"].Value.ToString();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (_selectedEmployeeId == 0)
            {
                MessageBox.Show("Please select an employee to edit.", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string name = tbFullname.Text, position = tbPosition.Text;
            string authority = cbbAuthority.Text, username = tbUsername.Text;
            string password = tbPassword.Text, phone = tbPhone.Text, email = tbEmail.Text;

            if (!ValidateData(name, position, authority, username, password, phone, email)) return;

            using (SqlConnection connection = DatabaseConnection.GetConnection())
            {
                if (connection == null) return;
                connection.Open();

                string checkSql = "SELECT COUNT(*) FROM EMPLOYEE WHERE Username = @Username AND Employee_ID != @Id";
                using (SqlCommand checkCmd = new SqlCommand(checkSql, connection))
                {
                    checkCmd.Parameters.AddWithValue("@Username", username);
                    checkCmd.Parameters.AddWithValue("@Id", _selectedEmployeeId);
                    int count = Convert.ToInt32(checkCmd.ExecuteScalar());
                    if (count > 0)
                    {
                        MessageBox.Show("Username already exists. Please choose another.",
                            "Duplicate Username", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        tbUsername.Focus();
                        return;
                    }
                }

                string sql = @"UPDATE EMPLOYEE SET 
                       Employee_Name = @Name, Position = @Position, Authority = @Authority,
                       Username = @Username, Pass_word = @Password,
                       Phone_Number = @Phone, Email = @Email
                       WHERE Employee_ID = @Id";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@Position", position);
                    command.Parameters.AddWithValue("@Authority", authority);
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", password);
                    command.Parameters.AddWithValue("@Phone", phone);
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@Id", _selectedEmployeeId);

                    int result = command.ExecuteNonQuery();
                    if (result > 0)
                    {
                        MessageBox.Show("Employee updated successfully!", "Success",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ClearData();
                        LoadEmployeeData();
                    }
                    else
                    {
                        MessageBox.Show("Failed to update employee.", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {

            string name = tbFullname.Text, position = tbPosition.Text;
            string authority = cbbAuthority.Text, username = tbUsername.Text;
            string password = tbPassword.Text, phone = tbPhone.Text, email = tbEmail.Text;

            if (!ValidateData(name, position, authority, username, password, phone, email)) return;

            using (SqlConnection connection = DatabaseConnection.GetConnection())
            {
                if (connection == null) return;
                connection.Open();

               
                string checkSql = "SELECT COUNT(*) FROM EMPLOYEE WHERE Username = @Username";
                using (SqlCommand checkCmd = new SqlCommand(checkSql, connection))
                {
                    checkCmd.Parameters.AddWithValue("@Username", username);
                    int count = Convert.ToInt32(checkCmd.ExecuteScalar());
                    if (count > 0)
                    {
                        MessageBox.Show("Username already exists. Please choose another.",
                            "Duplicate Username", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        tbUsername.Focus();
                        return;
                    }
                }

                string sql = @"INSERT INTO EMPLOYEE (Employee_Name, Position, Authority, Username, Pass_word, Phone_Number, Email)
                       VALUES (@Name, @Position, @Authority, @Username, @Password, @Phone, @Email)";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@Position", position);
                    command.Parameters.AddWithValue("@Authority", authority);
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", password);
                    command.Parameters.AddWithValue("@Phone", phone);
                    command.Parameters.AddWithValue("@Email", email);

                    int result = command.ExecuteNonQuery();
                    if (result > 0)
                    {
                        MessageBox.Show("Employee added successfully!", "Success",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ClearData();
                        LoadEmployeeData();
                    }
                    else
                    {
                        MessageBox.Show("Failed to add employee.", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

            if (_selectedEmployeeId == 0)
            {
                MessageBox.Show("Please select an employee to delete.", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult confirm = MessageBox.Show(
                "Are you sure you want to deactivate this employee?",
                "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            using (SqlConnection connection = DatabaseConnection.GetConnection())
            {
                if (connection == null) return;
                connection.Open();

                string sql = "UPDATE EMPLOYEE SET IsActive = 0 WHERE Employee_ID = @Id";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Id", _selectedEmployeeId);
                    int result = command.ExecuteNonQuery();
                    if (result > 0)
                    {
                        MessageBox.Show("Employee deactivated successfully!", "Success",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ClearData();
                        LoadEmployeeData();
                    }
                }
            }
        }

    }
}

