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
    public partial class FormCustomer : Form
    {
        private int _selectedCustomerId = 0;

        public FormCustomer()
        {
            InitializeComponent();
            LoadCustomerData();
            LoadTheme();

        }
        private void LoadCustomerData(string search = "")
        {
            using (SqlConnection connection = DatabaseConnection.GetConnection())
            {
                if (connection == null) return;
                connection.Open();
                string sql = @"SELECT Customer_ID, Customer_Name, Add_ress, Phone_Number, Email 
                           FROM CUSTOMER
                           WHERE Customer_Name LIKE @Search 
                           OR Phone_Number LIKE @Search
                           OR Email LIKE @Search";
                SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
                adapter.SelectCommand.Parameters.AddWithValue("@Search", "%" + search + "%");
                DataTable table = new DataTable();
                adapter.Fill(table);
                dtgCustomer.DataSource = table;
            }
        }
        private void LoadTheme()
        {
            ApplyTheme(this);
        }

        private void ApplyTheme(Control parent)
        {
            foreach (Control ctrl in parent.Controls)
            {
                
                if (ctrl is Button btn && btn.Name != "btnSearch" && btn.Name != "btnRefresh")
                {
                    btn.BackColor = ThemeColor.PrimaryColor;
                    btn.ForeColor = Color.White;
                    btn.FlatAppearance.BorderColor = ThemeColor.SecondaryColor;
                }
                
                else if (ctrl is FontAwesome.Sharp.IconButton iconBtn && iconBtn.Name != "btnSearch" && iconBtn.Name != "btnRefresh")
                {
                    iconBtn.BackColor = ThemeColor.PrimaryColor;
                    iconBtn.ForeColor = Color.White;
                    iconBtn.FlatAppearance.BorderColor = ThemeColor.SecondaryColor;
                }

                if (ctrl.HasChildren)
                    ApplyTheme(ctrl);
            }
        }

        private bool ValidateData(string name, string phone, string email)
        {
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Customer Name cannot be blank", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                tbCustomerName.Focus(); return false;
            }
            if (string.IsNullOrEmpty(phone))
            {
                MessageBox.Show("Phone Number cannot be blank", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                tbPhone.Focus(); return false;
            }
            if (!System.Text.RegularExpressions.Regex.IsMatch(phone, @"^(0[3|5|7|8|9])[0-9]{8}$"))
            {
                MessageBox.Show("Invalid phone number.\nMust start with 03,05,07,08,09 and have 10 digits.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tbPhone.Focus(); return false;
            }
            if (!string.IsNullOrEmpty(email) &&
                !System.Text.RegularExpressions.Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                MessageBox.Show("Invalid email format.\nExample: example@gmail.com",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tbEmail.Focus(); return false;
            }
            return true;
        }

        private void tbCustomerName_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void tbAddress_TextChanged(object sender, EventArgs e)
        {

        }

        private void tbPhone_TextChanged(object sender, EventArgs e)
        {

        }

        private void tbEmail_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (_selectedCustomerId == 0)
            {
                MessageBox.Show("Please select a customer to delete.", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

           
            using (SqlConnection connection = DatabaseConnection.GetConnection())
            {
                if (connection == null) return;
                connection.Open();

                string checkSql = "SELECT COUNT(*) FROM SALE_ORDER WHERE Customer_ID = @Id";
                using (SqlCommand checkCmd = new SqlCommand(checkSql, connection))
                {
                    checkCmd.Parameters.AddWithValue("@Id", _selectedCustomerId);
                    int count = Convert.ToInt32(checkCmd.ExecuteScalar());
                    if (count > 0)
                    {
                        MessageBox.Show("Cannot delete. Customer has existing orders.",
                            "Delete Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                DialogResult confirm = MessageBox.Show(
                    "Are you sure you want to delete this customer?",
                    "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirm != DialogResult.Yes) return;

                string sql = "DELETE FROM CUSTOMER WHERE Customer_ID = @Id";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Id", _selectedCustomerId);
                    int result = command.ExecuteNonQuery();
                    if (result > 0)
                    {
                        MessageBox.Show("Customer deleted successfully!", "Success",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ClearData();
                        LoadCustomerData();
                    }
                    else
                    {
                        MessageBox.Show("Failed to delete customer.", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (_selectedCustomerId == 0)
            {
                MessageBox.Show("Please select a customer to edit.", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string name = tbCustomerName.Text.Trim();
            string address = tbAddress.Text.Trim();
            string phone = tbPhone.Text.Trim();
            string email = tbEmail.Text.Trim();

            if (!ValidateData(name, phone, email)) return;

            using (SqlConnection connection = DatabaseConnection.GetConnection())
            {
                if (connection == null) return;
                connection.Open();

                
                string checkSql = "SELECT COUNT(*) FROM CUSTOMER WHERE Phone_Number = @Phone AND Customer_ID != @Id";
                using (SqlCommand checkCmd = new SqlCommand(checkSql, connection))
                {
                    checkCmd.Parameters.AddWithValue("@Phone", phone);
                    checkCmd.Parameters.AddWithValue("@Id", _selectedCustomerId);
                    int count = Convert.ToInt32(checkCmd.ExecuteScalar());
                    if (count > 0)
                    {
                        MessageBox.Show("Phone number already exists!", "Duplicate",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        tbPhone.Focus(); return;
                    }
                }

                string sql = @"UPDATE CUSTOMER SET 
                           Customer_Name = @Name, Add_ress = @Address,
                           Phone_Number = @Phone, Email = @Email
                           WHERE Customer_ID = @Id";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@Address", address);
                    command.Parameters.AddWithValue("@Phone", phone);
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@Id", _selectedCustomerId);

                    int result = command.ExecuteNonQuery();
                    if (result > 0)
                    {
                        MessageBox.Show("Customer updated successfully!", "Success",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ClearData();
                        LoadCustomerData();
                    }
                    else
                    {
                        MessageBox.Show("Failed to update customer.", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string name = tbCustomerName.Text.Trim();
            string address = tbAddress.Text.Trim();
            string phone = tbPhone.Text.Trim();
            string email = tbEmail.Text.Trim();

            if (!ValidateData(name, phone, email)) return;

            using (SqlConnection connection = DatabaseConnection.GetConnection())
            {
                if (connection == null) return;
                connection.Open();

                // Kiểm tra số điện thoại trùng
                string checkSql = "SELECT COUNT(*) FROM CUSTOMER WHERE Phone_Number = @Phone";
                using (SqlCommand checkCmd = new SqlCommand(checkSql, connection))
                {
                    checkCmd.Parameters.AddWithValue("@Phone", phone);
                    int count = Convert.ToInt32(checkCmd.ExecuteScalar());
                    if (count > 0)
                    {
                        MessageBox.Show("Phone number already exists!", "Duplicate",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        tbPhone.Focus(); return;
                    }
                }

                string sql = @"INSERT INTO CUSTOMER (Customer_Name, Add_ress, Phone_Number, Email)
                           VALUES (@Name, @Address, @Phone, @Email)";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@Address", address);
                    command.Parameters.AddWithValue("@Phone", phone);
                    command.Parameters.AddWithValue("@Email", email);

                    int result = command.ExecuteNonQuery();
                    if (result > 0)
                    {
                        MessageBox.Show("Customer added successfully!", "Success",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ClearData();
                        LoadCustomerData();
                    }
                    else
                    {
                        MessageBox.Show("Failed to add customer.", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }


        private void tbSearch_TextChanged(object sender, EventArgs e)
        {
            LoadCustomerData(tbSearch.Text.Trim());
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {

        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            tbSearch.Text = string.Empty;
            LoadCustomerData();
        }
        private void tbSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                LoadCustomerData(tbSearch.Text.Trim());
        }
        private void ClearData()
        {
            _selectedCustomerId = 0;
            tbCustomerName.Text = string.Empty;
            tbAddress.Text = string.Empty;
            tbPhone.Text = string.Empty;
            tbEmail.Text = string.Empty;
        }
        

        private void dtgCustomer_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = e.RowIndex;
            if (index < 0) return;

            _selectedCustomerId = Convert.ToInt32(dtgCustomer.Rows[index].Cells["Customer_ID"].Value);
            tbCustomerName.Text = dtgCustomer.Rows[index].Cells["Customer_Name"].Value.ToString();
            tbAddress.Text = dtgCustomer.Rows[index].Cells["Add_ress"].Value.ToString();
            tbPhone.Text = dtgCustomer.Rows[index].Cells["Phone_Number"].Value.ToString();
            tbEmail.Text = dtgCustomer.Rows[index].Cells["Email"].Value.ToString();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void dtgCustomer_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
