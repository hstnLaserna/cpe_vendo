using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Falcon_Vendo_2019
{
    public partial class VendoInterface : Form
    {
        string loadedEmployee = "LASERNA";
        int loadedCredit = 5025;
        int coinsInserted = 0;

        string a1item, a2item, a3item, b1item, b2item, b3item, c1item, c2item, c3item, d1item, d2item, d3item;
        int a1price, a2price, a3price, b1price, b2price, b3price, c1price, c2price, c3price, d1price, d2price, d3price;

        public VendoInterface()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            initializeForm();
            txtName.Text = "";
            btnCash.Enabled = false;
            btnCredit.Enabled = false;
            txtCoinsInserted.Text = Convert.ToString(coinsInserted);
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            initializeForm();
            if(txtName.Text =="")
            {
                btnCash.Enabled = false;
                btnCredit.Enabled = false;
            }
            txtCoinsInserted.Text = Convert.ToString(coinsInserted);
        }


        private void btnCash_Click(object sender, EventArgs e)
        {
            if (checkBuyer() == true)
            {
                btnCash.Enabled = false;
                btnCredit.Visible = false;
                btnCash.BackColor = System.Drawing.Color.LightCoral;
            }
            else {}
        }
        private void btnCredit_Click(object sender, EventArgs e)
        {
            if (checkBuyer() == true)
            {
                btnCash.Visible = false;
                btnCredit.Enabled = false;
                btnPayCredit.Visible = true;
                btnCredit.BackColor = System.Drawing.Color.LightCoral;
                
            }
            else {}
        }


        private void txtName_MouseClick(object sender, MouseEventArgs e)
        {
            txtName.SelectAll();
        }

        private void btn1_Click(object sender, EventArgs e)
        {
            coinsInserted = coinsInserted + 1;
            txtCoinsInserted.Text = Convert.ToString(coinsInserted);
        }
        private void btn5_Click(object sender, EventArgs e)
        {
            coinsInserted = coinsInserted + 5;
            txtCoinsInserted.Text = Convert.ToString(coinsInserted);
        }
        private void btn10_Click(object sender, EventArgs e)
        {
            coinsInserted = coinsInserted + 10;
            txtCoinsInserted.Text = Convert.ToString(coinsInserted);
        }

        private void productClick(object sender, EventArgs e)
        {
            activateButtons(false);
            initializeForm();
        }



        private bool checkBuyer()
        {
            if (txtName.Text == loadedEmployee)  // Hardcoded check for buyer. must be done in database 
            {
                activateButtons(true);
                txtCurrentCredit.Text = String.Format("{0:0,0.0}", loadedCredit);
                return true;
            }
            else
            {
                activateButtons(false);
                return false;
            }
        }
        private void initializeForm()
        {
            coinsInserted = 0;
            txtCurrentCredit.Text = "";
            activateButtons(false);
            btnCash.Enabled = true;
            btnCredit.Enabled = true;
            btnCash.Visible = true;
            btnCredit.Visible = true;
            btnPayCredit.Visible = false;
            btnPayCredit.Enabled = false;
            btnCash.BackColor = System.Drawing.Color.White;
            btnCredit.BackColor = System.Drawing.Color.White;
            txtCoinsInserted.Text = Convert.ToString(coinsInserted);
            //a1item = "Nescafe 3 in 1";
            a1item = a2item = a3item = b1item = b2item = "+";

            btnA1.Text = "A1 \n" + a1price + ".00"; btnA2.Text = "A2 \n" + a2price + ".00"; btnA3.Text = "A3 \n" + a3price + ".00";
            btnB1.Text = "B1 \n" + b1price + ".00"; btnB2.Text = "B2 \n" + b2price + ".00"; btnB3.Text = "B3 \n" + b3price + ".00";
            btnC1.Text = "C1 \n" + c1price + ".00"; btnC2.Text = "C2 \n" + c2price + ".00"; btnC3.Text = "C3 \n" + c3price + ".00";
            btnD1.Text = "D1 \n" + d1price + ".00"; btnD2.Text = "D2 \n" + d2price + ".00"; btnD3.Text = "D3 \n" + d3price + ".00";
            lblProductA1.Text = a1item; lblProductA2.Text = a2item; lblProductA3.Text = a3item; 
            lblProductB1.Text = b1item; lblProductB2.Text = b2item; lblProductB3.Text = b3item; 
            lblProductC1.Text = c1item; lblProductC2.Text = c2item; lblProductC3.Text = c3item;
            lblProductD1.Text = d1item; lblProductD2.Text = d2item; lblProductD3.Text = d3item;
            formResize();
        }
        private void activateButtons(bool buttonState)
        {
            btnA1.Enabled = buttonState; btnA2.Enabled = buttonState; btnA3.Enabled = buttonState;
            btnB1.Enabled = buttonState; btnB2.Enabled = buttonState; btnB3.Enabled = buttonState;
            btnC1.Enabled = buttonState; btnC2.Enabled = buttonState; btnC3.Enabled = buttonState;
            btnD1.Enabled = buttonState; btnD2.Enabled = buttonState; btnD3.Enabled = buttonState;
        }

        private void VendoInterface_Resize(object sender, EventArgs e)
        {
            formResize();
        }

        private void formResize()
        {
            grpVendoUI.Location = new System.Drawing.Point(ClientSize.Width / 2 - ((grpVendoUI.Size.Width) / 2), grpVendoUI.Location.Y);
            txtName.Size = new System.Drawing.Size(grpVendoUI.Size.Width - 4, 45);
            txtCurrentCredit.Size = new System.Drawing.Size(txtName.Size.Width - lblCreditAmount.Size.Width, 45);
            txtCurrentCredit.Location = new System.Drawing.Point(lblCreditAmount.Size.Width + 2, txtName.Location.Y + txtName.Size.Height + 5);
            btnCash.Location = new System.Drawing.Point((grpVendoUI.Size.Width / 3) - (btnCash.Size.Width / 2) - (50), txtCurrentCredit.Location.Y + txtCurrentCredit.Size.Height + 15);
            btnCredit.Location = new System.Drawing.Point((grpVendoUI.Size.Width * 2 / 3) - (btnCredit.Size.Width / 2) + (50), btnCash.Location.Y);
            txtCoinsInserted.Location = new System.Drawing.Point((grpVendoUI.Size.Width / 2) - (txtCoinsInserted.Size.Width / 2), btnPayCredit.Location.Y + (btnPayCredit.Size.Height / 2 - txtCoinsInserted.Size.Height / 2));
            btnPayCredit.Location = new System.Drawing.Point(btnCredit.Location.X + btnCredit.Size.Width - btnPayCredit.Size.Width, btnCredit.Location.Y + btnCredit.Size.Height + 15);
            btnA1.Location = new System.Drawing.Point((grpVendoUI.Size.Width * 1 / 4) - (btnA1.Size.Width / 2), btnPayCredit.Location.Y + btnPayCredit.Size.Height + 20);
            btnA2.Location = new System.Drawing.Point((grpVendoUI.Size.Width * 2 / 4) - (btnA2.Size.Width / 2), btnA1.Location.Y);
            btnA3.Location = new System.Drawing.Point((grpVendoUI.Size.Width * 3 / 4) - (btnA3.Size.Width / 2), btnA1.Location.Y);
            btnB1.Location = new System.Drawing.Point(btnA1.Location.X, btnA1.Location.Y + btnA1.Size.Height + 30);
            btnB2.Location = new System.Drawing.Point(btnA2.Location.X, btnB1.Location.Y);
            btnB3.Location = new System.Drawing.Point(btnA3.Location.X, btnB1.Location.Y);
            btnC1.Location = new System.Drawing.Point(btnB1.Location.X, btnB1.Location.Y + btnB1.Size.Height + 30);
            btnC2.Location = new System.Drawing.Point(btnB2.Location.X, btnC1.Location.Y);
            btnC3.Location = new System.Drawing.Point(btnB3.Location.X, btnC1.Location.Y);
            btnD1.Location = new System.Drawing.Point(btnC1.Location.X, btnC1.Location.Y + btnC1.Size.Height + 30);
            btnD2.Location = new System.Drawing.Point(btnC2.Location.X, btnD1.Location.Y);
            btnD3.Location = new System.Drawing.Point(btnC3.Location.X, btnD1.Location.Y);
        }
        private void btnPayCredit_Click(object sender, EventArgs e)
        {
            loadedCredit = loadedCredit - coinsInserted;
            coinsInserted = 0;
            txtCurrentCredit.Text = String.Format("{0:0,0.0}", loadedCredit);
            txtCoinsInserted.Text = Convert.ToString(coinsInserted);
        }

        private void txtCoinsInserted_TextChanged(object sender, EventArgs e)
        {
            if (coinsInserted > 0)
            {
                btnPayCredit.Enabled = true;
                btnCancel.Enabled = true;
            }
            else
            {
                btnPayCredit.Enabled = false;
                btnCancel.Enabled = false;
            }
        }

    }
}