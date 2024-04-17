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
using System.Data.Sql;
using System.Runtime.Remoting.Messaging;
using System.IO;


namespace ATMApp
{
    

    public partial class login_form : Form
    {
        
        public login_form()
        {
            InitializeComponent();
        }

        private readonly string connectionString = File.ReadAllText("conn.txt");
       
        private void button1_Click(object sender, EventArgs e)
        {
            string Account_No;
            string PIN;

            SqlConnection conn = new SqlConnection(connectionString);
           
            try
            {
                string query = "SELECT * FROM accounts WHERE Ac_no = "   + textBox1.Text + "  AND PIN="  + textBox2.Text + "";
                SqlDataAdapter sda = new SqlDataAdapter(query, conn);
                DataTable dtable = new DataTable();
                sda.Fill(dtable);
                 
                if(dtable.Rows.Count > 0) 
                {
                    Account_No= textBox1.Text.ToString();
                    PIN = textBox2.Text.ToString();
                    

                    MessageBox.Show("You Are Logged in Successfully");
                   ATM ATM = new ATM(Account_No);

                    ATM.ShowDialog();
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
            
            finally
            {
                MessageBox.Show("Thank You");

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            New_user form3 = new New_user();

            form3.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AdminLogin form4=new AdminLogin();
            form4.ShowDialog();
        }

        private void login_form_Load(object sender, EventArgs e)
        {

        }
    }
}
