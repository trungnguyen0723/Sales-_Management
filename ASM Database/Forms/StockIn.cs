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
    public partial class FormStockIn : Form
    {
        public FormStockIn()
        {
            InitializeComponent();
            LoadTheme();
            LoadProductData();
            LoadCategoryData();
        }
        private int _selectedProductId = 0;
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

            label1.ForeColor = ThemeColor.SecondaryColor;
            label9.ForeColor = ThemeColor.PrimaryColor;
        }
        private void LoadProductData()
        {
            using (SqlConnection connection = DatabaseConnection.GetConnection())
            {
                if (connection == null) return;
                connection.Open();
                string sql = @"SELECT p.Product_ID, p.Product_Name, p.Selling_Price, 
                       p.Inventory_Quantity, c.Category_Name 
                       FROM PRO_DUCT p 
                       LEFT JOIN CATEGORY c ON p.Category_ID = c.Category_ID
                       WHERE p.IsActive = 1";  
                SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
                DataTable table = new DataTable();
                adapter.Fill(table);
                dtgProduct.DataSource = table;
            }
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
                cbbCategory.DataSource = table;
                cbbCategory.DisplayMember = "Category_Name";
                cbbCategory.ValueMember = "Category_ID";
            }
        }
        private bool ValidateData(string productName, string price, string stock)
        {
            if (string.IsNullOrEmpty(productName))
            {
                MessageBox.Show("Product Name cannot be blank", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                tbProductName.Focus(); return false;
            }
            if (string.IsNullOrEmpty(price))
            {
                MessageBox.Show("Price cannot be blank", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                tbPrice.Focus(); return false;
            }
            if (!decimal.TryParse(price, out decimal p) || p < 0)
            {
                MessageBox.Show("Price must be a valid positive number", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                tbPrice.Focus(); return false;
            }
            if (string.IsNullOrEmpty(stock))
            {
                MessageBox.Show("Stock cannot be blank", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                tbStock.Focus(); return false;
            }
            if (!int.TryParse(stock, out int s) || s < 0)
            {
                MessageBox.Show("Stock must be a valid positive number", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                tbStock.Focus(); return false;
            }
            return true;
        }
        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void FormStockIn_Load(object sender, EventArgs e)
        {

        }

        

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dtgProduct_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = e.RowIndex;
            if (index < 0) return;

            _selectedProductId = Convert.ToInt32(dtgProduct.Rows[index].Cells["Product_ID"].Value);
            tbProductName.Text = dtgProduct.Rows[index].Cells["Product_Name"].Value.ToString();
            tbPrice.Text = dtgProduct.Rows[index].Cells["Selling_Price"].Value.ToString();
            tbStock.Text = dtgProduct.Rows[index].Cells["Inventory_Quantity"].Value.ToString();

            string categoryName = dtgProduct.Rows[index].Cells["Category_Name"].Value.ToString();
            foreach (DataRowView row in cbbCategory.Items)
            {
                if (row["Category_Name"].ToString() == categoryName)
                {
                    cbbCategory.SelectedItem = row;
                    break;
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string name = tbProductName.Text.Trim();
            string price = tbPrice.Text.Trim();
            string stock = tbStock.Text.Trim();

            if (!ValidateData(name, price, stock)) return;
            if (cbbCategory.SelectedItem == null)
            {
                MessageBox.Show("Please select a category.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int categoryId = Convert.ToInt32((cbbCategory.SelectedItem as DataRowView)["Category_ID"]);

            using (SqlConnection connection = DatabaseConnection.GetConnection())
            {
                if (connection == null) return;
                connection.Open();
                string sql = @"INSERT INTO PRO_DUCT (Product_Name, Selling_Price, Inventory_Quantity, Category_ID)
                           VALUES (@Name, @Price, @Stock, @CategoryID)";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@Price", decimal.Parse(price));
                    command.Parameters.AddWithValue("@Stock", int.Parse(stock));
                    command.Parameters.AddWithValue("@CategoryID", categoryId);

                    int result = command.ExecuteNonQuery();
                    if (result > 0)
                    {
                        MessageBox.Show("Product added successfully!", "Success",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ClearData();
                        LoadProductData();
                    }
                    else
                    {
                        MessageBox.Show("Failed to add product.", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (_selectedProductId == 0)
            {
                MessageBox.Show("Please select a product to edit.", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string name = tbProductName.Text.Trim();
            string price = tbPrice.Text.Trim();
            string stock = tbStock.Text.Trim();

            if (!ValidateData(name, price, stock)) return;
            if (cbbCategory.SelectedItem == null)
            {
                MessageBox.Show("Please select a category.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int categoryId = Convert.ToInt32((cbbCategory.SelectedItem as DataRowView)["Category_ID"]);

            using (SqlConnection connection = DatabaseConnection.GetConnection())
            {
                if (connection == null) return;
                connection.Open();
                string sql = @"UPDATE PRO_DUCT SET 
                           Product_Name = @Name, Selling_Price = @Price,
                           Inventory_Quantity = @Stock, Category_ID = @CategoryID
                           WHERE Product_ID = @Id";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@Price", decimal.Parse(price));
                    command.Parameters.AddWithValue("@Stock", int.Parse(stock));
                    command.Parameters.AddWithValue("@CategoryID", categoryId);
                    command.Parameters.AddWithValue("@Id", _selectedProductId);

                    int result = command.ExecuteNonQuery();
                    if (result > 0)
                    {
                        MessageBox.Show("Product updated successfully!", "Success",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ClearData();
                        LoadProductData();
                    }
                    else
                    {
                        MessageBox.Show("Failed to update product.", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        private void ClearData()
        {
            _selectedProductId = 0;
            tbProductName.Text = string.Empty;
            tbPrice.Text = string.Empty;
            tbStock.Text = string.Empty;
            cbbCategory.SelectedIndex = 0;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

            if (_selectedProductId == 0)
            {
                MessageBox.Show("Please select a product to delete.", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult confirm = MessageBox.Show(
                "Are you sure you want to delete this product?\nTransaction history will be preserved.",
                "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            using (SqlConnection connection = DatabaseConnection.GetConnection())
            {
                if (connection == null) return;
                connection.Open();

               
                string sql = "UPDATE PRO_DUCT SET IsActive = 0 WHERE Product_ID = @Id";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Id", _selectedProductId);
                    int result = command.ExecuteNonQuery();
                    if (result > 0)
                    {
                        MessageBox.Show("Product deleted successfully!", "Success",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ClearData();
                        LoadProductData();
                    }
                    else
                    {
                        MessageBox.Show("Failed to delete product.", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
