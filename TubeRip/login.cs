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
    public partial class login : Form
    {
        string MyConString = "SERVER=djlyriz.myftp.org;" + "DATABASE=" + CryptorEngine.Decrypt("BzEpYdzC9aw=", true) + ";" + "UID=" + CryptorEngine.Decrypt("SkHRZHPLQDk=", true) + ";" + "PASSWORD=" + CryptorEngine.Decrypt("G0M8PQlIBUg=", true) + ";";
        public login()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if ((String.IsNullOrEmpty(textBox1.Text)) || (String.IsNullOrEmpty(textBox2.Text)))
            {
                MessageBox.Show("Error! You have not entered your username/password correctly! Please try again.");
            }
            else
            {
                try
                {
                    MySqlConnection connection = new MySqlConnection(MyConString);
                    MySqlCommand command = connection.CreateCommand();
                    MySqlDataReader Reader;
                    //Change the "username" and "password" to the corresponding names of these columns in your table
                    command.CommandText = "select * from users where username = @username AND password = @password";
                    command.Parameters.AddWithValue("@username", textBox1.Text); //assuming textbox 4 has the password
                    command.Parameters.AddWithValue("@password", CryptorEngine.Encrypt(textBox2.Text, true));
                    try
                    {
                        connection.Open();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        MessageBox.Show("There has been an error connecting to the user database! Please try again later.");
                    }

                    Reader = command.ExecuteReader();
                    if (Reader.HasRows)
                    {
                        MessageBox.Show("Successfully logged in!");
                        while (Reader.Read())
                        {
                            TubeRip.Properties.Settings.Default.username = Reader.GetString(1);
                            TubeRip.Properties.Settings.Default.password = Reader.GetString(2);
                            TubeRip.Properties.Settings.Default.email = Reader.GetString(6);
                            TubeRip.Properties.Settings.Default.age = Reader.GetString(3);
                            TubeRip.Properties.Settings.Default.videoswatched = Reader.GetInt32(7);
                            TubeRip.Properties.Settings.Default.videosdownloaded = Reader.GetInt32(8);
                            if (Reader.GetString(4) == "no")
                            {
                                TubeRip.Properties.Settings.Default.scriptstats = false;
                            }
                            else
                            {
                                TubeRip.Properties.Settings.Default.scriptstats = true;
                            }
                            if (Reader.GetString(5) == "no")
                            {
                                TubeRip.Properties.Settings.Default.isverified = false;
                            }
                            else
                            {
                                TubeRip.Properties.Settings.Default.isverified = true;
                            }
                            TubeRip.Properties.Settings.Default.Save();
                        }
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Wrong Username/Password Combination");
                    }
                    connection.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            register reg = new register();
            reg.Show();
            this.Close();
        }

        private void login_Load(object sender, EventArgs e)
        {

        }
    }
}
