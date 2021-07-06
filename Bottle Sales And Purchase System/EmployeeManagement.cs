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
    public partial class EmployeeManagement : Form
    {

        private int ID = -1;
        public bool flag_night_mode;
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        public SqlConnection con = new SqlConnection("Data Source=DESKTOP-CQBGF97\\SQLEXPRESS;Integrated Security=True");

        public EmployeeManagement(ref bool param)
        {
            InitializeComponent();
            this.flag_night_mode = param;
        }

        private void EmployeeManagement_Load(object sender, EventArgs e)
        {
            if (flag_night_mode == true) {
                this.BackColor = Color.DarkGray;
            }
            AutoFill();
            initilizeView();
            timer1.Start();
        }

        private void initilizeView()
        {
            try {
                String conString = "select * from db_plastic_management.dbo.LoadingWork";
                SqlCommand cmd = new SqlCommand(conString, con);

                con.Open();

                DataTable dt_job_view = new DataTable();
                dt_job_view.Load(cmd.ExecuteReader());

                jobs_view.DataSource = dt_job_view;
                con.Close();
            } catch (Exception ex) {
                MessageBox.Show("Failed to load data from database server!", "Parsing Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try {
                String conString = "select * from db_plastic_management.dbo.Employees";
                SqlCommand cmd = new SqlCommand(conString, con);

                con.Open();
                DataTable dt_emp_view = new DataTable();
                dt_emp_view.Load(cmd.ExecuteReader());
                employee_view.DataSource = dt_emp_view;
                con.Close();
            } catch (Exception ex) {
                MessageBox.Show("Failed to load data from database server!", "Parsing Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try {
                String conString = "select * from db_plastic_management.dbo.KabariaSeller";
                SqlCommand cmd = new SqlCommand(conString, con);

                con.Open();

                DataTable dt_seller_view = new DataTable();
                dt_seller_view.Load(cmd.ExecuteReader());

                seller_view.DataSource = dt_seller_view;
                con.Close();
            } catch (Exception ex) {
                MessageBox.Show("Failed to load data from database server!", "Parsing Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void AutoFill() {
            try {
                this.job_assignment_employee_id.Items.Clear();
                String conString = "select e_id from db_plastic_management.dbo.Employees";
                SqlCommand cmd = new SqlCommand(conString, con);

                DataTable data_employee_id = new DataTable();
                con.Open();
                data_employee_id.Load(cmd.ExecuteReader());

                foreach (DataRow row in data_employee_id.Rows) {
                    this.job_assignment_employee_id.Items.Add(row[0].ToString());
                }
                con.Close();
            } catch(Exception ex) {
                MessageBox.Show("Unable to autofill employee id, check database connectivity!", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            try {
                String conString = "select ks_id from db_plastic_management.dbo.KabariaSeller";
                SqlCommand cmd = new SqlCommand(conString, con);

                DataTable data_seller_id = new DataTable();
                con.Open();
                data_seller_id.Load(cmd.ExecuteReader());

                foreach (DataRow row in data_seller_id.Rows) {
                    this.job_assignment_seller_id.Items.Add(row[0].ToString());
                }
                con.Close();
            } catch (Exception ex) {
                MessageBox.Show("Unable to autofill employee id, check database connectivity!", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void employee_save_job_Click(object sender, EventArgs e)
        {
            if (!(job_assignment_employee_id.Text.Equals("")) && !(job_assignment_seller_id.Text.Equals("")) && !(txt_loaded_quantity.Text.Equals("") && !(txt_advance_money.Text.Equals("")))) {

                double actualPrice = -1D, paymentPrice = 0D, loan = 0D, actualQuantity = -1D, loadedQuantity = 0D;

                try {
                    String conString1 = "select price, quantityBought from db_plastic_management.dbo.KabariaSeller where ks_id = '" + job_assignment_seller_id.Text + "'";
                    SqlCommand cmd1 = new SqlCommand(conString1, con);

                    DataTable dt_priceVal = new DataTable();

                    con.Open();
                    dt_priceVal.Load(cmd1.ExecuteReader());

                    foreach (DataRow row in dt_priceVal.Rows) {
                        actualPrice = Convert.ToDouble(row[0]);
                        actualQuantity = Convert.ToDouble(row[1]);
                    }
                    con.Close();
                } catch (Exception ex) {
                    MessageBox.Show("Connectivity Error!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                try {
                    String conString = "select pay, loadedQuantity from db_plastic_management.dbo.LoadingWork where ks_id = '" + job_assignment_seller_id.Text + "'";
                    SqlCommand cmd = new SqlCommand(conString, con);

                    DataTable dt_payVal = new DataTable();

                    con.Open();
                    dt_payVal.Load(cmd.ExecuteReader());

                    foreach (DataRow row in dt_payVal.Rows) {
                        double tempVal = Convert.ToDouble(row[0]);
                        double tempVal1 = Convert.ToDouble(row[1]);
                        paymentPrice += tempVal;
                        loadedQuantity += tempVal1;
                    }
                    paymentPrice += Convert.ToDouble(txt_calculated_pay.Text);
                    con.Close();
                } catch (Exception ex) {
                    MessageBox.Show("Connectivity Error!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if ((paymentPrice <= actualPrice) && (loadedQuantity <= actualQuantity)) {
                    try {
                        String conString = "insert into db_plastic_management.dbo.LoadingWork(e_id, ks_id, loadedQuantity, pay, advance) values('" + job_assignment_employee_id.Text + "' , '" + job_assignment_seller_id.Text + "' , '" + txt_loaded_quantity.Text + "' , '" + txt_calculated_pay.Text + "' , '" + txt_advance_money.Text + "')";
                        SqlCommand cmd = new SqlCommand(conString, con);

                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    } catch (Exception ex) {
                        MessageBox.Show("Unable to insert job into the database server!", "Insertion Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    try {
                        String conString = "select loan from db_plastic_management.dbo.Employees where e_id = '" + job_assignment_employee_id.Text + "'";
                        SqlCommand cmd = new SqlCommand(conString, con);

                        con.Open();

                        DataTable dt_loanVal = new DataTable();
                        dt_loanVal.Load(cmd.ExecuteReader());

                        foreach(DataRow row in dt_loanVal.Rows) {
                            double tempVal = Convert.ToDouble(row[0]);
                            loan += tempVal;
                        }

                        loan += Convert.ToDouble(txt_advance_money.Text);
                        con.Close();
                    } catch (Exception ex) {
                        MessageBox.Show("Unable to fetch previous loan!", "Connectivity Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    try {
                        String conString = "update db_plastic_management.dbo.Employees set loan = '" + loan + "' where e_id = '" + job_assignment_employee_id.Text + "'";
                        SqlCommand cmd = new SqlCommand(conString, con);

                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                        MessageBox.Show("Insertion of a new job was sucessful!", "Insertion Sucess", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        refresh();
                    } catch (Exception ex) {
                        MessageBox.Show("Unable to update Employee Loan", "Updation Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                } else {
                    if(loadedQuantity < actualQuantity) {
                        MessageBox.Show("Unable to assign job as the Employee Loaded Quantity: \"" + loadedQuantity + "\" has exceeded the amount of the Seller Total Quantity: \"" + actualQuantity + "\".\nTherefore you can not assign further jobs on this seller.", "Amount Limit Reached", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        job_assignment_employee_id.Text = "";
                        job_assignment_seller_id.Text = "";
                        return;
                    }else {
                        MessageBox.Show("Unable to assign job as the Employee Payment Price: \"" + paymentPrice + "\" has exceeded the amount of the Seller Buying Price: \"" + actualPrice + "\".\nTherefore you can not assign further jobs on this seller.", "Amount Limit Reached", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        job_assignment_employee_id.Text = "";
                        job_assignment_seller_id.Text = "";
                        return;
                    }
                }
            } else {
                if (job_assignment_employee_id.Equals("")) {
                    MessageBox.Show("You must select an employee id to assign the job!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                } else if (job_assignment_seller_id.Equals("")) {
                    MessageBox.Show("You must select a seller id to assign the job to an employee!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }else if (txt_loaded_quantity.Text.Equals("")) {
                    MessageBox.Show("You must enter the loaded quantity by an employee to finish the job assignment!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!(txt_loaded_quantity.Text.Equals("")) && !(job_assignment_employee_id.Text.Equals("")) && !(job_assignment_seller_id.Text.Equals(""))) {
                try {
                    double loadedQuantity = -1D, wholeQuantity = -1D, actualPrice = -1D;
                    try {
                        loadedQuantity = Convert.ToDouble(txt_loaded_quantity.Text);
                    } catch (Exception ex) {
                        timer1.Stop();
                        MessageBox.Show("Couldn't convert the entered LoadedQuantity as it contains some other charater than floating point numbers...", "Conversion Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txt_loaded_quantity.Text = "";
                        txt_calculated_pay.Text = "";
                        return;
                    }

                    String conString = "select quantityBought, price from db_plastic_management.dbo.KabariaSeller where ks_id = '" + job_assignment_seller_id.Text + "'";
                    SqlCommand cmd = new SqlCommand(conString, con);

                    DataTable dt_wholeQuantity = new DataTable();

                    con.Open();
                    dt_wholeQuantity.Load(cmd.ExecuteReader());

                    foreach(DataRow row in dt_wholeQuantity.Rows) {
                        wholeQuantity = Convert.ToDouble(row[0]);
                        actualPrice = Convert.ToDouble(row[1]);
                    }
                    
                    if (loadedQuantity > wholeQuantity) {
                        MessageBox.Show("LoadedQuantity can't be greater than \'" + wholeQuantity + "\' for this seller.", "Quantity Overloaded", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txt_loaded_quantity.Text = "";
                        txt_calculated_pay.Text = "";
                        timer1.Stop();
                    } else {
                        double pricePerKG = (double)(actualPrice / wholeQuantity);
                        txt_calculated_pay.Text = (loadedQuantity * pricePerKG).ToString();
                    }
                    con.Close();
                    timer1.Stop();
                } catch (Exception ex) {
                    timer1.Stop();
                    MessageBox.Show("Conversion Failed!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void txt_loaded_quantity_OnValueChanged(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void save_employee_Click(object sender, EventArgs e)
        {
            if (!(txt_employee_name.Text.Equals(""))) {
                double loan = -1D;
                try {
                    try {
                        loan = Convert.ToDouble(txt_employee_loan.Text);
                    } catch (Exception ex) {
                        MessageBox.Show("Unbale to convert Loan: \"" + txt_employee_loan.Text + "\", as tc contains some other character than floating point value.", "Conversion Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txt_employee_loan.Text = "";
                        return;
                    }

                    String conString = "insert into db_plastic_management.dbo.Employees(name, cnic, loan, usr_address) values('" + txt_employee_name.Text + "' , '" + txt_employee_cnic.Text + "' , '" + loan + "' , '" + txt_employee_address.Text + "')";
                    SqlCommand cmd = new SqlCommand(conString, con);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Insertion of a new employee was sucessful!", "Insertion Sucess", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    refresh();
                    AutoFill();
                } catch (Exception ex) {
                    MessageBox.Show("Unable to insert the employee.", "Insertion Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            } else {
                MessageBox.Show("You must enter a name for the employee in order to save it!", "Insertion Aborted", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void update_employee_Click(object sender, EventArgs e)
        {
            if (!(txt_employee_name.Text.Equals("")) && !(this.ID == -1)) {
                try {
                    String conString = "update db_plastic_management.dbo.Employees set name = '" + txt_employee_name.Text + "', cnic = '" + txt_employee_cnic.Text + "', loan = '" + txt_employee_loan.Text + "', usr_address = '" + txt_employee_address.Text + "' where e_id = '" + this.ID + "'";
                    SqlCommand cmd = new SqlCommand(conString, con);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Updation of the selected employee was sucessful!", "Updation Sucess", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    refresh();
                    AutoFill();
                } catch (Exception ex) {
                    MessageBox.Show("Unable to Update the employee.", "Updation Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            } else {
                MessageBox.Show("You must enter a Employee Name in order to update it!", "Updation Aborted", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void delete_employee_Click(object sender, EventArgs e)
        {
            if (!(txt_employee_name.Text.Equals("")) && !(this.ID == -1)) {
                try {
                    String conString = "delete from db_plastic_management.dbo.Employees where e_id = '" + this.ID + "'";
                    SqlCommand cmd = new SqlCommand(conString, con);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Deletion of the selected employee was sucessful!", "Deletion Sucess", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    refresh();
                } catch (Exception ex) {
                    MessageBox.Show("Unable to Update the employee.", "Updation Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            } else {
                MessageBox.Show("You must enter a Employee Name in order to update it!", "Updation Aborted", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void job_view_refresh_Click(object sender, EventArgs e)
        {
            try {
                String conString = "select * from db_plastic_management.dbo.LoadingWork";
                SqlCommand cmd = new SqlCommand(conString, con);

                con.Open();

                DataTable dt_job_view = new DataTable();
                dt_job_view.Load(cmd.ExecuteReader());

                jobs_view.DataSource = dt_job_view;
                con.Close();
            } catch (Exception ex) {
                MessageBox.Show("Failed to load data from database server!", "Parsing Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void employee_view_refresh_Click(object sender, EventArgs e)
        {
            try {
                String conString = "select * from db_plastic_management.dbo.Employees";
                SqlCommand cmd = new SqlCommand(conString, con);

                con.Open();
                DataTable dt_emp_view = new DataTable();
                dt_emp_view.Load(cmd.ExecuteReader());
                employee_view.DataSource = dt_emp_view;
                con.Close();
            } catch (Exception ex) {
                MessageBox.Show("Failed to load data from database server!", "Parsing Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void seller_view_refresh_Click(object sender, EventArgs e)
        {
            try {
                String conString = "select * from db_plastic_management.dbo.KabariaSeller";
                SqlCommand cmd = new SqlCommand(conString, con);

                con.Open();
                DataTable dt_seller_view = new DataTable();
                dt_seller_view.Load(cmd.ExecuteReader());
                seller_view.DataSource = dt_seller_view;
                con.Close();
            } catch (Exception ex) {
                MessageBox.Show("Failed to load data from database server!", "Parsing Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void employee_update_job_Click(object sender, EventArgs e)
        {
            if(!(this.ID == -1)) {
                try {
                    String conString = "update db_plastic_management.dbo.LoadingWork set e_id = '" + job_assignment_employee_id.Text + "', ks_id = '" + job_assignment_seller_id.Text + "', loadedQuantity = '" + txt_loaded_quantity.Text + "', pay = '" + txt_calculated_pay.Text + "', advance = '" + txt_advance_money.Text + "' where j_id = '" + this.ID + "'";
                    SqlCommand cmd = new SqlCommand(conString, con);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Updation of the selected job was sucessful", "Updation Sucess", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    refresh();
                } catch (Exception ex) {
                    MessageBox.Show("Failed to update the job in the database server!", "Updation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            } else {
                MessageBox.Show("You must select a job from below in order to update it!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void employee_delete_job_Click(object sender, EventArgs e)
        {
            if (!(this.ID == -1)) {
                try {
                    String conString = "delete from db_plastic_management.dbo.LoadingWork where j_id = '" + this.ID + "'";
                    SqlCommand cmd = new SqlCommand(conString, con);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    refresh();
                    MessageBox.Show("Deletion of the selected job was sucessful", "Deletion Sucess", MessageBoxButtons.OK, MessageBoxIcon.Information);
                } catch (Exception ex) {
                    MessageBox.Show("Unable to delete the selected job, Check database server connectivity!", "Deletion Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            } else {
                MessageBox.Show("You must select a job from below in order to delte it!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void refresh()
        {
            job_assignment_employee_id.Text = "";
            job_assignment_seller_id.Text = "";
            txt_loaded_quantity.Text = "";
            txt_calculated_pay.Text = "";
            txt_advance_money.Text = "";
            txt_employee_name.Text = "";
            txt_employee_cnic.Text = "";
            txt_employee_loan.Text = "";
            txt_employee_address.Text = "";

            try {
                String conString = "select * from db_plastic_management.dbo.LoadingWork";
                SqlCommand cmd = new SqlCommand(conString, con);

                con.Open();

                DataTable dt_job_view = new DataTable();
                dt_job_view.Load(cmd.ExecuteReader());

                jobs_view.DataSource = dt_job_view;
                con.Close();
            } catch (Exception ex) {
                MessageBox.Show("Failed to load data from database server!", "Parsing Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            try {
                String conString = "select * from db_plastic_management.dbo.Employees";
                SqlCommand cmd = new SqlCommand(conString, con);

                con.Open();
                DataTable dt_emp_view = new DataTable();
                dt_emp_view.Load(cmd.ExecuteReader());
                employee_view.DataSource = dt_emp_view;
                con.Close();
            } catch (Exception ex) {
                MessageBox.Show("Failed to load data from database server!", "Parsing Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            try {
                String conString = "select * from db_plastic_management.dbo.KabariaSeller";
                SqlCommand cmd = new SqlCommand(conString, con);

                con.Open();
                DataTable dt_seller_view = new DataTable();
                dt_seller_view.Load(cmd.ExecuteReader());
                seller_view.DataSource = dt_seller_view;
                con.Close();
            } catch (Exception ex) {
                MessageBox.Show("Failed to load data from database server!", "Parsing Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void jobs_view_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try {
                this.ID = Convert.ToInt32(jobs_view.CurrentRow.Cells[0].Value);
                this.job_assignment_employee_id.Text = (jobs_view.CurrentRow.Cells[1].Value).ToString();
                this.job_assignment_seller_id.Text = (jobs_view.CurrentRow.Cells[2].Value).ToString();
                this.txt_loaded_quantity.Text = (jobs_view.CurrentRow.Cells[3].Value).ToString();
                this.txt_calculated_pay.Text = (jobs_view.CurrentRow.Cells[4].Value).ToString();

                String conString = "select advance from db_plastic_management.dbo.LoadingWork";
                SqlCommand cmd = new SqlCommand(conString, con);

                con.Open();
                DataTable advance = new DataTable();
                advance.Load(cmd.ExecuteReader());

                foreach (DataRow row in advance.Rows) {
                    txt_advance_money.Text = (row[0]).ToString();
                }
                con.Close();
            }
            catch (Exception ex) {
                MessageBox.Show("Unable to forcast values!", "Conversion Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void EmployeeManagement_MouseDown(object sender, MouseEventArgs e)
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

        private void bunifuImageButton4_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void bunifuImageButton3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void employee_view_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try {
                this.ID = Convert.ToInt32(employee_view.CurrentRow.Cells[0].Value.ToString());
                this.txt_employee_name.Text = employee_view.CurrentRow.Cells[1].Value.ToString();
                this.txt_employee_cnic.Text = employee_view.CurrentRow.Cells[2].Value.ToString();
                this.txt_employee_loan.Text = employee_view.CurrentRow.Cells[3].Value.ToString();
                this.txt_employee_address.Text = employee_view.CurrentRow.Cells[4].Value.ToString();
            } catch (Exception ex) {
                MessageBox.Show("Unable to convert values.", "Forcasting Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
