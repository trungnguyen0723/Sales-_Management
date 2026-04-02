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
    public partial class FormOrders : Form
    {
        private int _employeeId;
        private DataTable _orderTable = new DataTable();
        private bool _isLoading = false;
        public FormOrders(int employeeId)
        {
            InitializeComponent();
            LoadTheme();
            _employeeId = employeeId;
            InitOrderTable();
            LoadProductData();
            LoadCategoryData();
            LoadCustomerData();
        }
        private void InitOrderTable()
        {
            _orderTable.Columns.Add("Product_ID", typeof(int));
            _orderTable.Columns.Add("Product_Name", typeof(string));
            _orderTable.Columns.Add("Quantity", typeof(int));
            _orderTable.Columns.Add("Unit_Price", typeof(decimal));
            _orderTable.Columns.Add("Subtotal", typeof(decimal));
            dtgOrder.DataSource = _orderTable.DefaultView;
        }
        private void LoadProductData()
        {
            using (SqlConnection connection = DatabaseConnection.GetConnection())
            {
                if (connection == null) return;
                connection.Open();
                
                string sql = "SELECT Product_ID, Product_Name, Selling_Price, Inventory_Quantity FROM PRO_DUCT WHERE Inventory_Quantity > 0 AND IsActive = 1";
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

        private void LoadTheme()
        {
            
            ApplyThemeToControls(this);

            
            label1.ForeColor = ThemeColor.SecondaryColor;
            label10.ForeColor = ThemeColor.SecondaryColor;
            label11.ForeColor = ThemeColor.PrimaryColor;
        }

        private void ApplyThemeToControls(Control parent)
        {
            foreach (Control ctrl in parent.Controls)
            {
                
                if (ctrl is Button btn)
                {
                    btn.BackColor = ThemeColor.PrimaryColor;
                    btn.ForeColor = Color.White;
                    btn.FlatAppearance.BorderColor = ThemeColor.SecondaryColor;
                    btn.FlatStyle = FlatStyle.Flat;

                   
                    btn.FlatAppearance.MouseOverBackColor = ControlPaint.Light(ThemeColor.PrimaryColor);
                    btn.FlatAppearance.MouseDownBackColor = ControlPaint.Dark(ThemeColor.PrimaryColor);
                }

               
                if (ctrl.HasChildren)
                {
                    ApplyThemeToControls(ctrl);
                }
            }
        }
        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void FormOrders_Load(object sender, EventArgs e)
        {

        }

        private void CbbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_isLoading) return; 
            if (!(cbbCategory.SelectedItem is DataRowView row)) return;

            int categoryId = Convert.ToInt32(row["Category_ID"]);
            _isLoading = true;

            using (SqlConnection connection = DatabaseConnection.GetConnection())
            {
                if (connection == null) { _isLoading = false; return; }
                connection.Open();
                string sql = "SELECT Product_ID, Product_Name FROM PRO_DUCT WHERE Category_ID = @CategoryID AND Inventory_Quantity > 0";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@CategoryID", categoryId);
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    cbbProductID.DataSource = table;
                    cbbProductID.DisplayMember = "Product_ID";
                    cbbProductID.ValueMember = "Product_ID";
                }
            }
            _isLoading = false;
        }

        private void cbbProductID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbbProductID.SelectedItem == null) return;

            // ✅ Sửa: lấy giá trị từ SelectedItem thay vì SelectedValue
            if (cbbProductID.SelectedItem is DataRowView row)
            {
                int productId = Convert.ToInt32(row["Product_ID"]);

                using (SqlConnection connection = DatabaseConnection.GetConnection())
                {
                    if (connection == null) return;
                    connection.Open();
                    string sql = "SELECT Product_Name, Selling_Price FROM PRO_DUCT WHERE Product_ID = @ProductID";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@ProductID", productId);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                tbProductName.Text = reader["Product_Name"].ToString();
                                tbPrice.Text = reader["Selling_Price"].ToString();
                            }
                        }
                    }
                }
            }
        }

        private void tbProductName_TextChanged(object sender, EventArgs e)
        {

        }

        private void tbPrice_TextChanged(object sender, EventArgs e)
        {

        }

        private void tbQuantity_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {

            if (_isLoading) return;

            if (cbbProductID.SelectedItem == null)
            {
                MessageBox.Show("Please select a product.", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!(cbbProductID.SelectedItem is DataRowView row)) return;
            int productId = Convert.ToInt32(row["Product_ID"]);

            if (string.IsNullOrEmpty(tbQuantity.Text))
            {
                MessageBox.Show("Please enter quantity.", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tbQuantity.Focus();
                return;
            }
            if (!int.TryParse(tbQuantity.Text, out int quantity) || quantity <= 0)
            {
                MessageBox.Show("Quantity must be a positive number.", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tbQuantity.Focus();
                return;
            }
            if (string.IsNullOrEmpty(tbPrice.Text) || !decimal.TryParse(tbPrice.Text, out decimal price))
            {
                MessageBox.Show("Please select a product first.", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string productName = tbProductName.Text;

            // Kiểm tra tồn kho
            using (SqlConnection connection = DatabaseConnection.GetConnection())
            {
                if (connection == null) return;
                connection.Open();
                string sql = "SELECT Inventory_Quantity FROM PRO_DUCT WHERE Product_ID = @ProductID";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@ProductID", productId);
                    int stock = Convert.ToInt32(command.ExecuteScalar());
                    if (quantity > stock)
                    {
                        MessageBox.Show($"Not enough stock. Available: {stock}", "Warning",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
            }

            //  Cùng sản phẩm → gộp vào 1 dòng, cộng thêm số lượng
            foreach (DataRow orderRow in _orderTable.Rows)
            {
                if (Convert.ToInt32(orderRow["Product_ID"]) == productId)
                {
                    int newQty = Convert.ToInt32(orderRow["Quantity"]) + quantity;
                    orderRow["Quantity"] = newQty;
                    orderRow["Subtotal"] = newQty * price;
                    UpdateTotal();
                    ClearInputData();
                    return;
                }
            }

            // Sản phẩm mới → thêm 1 dòng mới riêng
            _orderTable.Rows.Add(productId, productName, quantity, price, quantity * price);
            UpdateTotal();
            ClearInputData();
        }
        

        private void btRemove_Click(object sender, EventArgs e)
        {
            if (dtgOrder.CurrentRow == null)
            {
                MessageBox.Show("Please select an order to remove.", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int index = dtgOrder.CurrentRow.Index;
            _orderTable.Rows.RemoveAt(index);
            UpdateTotal();
        }
        private void LoadCustomerData()
        {
            using (SqlConnection connection = DatabaseConnection.GetConnection())
            {
                if (connection == null) return;
                connection.Open();
                string sql = "SELECT Customer_ID, Customer_Name, Phone_Number FROM CUSTOMER";
                SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
                DataTable table = new DataTable();
                adapter.Fill(table);

                cbbCustomer.DataSource = table;
                cbbCustomer.DisplayMember = "Customer_Name";
                cbbCustomer.ValueMember = "Customer_ID";

                // ✅ Autocomplete - gõ tên hoặc ID tự nhảy
                cbbCustomer.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                cbbCustomer.AutoCompleteSource = AutoCompleteSource.ListItems;
            }
        }

        private void btClear_Click(object sender, EventArgs e)
        {
            _orderTable.Rows.Clear();
            UpdateTotal();
            ClearInputData();
            ClearOrder();
        }

        private void ClearOrder()
        {
            _orderTable.Rows.Clear();
            UpdateTotal();
            ClearInputData();
        }

        private void btnPayOrder_Click(object sender, EventArgs e)
        {
            if (_orderTable.Rows.Count == 0)
            {
                MessageBox.Show("No products in order.", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            
            int customerId = 0;
            string customerName = "Guest";
            if (cbbCustomer.SelectedItem is DataRowView customerRow)
            {
                customerId = Convert.ToInt32(customerRow["Customer_ID"]);
                customerName = customerRow["Customer_Name"].ToString();
            }

            decimal total = 0;
            foreach (DataRow row in _orderTable.Rows)
                total += Convert.ToDecimal(row["Subtotal"]);

            DialogResult confirm = MessageBox.Show(
                $"Customer: {customerName}\nTotal: {total:N0} VND\n\nConfirm payment?",
                "Confirm Payment", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            using (SqlConnection connection = DatabaseConnection.GetConnection())
            {
                if (connection == null) return;
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();
                try
                {
                   
                    string orderSql = @"INSERT INTO SALE_ORDER (Sales_Date, Customer_ID, Employee_ID, Total_Amount, Discount_Amount, Notes)
                                VALUES (GETDATE(), @CustomerID, @EmployeeID, @Total, 0, N'POS Order');
                                SELECT SCOPE_IDENTITY();";
                    int orderId = 0;
                    using (SqlCommand cmd = new SqlCommand(orderSql, connection, transaction))
                    {
                        cmd.Parameters.AddWithValue("@CustomerID", customerId == 0 ? (object)DBNull.Value : customerId);
                        cmd.Parameters.AddWithValue("@EmployeeID", _employeeId);
                        cmd.Parameters.AddWithValue("@Total", total);
                        orderId = Convert.ToInt32(cmd.ExecuteScalar());
                    }

                    foreach (DataRow row in _orderTable.Rows)
                    {
                        string detailSql = @"INSERT INTO SALES_DETAIL (Sales_Order_ID, Product_ID, Sales_Quantity, Unit_Price, Subtotal)
                                     VALUES (@OrderID, @ProductID, @Quantity, @Price, @Subtotal)";
                        using (SqlCommand cmd = new SqlCommand(detailSql, connection, transaction))
                        {
                            cmd.Parameters.AddWithValue("@OrderID", orderId);
                            cmd.Parameters.AddWithValue("@ProductID", row["Product_ID"]);
                            cmd.Parameters.AddWithValue("@Quantity", row["Quantity"]);
                            cmd.Parameters.AddWithValue("@Price", row["Unit_Price"]);
                            cmd.Parameters.AddWithValue("@Subtotal", row["Subtotal"]);
                            cmd.ExecuteNonQuery();
                        }

                        string stockSql = "UPDATE PRO_DUCT SET Inventory_Quantity = Inventory_Quantity - @Quantity WHERE Product_ID = @ProductID";
                        using (SqlCommand cmd = new SqlCommand(stockSql, connection, transaction))
                        {
                            cmd.Parameters.AddWithValue("@Quantity", row["Quantity"]);
                            cmd.Parameters.AddWithValue("@ProductID", row["Product_ID"]);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    transaction.Commit();
                    MessageBox.Show("Payment successful!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearOrder();
                    LoadProductData();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show("Payment failed: " + ex.Message, "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void ClearInputData()
        {
            tbProductName.Text = string.Empty;
            tbPrice.Text = string.Empty;
            tbQuantity.Text = string.Empty;
        }

        private void btnReceipt_Click(object sender, EventArgs e)
        {
            if (_orderTable.Rows.Count == 0)
            {
                MessageBox.Show("No orders to print.", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // ✅ Lấy thông tin khách hàng
            string customerId = "N/A";
            string customerName = "Guest";
            if (cbbCustomer.SelectedItem is DataRowView customerRow)
            {
                customerId = customerRow["Customer_ID"].ToString();
                customerName = customerRow["Customer_Name"].ToString();
            }

            string receipt = "========== RECEIPT ==========\n";
            receipt += $"Date: {DateTime.Now:dd/MM/yyyy HH:mm}\n";
            receipt += "------------------------------\n";
            receipt += $"Customer ID : {customerId}\n";      // ✅ Thêm
            receipt += $"Customer    : {customerName}\n";    // ✅ Thêm
            receipt += "------------------------------\n";
            foreach (DataRow row in _orderTable.Rows)
            {
                receipt += $"{row["Product_Name"]}\n";
                receipt += $"  {row["Quantity"]} x {Convert.ToDecimal(row["Unit_Price"]):N0} = {Convert.ToDecimal(row["Subtotal"]):N0} VND\n";
            }
            receipt += "------------------------------\n";
            receipt += $"TOTAL: {tbTotalPrice.Text}\n";
            receipt += "==============================\n";
            receipt += "     Thank you! Come again!   ";

            MessageBox.Show(receipt, "Receipt", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void UpdateTotal()
        {
            decimal total = 0;
            foreach (DataRow row in _orderTable.Rows)
                total += Convert.ToDecimal(row["Subtotal"]);

            tbTotalPrice.Text = total.ToString("N0") + " VND";
        }

        private void dtgProduct_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = e.RowIndex;
            if (index < 0) return;

            tbProductName.Text = dtgProduct.Rows[index].Cells["Product_Name"].Value.ToString();
            tbPrice.Text = dtgProduct.Rows[index].Cells["Selling_Price"].Value.ToString();
            cbbProductID.Text = dtgProduct.Rows[index].Cells["Product_ID"].Value.ToString();
        }

        private void dtgOrder_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = e.RowIndex;
            if (index < 0) return;

            _isLoading = true; //  Chặn trigger SelectedIndexChanged khi set value

            // Lấy thông tin từ row được chọn
            int productId = Convert.ToInt32(dtgProduct.Rows[index].Cells["Product_ID"].Value);
            string productName = dtgProduct.Rows[index].Cells["Product_Name"].Value.ToString();
            string price = dtgProduct.Rows[index].Cells["Selling_Price"].Value.ToString();

            // Điền vào textbox
            tbProductName.Text = productName;
            tbPrice.Text = price;

            // Lấy Category_ID của sản phẩm từ database
            using (SqlConnection connection = DatabaseConnection.GetConnection())
            {
                if (connection == null) { _isLoading = false; return; }
                connection.Open();

                string sql = "SELECT Category_ID FROM PRO_DUCT WHERE Product_ID = @ProductID";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@ProductID", productId);
                    int categoryId = Convert.ToInt32(command.ExecuteScalar());

                    // ✅ Set Category ComboBox theo categoryId
                    foreach (DataRowView rowView in cbbCategory.Items)
                    {
                        if (Convert.ToInt32(rowView["Category_ID"]) == categoryId)
                        {
                            cbbCategory.SelectedItem = rowView;
                            break;
                        }
                    }
                }

                //  Load Product ID theo category vừa chọn
                string sqlProduct = "SELECT Product_ID, Product_Name FROM PRO_DUCT WHERE Category_ID = (SELECT Category_ID FROM PRO_DUCT WHERE Product_ID = @ProductID) AND Inventory_Quantity > 0";
                using (SqlCommand command = new SqlCommand(sqlProduct, connection))
                {
                    command.Parameters.AddWithValue("@ProductID", productId);
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    cbbProductID.DataSource = table;
                    cbbProductID.DisplayMember = "Product_ID";
                    cbbProductID.ValueMember = "Product_ID";
                }

                //  Set Product ID ComboBox theo productId
                foreach (DataRowView rowView in cbbProductID.Items)
                {
                    if (Convert.ToInt32(rowView["Product_ID"]) == productId)
                    {
                        cbbProductID.SelectedItem = rowView;
                        break;
                    }
                }
            }

            _isLoading = false;
        }

        private void dtgProduct_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void cbbCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }
    }
}
