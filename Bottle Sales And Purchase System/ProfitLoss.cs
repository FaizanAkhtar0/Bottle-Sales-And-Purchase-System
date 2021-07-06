using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bottle_Sales_And_Purchase_System
{
    public partial class ProfitLoss : Form
    {
        private int ID = -1, Gain = 0, Loss = 0;
        private double totalPurchase = 0D, totalSales = 0D, totalPayRoll = 0D, totalLoan = 0D, totalSellerAdvance = 0D, totalBuyerAdvance = 0D;
        private List<double> payrolls = new List<double>();
        private List<double> AllLoans = new List<double>();
        private List<double> AllAdvance = new List<double>();

        public bool flag_night_mode;

        public SqlConnection con = new SqlConnection("Data Source=DESKTOP-CQBGF97\\SQLEXPRESS;Integrated Security=True");

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        private void timer1_Tick(object sender, EventArgs e)
        {
            if(!(profit_progress.Value == this.Gain)) {
                profit_progress.Value += 1;
            } else {
                timer1.Stop();
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if(!(loss_progress.Value == this.Loss)) {
                loss_progress.Value += 1;
            } else {
                timer2.Stop();
            }
        }

        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        public ProfitLoss(ref bool param)
        {
            InitializeComponent();
            this.flag_night_mode = param;
        }

        private void bunifuImageButton4_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btn_close_form_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void bunifuShadowPanel3_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void bunifuShadowPanel2_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void bunifuShadowPanel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void bunifuShadowPanel4_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void ProfitLoss_Load(object sender, EventArgs e)
        {
            initilizeView();
            if (flag_night_mode == true) {
                this.BackColor = Color.DarkGray;
            }
        }

        private void initilizeView()
        {
            try {
                String conString = "select price from db_plastic_management.dbo.KabariaSeller";
                SqlCommand cmd = new SqlCommand(conString, con);

                DataTable dt_total_purchase = new DataTable();
                con.Open();
                dt_total_purchase.Load(cmd.ExecuteReader());

                foreach(DataRow row in dt_total_purchase.Rows) {
                    this.totalPurchase += Convert.ToDouble(row[0]);
                }

                if (!(this.totalPurchase == 0D)) {
                    lbl_total_purchase.Text = this.totalPurchase.ToString();
                    lbl_total_purchase1.Text = this.totalPurchase.ToString();
                }
                con.Close();
            } catch (Exception ex) {
                return;
            }

            try
            {
                String conString = "select price from db_plastic_management.dbo.KabariaBuyer";
                SqlCommand cmd = new SqlCommand(conString, con);

                DataTable dt_total_sale = new DataTable();
                con.Open();
                dt_total_sale.Load(cmd.ExecuteReader());

                foreach (DataRow row in dt_total_sale.Rows)
                {
                    this.totalSales += Convert.ToDouble(row[0]);
                }

                if (!(this.totalPurchase == 0D))
                {
                    lbl_total_sale.Text = this.totalSales.ToString();
                    lbl_total_sale1.Text = this.totalSales.ToString();
                }
                con.Close();
            }
            catch (Exception ex)
            {
                return;
            }


            if (this.totalSales < this.totalPurchase) {
                Loss = Convert.ToInt32(Math.Round(((this.totalPurchase - this.totalSales) / (this.totalPurchase)) * 100));
                Gain = 0;
                timer1.Start();
                timer2.Start();
            }else if (this.totalSales == this.totalPurchase) {
               
            } else {
                Gain = Convert.ToInt32(Math.Round(((this.totalSales - this.totalPurchase) / (this.totalSales)) * 100));
                Loss = 0;
                timer1.Start();
                timer2.Start();
            }

            CalculateFinalPayRoll();
        }

        private void CalculateFinalPayRoll() {
            CalculateLoaderPayRoll();
            CalculateSeparatorPayRoll();
            CalculateCrusherPayRoll();
            CalculateAdvances();
            foreach (double pay in payrolls) {
                totalPayRoll += pay;
            }

            foreach (double loan in AllLoans) {
                totalLoan += loan;
            }

            if(!(this.totalLoan == 0D)) {
                lbl_loan.Text = totalLoan.ToString();
            }

            if (!(this.totalPayRoll == 0D)) {
                lbl_payroll.Text = this.totalPayRoll.ToString();
            }
        }

        private void CalculateAdvances()
        {
            try {
                String conString = "select advance_money from db_plastic_management.dbo.KabariaSeller";
                SqlCommand cmd = new SqlCommand(conString, con);

                DataTable dt_advances = new DataTable();

                con.Open();
                dt_advances.Load(cmd.ExecuteReader());

                foreach(DataRow row in dt_advances.Rows) {
                    totalSellerAdvance += Convert.ToDouble(row[0].ToString());
                }
                con.Close();

                if (!(totalSellerAdvance == 0D)) {
                    lbl_seller_advance.Text = this.totalSellerAdvance.ToString();
                }
            } catch (Exception ex) {
                return;
            }

            try {
                String conString = "select advance_money from db_plastic_management.dbo.KabariaBuyer";
                SqlCommand cmd = new SqlCommand(conString, con);

                DataTable dt_advances = new DataTable();

                con.Open();
                dt_advances.Load(cmd.ExecuteReader());

                foreach(DataRow row in dt_advances.Rows) {
                    totalBuyerAdvance += Convert.ToDouble(row[0].ToString());
                }
                con.Close();

                if (!(totalBuyerAdvance == 0D)) {
                    lbl_buyer_advance.Text = this.totalBuyerAdvance.ToString();
                }
            } catch (Exception ex) {
                return;
            }
        }

        private void CalculateSeparatorPayRoll()
        {
            List<int> ids = new List<int>();
            List<string> names = new List<string>();
            List<double> loans = new List<double>();
            List<double> advances = new List<double>();
            List<double> payments = new List<double>();
            

            if (!(this.ID == -1))
            {

            }
            else
            {
                String conString = "select e_id, name, loan from db_plastic_management.dbo.SeparatorEmployees";
                SqlCommand cmd = new SqlCommand(conString, con);

                DataTable dt_loans = new DataTable();

                con.Open();
                dt_loans.Load(cmd.ExecuteReader());

                foreach (DataRow row in dt_loans.Rows)
                {
                    ids.Add(Convert.ToInt32(row[0].ToString()));
                    names.Add(row[1].ToString());
                    loans.Add(Convert.ToDouble(row[2]));
                }



                foreach (int id in ids)
                {
                    String conString1 = "select advance from db_plastic_management.dbo.SeparatorWork where e_id = '" + id + "'";
                    SqlCommand cmd1 = new SqlCommand(conString1, con);

                    DataTable dt_advances = new DataTable();

                    dt_advances.Load(cmd1.ExecuteReader());

                    double value = 0D;

                    foreach (DataRow row in dt_advances.Rows)
                    {
                        double tempVal = Convert.ToDouble(row[0]);
                        value += tempVal;
                    }

                    advances.Add(value);
                }
                ids.Sort();
                foreach (int id in ids)
                {
                    String conString1 = "select pay from db_plastic_management.dbo.SeparatorWork where e_id = '" + id + "'";
                    SqlCommand cmd1 = new SqlCommand(conString1, con);

                    DataTable dt_payments = new DataTable();

                    dt_payments.Load(cmd1.ExecuteReader());

                    double value = 0.0D;

                    foreach (DataRow row in dt_payments.Rows)
                    {
                        double tempVal = Convert.ToDouble(row[0].ToString());
                        value += tempVal;
                    }
                    payments.Add(value);
                }

                //loader_emp_view.DataSource = advances.Select(x => new { Value = x }).ToList();

                DataTable dt = new DataTable();
                for (int i = 0; i < loans.Count; i++)
                {
                    double tempLoan = loans[i];
                    AllLoans.Add(loans[i]);
                    double tempAdvance = advances[i];
                    double tempPayment = payments[i];
                    double tempPayRoll = (tempPayment - tempLoan);
                    payrolls.Add(tempPayRoll);
                }

                con.Close();
            }
        }

        private void CalculateLoaderPayRoll()
        {
            List<int> ids = new List<int>();
            List<string> names = new List<string>();
            List<double> loans = new List<double>();
            List<double> advances = new List<double>();
            List<double> payments = new List<double>();


            if (!(this.ID == -1))
            {

            }
            else
            {
                String conString = "select e_id, name, loan from db_plastic_management.dbo.Employees";
                SqlCommand cmd = new SqlCommand(conString, con);

                DataTable dt_loans = new DataTable();

                con.Open();
                dt_loans.Load(cmd.ExecuteReader());

                foreach (DataRow row in dt_loans.Rows)
                {
                    ids.Add(Convert.ToInt32(row[0].ToString()));
                    names.Add(row[1].ToString());
                    loans.Add(Convert.ToDouble(row[2]));
                }



                foreach (int id in ids)
                {
                    String conString1 = "select advance from db_plastic_management.dbo.LoadingWork where e_id = '" + id + "'";
                    SqlCommand cmd1 = new SqlCommand(conString1, con);

                    DataTable dt_advances = new DataTable();

                    dt_advances.Load(cmd1.ExecuteReader());

                    double value = 0D;

                    foreach (DataRow row in dt_advances.Rows)
                    {
                        double tempVal = Convert.ToDouble(row[0]);
                        value += tempVal;
                    }

                    advances.Add(value);
                }
                ids.Sort();
                foreach (int id in ids)
                {
                    String conString1 = "select pay from db_plastic_management.dbo.LoadingWork where e_id = '" + id + "'";
                    SqlCommand cmd1 = new SqlCommand(conString1, con);

                    DataTable dt_payments = new DataTable();

                    dt_payments.Load(cmd1.ExecuteReader());

                    double value = 0.0D;

                    foreach (DataRow row in dt_payments.Rows)
                    {
                        double tempVal = Convert.ToDouble(row[0].ToString());
                        value += tempVal;
                    }
                    payments.Add(value);
                }

                //loader_emp_view.DataSource = advances.Select(x => new { Value = x }).ToList();

                DataTable dt = new DataTable();
                for (int i = 0; i < loans.Count; i++)
                {
                    double tempLoan = loans[i];
                    AllLoans.Add(loans[i]);
                    double tempAdvance = advances[i];
                    double tempPayment = payments[i];
                    double tempPayRoll = (tempPayment - tempLoan);
                    payrolls.Add(tempPayRoll);
                }

                con.Close();
            }
        }

        private void CalculateCrusherPayRoll()
        {
            List<int> ids = new List<int>();
            List<string> names = new List<string>();
            List<double> loans = new List<double>();
            List<double> advances = new List<double>();
            List<double> payments = new List<double>();


            if (!(this.ID == -1))
            {

            }
            else
            {
                String conString = "select e_id, name, loan from db_plastic_management.dbo.BottleCrusherEmployees";
                SqlCommand cmd = new SqlCommand(conString, con);

                DataTable dt_loans = new DataTable();

                con.Open();
                dt_loans.Load(cmd.ExecuteReader());

                foreach (DataRow row in dt_loans.Rows)
                {
                    ids.Add(Convert.ToInt32(row[0].ToString()));
                    names.Add(row[1].ToString());
                    loans.Add(Convert.ToDouble(row[2]));
                }



                foreach (int id in ids)
                {
                    String conString1 = "select advance from db_plastic_management.dbo.CrusherWork where e_id = '" + id + "'";
                    SqlCommand cmd1 = new SqlCommand(conString1, con);

                    DataTable dt_advances = new DataTable();

                    dt_advances.Load(cmd1.ExecuteReader());

                    double value = 0D;

                    foreach (DataRow row in dt_advances.Rows)
                    {
                        double tempVal = Convert.ToDouble(row[0]);
                        value += tempVal;
                    }

                    advances.Add(value);
                }
                ids.Sort();
                foreach (int id in ids)
                {
                    String conString1 = "select pay from db_plastic_management.dbo.CrusherWork where e_id = '" + id + "'";
                    SqlCommand cmd1 = new SqlCommand(conString1, con);

                    DataTable dt_payments = new DataTable();

                    dt_payments.Load(cmd1.ExecuteReader());

                    double value = 0.0D;

                    foreach (DataRow row in dt_payments.Rows)
                    {
                        double tempVal = Convert.ToDouble(row[0].ToString());
                        value += tempVal;
                    }
                    payments.Add(value);
                }

                //loader_emp_view.DataSource = advances.Select(x => new { Value = x }).ToList();

                DataTable dt = new DataTable();
                for (int i = 0; i < loans.Count; i++)
                {
                    double tempLoan = loans[i];
                    AllLoans.Add(loans[i]);
                    double tempAdvance = advances[i];
                    double tempPayment = payments[i];
                    double tempPayRoll = (tempPayment - tempLoan);
                    payrolls.Add(tempPayRoll);
                }
                con.Close();
            }
        }
    }
}
