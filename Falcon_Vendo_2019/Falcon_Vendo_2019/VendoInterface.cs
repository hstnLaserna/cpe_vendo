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

        string loadedEmployee /*test*/ = "123"; //2623347780
        string loadedEmployeeName /*test*/ = "LASERA, JI";
        int loadedCredit /*test*/ = -864;
        int creditLimit /*test*/ = -1000;
        int highestPrice /*test*/ = 100;
        string transactionMode;    // "cash" or "credit"
        int cancelAttempts, coinsInserted = 0;

        string item12, item11, item10, item9, item8, item7, item6, item5, item4, item3, item2, item1;
        int price12, price11, price10, price9, price8, price7, price6, price5, price4, price3, price2, price1;
        Label[] labels;
        Button[] buttons;


        //
        //  Initialization
        //

        public VendoInterface()
        {
            InitializeComponent();
            lblCoinsInserted.BackColor = System.Drawing.Color.Black;
            lblCoinsInserted.ForeColor = System.Drawing.Color.Red;
        }

        private void VendoInterface_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            initializeForm();
            txtName.Text = "";
            txtEmpID.Select();
            lblCoinsInserted.Text = Convert.ToString(coinsInserted);
            lblCoinsInserted.BackColor = System.Drawing.Color.Black;
            lblCoinsInserted.ForeColor = System.Drawing.Color.Red;

            /*
            labels = new Label[12];
            for (int i = 0; i < labels.Length; i++)
            {
                labels[i] = new Label();

            }
            */
        }

        private void VendoInterface_Resize(object sender, EventArgs e)
        {
            formResize();
        }


        //
        // Transaction events
        //

        private void btnCash_Click(object sender, EventArgs e)
        {
            if (checkBuyer() == true)
            {
                transactionMode = "cash";
                btnCash.Enabled = false;
                btnCredit.Visible = false;
                //btnCash.BackColor = System.Drawing.Color.LightCoral;
                txtCurrentCredit.Text = String.Format("{0:0,0.0}", loadedCredit);
            }
            else { }
            if (checkBuyer() == true && coinsInserted >= 10)
            {
                activateButtons(true);
            }
            else { }
        }
        private void btnCredit_Click(object sender, EventArgs e)
        {
            if (checkBuyer() == true && maxedCredit() == false)
            {
                transactionMode = "credit";
                activateButtons(true);
                btnCash.Visible = false;
                btnCredit.Enabled = false;
                //btnCredit.BackColor = System.Drawing.Color.LightCoral;
            }
            else if (checkBuyer() == true &&maxedCredit() == true)
            {
                MessageBox.Show("You almost reached your credit limit! \n Current Balance: " + loadedCredit + "\n Credit Limit: " + creditLimit + "\n Available for Purchase: " + (creditLimit - loadedCredit), "Insufficient Credit", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else { }
            txtCurrentCredit.Text = String.Format("{0:0,0.0}", loadedCredit);

            txtEmpID.SelectAll();
        }


        private void btnCoin_Click(object sender, EventArgs e)
        {
            if (txtName.Text != "" && transactionMode != "")
            {
                Button btnCoins = (Button)sender;
                coinsInserted = coinsInserted + Convert.ToInt32(btnCoins.Text);
                lblCoinsInserted.Text = Convert.ToString(coinsInserted);
                if (coinsInserted >= price1 && transactionMode == "cash")
                {
                    activateButtons(true);
                }
                if (coinsInserted >= price1 && transactionMode == "credit" && maxedCredit() == false)
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
                loadedCredit = loadedCredit - (price1 - coinsInserted);
            }
            else if (transactionMode == "credit")
            {
                // contains code if transaction is cash
                loadedCredit = loadedCredit - (price1 - coinsInserted);
            }
            else { }
            initializeForm();
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {

            loadedCredit = loadedCredit + coinsInserted;
            initializeForm();
            txtEmpID.SelectAll();

            /*
            if (coinsInserted == 0)
            {
            }
            else if (cancelAttempts <= 2)
            {
                cancelAttempts++;
                MessageBox.Show("You still have Coins Inserted! \n Attemps: " + cancelAttempts, "Vendo", MessageBoxButtons.YesNoCancel);

            }
            else { }
            */
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

        private void txtCurrentCredit_TextChanged(object sender, EventArgs e)
        {
            if(txtCurrentCredit.Text == "" || txtCurrentCredit.Text == "0.00")
            {
                this.txtCurrentCredit.BackColor = System.Drawing.Color.MintCream;
                this.txtName.BackColor = System.Drawing.Color.MintCream;
            }
            else
            {
                this.txtCurrentCredit.BackColor = System.Drawing.Color.PaleGreen;
                this.txtName.BackColor = System.Drawing.Color.PaleGreen;
            }
        }

        private void txtEmpID_TextChanged(object sender, EventArgs e)
        {
            if (txtEmpID.Text == "bye")
            {
                this.Close();
            }
        }

        private void txtEmpID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                if (checkBuyer())
                {
                    txtName.Text = loadedEmployeeName;
                    btnCash.Enabled = true;
                    btnCredit.Enabled = true;
                    btnCancel.Enabled = true;
                    txtCurrentCredit.Text = String.Format("{0:0,0.0}", loadedCredit);
                    txtEmpID.SelectAll();
                }
                else
                {
                    initializeForm();
                    txtEmpID.SelectAll();

                }
            }
        }

        private void txtName_Click(object sender, EventArgs e)
        {
            txtEmpID.SelectAll();
        }

        private void txtEmpID_Leave(object sender, EventArgs e)
        {

            txtEmpID.SelectAll();
        }

        private void lblCoinsInserted_TextChanged(object sender, EventArgs e)
        {
            if (coinsInserted > 0)
            {
                btnCancel.Text = "Pay Credit and Cancel";
                txtEmpID.ReadOnly = true;
            }
            else
            {
                btnCancel.Text = "Cancel Transaction";
                txtEmpID.ReadOnly = false;
                txtEmpID.SelectAll();
            }
        }

        private void enterEmployee(object sender, EventArgs e)
        {
            txtEmpID.Select();
            txtEmpID.SelectAll();
        }

        private void enterEmployee(object sender, MouseEventArgs e)
        {
            txtEmpID.SelectAll();
        }



        //
        //  Methods
        //

        private void initializeForm()
        {
            transactionMode = "";
            cancelAttempts = coinsInserted = 0;
            activateButtons(false);
            txtCurrentCredit.Text = "";
            txtName.Text = "";
            btnCash.Enabled = false;
            btnCredit.Enabled = false;
            btnCancel.Enabled = false;
            btnCash.Visible = true;
            btnCredit.Visible = true;
            lblCoinsInserted.Text = Convert.ToString(coinsInserted);
            item1 = item2 = item3 = item4 = item5 = item6 = item8 = item12 = item11 = item10 = item9 = item7 = "+";
            price1 = price2 = price3 = price4 = price5 = price6 = price7 = price8 = price9 = price10 = price11 = price12 = 10;

            btnItem1.Text = "A1 \n" + price1 + ".00"; btnItem2.Text = "A2 \n" + price2 + ".00"; btnItem3.Text = "A3 \n" + price3 + ".00";
            btnItem4.Text = "B1 \n" + price4 + ".00"; btnItem5.Text = "B2 \n" + price5 + ".00"; btnItem6.Text = "B3 \n" + price6 + ".00";
            btnItem7.Text = "C1 \n" + price7 + ".00"; btnItem8.Text = "C2 \n" + price8 + ".00"; btnItem9.Text = "C3 \n" + price9 + ".00";
            btnItem10.Text = "D1 \n" + price10 + ".00"; btnItem11.Text = "D2 \n" + price11 + ".00"; btnItem12.Text = "D3 \n" + price12 + ".00";

            lblProduct1.Text = item1; lblProduct2.Text = item2; lblProduct3.Text = item3;
            lblProduct4.Text = item4; lblProduct5.Text = item5; lblProduct6.Text = item6;
            lblProduct7.Text = item7; lblProduct8.Text = item8; lblProduct9.Text = item9;
            lblProduct10.Text = item10; lblProduct11.Text = item11; lblProduct12.Text = item12;
            formResize();
        }


        private bool checkBuyer()
        {
            if (txtEmpID.Text == loadedEmployee) // Hardcoded check for buyer. must be done in database 
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
            else if (transactionMode == "credit" && ((loadedCredit - creditLimit) > price1))
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
            txtName.Size = new System.Drawing.Size(grpVendoUI.Size.Width - 4, 45);
            txtCurrentCredit.Size = new System.Drawing.Size(grpVendoUI.Size.Width - lblCreditAmount.Size.Width, 45);
            txtCurrentCredit.Location = new System.Drawing.Point(lblCreditAmount.Size.Width + 2, txtName.Location.Y + txtName.Size.Height + 5);
            btnCash.Location = new System.Drawing.Point((grpVendoUI.Size.Width / 3) - (btnCash.Size.Width / 2) - (50), txtCurrentCredit.Location.Y + txtCurrentCredit.Size.Height + 15);
            btnCredit.Location = new System.Drawing.Point((grpVendoUI.Size.Width * 2 / 3) - (btnCredit.Size.Width / 2) + (50), btnCash.Location.Y);
            lblCoinsInserted.Location = new System.Drawing.Point((grpVendoUI.Size.Width / 2) - (lblCoinsInserted.Size.Width / 2), btnCredit.Location.Y + btnCredit.Size.Height + 15);
            btnCancel.Location = new System.Drawing.Point(grpVendoUI.Size.Width / 2 - ((btnCancel.Size.Width) / 2), lblProduct10.Location.Y + lblProduct10.Size.Height + 15);
            txtEmpID.Location = new System.Drawing.Point(ClientSize.Width / 2, ClientSize.Height / 2);

            btnItem4.Location = new System.Drawing.Point(btnItem1.Location.X, btnItem1.Location.Y + btnItem1.Size.Height + 30);
            btnItem5.Location = new System.Drawing.Point(btnItem2.Location.X, btnItem4.Location.Y);
            btnItem1.Location = new System.Drawing.Point((grpVendoUI.Size.Width * 1 / 4) - (btnItem1.Size.Width / 2), lblCoinsInserted.Location.Y + lblCoinsInserted.Size.Height + 20);
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

            lblProduct1.Location = new System.Drawing.Point((btnItem1.Location.X + (btnItem1.Size.Width) / 2) - lblProduct1.Size.Width / 2, btnItem1.Location.Y + btnItem1.Size.Height + 5);
            lblProduct2.Location = new System.Drawing.Point((btnItem2.Location.X + (btnItem2.Size.Width) / 2) - lblProduct2.Size.Width / 2, lblProduct1.Location.Y);
            lblProduct3.Location = new System.Drawing.Point((btnItem3.Location.X + (btnItem3.Size.Width) / 2) - lblProduct3.Size.Width / 2, lblProduct1.Location.Y);
            lblProduct4.Location = new System.Drawing.Point((btnItem4.Location.X + (btnItem4.Size.Width) / 2) - lblProduct4.Size.Width / 2, btnItem4.Location.Y + btnItem4.Size.Height + 5);
            lblProduct5.Location = new System.Drawing.Point((btnItem5.Location.X + (btnItem5.Size.Width) / 2) - lblProduct5.Size.Width / 2, lblProduct4.Location.Y);
            lblProduct6.Location = new System.Drawing.Point((btnItem6.Location.X + (btnItem6.Size.Width) / 2) - lblProduct6.Size.Width / 2, lblProduct4.Location.Y);
            lblProduct7.Location = new System.Drawing.Point((btnItem7.Location.X + (btnItem7.Size.Width) / 2) - lblProduct7.Size.Width / 2, btnItem7.Location.Y + btnItem7.Size.Height + 5);
            lblProduct8.Location = new System.Drawing.Point((btnItem8.Location.X + (btnItem8.Size.Width) / 2) - lblProduct8.Size.Width / 2, lblProduct7.Location.Y);
            lblProduct9.Location = new System.Drawing.Point((btnItem9.Location.X + (btnItem9.Size.Width) / 2) - lblProduct9.Size.Width / 2, lblProduct7.Location.Y);
            lblProduct10.Location = new System.Drawing.Point((btnItem10.Location.X + (btnItem10.Size.Width) / 2) - lblProduct10.Size.Width / 2, btnItem10.Location.Y + btnItem10.Size.Height + 5);
            lblProduct11.Location = new System.Drawing.Point((btnItem11.Location.X + (btnItem11.Size.Width) / 2) - lblProduct11.Size.Width / 2, lblProduct10.Location.Y);
            lblProduct12.Location = new System.Drawing.Point((btnItem12.Location.X + (btnItem12.Size.Width) / 2) - lblProduct12.Size.Width / 2, lblProduct10.Location.Y);
        }

    }
}