using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Net;
using System.IO;

namespace TubeRip
{
    public partial class addfeats : Form
    {
        string dlurl = "";
        string addon = "";
        string services = "test_";
        string name = "";
        string addtodb = "";
        string saveloc = Directory.GetCurrentDirectory() + @"\Data\Addons\";
        string MyConString = "SERVER=djlyriz.myftp.org;" + "DATABASE=" + CryptorEngine.Decrypt("BzEpYdzC9aw=", true) + ";" + "UID=" + CryptorEngine.Decrypt("SkHRZHPLQDk=", true) + ";" + "PASSWORD=" + CryptorEngine.Decrypt("G0M8PQlIBUg=", true) + ";";
        public addfeats()
        {
            InitializeComponent();
        }

        private void addfeats_Load(object sender, EventArgs e)
        {
            #region loadbgsettings
            if (!String.IsNullOrEmpty(TubeRip.Properties.Settings.Default.backgroundloc))
            {
                this.BackgroundImage = Image.FromFile(TubeRip.Properties.Settings.Default.backgroundloc);
                if (TubeRip.Properties.Settings.Default.bgstyle == "Tile")
                {
                    this.BackgroundImageLayout = ImageLayout.Tile;
                }
                else if (TubeRip.Properties.Settings.Default.bgstyle == "Stretch")
                {
                    this.BackgroundImageLayout = ImageLayout.Stretch;
                }
                else if (TubeRip.Properties.Settings.Default.bgstyle == "Center")
                {
                    this.BackgroundImageLayout = ImageLayout.Center;
                }
                else
                {
                    this.BackgroundImageLayout = ImageLayout.Tile;
                }
            }
            #endregion
            #region load plugins paid for
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
                            services = Reader.GetString(9) + "";
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
            #endregion
            loadavailplugins();
        }
        private void loadavailplugins()
        {
            try
            {
                MySqlConnection connection = new MySqlConnection(MyConString);
                MySqlCommand command = connection.CreateCommand();
                MySqlDataReader Reader;
                command.CommandText = "select * from plugins";
                connection.Open();
                Reader = command.ExecuteReader();
                while (Reader.Read())
                {
                    ListViewItem lvi = new ListViewItem(Reader.GetString(1));
                    lvi.SubItems.Add(Reader.GetString(2));
                    lvi.SubItems.Add(Reader.GetString(3));
                    lvi.SubItems.Add(Reader.GetString(4));
                    lvi.SubItems.Add(Reader.GetString(5));
                    lvi.SubItems.Add(Reader.GetString(6));

                    // Add the list items to the ListView
                    listView1.Items.Add(lvi);
                }
                connection.Close();
            }
            catch (Exception ex)
            {
              MessageBox.Show("As the system has just been added, there are many bugs still being worked out. If the problem persists, please use the feedback/support form and let the developers know.");
              this.Close();
            }
        }

        private void listView1_ItemActivate(object sender, EventArgs e)
        {
            string hyperurl = listView1.FocusedItem.SubItems[4].Text;
            dlurl = hyperurl;
            name = listView1.FocusedItem.SubItems[0].Text;
            addon = listView1.FocusedItem.SubItems[5].Text;
            //MessageBox.Show(dlurl);
            //MessageBox.Show(addon);
            MessageBox.Show(saveloc);
            textBox1.Text = listView1.FocusedItem.SubItems[3].Text;
            button1.Text = "Download Addon: " + listView1.FocusedItem.SubItems[0].Text;
            button1.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            addtodb = services + "_" + name;
            services = addtodb;
            progressBar1.Visible = true;
            WebClient client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;
            Uri update = new Uri(dlurl);
            client.DownloadFileAsync(update, saveloc + addon);
            client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client_DownloadProgressChanged);
            client.DownloadFileCompleted += new AsyncCompletedEventHandler(client_DownloadFileCompleted);
        }
        void client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            MessageBox.Show("Download Complete!" + Environment.NewLine + "Please Restart TubeRip to see your new addon in the Plugins menu!");
            progressBar1.Value = 0;
            button1.Enabled = false;
            progressBar1.Visible = false;
        }
        void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            button1.Enabled = false;
            progressBar1.Value = e.ProgressPercentage;
        }

        private void saveaddedfeatures()
        {
            try
            {
                MySqlConnection connection = new MySqlConnection(MyConString);
                //An SQL command must then be created:
                MySqlCommand command = connection.CreateCommand();
                //And its command text loaded with a suitable SQL insert statement:
                command.CommandText = "UPDATE `users` SET `paidaddons` = '" + addtodb + "' WHERE username = '" + TubeRip.Properties.Settings.Default.username +"'";
                try
                {
                    connection.Open();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("There has been an error connecting to the user database! Please try again later.");
                }

                //At this point (especially during testing) it may be worthwhile printing the SQL statement to the screen:
                //MessageBox.Show(command.CommandText);

                ////The application can then execute the command on the database:
                MySqlDataReader result = command.ExecuteReader();

                //And with that a new record will have been inserted into the database.
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                MessageBox.Show("There has been an error verifying your addon! Please try again later. \r\n If the problem persists, contact the developers.");
            }
        }

        private void addfeats_FormClosing(object sender, FormClosingEventArgs e)
        {
            saveaddedfeatures();
        }
    }
}
