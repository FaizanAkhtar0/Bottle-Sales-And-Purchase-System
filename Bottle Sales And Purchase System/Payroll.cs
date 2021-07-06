using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;


namespace Bottle_Sales_And_Purchase_System
{
    public partial class Payroll : Form
    {
        private int ID = -1;
        private double actualPrice = -1D, actualLoadingPrice = -1D, actualDieselPrice = -1D;

        public bool flag_night_mode;

        public SqlConnection con = new SqlConnection("Data Source=DESKTOP-CQBGF97\\SQLEXPRESS;Integrated Security=True");

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        public Payroll(ref bool param)
        {
            InitializeComponent();
            this.flag_night_mode = param;
        }

        private void bunifuImageButton3_Click(object sender, EventArgs e)
        {
            CalculateLoaderPayRoll();
        }

        private void separator_emp_Click(object sender, EventArgs e)
        {
            CalculateSeparatorPayRoll();
        }

        private void CalculateSeparatorPayRoll()
        {
            List<int> ids = new List<int>();
            List<string> names = new List<string>();
            List<double> loans = new List<double>();
            List<double> advances = new List<double>();
            List<double> payments = new List<double>();
            List<double> payrolls = new List<double>();

            if (!(this.ID == -1)) {

            } else {
                String conString = "select e_id, name, loan from db_plastic_management.dbo.SeparatorEmployees";
                SqlCommand cmd = new SqlCommand(conString, con);

                DataTable dt_loans = new DataTable();

                con.Open();
                dt_loans.Load(cmd.ExecuteReader());

                foreach(DataRow row in dt_loans.Rows) {
                    ids.Add(Convert.ToInt32(row[0].ToString()));
                    names.Add(row[1].ToString());
                    loans.Add(Convert.ToDouble(row[2]));
                }

                

                foreach(int id in ids) {
                    String conString1 = "select advance from db_plastic_management.dbo.SeparatorWork where e_id = '" + id + "'";
                    SqlCommand cmd1 = new SqlCommand(conString1, con);

                    DataTable dt_advances = new DataTable();

                    dt_advances.Load(cmd1.ExecuteReader());

                    double value = 0D;

                    foreach(DataRow row in dt_advances.Rows) {
                        double tempVal = Convert.ToDouble(row[0]);
                        value += tempVal;
                    }

                    advances.Add(value);
                }
                ids.Sort();
                foreach (int id in ids) {
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
                    double tempAdvance = advances[i];
                    double tempPayment = payments[i];
                    double tempPayRoll = (tempPayment - tempLoan);
                    payrolls.Add(tempPayRoll);
                }

                
                dt.Columns.Add("Names");
                dt.Columns.Add("IDS");
                dt.Columns.Add("Total Pays");
                dt.Columns.Add("loans");
                dt.Columns.Add("PayRolls");
                for(int i = 0; i < ids.Count; i++) {
                    
                    dt.Rows.Add(names[i]);
                    dt.Rows[i]["PayRolls"] = payrolls[i].ToString();
                    dt.Rows[i]["loans"] = loans[i].ToString();
                    dt.Rows[i]["Total Pays"] = payments[i].ToString();
                    dt.Rows[i]["IDS"] = ids[i].ToString();
                }
                separator_emp_view.DataSource = dt;
                separator_emp_view.Show();
                con.Close();


                try {
                    SaveFileDialog savefile = new SaveFileDialog();
                    savefile.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm";

                    if (savefile.ShowDialog() == DialogResult.OK) {
                        DataSet ds = new DataSet("New_DataSet");

                        string saveDirectory = savefile.FileName;
                        ds.Locale = System.Threading.Thread.CurrentThread.CurrentCulture;
                        dt.Locale = System.Threading.Thread.CurrentThread.CurrentCulture;

                        ds.Tables.Add(dt);

                        ExcelLibrary.DataSetHelper.CreateWorkbook(saveDirectory + "Payroll_Separator_Employees_Report.xls", ds);

                        advances = new List<double>();
                        payments = new List<double>();
                        payrolls = new List<double>();
                    }
                } catch (Exception ex) {
                    MessageBox.Show("Unable to save the file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void crusher_emp_Click(object sender, EventArgs e)
        {
            List<int> ids = new List<int>();
            List<string> names = new List<string>();
            List<double> loans = new List<double>();
            List<double> advances = new List<double>();
            List<double> payments = new List<double>();
            List<double> payrolls = new List<double>();

            if (!(this.ID == -1)) {

            } else {
                String conString = "select e_id, name, loan from db_plastic_management.dbo.BottleCrusherEmployees";
                SqlCommand cmd = new SqlCommand(conString, con);

                DataTable dt_loans = new DataTable();

                con.Open();
                dt_loans.Load(cmd.ExecuteReader());

                foreach(DataRow row in dt_loans.Rows) {
                    ids.Add(Convert.ToInt32(row[0].ToString()));
                    names.Add(row[1].ToString());
                    loans.Add(Convert.ToDouble(row[2]));
                }

                

                foreach(int id in ids) {
                    String conString1 = "select advance from db_plastic_management.dbo.CrusherWork where e_id = '" + id + "'";
                    SqlCommand cmd1 = new SqlCommand(conString1, con);

                    DataTable dt_advances = new DataTable();

                    dt_advances.Load(cmd1.ExecuteReader());

                    double value = 0D;

                    foreach(DataRow row in dt_advances.Rows) {
                        double tempVal = Convert.ToDouble(row[0]);
                        value += tempVal;
                    }

                    advances.Add(value);
                }
                ids.Sort();
                foreach (int id in ids) {
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
                    double tempAdvance = advances[i];
                    double tempPayment = payments[i];
                    double tempPayRoll = (tempPayment - tempLoan);
                    payrolls.Add(tempPayRoll);
                }

                
                dt.Columns.Add("Names");
                dt.Columns.Add("IDS");
                dt.Columns.Add("Total Pays");
                dt.Columns.Add("loans");
                dt.Columns.Add("PayRolls");
                for(int i = 0; i < ids.Count; i++) {
                    
                    dt.Rows.Add(names[i]);
                    dt.Rows[i]["PayRolls"] = payrolls[i].ToString();
                    dt.Rows[i]["loans"] = loans[i].ToString();
                    dt.Rows[i]["Total Pays"] = payments[i].ToString();
                    dt.Rows[i]["IDS"] = ids[i].ToString();
                }
                crusher_emp_view.DataSource = dt;
                crusher_emp_view.Show();
                con.Close();

                try {
                    SaveFileDialog savefile = new SaveFileDialog();
                    savefile.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm";

                    if (savefile.ShowDialog() == DialogResult.OK) {
                        DataSet ds = new DataSet("New_DataSet");

                        string saveDirectory = savefile.FileName;
                        ds.Locale = System.Threading.Thread.CurrentThread.CurrentCulture;
                        dt.Locale = System.Threading.Thread.CurrentThread.CurrentCulture;

                        ds.Tables.Add(dt);

                        ExcelLibrary.DataSetHelper.CreateWorkbook(saveDirectory + "Payroll_Crusher_Employees_Report.xls", ds);

                        advances = new List<double>();
                        payments = new List<double>();
                        payrolls = new List<double>();
                    }
                } catch (Exception ex) {
                    MessageBox.Show("Unable to save the file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
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

        private void Payroll_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void bunifuImageButton4_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btn_close_form_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void loader_emp_refresh_Click(object sender, EventArgs e)
        {
            List<int> ids = new List<int>();
            List<string> names = new List<string>();
            List<double> loans = new List<double>();
            List<double> advances = new List<double>();
            List<double> payments = new List<double>();
            List<double> payrolls = new List<double>();

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
                    double tempAdvance = advances[i];
                    double tempPayment = payments[i];
                    double tempPayRoll = (tempPayment - tempLoan);
                    payrolls.Add(tempPayRoll);
                }


                dt.Columns.Add("Names");
                dt.Columns.Add("IDS");
                dt.Columns.Add("Total Pays");
                dt.Columns.Add("loans");
                dt.Columns.Add("PayRolls");
                for (int i = 0; i < ids.Count; i++)
                {

                    dt.Rows.Add(names[i]);
                    dt.Rows[i]["PayRolls"] = payrolls[i].ToString();
                    dt.Rows[i]["loans"] = loans[i].ToString();
                    dt.Rows[i]["Total Pays"] = payments[i].ToString();
                    dt.Rows[i]["IDS"] = ids[i].ToString();
                }
                loader_emp_view.DataSource = dt;
                loader_emp_view.Show();
                con.Close();

                advances = new List<double>();
                payments = new List<double>();
                payrolls = new List<double>();
            }
        }

        private void initilizeView() {
            try
            {
                List<int> ids = new List<int>();
                List<string> names = new List<string>();
                List<double> loans = new List<double>();
                List<double> advances = new List<double>();
                List<double> payments = new List<double>();
                List<double> payrolls = new List<double>();

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
                        double tempAdvance = advances[i];
                        double tempPayment = payments[i];
                        double tempPayRoll = (tempPayment - tempLoan);
                        payrolls.Add(tempPayRoll);
                    }


                    dt.Columns.Add("Names");
                    dt.Columns.Add("IDS");
                    dt.Columns.Add("Total Pays");
                    dt.Columns.Add("loans");
                    dt.Columns.Add("PayRolls");
                    for (int i = 0; i < ids.Count; i++)
                    {

                        dt.Rows.Add(names[i]);
                        dt.Rows[i]["PayRolls"] = payrolls[i].ToString();
                        dt.Rows[i]["loans"] = loans[i].ToString();
                        dt.Rows[i]["Total Pays"] = payments[i].ToString();
                        dt.Rows[i]["IDS"] = ids[i].ToString();
                    }
                    loader_emp_view.DataSource = dt;
                    loader_emp_view.Show();
                    con.Close();

                    advances = new List<double>();
                    payments = new List<double>();
                    payrolls = new List<double>();
                }
            } catch (Exception ex) {
                MessageBox.Show("Unable to initilize Loader Employees View", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try {
                List<int> ids = new List<int>();
                List<string> names = new List<string>();
                List<double> loans = new List<double>();
                List<double> advances = new List<double>();
                List<double> payments = new List<double>();
                List<double> payrolls = new List<double>();

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
                        double tempAdvance = advances[i];
                        double tempPayment = payments[i];
                        double tempPayRoll = (tempPayment - tempLoan);
                        payrolls.Add(tempPayRoll);
                    }


                    dt.Columns.Add("Names");
                    dt.Columns.Add("IDS");
                    dt.Columns.Add("Total Pays");
                    dt.Columns.Add("loans");
                    dt.Columns.Add("PayRolls");
                    for (int i = 0; i < ids.Count; i++)
                    {

                        dt.Rows.Add(names[i]);
                        dt.Rows[i]["PayRolls"] = payrolls[i].ToString();
                        dt.Rows[i]["loans"] = loans[i].ToString();
                        dt.Rows[i]["Total Pays"] = payments[i].ToString();
                        dt.Rows[i]["IDS"] = ids[i].ToString();
                    }
                    separator_emp_view.DataSource = dt;
                    separator_emp_view.Show();
                    con.Close();

                    advances = new List<double>();
                    payments = new List<double>();
                    payrolls = new List<double>();
                }
            } catch (Exception ex) {
                MessageBox.Show("Unable to initilize Separator Employees View", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try {

                List<int> ids = new List<int>();
                List<string> names = new List<string>();
                List<double> loans = new List<double>();
                List<double> advances = new List<double>();
                List<double> payments = new List<double>();
                List<double> payrolls = new List<double>();

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
                        double tempAdvance = advances[i];
                        double tempPayment = payments[i];
                        double tempPayRoll = (tempPayment - tempLoan);
                        payrolls.Add(tempPayRoll);
                    }


                    dt.Columns.Add("Names");
                    dt.Columns.Add("IDS");
                    dt.Columns.Add("Total Pays");
                    dt.Columns.Add("loans");
                    dt.Columns.Add("PayRolls");
                    for (int i = 0; i < ids.Count; i++)
                    {

                        dt.Rows.Add(names[i]);
                        dt.Rows[i]["PayRolls"] = payrolls[i].ToString();
                        dt.Rows[i]["loans"] = loans[i].ToString();
                        dt.Rows[i]["Total Pays"] = payments[i].ToString();
                        dt.Rows[i]["IDS"] = ids[i].ToString();
                    }
                    crusher_emp_view.DataSource = dt;
                    crusher_emp_view.Show();
                    con.Close();

                    advances = new List<double>();
                    payments = new List<double>();
                    payrolls = new List<double>();
                }
            } catch (Exception ex) {
                MessageBox.Show("Unable to initilize Crusher Employees View", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }

        private void separator_emp_refresh_Click(object sender, EventArgs e)
        {
            List<int> ids = new List<int>();
            List<string> names = new List<string>();
            List<double> loans = new List<double>();
            List<double> advances = new List<double>();
            List<double> payments = new List<double>();
            List<double> payrolls = new List<double>();

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
                    double tempAdvance = advances[i];
                    double tempPayment = payments[i];
                    double tempPayRoll = (tempPayment - tempLoan);
                    payrolls.Add(tempPayRoll);
                }


                dt.Columns.Add("Names");
                dt.Columns.Add("IDS");
                dt.Columns.Add("Total Pays");
                dt.Columns.Add("loans");
                dt.Columns.Add("PayRolls");
                for (int i = 0; i < ids.Count; i++)
                {

                    dt.Rows.Add(names[i]);
                    dt.Rows[i]["PayRolls"] = payrolls[i].ToString();
                    dt.Rows[i]["loans"] = loans[i].ToString();
                    dt.Rows[i]["Total Pays"] = payments[i].ToString();
                    dt.Rows[i]["IDS"] = ids[i].ToString();
                }
                separator_emp_view.DataSource = dt;
                separator_emp_view.Show();
                con.Close();

                advances = new List<double>();
                payments = new List<double>();
                payrolls = new List<double>();
            }
        }

        private void Crusher_emp_refresh_Click(object sender, EventArgs e)
        {
            List<int> ids = new List<int>();
            List<string> names = new List<string>();
            List<double> loans = new List<double>();
            List<double> advances = new List<double>();
            List<double> payments = new List<double>();
            List<double> payrolls = new List<double>();

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
                    double tempAdvance = advances[i];
                    double tempPayment = payments[i];
                    double tempPayRoll = (tempPayment - tempLoan);
                    payrolls.Add(tempPayRoll);
                }


                dt.Columns.Add("Names");
                dt.Columns.Add("IDS");
                dt.Columns.Add("Total Pays");
                dt.Columns.Add("loans");
                dt.Columns.Add("PayRolls");
                for (int i = 0; i < ids.Count; i++)
                {

                    dt.Rows.Add(names[i]);
                    dt.Rows[i]["PayRolls"] = payrolls[i].ToString();
                    dt.Rows[i]["loans"] = loans[i].ToString();
                    dt.Rows[i]["Total Pays"] = payments[i].ToString();
                    dt.Rows[i]["IDS"] = ids[i].ToString();
                }
                crusher_emp_view.DataSource = dt;
                crusher_emp_view.Show();
                con.Close();

                advances = new List<double>();
                payments = new List<double>();
                payrolls = new List<double>();
            }
        }

        private void export_loader_pdf_Click(object sender, EventArgs e)
        {
            List<int> ids = new List<int>();
            List<string> names = new List<string>();
            List<double> loans = new List<double>();
            List<double> advances = new List<double>();
            List<double> payments = new List<double>();
            List<double> payrolls = new List<double>();

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
                    double tempAdvance = advances[i];
                    double tempPayment = payments[i];
                    double tempPayRoll = (tempPayment - tempLoan);
                    payrolls.Add(tempPayRoll);
                }


                dt.Columns.Add("Names");
                dt.Columns.Add("IDS");
                dt.Columns.Add("Total Pays");
                dt.Columns.Add("loans");
                dt.Columns.Add("PayRolls");
                for (int i = 0; i < ids.Count; i++)
                {

                    dt.Rows.Add(names[i]);
                    dt.Rows[i]["PayRolls"] = payrolls[i].ToString();
                    dt.Rows[i]["loans"] = loans[i].ToString();
                    dt.Rows[i]["Total Pays"] = payments[i].ToString();
                    dt.Rows[i]["IDS"] = ids[i].ToString();
                }
                loader_emp_view.DataSource = dt;
                loader_emp_view.Show();
                con.Close();
            }

            SaveFileDialog savepdf = new SaveFileDialog();
            if (savepdf.ShowDialog() == DialogResult.OK) {

                string saveDirectory = savepdf.FileName;
                Document document = new Document();
                Paragraph paragraph = new Paragraph();
                

                document.Open();
                paragraph.Add(new Chunk("\nDate : " + DateTime.Now.ToShortDateString()));
                paragraph.Add(new Chunk("\n"));
                document.Add(paragraph);

                PdfPTable pdfTable = new PdfPTable(loader_emp_view.ColumnCount);

                pdfTable.DefaultCell.Padding = 3;

                pdfTable.WidthPercentage = 70;
                pdfTable.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfTable.DefaultCell.BorderWidth = 1;

                foreach (DataGridViewColumn column in loader_emp_view.Columns) {
                    PdfPCell cell = new PdfPCell(new Phrase(column.HeaderText));
                    cell.BackgroundColor = new iTextSharp.text.BaseColor(240, 240, 240);
                    pdfTable.AddCell(cell);
                }

                foreach (DataGridViewRow row in loader_emp_view.Rows) {
                    foreach (DataGridViewCell cell in row.Cells) {
                        if (cell.Value == null) {

                        } else {
                            pdfTable.AddCell(cell.Value.ToString());
                        }
                    }
                }

                using (FileStream stream = new FileStream(saveDirectory + "_Loader_Employee.pdf", FileMode.Create))
                {
                    Document pdfDoc = new Document(PageSize.A4);

                    PdfWriter.GetInstance(pdfDoc, stream);
                    pdfDoc.Open();
                    pdfDoc.Add(pdfTable);
                    pdfDoc.Close();
                    stream.Close();
                }


                advances = new List<double>();
                payments = new List<double>();
                payrolls = new List<double>();
            }
        }

