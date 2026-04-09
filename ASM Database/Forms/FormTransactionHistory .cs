using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;
using System.Windows.Forms;

namespace ASM_Database.Forms
{
    public partial class FormTransactionHistory : Form
    {
        public FormTransactionHistory()
        {
            InitializeComponent();
            btnSearch.Click += btnSearch_Click;
            btnRefresh.Click += btnRefresh_Click;
            LoadData();
        }

        private void LoadData(string keyword = "")
        {
            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string sql = @"
                        SELECT 
                            so.Sales_Order_ID AS Order_ID,
                            so.Sales_Date AS Order_Date,
                            c.Customer_ID,
                            c.Customer_Name,
                            so.Total_Amount,
                            so.Notes AS Status
                        FROM SALE_ORDER so
                        LEFT JOIN CUSTOMER c ON so.Customer_ID = c.Customer_ID
                        WHERE 1=1 ";

                    if (!string.IsNullOrWhiteSpace(keyword))
                    {
                        sql += "AND (c.Customer_Name LIKE @keyword";
                        if (int.TryParse(keyword, out int customerId))
                        {
                            sql += " OR c.Customer_ID = @customerId";
                        }
                        sql += ")";
                    }

                    sql += " ORDER BY so.Sales_Date DESC";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        if (!string.IsNullOrWhiteSpace(keyword))
                        {
                            cmd.Parameters.AddWithValue("@keyword", "%" + keyword + "%");
                            if (int.TryParse(keyword, out int customerId))
                            {
                                cmd.Parameters.AddWithValue("@customerId", customerId);
                            }
                        }

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        dtgTransaction.DataSource = dt;

                        if (dtgTransaction.Columns.Count > 0)
                        {
                            dtgTransaction.Columns["Order_ID"].HeaderText = "OrderCode";
                            dtgTransaction.Columns["Order_Date"].HeaderText = "Date";
                            dtgTransaction.Columns["Customer_ID"].HeaderText = "Customer Code";
                            dtgTransaction.Columns["Customer_Name"].HeaderText = "Customer";
                            dtgTransaction.Columns["Total_Amount"].HeaderText = "Total Amount";
                            dtgTransaction.Columns["Status"].HeaderText = "Status";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            LoadData(txtSearch.Text.Trim());
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            LoadData();
        }

        

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (dtgTransaction.Rows.Count == 0)
            {
                MessageBox.Show("There is no data to export.", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "Excel files (*.xls)|*.xls|CSV files (*.csv)|*.csv|All files (*.*)|*.*";
                sfd.FilterIndex = 1;
                sfd.FileName = "TransactionHistory_" + DateTime.Now.ToString("yyyyMMdd_HHmmss");
                sfd.Title = "Export transaction list";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        ExportToExcel(sfd.FileName);
                        MessageBox.Show("Export file successfully!\nPath: " + sfd.FileName, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi xuất file: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void ExportToExcel(string filePath)
        {
            
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            
            string[] headers = new string[dtgTransaction.Columns.Count];
            for (int i = 0; i < dtgTransaction.Columns.Count; i++)
            {
                headers[i] = dtgTransaction.Columns[i].HeaderText;
            }
            sb.AppendLine(string.Join(",", headers));

            
            foreach (DataGridViewRow row in dtgTransaction.Rows)
            {
                if (row.IsNewRow) continue;
                string[] fields = new string[dtgTransaction.Columns.Count];
                for (int i = 0; i < dtgTransaction.Columns.Count; i++)
                {
                    object cellValue = row.Cells[i].Value;
                    string field = cellValue?.ToString() ?? "";
                   
                    if (field.Contains(",") || field.Contains("\""))
                    {
                        field = "\"" + field.Replace("\"", "\"\"") + "\"";
                    }
                    fields[i] = field;
                }
                sb.AppendLine(string.Join(",", fields));
            }

            File.WriteAllText(filePath, sb.ToString(), System.Text.Encoding.UTF8);
        }

        private void dtgTransaction_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = e.RowIndex;
            if (index < 0) return;
            int orderId = Convert.ToInt32(dtgTransaction.Rows[index].Cells[0].Value);

            
            using (SqlConnection connection = DatabaseConnection.GetConnection())
            {
                if (connection == null) return;
                connection.Open();

               
                string orderSql = @"SELECT s.Sales_Order_ID, s.Sales_Date, s.Total_Amount, s.Notes,
                            c.Customer_ID, c.Customer_Name
                            FROM SALE_ORDER s
                            LEFT JOIN CUSTOMER c ON s.Customer_ID = c.Customer_ID
                            WHERE s.Sales_Order_ID = @OrderID";

                string customerId = "N/A";
                string customerName = "Guest";
                string date = "";
                decimal totalAmount = 0;
                string notes = "";

                using (SqlCommand cmd = new SqlCommand(orderSql, connection))
                {
                    cmd.Parameters.AddWithValue("@OrderID", orderId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            customerId = reader["Customer_ID"] == DBNull.Value ? "N/A" : reader["Customer_ID"].ToString();
                            customerName = reader["Customer_Name"] == DBNull.Value ? "Guest" : reader["Customer_Name"].ToString();
                            date = Convert.ToDateTime(reader["Sales_Date"]).ToString("dd/MM/yyyy HH:mm");
                            totalAmount = Convert.ToDecimal(reader["Total_Amount"]);
                            notes = reader["Notes"].ToString();
                        }
                    }
                }

                string detailSql = @"SELECT p.Product_Name, sd.Sales_Quantity, sd.Unit_Price, sd.Subtotal
                             FROM SALES_DETAIL sd
                             JOIN PRO_DUCT p ON sd.Product_ID = p.Product_ID
                             WHERE sd.Sales_Order_ID = @OrderID";

                string receipt = "========== RECEIPT ==========\n";
                receipt += $"Order ID    : {orderId}\n";
                receipt += $"Date        : {date}\n";
                receipt += "------------------------------\n";
                receipt += $"Customer ID : {customerId}\n";
                receipt += $"Customer    : {customerName}\n";
                receipt += "------------------------------\n";

                using (SqlCommand cmd = new SqlCommand(detailSql, connection))
                {
                    cmd.Parameters.AddWithValue("@OrderID", orderId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            receipt += $"{reader["Product_Name"]}\n";
                            receipt += $"  {reader["Sales_Quantity"]} x {Convert.ToDecimal(reader["Unit_Price"]):N0} = {Convert.ToDecimal(reader["Subtotal"]):N0} VND\n";
                        }
                    }
                }

                receipt += "------------------------------\n";
                receipt += $"TOTAL       : {totalAmount:N0} VND\n";
                receipt += $"Notes       : {notes}\n";
                receipt += "==============================\n";
                receipt += "     Thank you! Come again!   ";

                MessageBox.Show(receipt, $"Receipt - Order #{orderId}",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void dtgTransaction_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
   