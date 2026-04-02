using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace ASM_Database.Forms
{
    public partial class FormHome : Form
    {
        
        public FormHome()
        {
            InitializeComponent();
            
        }

        private void FormHome_Load(object sender, EventArgs e)
        {
          
            panelOrders.Paint += (s, pe) =>
            {
                var panel = s as Panel;
                using (var brush = new SolidBrush(Color.FromArgb(74, 144, 226)))
                    pe.Graphics.FillRectangle(brush, 0, 0, 5, panel.Height);
            };

           
            picOrders.Paint += picOrders_Paint;
            picCategory.Paint += picCategory_Paint;
            picStock.Paint += picStock_Paint;

            LoadDashboardData();
        }

      
        private void LoadDashboardData()
        {
            try
            {
                using (SqlConnection conn = DatabaseConnection.GetConnection())
                {
                    if (conn == null) return;
                    conn.Open();

                    
                    string sqlOrders = @"
                        SELECT 
                            COUNT(*) AS Total,
                            SUM(CASE WHEN CAST(Sales_Date AS DATE) = CAST(GETDATE() AS DATE) THEN 1 ELSE 0 END) AS Today
                        FROM SALE_ORDER";
                    using (var cmd = new SqlCommand(sqlOrders, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            lblOrdersCount.Text = Convert.ToInt32(reader["Total"]).ToString("N0");
                            int today = Convert.ToInt32(reader["Today"]);
                            lblNewOrders.Text = today + " New Orders";
                        }
                    }

                   
                    using (var cmd = new SqlCommand("SELECT COUNT(*) FROM CATEGORY", conn))
                        lblCategoryCount.Text = Convert.ToInt32(cmd.ExecuteScalar()).ToString("N0");

                    
                    using (var cmd = new SqlCommand("SELECT ISNULL(SUM(Inventory_Quantity), 0) FROM PRO_DUCT", conn))
                    {
                        object result = cmd.ExecuteScalar();
                        int totalStock = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                        lblStockCount.Text = totalStock.ToString("N0");
                    }

                    
                    string sqlBest = @"
                        SELECT TOP 7 
                            p.Product_Name,
                            SUM(sd.Sales_Quantity) AS TotalSold
                        FROM SALES_DETAIL sd
                        INNER JOIN PRO_DUCT p ON sd.Product_ID = p.Product_ID
                        GROUP BY p.Product_Name
                        ORDER BY TotalSold DESC";

                    
                    if (chart1.Series.Count == 0)
                        chart1.Series.Add("Series1");
                    var series = chart1.Series["Series1"];
                    series.Points.Clear();

                    using (var cmd = new SqlCommand(sqlBest, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string name = reader["Product_Name"].ToString();
                            if (name.Length > 16) name = name.Substring(0, 14) + "…";
                            int qty = Convert.ToInt32(reader["TotalSold"]);
                            series.Points.AddXY(name, qty);
                        }
                    }

                    if (series.Points.Count == 0)
                        LoadDemoChart();
                }
            }
            catch
            {
               
                lblOrdersCount.Text = "—";
                lblCategoryCount.Text = "—";
                lblStockCount.Text = "—";
                lblNewOrders.Text = "0 New Orders";
                LoadDemoChart();
            }
        }

        private void LoadDemoChart()
        {
            if (chart1.Series.Count == 0)
                chart1.Series.Add("Series1");
            var series = chart1.Series["Series1"];
            series.Points.Clear();

            string[] names = { "iPhone 15 Pro", "S24 Ultra", "Oppo Find X6", "Xiaomi 14", "Vivo X100", "Samsung A55", "Oppo A79" };
            int[] vals = { 15, 10, 12, 8, 9, 6, 4 };
            for (int i = 0; i < names.Length; i++)
                series.Points.AddXY(names[i], vals[i]);
        }

      
        private void picOrders_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            var c = Color.FromArgb(74, 144, 226);

            using (var pen = new Pen(c, 2f))
            using (var brush = new SolidBrush(Color.FromArgb(60, c)))
            {
                
                g.DrawEllipse(pen, 2, 2, 28, 28);
                using (var f = new Font("Segoe UI", 13f, FontStyle.Bold))
                using (var b = new SolidBrush(c))
                    g.DrawString("$", f, b, 9, 5);

               
                using (var b2 = new SolidBrush(c))
                {
                    g.FillRectangle(b2, 38, 38, 10, 25);
                    g.FillRectangle(b2, 52, 28, 10, 35);
                    g.FillRectangle(b2, 66, 18, 10, 45);
                }
              
                g.DrawLine(pen, 34, 63, 80, 63);
            }
        }

        private void picCategory_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            var c = Color.FromArgb(150, 90, 75);

            using (var pen = new Pen(c, 2f))
            {
                
                var tag1 = new PointF[] {
                    new PointF(18, 8), new PointF(52, 8),
                    new PointF(68, 26), new PointF(52, 54),
                    new PointF(18, 54)
                };
                using (var p2 = new Pen(Color.FromArgb(100, c), 1.5f))
                    g.DrawPolygon(p2, tag1);

               
                var tag2 = new PointF[] {
                    new PointF(4, 14), new PointF(42, 14),
                    new PointF(60, 34), new PointF(42, 60),
                    new PointF(4, 60)
                };
                g.DrawPolygon(pen, tag2);

               
                g.DrawEllipse(pen, 10, 22, 10, 10);
            }
        }

        private void picStock_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            var c = Color.FromArgb(200, 230, 255);

            using (var pen = new Pen(c, 2f))
            {
               
                var lid = new PointF[] {
                    new PointF(8, 26), new PointF(40, 10),
                    new PointF(72, 26), new PointF(40, 38)
                };
                g.DrawPolygon(pen, lid);

                
                var left = new PointF[] {
                    new PointF(8, 26), new PointF(8, 60),
                    new PointF(40, 70), new PointF(40, 38)
                };
                g.DrawPolygon(pen, left);

                
                var right = new PointF[] {
                    new PointF(40, 38), new PointF(40, 70),
                    new PointF(72, 60), new PointF(72, 26)
                };
                g.DrawPolygon(pen, right);

                
                using (var tickPen = new Pen(c, 2.5f))
                {
                    tickPen.StartCap = LineCap.Round;
                    tickPen.EndCap = LineCap.Round;
                    g.DrawLine(tickPen, 26, 52, 36, 62);
                    g.DrawLine(tickPen, 36, 62, 54, 44);
                }
            }
        }

     
        private void label1_Click(object sender, EventArgs e) { }
        private void label1_Click_1(object sender, EventArgs e) { }
        private void chart1_Click(object sender, EventArgs e) { }
        private void lblStockCount_Click(object sender, EventArgs e) { }
        private void lblCategoryCount_Click(object sender, EventArgs e) { }
        private void lblOrdersCount_Click(object sender, EventArgs e) { }
    }
}