        private void export_separator_pdf_Click(object sender, EventArgs e)
        {
            List<int> ids = new List<int>();
            List<string> names = new List<string>();
            List<double> loans = new List<double>();
            List<double> advances = new List<double>();
            List<double> payments = new List<double>();
            List<double> payrolls = new List<double>();

            if (!(this.ID == -1)) {

            } else {
                String conString = "select e_id, name, loan from db_plastic_management.dbo.SeparatorEmployees";
                SqlCommand cmd = new SqlCommand(conString, con);

                DataTable dt_loans = new DataTable();

                con.Open();
                dt_loans.Load(cmd.ExecuteReader());

                foreach (DataRow row in dt_loans.Rows) {
                    ids.Add(Convert.ToInt32(row[0].ToString()));
                    names.Add(row[1].ToString());
                    loans.Add(Convert.ToDouble(row[2]));
                }



                foreach (int id in ids) {
                    String conString1 = "select advance from db_plastic_management.dbo.SeparatorWork where e_id = '" + id + "'";
                    SqlCommand cmd1 = new SqlCommand(conString1, con);

                    DataTable dt_advances = new DataTable();

                    dt_advances.Load(cmd1.ExecuteReader());

                    double value = 0D;

                    foreach (DataRow row in dt_advances.Rows) {
                        double tempVal = Convert.ToDouble(row[0]);
                        value += tempVal;
                    }

                    advances.Add(value);
                }
                ids.Sort();
                foreach (int id in ids) {
                    String conString1 = "select pay from db_plastic_management.dbo.SeparatorWork where e_id = '" + id + "'";
                    SqlCommand cmd1 = new SqlCommand(conString1, con);

                    DataTable dt_payments = new DataTable();

                    dt_payments.Load(cmd1.ExecuteReader());

                    double value = 0.0D;

                    foreach (DataRow row in dt_payments.Rows) {
                        double tempVal = Convert.ToDouble(row[0].ToString());
                        value += tempVal;
                    }
                    payments.Add(value);
                }

                //loader_emp_view.DataSource = advances.Select(x => new { Value = x }).ToList();

                DataTable dt = new DataTable();
                for (int i = 0; i < loans.Count; i++) {
                    double tempLoan = loans[i];
                    double tempAdvance = advances[i];
                    double tempPayment = payments[i];
                    double tempPayRoll = (tempPayment - tempLoan);
                    payrolls.Add(tempPayRoll);
                }


                dt.Columns.Add("Names");
                dt.Columns.Add("IDS");
                dt.Columns.Add("Total Pays");
                dt.Columns.Add("loans");
                dt.Columns.Add("PayRolls");
                for (int i = 0; i < ids.Count; i++) {

                    dt.Rows.Add(names[i]);
                    dt.Rows[i]["PayRolls"] = payrolls[i].ToString();
                    dt.Rows[i]["loans"] = loans[i].ToString();
                    dt.Rows[i]["Total Pays"] = payments[i].ToString();
                    dt.Rows[i]["IDS"] = ids[i].ToString();
                }
                separator_emp_view.DataSource = dt;
                separator_emp_view.Show();
                con.Close();
            }


            SaveFileDialog savepdf = new SaveFileDialog();
            if (savepdf.ShowDialog() == DialogResult.OK) {

                string saveDirectory = savepdf.FileName;
                Document document = new Document();
                Paragraph paragraph = new Paragraph();
                

                document.Open();
                paragraph.Add(new Chunk("\nDate : " + DateTime.Now.ToShortDateString()));
                paragraph.Add(new Chunk("\n"));
                document.Add(paragraph);

                PdfPTable pdfTable = new PdfPTable(separator_emp_view.ColumnCount);

                pdfTable.DefaultCell.Padding = 3;

                pdfTable.WidthPercentage = 70;
                pdfTable.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfTable.DefaultCell.BorderWidth = 1;

                foreach (DataGridViewColumn column in separator_emp_view.Columns) {
                    PdfPCell cell = new PdfPCell(new Phrase(column.HeaderText));
                    cell.BackgroundColor = new iTextSharp.text.BaseColor(240, 240, 240);
                    pdfTable.AddCell(cell);
                }

                foreach (DataGridViewRow row in separator_emp_view.Rows) {
                    foreach (DataGridViewCell cell in row.Cells) {
                        if (cell.Value == null) {

                        } else {
                            pdfTable.AddCell(cell.Value.ToString());
                        }
                    }
                }

                using (FileStream stream = new FileStream(saveDirectory + "_Separator_Employee.pdf", FileMode.Create))
                {
                    Document pdfDoc = new Document(PageSize.A4);

                    PdfWriter.GetInstance(pdfDoc, stream);
                    pdfDoc.Open();
                    pdfDoc.Add(pdfTable);
                    pdfDoc.Close();
                    stream.Close();
                }


                advances = new List<double>();
                payments = new List<double>();
                payrolls = new List<double>();
            }
        }

