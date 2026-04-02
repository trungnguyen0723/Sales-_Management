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
    public partial class FormCategory : Form
    {
        private int _selectedCategoryId = 0;

        public FormCategory()
        {
            InitializeComponent();
            LoadTheme();
            LoadCategoryData();
        }

        private void label10_Click(object sender, EventArgs e)
        {

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

        private void LoadCategoryData()
        {
            using (SqlConnection connection = DatabaseConnection.GetConnection())
            {
                if (connection == null) return;
                connection.Open();
                string sql = "SELECT Category_ID, Category_Name FROM CATEGORY";
                SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
                DataTable table = new DataTable();
                adapter.Fill(table);
                dtgCategory.DataSource = table;
            }
        }
        private bool ValidateData(string categoryName)
        {
            if (string.IsNullOrEmpty(categoryName))
            {
                MessageBox.Show("Category Name cannot be blank", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                tbCategoryName.Focus();
                return false;
            }
            if (categoryName.Length > 100)
            {
                MessageBox.Show("Category Name cannot exceed 100 characters", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                tbCategoryName.Focus();
                return false;
            }
            return true;
        }

        // ✅ Click row → điền vào textbox
        private void dtgCategory_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = e.RowIndex;
            if (index < 0) return;

            _selectedCategoryId = Convert.ToInt32(dtgCategory.Rows[index].Cells["Category_ID"].Value);
            tbCategoryName.Text = dtgCategory.Rows[index].Cells["Category_Name"].Value.ToString();
        }


        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void FormCategory_Load(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btn2_Click(object sender, EventArgs e)
        {
            if (_selectedCategoryId == 0)
            {
                MessageBox.Show("Please select a category to edit.", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string categoryName = tbCategoryName.Text.Trim();
            if (!ValidateData(categoryName)) return;

            using (SqlConnection connection = DatabaseConnection.GetConnection())
            {
                if (connection == null) return;
                connection.Open();

                // Kiểm tra trùng tên với category khác
                string checkSql = "SELECT COUNT(*) FROM CATEGORY WHERE Category_Name = @Name AND Category_ID != @Id";
                using (SqlCommand checkCmd = new SqlCommand(checkSql, connection))
                {
                    checkCmd.Parameters.AddWithValue("@Name", categoryName);
                    checkCmd.Parameters.AddWithValue("@Id", _selectedCategoryId);
                    int count = Convert.ToInt32(checkCmd.ExecuteScalar());
                    if (count > 0)
                    {
                        MessageBox.Show("Category Name already exists!", "Duplicate",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        tbCategoryName.Focus();
                        return;
                    }
                }

                string sql = "UPDATE CATEGORY SET Category_Name = @Name WHERE Category_ID = @Id";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Name", categoryName);
                    command.Parameters.AddWithValue("@Id", _selectedCategoryId);
                    int result = command.ExecuteNonQuery();
                    if (result > 0)
                    {
                        MessageBox.Show("Category updated successfully!", "Success",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ClearData();
                        LoadCategoryData();
                    }
                    else
                    {
                        MessageBox.Show("Failed to update category.", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btn_Click(object sender, EventArgs e)
        {
            if (_selectedCategoryId == 0)
            {
                MessageBox.Show("Please select a category to delete.", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Kiểm tra category có đang được dùng trong PRO_DUCT không
            using (SqlConnection connection = DatabaseConnection.GetConnection())
            {
                if (connection == null) return;
                connection.Open();

                string checkSql = "SELECT COUNT(*) FROM PRO_DUCT WHERE Category_ID = @Id";
                using (SqlCommand checkCmd = new SqlCommand(checkSql, connection))
                {
                    checkCmd.Parameters.AddWithValue("@Id", _selectedCategoryId);
                    int count = Convert.ToInt32(checkCmd.ExecuteScalar());
                    if (count > 0)
                    {
                        MessageBox.Show(
                            "Cannot delete this category because it has existing products.\nPlease remove related products first.",
                            "Delete Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                DialogResult confirm = MessageBox.Show(
                    "Are you sure you want to delete this category?",
                    "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirm != DialogResult.Yes) return;

                string sql = "DELETE FROM CATEGORY WHERE Category_ID = @Id";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Id", _selectedCategoryId);
                    int result = command.ExecuteNonQuery();
                    if (result > 0)
                    {
                        MessageBox.Show("Category deleted successfully!", "Success",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ClearData();
                        LoadCategoryData();
                    }
                    else
                    {
                        MessageBox.Show("Failed to delete category.", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        private void ClearData()
        {
            _selectedCategoryId = 0;
            tbCategoryName.Text = string.Empty;
        }

        private void btn1_Click(object sender, EventArgs e)
        {
            string categoryName = tbCategoryName.Text.Trim();
            if (!ValidateData(categoryName)) return;

            using (SqlConnection connection = DatabaseConnection.GetConnection())
            {
                if (connection == null) return;
                connection.Open();

                // Kiểm tra trùng tên
                string checkSql = "SELECT COUNT(*) FROM CATEGORY WHERE Category_Name = @Name";
                using (SqlCommand checkCmd = new SqlCommand(checkSql, connection))
                {
                    checkCmd.Parameters.AddWithValue("@Name", categoryName);
                    int count = Convert.ToInt32(checkCmd.ExecuteScalar());
                    if (count > 0)
                    {
                        MessageBox.Show("Category Name already exists!", "Duplicate",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        tbCategoryName.Focus();
                        return;
                    }
                }

                string sql = "INSERT INTO CATEGORY (Category_Name) VALUES (@Name)";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Name", categoryName);
                    int result = command.ExecuteNonQuery();
                    if (result > 0)
                    {
                        MessageBox.Show("Category added successfully!", "Success",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ClearData();
                        LoadCategoryData();
                    }
                    else
                    {
                        MessageBox.Show("Failed to add category.", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void dtgCategory_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dtgCategory_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            int index = e.RowIndex;
            if (index < 0) return;

            _selectedCategoryId = Convert.ToInt32(dtgCategory.Rows[index].Cells["Category_ID"].Value);
            tbCategoryName.Text = dtgCategory.Rows[index].Cells["Category_Name"].Value.ToString();
        }
    }
}
