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
using System.IO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ATMApp
{
    public partial class New_user : Form
    {
        private readonly string connectionString = File.ReadAllText("conn.txt");
        public New_user()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlConnection conn =new SqlConnection (connectionString);
            
            SqlCommand cmd = new SqlCommand("sp_accounts",conn);
             conn.Open();

            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter adapter = new SqlDataAdapter();
            
            try
            {
                cmd.Parameters.AddWithValue("@mode", "Insert");
               
                cmd.Parameters.AddWithValue("Name", textBox2.Text);
                cmd.Parameters.AddWithValue("PIN", textBox3.Text);
                cmd.Parameters.AddWithValue("Ac_Balance", textBox4.Text);
                cmd.Parameters.AddWithValue("Adhar_No", textBox5.Text);
                cmd.Parameters.AddWithValue("Address", textBox6.Text);
                cmd.Parameters.AddWithValue("User_image", getimage());
                cmd.Parameters.AddWithValue("Ac_type", getradiovalue());

               
                cmd.ExecuteNonQuery();

                MessageBox.Show("Account Created Successfully");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            finally
            {
                MessageBox.Show("Account Created Successfully");
                conn.Close();
                textBox2.Text = " ";
                textBox3.Text = " ";
                textBox4.Text = " ";
                textBox5.Text = " ";
                textBox6.Text = " ";
            }
        }

        private string getradiovalue()
        {
            if(Actype.Checked) { return Actype.Text; } else {return radioButton2.Text;}
        }

        private byte[] getimage()
        {
            MemoryStream stream= new MemoryStream ();
            pictureBox1.Image.Save(stream, pictureBox1.Image.RawFormat);
            return stream.GetBuffer();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog ();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image=new Bitmap(openFileDialog.FileName);
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }
    }
}