        private void export_crusher_pdf_Click(object sender, EventArgs e)
        {
            List<int> ids = new List<int>();
            List<string> names = new List<string>();
            List<double> loans = new List<double>();
            List<double> advances = new List<double>();
            List<double> payments = new List<double>();
            List<double> payrolls = new List<double>();

            if (!(this.ID == -1)) {

            } else {
                String conString = "select e_id, name, loan from db_plastic_management.dbo.BottleCrusherEmployees";
                SqlCommand cmd = new SqlCommand(conString, con);

                DataTable dt_loans = new DataTable();

                con.Open();
                dt_loans.Load(cmd.ExecuteReader());

                foreach (DataRow row in dt_loans.Rows) {
                    ids.Add(Convert.ToInt32(row[0].ToString()));
                    names.Add(row[1].ToString());
                    loans.Add(Convert.ToDouble(row[2]));
                }



                foreach (int id in ids) {
                    String conString1 = "select advance from db_plastic_management.dbo.CrusherWork where e_id = '" + id + "'";
                    SqlCommand cmd1 = new SqlCommand(conString1, con);

                    DataTable dt_advances = new DataTable();

                    dt_advances.Load(cmd1.ExecuteReader());

                    double value = 0D;

                    foreach (DataRow row in dt_advances.Rows) {
                        double tempVal = Convert.ToDouble(row[0]);
                        value += tempVal;
                    }

                    advances.Add(value);
                }
                ids.Sort();
                foreach (int id in ids) {
                    String conString1 = "select pay from db_plastic_management.dbo.CrusherWork where e_id = '" + id + "'";
                    SqlCommand cmd1 = new SqlCommand(conString1, con);

                    DataTable dt_payments = new DataTable();

                    dt_payments.Load(cmd1.ExecuteReader());

                    double value = 0.0D;

                    foreach (DataRow row in dt_payments.Rows) {
                        double tempVal = Convert.ToDouble(row[0].ToString());
                        value += tempVal;
                    }
                    payments.Add(value);
                }

                //loader_emp_view.DataSource = advances.Select(x => new { Value = x }).ToList();

                DataTable dt = new DataTable();
                for (int i = 0; i < loans.Count; i++) {
                    double tempLoan = loans[i];
                    double tempAdvance = advances[i];
                    double tempPayment = payments[i];
                    double tempPayRoll = (tempPayment - tempLoan);
                    payrolls.Add(tempPayRoll);
                }


                dt.Columns.Add("Names");
                dt.Columns.Add("IDS");
                dt.Columns.Add("Total Pays");
                dt.Columns.Add("loans");
                dt.Columns.Add("PayRolls");
                for (int i = 0; i < ids.Count; i++) {

                    dt.Rows.Add(names[i]);
                    dt.Rows[i]["PayRolls"] = payrolls[i].ToString();
                    dt.Rows[i]["loans"] = loans[i].ToString();
                    dt.Rows[i]["Total Pays"] = payments[i].ToString();
                    dt.Rows[i]["IDS"] = ids[i].ToString();
                }
                crusher_emp_view.DataSource = dt;
                crusher_emp_view.Show();
                con.Close();
            }

            SaveFileDialog savepdf = new SaveFileDialog();
            if (savepdf.ShowDialog() == DialogResult.OK) {

                string saveDirectory = savepdf.FileName;
                Document document = new Document();
                Paragraph paragraph = new Paragraph();


                document.Open();
                paragraph.Add(new Chunk("\nDate : " + DateTime.Now.ToShortDateString()));
                paragraph.Add(new Chunk("\n"));
                document.Add(paragraph);

                PdfPTable pdfTable = new PdfPTable(crusher_emp_view.ColumnCount);

                pdfTable.DefaultCell.Padding = 3;

                pdfTable.WidthPercentage = 70;
                pdfTable.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfTable.DefaultCell.BorderWidth = 1;

                foreach (DataGridViewColumn column in crusher_emp_view.Columns)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(column.HeaderText));
                    cell.BackgroundColor = new iTextSharp.text.BaseColor(240, 240, 240);
                    pdfTable.AddCell(cell);
                }

