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

namespace ATMApp
{
    public partial class ATM : Form
    {
        private readonly string connectionString = File.ReadAllText("conn.txt");
        public ATM()
        {
            InitializeComponent();
            
        }

        private string accountNo;


        public ATM(string accountNo)
        {
            InitializeComponent();
            this.accountNo = accountNo;
            this.Activated += ATM_Activated;
        }

        private void ATM_Activated(object sender, EventArgs e)
        {
            // Update the balance label whenever the form gets focus
            UpdateBalanceLabel();
        }

        private void UpdateBalanceLabel()
        {
            SqlConnection conn = new SqlConnection(connectionString);

            try
            {
                conn.Open();

                string query = "SELECT Ac_Balance FROM accounts WHERE Ac_no = @account";
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@account", accountNo);

                    object result = command.ExecuteScalar();

                    if (result != null)
                    {
                        decimal balance = Convert.ToDecimal(result);
                        label3.Text = $"Balance: {balance:C2}";
                    }
                    else
                    {
                        MessageBox.Show("Account not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Database Error: " + ex.ToString());
            }
            finally
            {
                conn.Close();
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(connectionString);

            try
            {
                conn.Open();

                string query = "SELECT Ac_Balance FROM accounts WHERE Ac_no = @account";
                using (SqlCommand command = new SqlCommand(query, conn))
                {

                    command.Parameters.AddWithValue("@account", accountNo);

                    object result = command.ExecuteScalar();

                    if (result != null)
                    {
                        decimal balance = Convert.ToDecimal(result);
                        label3.Text = $"Balance: {balance:C2}";
                    }
                    else
                    {
                        MessageBox.Show("Account not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Database Error: " + ex.ToString());
            }

            finally
            {
                conn.Close();
            }

        }


        private void button1_Click(object sender, EventArgs e)
        {
           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(connectionString);

            try
            {

                conn.Open();

                string query = "SELECT Ac_Balance FROM accounts WHERE Ac_no = @account";
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@account", accountNo);

                    object result = command.ExecuteScalar();


                    int withdrawalAmount = Convert.ToInt32(textBox1.Text);


                    if (result != null)
                    {
                        decimal balance = Convert.ToDecimal(result);

                        if (withdrawalAmount >= 100  && balance - withdrawalAmount >= 500)
                        {
                           
                            if (UpdateBalanceInDatabase(withdrawalAmount))
                            {
                                MessageBox.Show($"Withdrawal successful. Remaining balance: {(balance - withdrawalAmount):C2}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else  
                            {
                                MessageBox.Show("Error updating balance.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Invalid withdrawal amount or insufficient funds after withdrawal.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Account not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Database Error: " + ex.ToString());
            }
            finally
            {
                conn.Close();
            }
        }

        private bool UpdateBalanceInDatabase(int withdrawalAmount)
        {
            SqlConnection conn = new SqlConnection(connectionString);

            try
                {
                    conn.Open();

                    string updateQuery = "UPDATE accounts SET Ac_Balance = Ac_Balance - @withdrawalAmount WHERE Ac_no = @account";

                    using (SqlCommand updateCommand = new SqlCommand(updateQuery, conn))
                    {
                      
                        updateCommand.Parameters.AddWithValue("@withdrawalAmount", withdrawalAmount);
                        updateCommand.Parameters.AddWithValue("@account", accountNo);

                        int rowsAffected = updateCommand.ExecuteNonQuery();
                      
                        return rowsAffected > 0;
                    }
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Database Error: " + ex.ToString());
                    return false;
                }
                finally
                {
                    conn.Close();
                }
            }

        private void button3_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(connectionString);

            try
            {

                conn.Open();

                string query = "SELECT Ac_Balance FROM accounts where Ac_no = @account";
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@account", accountNo);

                    object result = command.ExecuteScalar();

                    int depositamount = Convert.ToInt32(textBox2.Text);

                    if (result != null)
                    {
                        decimal balance = Convert.ToDecimal(result);

                        if (depositamount >= 0)
                        {

                            if (Deposit(depositamount))
                            {
                                MessageBox.Show($"Deposit successful. New balance is  : {(balance + depositamount):C2}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                MessageBox.Show("Error updating balance.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Invalid withdrawal amount or insufficient funds after withdrawal.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Account not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Database Error: " + ex.ToString());
            }
            finally
            {
                conn.Close();
            }
        }
        private bool Deposit(int depositamount)
        {
            SqlConnection conn = new SqlConnection(connectionString);

            try
            {
                conn.Open();

                string updateQuery = "UPDATE accounts SET Ac_Balance = Ac_Balance + @depositamount  WHERE Ac_no = @account";

                using (SqlCommand updateCommand = new SqlCommand(updateQuery, conn))
                {

                    updateCommand.Parameters.AddWithValue("@depositamount", depositamount);
                    updateCommand.Parameters.AddWithValue("@account", accountNo);

                    int rowsAffected = updateCommand.ExecuteNonQuery();


                    return rowsAffected > 0;
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Database Error: " + ex.ToString());
                return false;
            }
            finally
            {
                conn.Close();
            }
        }
    }
 }
        


    

       

    

