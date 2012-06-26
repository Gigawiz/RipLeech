using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace TubeRip
{
    public partial class viewdetails : Form
    {
        string services = "";
        string MyConString = "SERVER=djlyriz.myftp.org;" + "DATABASE=" + CryptorEngine.Decrypt("BzEpYdzC9aw=", true) + ";" + "UID=" + CryptorEngine.Decrypt("SkHRZHPLQDk=", true) + ";" + "PASSWORD=" + CryptorEngine.Decrypt("G0M8PQlIBUg=", true) + ";";
        public viewdetails()
        {
            InitializeComponent();
        }

        private void viewdetails_Load(object sender, EventArgs e)
        {
            try
            {
                MySqlConnection connection = new MySqlConnection(MyConString);
                MySqlCommand command = connection.CreateCommand();
                MySqlDataReader Reader;
                //Change the "username" and "password" to the corresponding names of these columns in your table
                command.CommandText = "select * from users where username = @username";
                command.Parameters.AddWithValue("@username", TubeRip.Properties.Settings.Default.username); //assuming textbox 4 has the password
                try
                {
                    connection.Open();
                    Reader = command.ExecuteReader();
                    if (Reader.HasRows)
                    {
                        while (Reader.Read())
                        {
                            services = Reader.GetString(9);
                        }
                    }
                    else
                    {
                        MessageBox.Show("User Does Not Exist!?");
                    }
                    connection.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    MessageBox.Show("There has been an error connecting to the user database! Please try again later.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            if (!String.IsNullOrEmpty(services))
            {
                string[] split = services.Split('_');

                foreach (string item in split)
                {
                    listBox1.Items.Add(item);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != CryptorEngine.Decrypt(TubeRip.Properties.Settings.Default.password, true))
            {
                MessageBox.Show("Invalid Password!");
                this.Close();
            }
            else
            {
                label1.Visible = true;
                label2.Visible = true;
                label3.Visible = true;
                label4.Visible = true;
                label5.Visible = true;
                label6.Visible = true;
                label7.Visible = true;
                label8.Visible = true;
                label9.Visible = false;
                label10.Visible = true;
                label11.Visible = true;
                label12.Visible = true;
                label13.Visible = true;
                label14.Visible = true;
                label15.Visible = true;
                label16.Visible = true;
                label17.Visible = true;
                textBox1.Visible = false;
                label18.Visible = true;
                listBox1.Visible = true;
                button1.Visible = false;
                label2.Text = TubeRip.Properties.Settings.Default.username;
                label4.Text = CryptorEngine.Decrypt(TubeRip.Properties.Settings.Default.password, true);
                label6.Text = TubeRip.Properties.Settings.Default.email;
                label8.Text = TubeRip.Properties.Settings.Default.age;
                label10.Text = TubeRip.Properties.Settings.Default.videoswatched.ToString();
                label12.Text = TubeRip.Properties.Settings.Default.videosdownloaded.ToString();
                if (TubeRip.Properties.Settings.Default.isverified == false)
                {
                    label15.Text = "Not Email Verified";
                }
                else
                {
                    label15.Text = "Successfully Verified Email!";
                }
                if (TubeRip.Properties.Settings.Default.scriptstats == false)
                {
                    label17.Text = "Free";
                }
                else
                {
                    label17.Text = "Subscribed";
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
