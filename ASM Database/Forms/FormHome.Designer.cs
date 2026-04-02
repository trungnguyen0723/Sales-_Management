namespace ASM_Database.Forms
{
    partial class FormHome
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.panelOrders = new System.Windows.Forms.Panel();
            this.lblNewOrders = new System.Windows.Forms.Label();
            this.lblOrdersCount = new System.Windows.Forms.Label();
            this.lbl = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblCategoryCount = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.lblStockCount = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.picOrders = new System.Windows.Forms.PictureBox();
            this.picCategory = new System.Windows.Forms.PictureBox();
            this.picStock = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.panelOrders.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picOrders)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picCategory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picStock)).BeginInit();
            this.SuspendLayout();
            // 
            // chart1
            // 
            chartArea2.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            this.chart1.Legends.Add(legend2);
            this.chart1.Location = new System.Drawing.Point(49, 230);
            this.chart1.Name = "chart1";
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            this.chart1.Series.Add(series2);
            this.chart1.Size = new System.Drawing.Size(917, 289);
            this.chart1.TabIndex = 1;
            this.chart1.Text = "chart1";
            this.chart1.Click += new System.EventHandler(this.chart1_Click);
            // 
            // panelOrders
            // 
            this.panelOrders.BackColor = System.Drawing.Color.Gainsboro;
            this.panelOrders.Controls.Add(this.picOrders);
            this.panelOrders.Controls.Add(this.lblNewOrders);
            this.panelOrders.Controls.Add(this.lblOrdersCount);
            this.panelOrders.Controls.Add(this.lbl);
            this.panelOrders.Location = new System.Drawing.Point(49, 45);
            this.panelOrders.Name = "panelOrders";
            this.panelOrders.Size = new System.Drawing.Size(261, 113);
            this.panelOrders.TabIndex = 2;
            // 
            // lblNewOrders
            // 
            this.lblNewOrders.AutoSize = true;
            this.lblNewOrders.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNewOrders.Location = new System.Drawing.Point(23, 79);
            this.lblNewOrders.Name = "lblNewOrders";
            this.lblNewOrders.Size = new System.Drawing.Size(102, 14);
            this.lblNewOrders.TabIndex = 2;
            this.lblNewOrders.Text = "15 New Orders";
            // 
            // lblOrdersCount
            // 
            this.lblOrdersCount.AutoSize = true;
            this.lblOrdersCount.Font = new System.Drawing.Font("Verdana", 24.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOrdersCount.Location = new System.Drawing.Point(19, 39);
            this.lblOrdersCount.Name = "lblOrdersCount";
            this.lblOrdersCount.Size = new System.Drawing.Size(121, 40);
            this.lblOrdersCount.TabIndex = 1;
            this.lblOrdersCount.Text = "1.258";
            this.lblOrdersCount.Click += new System.EventHandler(this.lblOrdersCount_Click);
            // 
            // lbl
            // 
            this.lbl.AutoSize = true;
            this.lbl.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl.Location = new System.Drawing.Point(23, 14);
            this.lbl.Name = "lbl";
            this.lbl.Size = new System.Drawing.Size(53, 14);
            this.lbl.TabIndex = 0;
            this.lbl.Text = "Orders";
            this.lbl.Click += new System.EventHandler(this.label1_Click_1);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.RosyBrown;
            this.panel2.Controls.Add(this.picCategory);
            this.panel2.Controls.Add(this.lblCategoryCount);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Location = new System.Drawing.Point(374, 45);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(261, 113);
            this.panel2.TabIndex = 3;
            // 
            // lblCategoryCount
            // 
            this.lblCategoryCount.AutoSize = true;
            this.lblCategoryCount.Font = new System.Drawing.Font("Verdana", 24.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCategoryCount.Location = new System.Drawing.Point(19, 39);
            this.lblCategoryCount.Name = "lblCategoryCount";
            this.lblCategoryCount.Size = new System.Drawing.Size(121, 40);
            this.lblCategoryCount.TabIndex = 1;
            this.lblCategoryCount.Text = "1.258";
            this.lblCategoryCount.Click += new System.EventHandler(this.lblCategoryCount_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(23, 14);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(67, 14);
            this.label6.TabIndex = 0;
            this.label6.Text = "Category";
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.SystemColors.Highlight;
            this.panel3.Controls.Add(this.picStock);
            this.panel3.Controls.Add(this.lblStockCount);
            this.panel3.Controls.Add(this.label9);
            this.panel3.Location = new System.Drawing.Point(708, 45);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(261, 113);
            this.panel3.TabIndex = 4;
            // 
            // lblStockCount
            // 
            this.lblStockCount.AutoSize = true;
            this.lblStockCount.Font = new System.Drawing.Font("Verdana", 24.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStockCount.Location = new System.Drawing.Point(19, 39);
            this.lblStockCount.Name = "lblStockCount";
            this.lblStockCount.Size = new System.Drawing.Size(121, 40);
            this.lblStockCount.TabIndex = 1;
            this.lblStockCount.Text = "1.258";
            this.lblStockCount.Click += new System.EventHandler(this.lblStockCount_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(23, 14);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(117, 14);
            this.label9.TabIndex = 0;
            this.label9.Text = "Product In Stock";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Verdana", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(60, 192);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(138, 25);
            this.label4.TabIndex = 5;
            this.label4.Text = "Best Seller";
            // 
            // picOrders
            // 
            this.picOrders.Location = new System.Drawing.Point(146, 14);
            this.picOrders.Name = "picOrders";
            this.picOrders.Size = new System.Drawing.Size(100, 79);
            this.picOrders.TabIndex = 3;
            this.picOrders.TabStop = false;
            // 
            // picCategory
            // 
            this.picCategory.Location = new System.Drawing.Point(146, 14);
            this.picCategory.Name = "picCategory";
            this.picCategory.Size = new System.Drawing.Size(100, 79);
            this.picCategory.TabIndex = 4;
            this.picCategory.TabStop = false;
            // 
            // picStock
            // 
            this.picStock.Location = new System.Drawing.Point(146, 14);
            this.picStock.Name = "picStock";
            this.picStock.Size = new System.Drawing.Size(100, 79);
            this.picStock.TabIndex = 4;
            this.picStock.TabStop = false;
            // 
            // FormHome
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1005, 603);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panelOrders);
            this.Controls.Add(this.chart1);
            this.Name = "FormHome";
            this.Text = "Home";
            this.Load += new System.EventHandler(this.FormHome_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.panelOrders.ResumeLayout(false);
            this.panelOrders.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picOrders)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picCategory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picStock)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.Panel panelOrders;
        private System.Windows.Forms.Label lblNewOrders;
        private System.Windows.Forms.Label lblOrdersCount;
        private System.Windows.Forms.Label lbl;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lblCategoryCount;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label lblStockCount;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.PictureBox picOrders;
        private System.Windows.Forms.PictureBox picCategory;
        private System.Windows.Forms.PictureBox picStock;
    }
}