using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Falcon_Vendo_2019
{
    public partial class VendoInterface : Form
    {
        //string connectionString = @"Server=localhost;Database=falcon_vendo;Uid=root;Pwd=root";

        SqlConnection conn = new SqlConnection(@"server=localhost;user id=root;persistsecurityinfo=True;database=falcon_vendo");

        string loadedEmployee /*test*/ = "2623347780";
        string loadedEmployeeName /*test*/ = "LASERA, JI";
        int loadedCredit /*test*/ = -864;
        int creditLimit /*test*/ = -1000;
        int highestPrice /*test*/ = 100;
        string transactionMode;    // "cash" or "credit"
        int cancelAttempts, coinsInserted = 0;

        string a1item, a2item, a3item, b1item, b2item, b3item, c1item, c2item, c3item, d1item, d2item, d3item;
        int a1price, a2price, a3price, b1price, b2price, b3price, c1price, c2price, c3price, d1price, d2price, d3price;


        //
        //  Initialization
        //

        public VendoInterface()
        {
            InitializeComponent();
        }

        private void VendoInterface_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            initializeForm();
            txtName.Text = "";
            txtName.Select();
            txtCoinsInserted.Text = Convert.ToString(coinsInserted);
            txtCoinsInserted.BackColor = System.Drawing.Color.White;
            txtCoinsInserted.ForeColor = System.Drawing.Color.Red;
        }

        private void VendoInterface_Resize(object sender, EventArgs e)
        {
            formResize();
        }


        private void txtName_TextChanged(object sender, EventArgs e)
        {
            if (checkBuyer() == true)
            {
                btnCash.Enabled = true;
                btnCredit.Enabled = true;
                btnCancel.Enabled = true;
            }
            txtCoinsInserted.Text = Convert.ToString(coinsInserted);
        }


        //
        // Transaction events
        //

        private void btnCash_Click(object sender, EventArgs e)
        {
            if(checkBuyer() == true)
            {
                transactionMode = "cash";
                btnCash.Enabled = false;
                btnCredit.Visible = false;
                //btnCash.BackColor = System.Drawing.Color.LightCoral;
                txtCurrentCredit.Text = String.Format("{0:0,0.0}", loadedCredit);
            }
        }
        private void btnCredit_Click(object sender, EventArgs e)
        {

            if (checkBuyer() == true && maxedCredit() == false)
            {
                transactionMode = "credit";
                activateButtons(true);
                btnCash.Visible = false;
                btnCredit.Enabled = false;
                btnPayCredit.Visible = true;
                //btnCredit.BackColor = System.Drawing.Color.LightCoral;
            }
            else if (checkBuyer() == true &&maxedCredit() == true)
            {
                btnPayCredit.Visible = true;
                MessageBox.Show("You almost reached your credit limit! \n Current Balance: " + loadedCredit + "\n Credit Limit: " + creditLimit + "\n Available for Purchase: " + (creditLimit - loadedCredit), "Insufficient Credit", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else { }
            txtCurrentCredit.Text = String.Format("{0:0,0.0}", loadedCredit);
        }


        private void btnCoin_Click(object sender, EventArgs e)
        {
            if (txtName.Text != "")
            {
                Button btnCoins = (Button)sender;
                coinsInserted = coinsInserted + Convert.ToInt32(btnCoins.Text);
                txtCoinsInserted.Text = Convert.ToString(coinsInserted);
                if (coinsInserted >= a1price && transactionMode == "cash")
                {
                    activateButtons(true);
                }
                if (coinsInserted >= a1price && transactionMode == "credit" && maxedCredit() == false)
                {
                    activateButtons(true);
                }
                else { }
            }
            else { }
        }

        private void productClick(object sender, EventArgs e)
        {
            if (transactionMode == "cash")
            {
                // contains code if transaction is cash
                loadedCredit = loadedCredit - (a1price - coinsInserted);
            }
            else if (transactionMode == "credit")
            {
                // contains code if transaction is cash
                loadedCredit = loadedCredit - (a1price - coinsInserted);
            }
            else { }
            initializeForm();
        }


        private void btnPayCredit_Click(object sender, EventArgs e)
        {
            loadedCredit = loadedCredit + coinsInserted;
            coinsInserted = 0;
            txtCurrentCredit.Text = String.Format("{0:0,0.0}", loadedCredit);
            txtCoinsInserted.Text = Convert.ToString(coinsInserted);
            if (maxedCredit() == false)
            {
                activateButtons(true);
            }
            else activateButtons(false);
        }

        private void txtCoinsInserted_TextChanged(object sender, EventArgs e)
        {
            if (coinsInserted > 0)
            {
                btnPayCredit.Enabled = true;
            }
            else
            {
                btnPayCredit.Enabled = false;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (coinsInserted == 0)
            {
                initializeForm();
                txtCurrentCredit.Text = "";
                txtName.Text = "";
                txtName.Select();
            }
            else if (cancelAttempts <= 2)
            {
                cancelAttempts++;
                MessageBox.Show("You still have Coins Inserted! \n Attemps: " + cancelAttempts, "Vendo", MessageBoxButtons.YesNoCancel);

            }
        }


        private void txtName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                if (checkBuyer())
                {
                    txtName.Text = loadedEmployeeName;
                    txtCurrentCredit.Text = String.Format("{0:0,0.0}", loadedCredit);
                }
                else
                {
                    txtName.Text = "";
                    txtName.Select();
                }
            }
        }

        private void txtName_Enter(object sender, EventArgs e)
        {
            txtName.SelectAll();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            initializeForm();
            txtName.Text = "";
            txtName.Select();

        }

        //
        //  Methods
        //

        private void initializeForm()
        {
            cancelAttempts = coinsInserted = 0;
            activateButtons(false);
            btnCash.Enabled = false;
            btnCredit.Enabled = false;
            btnCancel.Enabled = false;
            btnCash.Visible = true;
            btnCredit.Visible = true;
            btnPayCredit.Visible = false;
            btnPayCredit.Enabled = false;
            //btnCash.BackColor = System.Drawing.Color.White;
            //btnCredit.BackColor = System.Drawing.Color.White;
            txtCurrentCredit.Text = "0.00";
            txtCoinsInserted.Text = Convert.ToString(coinsInserted);
            a1item = a2item = a3item = b1item = b2item = b3item = c1item = c2item = c3item = d1item = d2item = d3item = "+";
            a1price = a2price = a3price = b1price = b2price = b3price = c1price = c2price = c3price = d1price = d2price = d3price = 10;
            /*
            // Start product name Simulation
            a1item = "Nescafe 3 in 1 Brown";
            a2item = "Kopiko Black";
            a3item = "Great Taste White";
            b1item = "Marlboro Black";
            b2item = "Marlboro Red";
            b3item = "Marlboro Lights";
            c1item = "Canton Chilimansi";
            c2item = "Lucky Me Chicken";
            c3item = "Lucky Me Beef";
            d1item = "Red Horse";
            d2item = "SanMig Light";
            d3item = "Sizzling Sisig";
            */
            btnItem1.Text = "A1 \n" + a1price + ".00"; btnItem2.Text = "A2 \n" + a2price + ".00"; btnItem3.Text = "A3 \n" + a3price + ".00";
            btnItem4.Text = "B1 \n" + b1price + ".00"; btnItem5.Text = "B2 \n" + b2price + ".00"; btnItem6.Text = "B3 \n" + b3price + ".00";
            btnItem7.Text = "C1 \n" + c1price + ".00"; btnItem8.Text = "C2 \n" + c2price + ".00"; btnItem9.Text = "C3 \n" + c3price + ".00";
            btnItem10.Text = "D1 \n" + d1price + ".00"; btnItem11.Text = "D2 \n" + d2price + ".00"; btnItem12.Text = "D3 \n" + d3price + ".00";
            lblProductA1.Text = a1item; lblProductA2.Text = a2item; lblProductA3.Text = a3item;
            lblProductB1.Text = b1item; lblProductB2.Text = b2item; lblProductB3.Text = b3item;
            lblProductC1.Text = c1item; lblProductC2.Text = c2item; lblProductC3.Text = c3item;
            lblProductD1.Text = d1item; lblProductD2.Text = d2item; lblProductD3.Text = d3item;
            formResize();
        }


        private bool checkBuyer()
        {
            //txtName.Text = 
            if (txtName.Text == loadedEmployee) // Hardcoded check for buyer. must be done in database 
            {
                //loadedEmployeeName ="";
                //loadedCredit = "";
                txtCurrentCredit.Text = String.Format("{0:0,0.0}", loadedCredit);
                return true;
            }
            else
            {
                return false;
            }
        }
        private void activateButtons(bool buttonState)
        {
            if (transactionMode == "cash")
            {
                btnItem1.Enabled = buttonState; btnItem2.Enabled = buttonState; btnItem3.Enabled = buttonState;
                btnItem4.Enabled = buttonState; btnItem5.Enabled = buttonState; btnItem6.Enabled = buttonState;
                btnItem7.Enabled = buttonState; btnItem8.Enabled = buttonState; btnItem9.Enabled = buttonState;
                btnItem10.Enabled = buttonState; btnItem11.Enabled = buttonState; btnItem12.Enabled = buttonState;
            }
            else if (transactionMode == "credit" && ((loadedCredit - creditLimit) > a1price))
            {
                btnItem1.Enabled = buttonState; btnItem2.Enabled = buttonState; btnItem3.Enabled = buttonState;
                btnItem4.Enabled = buttonState; btnItem5.Enabled = buttonState; btnItem6.Enabled = buttonState;
                btnItem7.Enabled = buttonState; btnItem8.Enabled = buttonState; btnItem9.Enabled = buttonState;
                btnItem10.Enabled = buttonState; btnItem11.Enabled = buttonState; btnItem12.Enabled = buttonState;
            }
            else
            {
                btnItem1.Enabled = false; btnItem2.Enabled = false; btnItem3.Enabled = false;
                btnItem4.Enabled = false; btnItem5.Enabled = false; btnItem6.Enabled = false;
                btnItem7.Enabled = false; btnItem8.Enabled = false; btnItem9.Enabled = false;
                btnItem10.Enabled = false; btnItem11.Enabled = false; btnItem12.Enabled = false;
            }
        }

        private bool maxedCredit()
        {
            if ((loadedCredit + (-highestPrice + 1)) <= creditLimit)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void formResize()
        {
            grpVendoUI.Size = new System.Drawing.Size(ClientSize.Width * 4 / 5, 850);
            grpVendoUI.Location = new System.Drawing.Point(ClientSize.Width / 2 - ((grpVendoUI.Size.Width) / 2), ClientSize.Height / 2 - ((grpVendoUI.Size.Height) / 2));
            txtName.Size = new System.Drawing.Size(grpVendoUI.Size.Width - btnNew.Size.Width - 4, 45);
            txtCurrentCredit.Size = new System.Drawing.Size(grpVendoUI.Size.Width - lblCreditAmount.Size.Width, 45);
            txtCurrentCredit.Location = new System.Drawing.Point(lblCreditAmount.Size.Width + 2, txtName.Location.Y + txtName.Size.Height + 5);
            btnCash.Location = new System.Drawing.Point((grpVendoUI.Size.Width / 3) - (btnCash.Size.Width / 2) - (50), txtCurrentCredit.Location.Y + txtCurrentCredit.Size.Height + 15);
            btnCredit.Location = new System.Drawing.Point((grpVendoUI.Size.Width * 2 / 3) - (btnCredit.Size.Width / 2) + (50), btnCash.Location.Y);
            txtCoinsInserted.Location = new System.Drawing.Point((grpVendoUI.Size.Width / 2) - (txtCoinsInserted.Size.Width / 2), btnPayCredit.Location.Y + (btnPayCredit.Size.Height / 2 - txtCoinsInserted.Size.Height / 2));
            btnPayCredit.Location = new System.Drawing.Point(btnCredit.Location.X + btnCredit.Size.Width - btnPayCredit.Size.Width, btnCredit.Location.Y + btnCredit.Size.Height + 15);
            btnCancel.Location = new System.Drawing.Point(grpVendoUI.Size.Width / 2 - ((btnCancel.Size.Width) / 2), lblProductD1.Location.Y + lblProductD1.Size.Height + 15);
            btnNew.Location = new System.Drawing.Point((txtName.Location.X + txtName.Size.Width), txtName.Location.Y);

            btnItem4.Location = new System.Drawing.Point(btnItem1.Location.X, btnItem1.Location.Y + btnItem1.Size.Height + 30);
            btnItem5.Location = new System.Drawing.Point(btnItem2.Location.X, btnItem4.Location.Y);
            btnItem1.Location = new System.Drawing.Point((grpVendoUI.Size.Width * 1 / 4) - (btnItem1.Size.Width / 2), btnPayCredit.Location.Y + btnPayCredit.Size.Height + 20);
            btnItem2.Location = new System.Drawing.Point((grpVendoUI.Size.Width * 2 / 4) - (btnItem2.Size.Width / 2), btnItem1.Location.Y);
            btnItem3.Location = new System.Drawing.Point((grpVendoUI.Size.Width * 3 / 4) - (btnItem3.Size.Width / 2), btnItem1.Location.Y);
            btnItem4.Location = new System.Drawing.Point(btnItem1.Location.X, btnItem1.Location.Y + btnItem1.Size.Height + 30);
            btnItem5.Location = new System.Drawing.Point(btnItem2.Location.X, btnItem4.Location.Y);
            btnItem6.Location = new System.Drawing.Point(btnItem3.Location.X, btnItem4.Location.Y);
            btnItem7.Location = new System.Drawing.Point(btnItem4.Location.X, btnItem4.Location.Y + btnItem4.Size.Height + 30);
            btnItem8.Location = new System.Drawing.Point(btnItem5.Location.X, btnItem7.Location.Y);
            btnItem9.Location = new System.Drawing.Point(btnItem6.Location.X, btnItem7.Location.Y);
            btnItem10.Location = new System.Drawing.Point(btnItem7.Location.X, btnItem7.Location.Y + btnItem7.Size.Height + 30);
            btnItem11.Location = new System.Drawing.Point(btnItem8.Location.X, btnItem10.Location.Y);
            btnItem12.Location = new System.Drawing.Point(btnItem9.Location.X, btnItem10.Location.Y);

            lblProductA1.Location = new System.Drawing.Point((btnItem1.Location.X + (btnItem1.Size.Width) / 2) - lblProductA1.Size.Width / 2, btnItem1.Location.Y + btnItem1.Size.Height + 5);
            lblProductA2.Location = new System.Drawing.Point((btnItem2.Location.X + (btnItem2.Size.Width) / 2) - lblProductA2.Size.Width / 2, lblProductA1.Location.Y);
            lblProductA3.Location = new System.Drawing.Point((btnItem3.Location.X + (btnItem3.Size.Width) / 2) - lblProductA3.Size.Width / 2, lblProductA1.Location.Y);
            lblProductB1.Location = new System.Drawing.Point((btnItem4.Location.X + (btnItem4.Size.Width) / 2) - lblProductB1.Size.Width / 2, btnItem4.Location.Y + btnItem4.Size.Height + 5);
            lblProductB2.Location = new System.Drawing.Point((btnItem5.Location.X + (btnItem5.Size.Width) / 2) - lblProductB2.Size.Width / 2, lblProductB1.Location.Y);
            lblProductB3.Location = new System.Drawing.Point((btnItem6.Location.X + (btnItem6.Size.Width) / 2) - lblProductB3.Size.Width / 2, lblProductB1.Location.Y);
            lblProductC1.Location = new System.Drawing.Point((btnItem7.Location.X + (btnItem7.Size.Width) / 2) - lblProductC1.Size.Width / 2, btnItem7.Location.Y + btnItem7.Size.Height + 5);
            lblProductC2.Location = new System.Drawing.Point((btnItem8.Location.X + (btnItem8.Size.Width) / 2) - lblProductC2.Size.Width / 2, lblProductC1.Location.Y);
            lblProductC3.Location = new System.Drawing.Point((btnItem9.Location.X + (btnItem9.Size.Width) / 2) - lblProductC3.Size.Width / 2, lblProductC1.Location.Y);
            lblProductD1.Location = new System.Drawing.Point((btnItem10.Location.X + (btnItem10.Size.Width) / 2) - lblProductD1.Size.Width / 2, btnItem10.Location.Y + btnItem10.Size.Height + 5);
            lblProductD2.Location = new System.Drawing.Point((btnItem11.Location.X + (btnItem11.Size.Width) / 2) - lblProductD2.Size.Width / 2, lblProductD1.Location.Y);
            lblProductD3.Location = new System.Drawing.Point((btnItem12.Location.X + (btnItem12.Size.Width) / 2) - lblProductD3.Size.Width / 2, lblProductD1.Location.Y);
        }

        private void btnCash_EnabledChanged(object sender, EventArgs e)
        {
            if (btnCash.Enabled == true)
            {
                btnCash.BackColor = System.Drawing.Color.White;
            }
            else
            {
                btnCash.BackColor = System.Drawing.Color.DimGray;
            }
        }
        private void btnCredit_EnabledChanged(object sender, EventArgs e)
        {
            if (btnCash.Enabled == true)
            {
                btnCredit.BackColor = System.Drawing.Color.White;
            }
            else
            {
                btnCredit.BackColor = System.Drawing.Color.DimGray;
            }
        }
        private void button_EnabledChanged(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            if (btn.Enabled == true)
            {
                btn.BackColor = System.Drawing.Color.White;
            }
            else
            {
                btn.BackColor = System.Drawing.Color.DimGray;
            }
        }

        private void txtName_Leave(object sender, EventArgs e)
        {
            if(txtName.Text == "bye")
            {
                this.Close();
            }
            txtName.Text = "";
            txtName.Select();
        }
    }
}