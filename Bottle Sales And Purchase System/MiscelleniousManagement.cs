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
    public partial class MiscelleniousManagement : Form
    {

        private int ID = -1;

        public bool flag_night_mode;

        public SqlConnection con = new SqlConnection("Data Source=DESKTOP-CQBGF97\\SQLEXPRESS;Integrated Security=True");

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        public MiscelleniousManagement(ref bool param)
        {
            InitializeComponent();
            this.flag_night_mode = param;
        }

        private void MiscelleniousManagement_Load(object sender, EventArgs e)
        {
            if (flag_night_mode == true) {
                this.BackColor = Color.DarkGray;
            }
            populateIDS();
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void MiscelleniousManagement_MouseDown(object sender, MouseEventArgs e)
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

        private void bunifuShadowPanel3_MouseDown(object sender, MouseEventArgs e)
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

        private void bunifuShadowPanel5_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void populateIDS() {
            try {
                entertainment_id.Items.Clear();
                String conString = "select e_id from db_plastic_management.dbo.Entertainment";
                SqlCommand cmd = new SqlCommand(conString, con);
                DataTable IDS = new DataTable();
                con.Close(); con.Open();
                IDS.Load(cmd.ExecuteReader());

                foreach(DataRow row in IDS.Rows) {
                    entertainment_id.Items.Add(row[0].ToString());
                }
                con.Close();
            } catch (Exception ex) {
                MessageBox.Show("Unable to populate ID sections.", "Updation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try {
                police_challan_id.Items.Clear();
                String conString = "select p_id from db_plastic_management.dbo.PoliceChallan";
                SqlCommand cmd = new SqlCommand(conString, con);
                DataTable IDS = new DataTable();
                con.Close(); con.Open();
                IDS.Load(cmd.ExecuteReader());

                foreach(DataRow row in IDS.Rows) {
                    police_challan_id.Items.Add(row[0].ToString());
                }
                con.Close();
            } catch (Exception ex) {
                MessageBox.Show("Unable to populate ID sections.", "Updation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try {
                toll_plaza_id.Items.Clear();
                String conString = "select t_id from db_plastic_management.dbo.TollPlaza";
                SqlCommand cmd = new SqlCommand(conString, con);
                DataTable IDS = new DataTable();
                con.Close(); con.Open();
                IDS.Load(cmd.ExecuteReader());

                foreach(DataRow row in IDS.Rows) {
                    toll_plaza_id.Items.Add(row[0].ToString());
                }
                con.Close();
            } catch (Exception ex) {
                MessageBox.Show("Unable to populate ID sections.", "Updation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try {
                tirector_id.Items.Clear();
                String conString = "select t_id from db_plastic_management.dbo.Tirector";
                SqlCommand cmd = new SqlCommand(conString, con);
                DataTable IDS = new DataTable();
                con.Close(); con.Open();
                IDS.Load(cmd.ExecuteReader());

                foreach(DataRow row in IDS.Rows) {
                    tirector_id.Items.Add(row[0].ToString());
                }
                con.Close();
            } catch (Exception ex) {
                MessageBox.Show("Unable to populate ID sections.", "Updation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void reset() {
            this.ID = -1;
            txt_car_seller_name.Text = "";
            txt_car_seller_price.Text = "";
            txt_car_seller_address.Text = "";
            entertainment_id.Text = "";
            entertainment_price.Text = "";
            entertainment_discription.Text = "";
            police_challan_id.Text = "";
            police_challan_price.Text = "";
            police_challan_discription.Text = "";
            toll_plaza_id.Text = "";
            toll_plaza_price.Text = "";
            toll_plaza_discription.Text = "";
            tirector_id.Text = "";
            tirector_price.Text = "";
            tirector_discription.Text = "";
        }

        private void save_Click(object sender, EventArgs e)
        {
            double sellerPrice = 0D, entertainmentPrice = 0D, challanPrice = 0D, plazaPrice = 0D, tirectorPrice = 0D;

            if (!(txt_car_seller_name.Text.Equals("")) && (entertainment_price.Text.Equals("")) && (police_challan_price.Text.Equals("")) && (toll_plaza_price.Text.Equals("")) && (tirector_price.Text.Equals(""))) {
                try {
                    sellerPrice = Convert.ToDouble(txt_car_seller_price.Text);
                } catch (Exception ex) {
                    MessageBox.Show("Unable to convert \"" + txt_car_seller_price.Text + "\", into a value.", "Conversion Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                try {
                    String conString = "insert into db_plastic_management.dbo.CarRent(name, price, usr_address) values('" + txt_car_seller_name.Text + "' , '" + txt_car_seller_price.Text + "' , '" + txt_car_seller_address.Text + "')";
                    SqlCommand cmd = new SqlCommand(conString, con);

                    con.Close(); con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close(); populateIDS();
                    MessageBox.Show("Insertion Sucessfull!", "Sucess", MessageBoxButtons.OK, MessageBoxIcon.Information);
                } catch (Exception ex) {
                    MessageBox.Show("Unable to insert a new car seller!", "Insertion Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            } else if (!(entertainment_price.Text.Equals("")) && (txt_car_seller_name.Text.Equals("")) && (police_challan_price.Text.Equals("")) && (toll_plaza_price.Text.Equals("")) && (tirector_price.Text.Equals(""))) {
                try {
                    entertainmentPrice = Convert.ToDouble(entertainment_price.Text);
                } catch (Exception ex) {
                    MessageBox.Show("Unable to convert \"" + entertainment_price.Text + "\", into a value.", "Conversion Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                try {
                    String conString = "insert into db_plastic_management.dbo.Entertainment(price, discription) values('" + entertainment_price.Text + "' , '" + entertainment_discription.Text + "')";
                    SqlCommand cmd = new SqlCommand(conString, con);

                    con.Close(); con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close(); populateIDS();
                    MessageBox.Show("Insertion Sucessfull!", "Sucess", MessageBoxButtons.OK, MessageBoxIcon.Information);
                } catch (Exception ex) {
                    MessageBox.Show("Unable to insert Entertainment!", "Insertion Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            } else if (!(police_challan_price.Text.Equals("")) && (entertainment_price.Text.Equals("")) && (txt_car_seller_name.Text.Equals("")) && (toll_plaza_price.Text.Equals("")) && (tirector_price.Text.Equals(""))) {
                 try {
                    challanPrice = Convert.ToDouble(police_challan_price.Text);
                } catch (Exception ex) {
                    MessageBox.Show("Unable to convert \"" + police_challan_price.Text + "\", into a value.", "Conversion Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                try {
                    String conString = "insert into db_plastic_management.dbo.PoliceChallan(price, discription) values('" + police_challan_price.Text + "' , '" + police_challan_discription.Text + "')";
                    SqlCommand cmd = new SqlCommand(conString, con);

                    con.Close(); con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close(); populateIDS();
                    MessageBox.Show("Insertion Sucessfull!", "Sucess", MessageBoxButtons.OK, MessageBoxIcon.Information);
                } catch (Exception ex) {
                    MessageBox.Show("Unable to insert a new Police Challan!", "Insertion Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            } else if (!(toll_plaza_price.Text.Equals("")) && (entertainment_price.Text.Equals("")) && (police_challan_price.Text.Equals("")) && (txt_car_seller_name.Text.Equals("")) && (tirector_price.Text.Equals(""))) {
                 try {
                    plazaPrice = Convert.ToDouble(toll_plaza_price.Text);
                } catch (Exception ex) {
                    MessageBox.Show("Unable to convert \"" + toll_plaza_price.Text + "\", into a value.", "Conversion Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                try {
                    String conString = "insert into db_plastic_management.dbo.TollPlaza(price, discription) values('" + toll_plaza_price.Text + "' , '" + toll_plaza_discription.Text + "')";
                    SqlCommand cmd = new SqlCommand(conString, con);

                    con.Close(); con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close(); populateIDS();
                    MessageBox.Show("Insertion Sucessfull!", "Sucess", MessageBoxButtons.OK, MessageBoxIcon.Information);
                } catch (Exception ex) {
                    MessageBox.Show("Unable to insert a new Tirector Challan!", "Insertion Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            } else if (!(tirector_price.Text.Equals("")) && (entertainment_price.Text.Equals("")) && (police_challan_price.Text.Equals("")) && (toll_plaza_price.Text.Equals("")) && (txt_car_seller_name.Text.Equals(""))) {
                 try {
                    tirectorPrice = Convert.ToDouble(tirector_price.Text);
                } catch (Exception ex) {
                    MessageBox.Show("Unable to convert \"" + tirector_price.Text + "\", into a value.", "Conversion Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                try {
                    String conString = "insert into db_plastic_management.dbo.Tirector(price, discription) values('" + tirector_price.Text + "' , '" + tirector_discription.Text + "')";
                    SqlCommand cmd = new SqlCommand(conString, con);

                    con.Close(); con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close(); populateIDS();
                    MessageBox.Show("Insertion Sucessfull!", "Sucess", MessageBoxButtons.OK, MessageBoxIcon.Information);
                } catch (Exception ex) {
                    MessageBox.Show("Unable to insert Tirector Overheads!", "Insertion Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            } else {
                MessageBox.Show("Each module will only work alone.\nUnable to insert due to the Invalid User Input.", "Limited MultiTasking", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            reset();
        }

        private void update_Click(object sender, EventArgs e)
        {
            double sellerPrice = 0D, entertainmentPrice = 0D, challanPrice = 0D, plazaPrice = 0D, tirectorPrice = 0D;

            if (!(txt_car_seller_name.Text.Equals("")) && (entertainment_id.Text.Equals("")) && (police_challan_id.Text.Equals("")) && (toll_plaza_id.Text.Equals("")) && (tirector_id.Text.Equals(""))) {
                 try {
                    sellerPrice = Convert.ToDouble(txt_car_seller_price.Text);
                } catch (Exception ex) {
                    MessageBox.Show("Unable to convert \"" + txt_car_seller_price.Text + "\", into a value.", "Conversion Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                try {
                    String conString = "select c_id from db_plastic_management.dbo.CarRent where name = '" + txt_car_seller_name.Text + "'";
                    SqlCommand cmd = new SqlCommand(conString, con);

                    con.Close(); con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Updation Sucessfull!", "Sucess", MessageBoxButtons.OK, MessageBoxIcon.Information);
                } catch (Exception ex) {
                    MessageBox.Show("Unable to update Entertainment!", "Updation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            } else if (!(entertainment_id.Text.Equals("")) && (txt_car_seller_name.Text.Equals("")) && (police_challan_id.Text.Equals("")) && (toll_plaza_id.Text.Equals("")) && (tirector_id.Text.Equals(""))) {
                try {
                    entertainmentPrice = Convert.ToDouble(entertainment_price.Text);
                } catch (Exception ex) {
                    MessageBox.Show("Unable to convert \"" + entertainment_price.Text + "\", into a value.", "Conversion Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                try {
                    String conString = "update db_plastic_management.dbo.Entertainment set price = '" + entertainment_price.Text + "', discription = '" + entertainment_discription.Text + "' where e_id = '" + entertainment_id.Text + "'";
                    SqlCommand cmd = new SqlCommand(conString, con);
                    con.Close();
                    con.Close(); con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Updation Sucessfull!", "Sucess", MessageBoxButtons.OK, MessageBoxIcon.Information);
                } catch (Exception ex) {
                    MessageBox.Show("Unable to update Entertainment!", "Updation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            } else if (!(police_challan_id.Text.Equals("")) && (entertainment_id.Text.Equals("")) && (txt_car_seller_name.Text.Equals("")) && (toll_plaza_id.Text.Equals("")) && (tirector_id.Text.Equals(""))) {
                try {
                    challanPrice = Convert.ToDouble(police_challan_price.Text);
                } catch (Exception ex) {
                    MessageBox.Show("Unable to convert \"" + police_challan_price.Text + "\", into a value.", "Conversion Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                try {
                    String conString = "update db_plastic_management.dbo.PoliceChallan set price = '" + police_challan_price.Text + "', discription = '" + police_challan_discription.Text + "' where p_id = '" + police_challan_id.Text + "'";
                    SqlCommand cmd = new SqlCommand(conString, con);

                    con.Close(); con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Updation Sucessfull!", "Sucess", MessageBoxButtons.OK, MessageBoxIcon.Information);
                } catch (Exception ex) {
                    MessageBox.Show("Unable to update Police Challan!", "Updation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            } else if (!(toll_plaza_id.Text.Equals("")) && (entertainment_id.Text.Equals("")) && (police_challan_id.Text.Equals("")) && (txt_car_seller_name.Text.Equals("")) && (tirector_id.Text.Equals(""))) {
                try {
                    plazaPrice = Convert.ToDouble(toll_plaza_price.Text);
                } catch (Exception ex) {
                    MessageBox.Show("Unable to convert \"" + toll_plaza_price.Text + "\", into a value.", "Conversion Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                try {
                    String conString = "update db_plastic_management.dbo.TollPlaza set price = '" + toll_plaza_price.Text + "', discription = '" + toll_plaza_discription.Text + "' where t_id = '" + toll_plaza_id.Text + "'";
                    SqlCommand cmd = new SqlCommand(conString, con);

                    con.Close(); con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Updation Sucessfull!", "Sucess", MessageBoxButtons.OK, MessageBoxIcon.Information);
                } catch (Exception ex) {
                    MessageBox.Show("Unable to update Toll Plaza expenses!", "Updation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            } else if (!(tirector_id.Text.Equals("")) && (entertainment_id.Text.Equals("")) && (police_challan_id.Text.Equals("")) && (toll_plaza_id.Text.Equals("")) && (txt_car_seller_name.Text.Equals(""))) {
                try {
                    tirectorPrice = Convert.ToDouble(tirector_price.Text);
                } catch (Exception ex) {
                    MessageBox.Show("Unable to convert \"" + tirector_price.Text + "\", into a value.", "Conversion Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                try {
                    String conString = "update db_plastic_management.dbo.Tirector set price = '" + tirector_price.Text + "', discription = '" + tirector_discription.Text + "' where t_id = '" + tirector_id.Text + "'";
                    SqlCommand cmd = new SqlCommand(conString, con);

                    con.Close(); con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Updation Sucessfull!", "Sucess", MessageBoxButtons.OK, MessageBoxIcon.Information);
                } catch (Exception ex) {
                    MessageBox.Show("Unable to update Tirector Overheads!", "Updation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            } else {
                MessageBox.Show("Unable to update due to limited MultiTasking degree!\nOr due to the Invalid User Input.", "Limited MultiTasking", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            reset();
        }

        private void delete_Click(object sender, EventArgs e)
        {
            if (!(txt_car_seller_name.Text.Equals("")) && (entertainment_id.Text.Equals("")) && (police_challan_id.Text.Equals("")) && (toll_plaza_id.Text.Equals("")) && (tirector_id.Text.Equals(""))) {
                try {
                    String conString = "select c_id from db_plastic_management.dbo.CarRent where name = '" + txt_car_seller_name.Text + "'";
                    SqlCommand cmd = new SqlCommand(conString, con);

                    DataTable dt_CID = new DataTable();
                    con.Close(); con.Open();
                    dt_CID.Load(cmd.ExecuteReader());

                    foreach(DataRow row in dt_CID.Rows) {
                        this.ID = Convert.ToInt32(row[0]);
                    }
                    con.Close();
                } catch (Exception ex) {
                    MessageBox.Show("Unbale to fetch CarRental id from database", "Selection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!(this.ID == -1)) {
                    try {
                        String conString = "delete from db_plastic_management.dbo.CarRent where c_id = '" + this.ID + "'";
                        SqlCommand cmd = new SqlCommand(conString, con);

                        con.Close(); con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                        MessageBox.Show("Deletion Sucessfull!", "Sucess", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    } catch (Exception ex) {
                        MessageBox.Show("Unable to delete car rentals!", "deleteion Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                } else {
                    MessageBox.Show("Invalid CarRental ID: \"" + this.ID + "\"", "Selection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            } else if (!(entertainment_id.Text.Equals("")) && (txt_car_seller_name.Text.Equals("")) && (police_challan_id.Text.Equals("")) && (toll_plaza_id.Text.Equals("")) && (tirector_id.Text.Equals(""))) {
                try {
                    String conString = "delete from db_plastic_management.dbo.Entertainment where e_id = '" + entertainment_id.Text + "'";
                    SqlCommand cmd = new SqlCommand(conString, con);

                    con.Close(); con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Deletion Sucessfull!", "Sucess", MessageBoxButtons.OK, MessageBoxIcon.Information);
                } catch (Exception ex) {
                    MessageBox.Show("Unable to delete Entertainment!", "deletion Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            } else if (!(police_challan_id.Text.Equals("")) && (entertainment_id.Text.Equals("")) && (txt_car_seller_name.Text.Equals("")) && (toll_plaza_id.Text.Equals("")) && (tirector_id.Text.Equals(""))) {
                try {
                    String conString = "delete from db_plastic_management.dbo.PoliceChallan where p_id = '" + police_challan_id.Text + "'";
                    SqlCommand cmd = new SqlCommand(conString, con);
                    con.Close();
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Deletion Sucessfull!", "Sucess", MessageBoxButtons.OK, MessageBoxIcon.Information);
                } catch (Exception ex) {
                    MessageBox.Show("Unable to delete Police Challan!", "deletion Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            } else if (!(toll_plaza_id.Text.Equals("")) && (entertainment_id.Text.Equals("")) && (police_challan_id.Text.Equals("")) && (txt_car_seller_name.Text.Equals("")) && (tirector_id.Text.Equals(""))) {
                try {
                    String conString = "delete from db_plastic_management.dbo.TollPlaza where t_id = '" + toll_plaza_id.Text + "'";
                    SqlCommand cmd = new SqlCommand(conString, con);

                    con.Close(); con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Deletion Sucessfull!", "Sucess", MessageBoxButtons.OK, MessageBoxIcon.Information);
                } catch (Exception ex) {
                    MessageBox.Show("Unable to delete Toll Plaza expenses!", "deletion Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            } else if (!(tirector_id.Text.Equals("")) && (entertainment_id.Text.Equals("")) && (police_challan_id.Text.Equals("")) && (toll_plaza_id.Text.Equals("")) && (txt_car_seller_name.Text.Equals(""))) {
                try {
                    String conString = "delete from db_plastic_management.dbo.Tirector where t_id = '" + tirector_id.Text + "'";
                    SqlCommand cmd = new SqlCommand(conString, con);

                    con.Close(); con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Deletion Sucessfull!", "Sucess", MessageBoxButtons.OK, MessageBoxIcon.Information);
                } catch (Exception ex) {
                    MessageBox.Show("Unable to delete Tirector Overheads!", "deletion Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            } else {
                MessageBox.Show("Unable to delete due to limited MultiTasking degree!\nOr due to the Invalid User Input.", "Limited MultiTasking", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            reset();
        }

        private void entertainment_id_SelectedIndexChanged(object sender, EventArgs e)
        {
            try {
                String conString = "select * from db_plastic_management.dbo.Entertainment where e_id = '" + entertainment_id.Text + "'";
                SqlCommand cmd = new SqlCommand(conString, con);

                DataTable entertainment = new DataTable();

                con.Close(); con.Open();
                entertainment.Load(cmd.ExecuteReader());

                foreach(DataRow row in entertainment.Rows) {
                    this.ID = Convert.ToInt32(row[0]);
                    entertainment_price.Text = row[1].ToString();
                    entertainment_discription.Text = row[2].ToString();
                }
            } catch (Exception ex) {
                MessageBox.Show("Unable to AutoFill!", "AutoFill Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void police_challan_id_SelectedIndexChanged(object sender, EventArgs e)
        {
            try {
                String conString = "select * from db_plastic_management.dbo.PoliceChallan where p_id = '" + police_challan_id.Text + "'";
                SqlCommand cmd = new SqlCommand(conString, con);

                DataTable challan = new DataTable();

                con.Close(); con.Open();
                challan.Load(cmd.ExecuteReader());

                foreach(DataRow row in challan.Rows) {
                    this.ID = Convert.ToInt32(row[0]);
                    police_challan_price.Text = row[1].ToString();
                    police_challan_discription.Text = row[2].ToString();
                }
            } catch (Exception ex) {
                MessageBox.Show("Unable to AutoFill!", "AutoFill Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void toll_plaza_id_SelectedIndexChanged(object sender, EventArgs e)
        {
            try {
                String conString = "select * from db_plastic_management.dbo.TollPlaza where t_id = '" + toll_plaza_id.Text + "'";
                SqlCommand cmd = new SqlCommand(conString, con);

                DataTable plaza = new DataTable();

                con.Close(); con.Open();
                plaza.Load(cmd.ExecuteReader());

                foreach(DataRow row in plaza.Rows) {
                    this.ID = Convert.ToInt32(row[0]);
                    toll_plaza_price.Text = row[1].ToString();
                    toll_plaza_discription.Text = row[2].ToString();
                }
            } catch (Exception ex) {
                MessageBox.Show("Unable to AutoFill!", "AutoFill Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tirector_id_SelectedIndexChanged(object sender, EventArgs e)
        {
            try {
                String conString = "select * from db_plastic_management.dbo.Tirector where t_id = '" + tirector_id.Text + "'";
                SqlCommand cmd = new SqlCommand(conString, con);

                DataTable tirector = new DataTable();

                con.Close(); con.Open();
                tirector.Load(cmd.ExecuteReader());

                foreach(DataRow row in tirector.Rows) {
                    this.ID = Convert.ToInt32(row[0]);
                    tirector_price.Text = row[1].ToString();
                    tirector_discription.Text = row[2].ToString();
                }
            } catch (Exception ex) {
                MessageBox.Show("Unable to AutoFill!", "AutoFill Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
