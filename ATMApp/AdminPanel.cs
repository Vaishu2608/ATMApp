using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Net.NetworkInformation;
using System.Net;

namespace ATMApp
{
    public partial class AdminPanel : Form
    {
        private readonly string connectionString = File.ReadAllText("conn.txt");
        string s = " SELECT Ac_No,Name,PIN,Ac_balance,Address,Ac_Type,Adhar_No FROM accounts";


        public AdminPanel()
        {
            InitializeComponent();
             LoadUserData();
            GetData();
            dataGridView1.CellContentClick += dataGridView1_CellContentClick;

        }

        private void LoadUserData()

        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlDataAdapter adapter = new SqlDataAdapter("SELECT Ac_No,Name,PIN,Ac_balance,Address,Ac_Type,Adhar_No FROM accounts", conn))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        // dataGridView1.DataSource = dataTable;







                    }


                }
            }
            catch (SqlException ex)
            {
                LogError(ex);
                MessageBox.Show("An error occurred while loading user data. Please contact support for assistance.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void GetData()
        {
            var select = "SELECT Ac_No,Name,PIN,Ac_balance,Address,Ac_Type,Adhar_No,User_image FROM accounts";
            var c = new SqlConnection(connectionString); // Your Connection String here
            var dataAdapter = new SqlDataAdapter(select, c);

            var commandBuilder = new SqlCommandBuilder(dataAdapter);
            var ds = new DataSet();
            dataAdapter.Fill(ds);
            dataGridView2.ReadOnly = true;
            dataGridView2.DataSource = ds.Tables[0];

        }
        private void LogError(SqlException ex)
        {
            Console.WriteLine($"Error: {ex.Message}\nStackTrace: {ex.StackTrace}");
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private string getradiovalue()
        {
            if (radioButton1.Checked)
            { return radioButton1.Text; }
            else { return radioButton2.Text; }
        }














        private void Updatebtn_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(connectionString);

            SqlCommand cmd = new SqlCommand("Sp_accounts", conn);

            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter adapter = new SqlDataAdapter();

            if (string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(textBox2.Text) || string.IsNullOrEmpty(textBox3.Text) || string.IsNullOrEmpty(textBox4.Text)
                || string.IsNullOrEmpty(textBox5.Text) | string.IsNullOrEmpty(textBox6.Text))
            {
                MessageBox.Show("Complete all Required Fields");
                return;

            }

            else
            {

                try
                {
                    cmd.Parameters.AddWithValue("@mode", "Update");
                    cmd.Parameters.AddWithValue("Ac_No", textBox1.Text);
                    cmd.Parameters.AddWithValue("Name", textBox2.Text);
                    cmd.Parameters.AddWithValue("PIN", textBox3.Text);
                    cmd.Parameters.AddWithValue("Ac_Balance", textBox4.Text);
                    cmd.Parameters.AddWithValue("Adhar_No", textBox5.Text);
                    cmd.Parameters.AddWithValue("Address", textBox6.Text);
                    cmd.Parameters.AddWithValue("User_image", imagepath);
                    cmd.Parameters.AddWithValue("Ac_type", "Savings");


                    conn.Open();
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Account Updated Successfully");

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

                finally
                {
                    LoadUserData();
                    conn.Close();
                    textBox1.Text = " ";
                    textBox2.Text = " ";
                    textBox3.Text = " ";
                    textBox4.Text = " ";
                    textBox5.Text = " ";
                    textBox6.Text = " ";
                    pictureBox1.Image = null;
                }
            }

        }

        private void CreateBtn_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("Sp_accounts", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter adapter = new SqlDataAdapter();

                try
                {
                    conn.Open();

                    // Set the parameter values
                    cmd.Parameters.AddWithValue("@mode", "Insert");
                    cmd.Parameters.AddWithValue("@Name", textBox2.Text);
                    cmd.Parameters.AddWithValue("@PIN", textBox3.Text);
                    cmd.Parameters.AddWithValue("@Ac_Balance", textBox4.Text);
                    cmd.Parameters.AddWithValue("@Adhar_No", textBox5.Text);
                    cmd.Parameters.AddWithValue("@Address", textBox6.Text);
                    cmd.Parameters.AddWithValue("@User_image", imagepath); // Make sure imagepath is correctly defined
                    cmd.Parameters.AddWithValue("@Ac_type", getradiovalue());

                    // Execute the stored procedure
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Account Created Successfully");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                finally
                {
                    LoadUserData();
                    textBox2.Text = "";
                    textBox3.Text = "";
                    textBox4.Text = "";
                    textBox5.Text = "";
                    textBox6.Text = "";

                    conn.Close();
                }
            }
        }

    



    private void Deletebtn_Click(object sender, EventArgs e)
        {

                    

                    SqlConnection conn = new SqlConnection(connectionString);

                    SqlCommand cmd = new SqlCommand("Sp_accounts", conn);

                    cmd.CommandType = CommandType.StoredProcedure;

                    try
                    {
                        cmd.Parameters.AddWithValue("@mode", "Delete");
                        cmd.Parameters.AddWithValue("Ac_No", textBox1.Text); 

                        conn.Open();
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Record deleted successfully!!!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                    finally
                    {
                       // LoadUserData(); 
                        conn.Close();
                        textBox1.Text = " ";
                        textBox2.Text = " ";
                        textBox3.Text = " ";
                        textBox4.Text = " ";
                        textBox5.Text = " ";
                        textBox6.Text = " ";
                    }
                }

        private string imagepath;
        private void pictureBox1_Click(object sender, EventArgs e)
        {

            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                openFileDialog.Filter = "Image Files (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png";



                pictureBox1.Image = Image.FromFile(openFileDialog.FileName);
                imagepath = openFileDialog.FileName;
            }

                    
        }

        private void AdminPanel_Load(object sender, EventArgs e)
        {
          
            GetData();
          //  LoadUserData();
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            int index;
            index = e.RowIndex;

            DataGridViewRow selectedRow = dataGridView2.Rows[index];
            textBox1.Text = selectedRow.Cells[0].Value.ToString();
            textBox2.Text = selectedRow.Cells[1].Value.ToString(); // Assuming Name is in the second column (index 1)
            textBox3.Text = selectedRow.Cells[2].Value.ToString(); // Assuming PIN is in the third column (index 2)
            textBox4.Text = selectedRow.Cells[3].Value.ToString(); // Assuming Ac_balance is in the fourth column (index 3)
            textBox5.Text = selectedRow.Cells[4].Value.ToString(); // Assuming Adhar_No is in the fifth column (index 4)
            textBox6.Text = selectedRow.Cells[5].Value.ToString(); // Assuming Address is in the sixth column (index 5)
                                                                   //string sss= selectedRow.Cells[5].Value.ToString();
                                                                   // Assuming the seventh column (index 6) contains the radio button value (Ac_type)
            if (selectedRow.Cells[6].Value.ToString() == "RadioButton1Value")
            {
                radioButton1.Checked = true;
            }
            else if (selectedRow.Cells[6].Value.ToString() == "RadioButton2Value")
            {
                radioButton2.Checked = true;
            }

            // Assuming the eighth column (index 7) contains the image path
            string imagePath = selectedRow.Cells[7].Value.ToString();
            if (!string.IsNullOrEmpty(imagePath) && File.Exists(imagePath))
            {
                pictureBox1.Image = Image.FromFile(imagePath);
            }
            else
            {
                // Handle the case when the image path is not valid or empty
                pictureBox1.Image = null; // Clear the image
            }


        }
    }

       
    }
    



  
    

