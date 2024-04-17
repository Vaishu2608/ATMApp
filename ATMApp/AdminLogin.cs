using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ATMApp
{
    public partial class AdminLogin : Form
    {
        public AdminLogin()
        {
            InitializeComponent();
            textBox2.PasswordChar = '*';
        }
        private readonly string connectionString = File.ReadAllText("conn.txt");
        private void button1_Click(object sender, EventArgs e)
        {
            string Account_No;
            string PIN;

            SqlConnection conn = new SqlConnection(connectionString);

            try
            {
                string query = "SELECT * FROM admin WHERE Admin_Id = " + textBox1.Text + "  AND Password=" + textBox2.Text + "";
                SqlDataAdapter sda = new SqlDataAdapter(query, conn);
                DataTable dtable = new DataTable();
                sda.Fill(dtable);

                if (dtable.Rows.Count > 0)
                {
                    Account_No = textBox1.Text.ToString();
                    PIN = textBox2.Text.ToString();

                    MessageBox.Show("Admin Logged in Successfully");
                    AdminPanel AdminPanel = new AdminPanel();

                         AdminPanel.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Invalid Account Details", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBox1.Clear();
                    textBox2.Clear();
                    textBox1.Focus();
                }

            }
            catch (SqlException ex)
            {
                MessageBox.Show("Database Error:" + ex.ToString());
            }


        }

        private void button2_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(connectionString);

            SqlCommand cmd = new SqlCommand("sp_AdminLogin", conn);

            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter adapter = new SqlDataAdapter();

            try
            {
                cmd.Parameters.AddWithValue("@mode", "Insert");

                cmd.Parameters.AddWithValue("Admin_Id", textBox1.Text);
                cmd.Parameters.AddWithValue("Password", textBox2.Text);


                conn.Open();
                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            finally
            {
                MessageBox.Show("Account Created Successfully");
                conn.Close();
                textBox1.Text = " ";
                textBox2.Text = " ";
            }
        }
    }
}