                foreach (DataGridViewRow row in crusher_emp_view.Rows)
                {
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.Value == null)
                        {

                        }
                        else
                        {
                            pdfTable.AddCell(cell.Value.ToString());
                        }
                    }
                }

                using (FileStream stream = new FileStream(saveDirectory + "_Crusher_Employee.pdf", FileMode.Create))
                {
                    Document pdfDoc = new Document(PageSize.A4);

                    PdfWriter.GetInstance(pdfDoc, stream);
                    pdfDoc.Open();
                    pdfDoc.Add(pdfTable);
                    pdfDoc.Close();
                    stream.Close();
                }


                advances = new List<double>();
                payments = new List<double>();
                payrolls = new List<double>();
            }
        }

        private void Payroll_Load(object sender, EventArgs e)
        {
            if (flag_night_mode == true) {
                this.BackColor = Color.DarkGray;
            }

            initilizeView();
        }

        private void CalculateLoaderPayRoll() {
            List<int> ids = new List<int>();
            List<string> names = new List<string>();
            List<double> loans = new List<double>();
            List<double> advances = new List<double>();
            List<double> payments = new List<double>();
            List<double> payrolls = new List<double>();

            if (!(this.ID == -1)) {

            } else {
                String conString = "select e_id, name, loan from db_plastic_management.dbo.Employees";
                SqlCommand cmd = new SqlCommand(conString, con);

                DataTable dt_loans = new DataTable();

                con.Open();
                dt_loans.Load(cmd.ExecuteReader());

                foreach(DataRow row in dt_loans.Rows) {
                    ids.Add(Convert.ToInt32(row[0].ToString()));
                    names.Add(row[1].ToString());
                    loans.Add(Convert.ToDouble(row[2]));
                }

                

                foreach(int id in ids) {
                    String conString1 = "select advance from db_plastic_management.dbo.LoadingWork where e_id = '" + id + "'";
                    SqlCommand cmd1 = new SqlCommand(conString1, con);

                    DataTable dt_advances = new DataTable();

                    dt_advances.Load(cmd1.ExecuteReader());

                    double value = 0D;

                    foreach(DataRow row in dt_advances.Rows) {
                        double tempVal = Convert.ToDouble(row[0]);
                        value += tempVal;
                    }

                    advances.Add(value);
                }
                ids.Sort();
                foreach (int id in ids) {
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
                    double tempAdvance = advances[i];
                    double tempPayment = payments[i];
                    double tempPayRoll = (tempPayment - tempLoan);
                    payrolls.Add(tempPayRoll);
                }

                
                dt.Columns.Add("Names");
                dt.Columns.Add("IDS");
                dt.Columns.Add("Total Pays");
                dt.Columns.Add("loans");
                dt.Columns.Add("PayRolls");
                for(int i = 0; i < ids.Count; i++) {
                    
                    dt.Rows.Add(names[i]);
                    dt.Rows[i]["PayRolls"] = payrolls[i].ToString();
                    dt.Rows[i]["loans"] = loans[i].ToString();
                    dt.Rows[i]["Total Pays"] = payments[i].ToString();
                    dt.Rows[i]["IDS"] = ids[i].ToString();
                }
                loader_emp_view.DataSource = dt;
                loader_emp_view.Show();
                con.Close();

                try {
                    SaveFileDialog savefile = new SaveFileDialog();
                    savefile.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm";

                    if (savefile.ShowDialog() == DialogResult.OK) {
                        DataSet ds = new DataSet("New_DataSet");

                        string saveDirectory = savefile.FileName;
                        ds.Locale = System.Threading.Thread.CurrentThread.CurrentCulture;
                        dt.Locale = System.Threading.Thread.CurrentThread.CurrentCulture;

                        ds.Tables.Add(dt);

                        ExcelLibrary.DataSetHelper.CreateWorkbook(saveDirectory + "Payroll_Loader_Employees_Report.xls", ds);

                        advances = new List<double>();
                        payments = new List<double>();
                        payrolls = new List<double>();
                    }
                } catch (Exception ex) {
                    MessageBox.Show("Unable to save the file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
