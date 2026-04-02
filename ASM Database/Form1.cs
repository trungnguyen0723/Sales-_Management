using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ASM_Database
{
    public partial class Form1 : Form
    {
        private Button currentButton;
        private Random random;
        private int tempIndex;
        private Form activeForm;

        private int _employeeId;
        private string _authority;

        // Constructor mặc định (không dùng nữa nhưng giữ để tránh lỗi build)
        public Form1()
        {
            InitializeComponent();
            CollapseMenu();
            random = new Random();
        }

        //  Constructor chính — nhận employeeId và authority
        public Form1(int employeeId, string authority)
        {
            InitializeComponent();
            CollapseMenu();
            random = new Random();
            _employeeId = employeeId;
            _authority = authority;
            ApplyRolePermissions();  //  Phân quyền menu
        }

        //  Phân quyền theo Authority
        private void ApplyRolePermissions()
        {
            switch (_authority.ToLower())
            {
                case "admin":
                    
                    break;

                case "staff":
                    btnEmployee.Visible = false;
                    btnCategory.Visible = false;
                    btnStockIn.Visible = false;
                    btnTransactionHistory.Visible = false;
                    break;

                case "warehouse":
                    
                    btnEmployee.Visible = false;
                    btnOrders.Visible = false;
                    btnTransactionHistory.Visible = false;
                    btnCustomer.Visible = false;
                    break;

                default:
                    
                    btnEmployee.Visible = false;
                    btnCategory.Visible = false;
                    btnOrders.Visible = false;
                    btnStockIn.Visible = false;
                    btnTransactionHistory.Visible = false;
                    btnCustomer.Visible = false;
                    break;
            }
        }

        private Color SelectThemeColor()
        {
            int index = random.Next(ThemeColor.ColorList.Count);
            while (tempIndex == index)
                index = random.Next(ThemeColor.ColorList.Count);
            tempIndex = index;
            string color = ThemeColor.ColorList[index];
            return ColorTranslator.FromHtml(color);
        }

        private void ActivateButton(object btnSender)
        {
            if (btnSender != null)
            {
                if (currentButton != (Button)btnSender)
                {
                    DisableButton();
                    Color color = SelectThemeColor();
                    currentButton = (Button)btnSender;
                    currentButton.BackColor = color;
                    currentButton.ForeColor = Color.White;
                    currentButton.Font = new System.Drawing.Font("Verdana", 12.5F,
                        System.Drawing.FontStyle.Regular,
                        System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    panelTitleBar.BackColor = color;
                    panelLogo.BackColor = ThemeColor.ChangeColorBrightness(color, -0.3);
                    ThemeColor.PrimaryColor = color;
                    ThemeColor.SecondaryColor = ThemeColor.ChangeColorBrightness(color, -0.3);
                }
            }
        }

        private void DisableButton()
        {
            foreach (Control previousBtn in panelMenu.Controls)
            {
                if (previousBtn is FontAwesome.Sharp.IconButton)
                {
                    previousBtn.BackColor = Color.FromArgb(9, 99, 192);
                    previousBtn.ForeColor = Color.Gainsboro;
                    previousBtn.Font = new System.Drawing.Font("Verdana", 10F,
                        System.Drawing.FontStyle.Regular,
                        System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                }
            }
        }

        private void OpenChildForm(Form childForm, object btnSender)
        {
            if (activeForm != null)
                activeForm.Close();

            ActivateButton(btnSender);
            activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            this.panelDeskTop.Controls.Add(childForm);
            this.panelDeskTop.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();

            string title = childForm.Text;
            if (title.StartsWith("Form"))
                title = title.Substring(4);
            lblTitle.Text = title;
        }

        private void Form1_Load(object sender, EventArgs e) { }

        private void iconButton2_Click(object sender, EventArgs e)
        {
            OpenChildForm(new Forms.FormHome(), sender);
        }

        private void iconButton1_Click(object sender, EventArgs e)
        {
            CollapseMenu();
        }

        private void CollapseMenu()
        {
            if (this.panelMenu.Width > 200)
            {
                panelMenu.Width = 70;
                pictureBox1.Visible = false;
                foreach (Control control in panelMenu.Controls)
                {
                    if (control is FontAwesome.Sharp.IconButton menuButton)
                    {
                        menuButton.ImageAlign = ContentAlignment.MiddleLeft;
                        menuButton.Padding = new Padding(10, 0, 0, 0);
                        menuButton.Text = "";
                        menuButton.TextImageRelation = TextImageRelation.Overlay;
                    }
                }
            }
            else
            {
                panelMenu.Width = 250;
                pictureBox1.Visible = true;
                foreach (Control control in panelMenu.Controls)
                {
                    if (control is FontAwesome.Sharp.IconButton menuButton)
                    {
                        menuButton.ImageAlign = ContentAlignment.MiddleLeft;
                        menuButton.Padding = new Padding(10, 0, 0, 0);
                        menuButton.Text = "   " + menuButton.Tag?.ToString();
                        menuButton.TextImageRelation = TextImageRelation.ImageBeforeText;
                        menuButton.TextAlign = ContentAlignment.MiddleLeft;
                    }
                }
            }
        }

        private void btnEmployee_Click(object sender, EventArgs e)
        {

            OpenChildForm(new Forms.FormEmployee(), sender);
        }

        private void btnCategory_Click(object sender, EventArgs e)
        {
            OpenChildForm(new Forms.FormCategory(), sender);
        }

        private void btnStockIn_Click(object sender, EventArgs e)
        {
            OpenChildForm(new Forms.FormStockIn(), sender);
        }

        private void btnOrders_Click(object sender, EventArgs e)
        {
            OpenChildForm(new Forms.FormOrders(_employeeId), sender);
        }

        private void iconButton3_Click(object sender, EventArgs e)
        {
            DialogResult confirm = MessageBox.Show(
                "Are you sure you want to sign out?",
                "Sign Out", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                Login loginForm = new Login();
                loginForm.Show();
                this.Close();
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e) { }
        private void panelDeskTop_Paint(object sender, PaintEventArgs e) { }
        private void textBox1_TextChanged(object sender, EventArgs e) { }
        private void lblTitle_Click(object sender, EventArgs e) { }
        private void panelDesktopPanel_Paint(object sender, PaintEventArgs e) { }
        private void panel1_Paint(object sender, PaintEventArgs e) { }
        private void panelMenu_Paint(object sender, PaintEventArgs e) { }

        private void btnTransactionHistory_Click(object sender, EventArgs e)
        {
            OpenChildForm(new Forms.FormTransactionHistory(), sender);
        }

        private void btnCustomer_Click(object sender, EventArgs e)
        {
            OpenChildForm(new Forms.FormCustomer(), sender);
        }

        private void panelTitleBar_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}