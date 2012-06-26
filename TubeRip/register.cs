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
using System.Net.Mail;

namespace TubeRip
{
    public partial class register : Form
    {
        string MyConString = "SERVER=djlyriz.myftp.org;" + "DATABASE=" + CryptorEngine.Decrypt("BzEpYdzC9aw=", true) + ";" + "UID=" + CryptorEngine.Decrypt("SkHRZHPLQDk=", true) + ";" + "PASSWORD=" + CryptorEngine.Decrypt("G0M8PQlIBUg=", true) + ";";
        public register()
        {
            InitializeComponent();
        }

        private void register_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if ((String.IsNullOrEmpty(textBox1.Text)) || (String.IsNullOrEmpty(textBox2.Text)) || (String.IsNullOrEmpty(textBox3.Text)) || (String.IsNullOrEmpty(textBox4.Text)) || (textBox4.Text.Contains("example")))
            {
                MessageBox.Show("Please fill in all fields before registering");
            }
            else
            {
                dousercheck(); 
            }
        }
        private void dousercheck()
        {
            MySqlConnection connection = new MySqlConnection(MyConString);
            MySqlCommand command = connection.CreateCommand();
            MySqlDataReader Reader;
            //Change the "username" and "password" to the corresponding names of these columns in your table
            command.CommandText = "select * from users where username = @username";
            command.Parameters.AddWithValue("@username", textBox1.Text); //assuming textbox 4 has the password
            try
            {
                connection.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show("There has been an error connecting to the user database! Please try again later.");
            }
            Reader = command.ExecuteReader();
            if (Reader.HasRows)
            {
                MessageBox.Show("User Allready Exists!");
            }
            else
            {
                doreg();
            }
            connection.Close();
        }
        private void doreg()
        {
            try
            {
                MySqlConnection connection = new MySqlConnection(MyConString);
                //An SQL command must then be created:
                MySqlCommand command = connection.CreateCommand();
                string encryptpass = CryptorEngine.Encrypt(textBox2.Text, true);
                //And its command text loaded with a suitable SQL insert statement:
                command.CommandText = "insert into users (username, password, dob, ispaid, isverified, email, videoviews, videosdownloaded, paidaddons)" + " values " + "('" + textBox1.Text + "', '" + encryptpass + "', '" + textBox4.Text + "', 'no', 'no', '" + textBox3.Text + "', '0', '0', '')";
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
                sendverificationemail(textBox3.Text, textBox1.Text);
            }
            catch
            {
                MessageBox.Show("There has been an error with your registration! Please try again later. \r\n If the problem persists, contact the developers.");
            }
        }
        private void sendverificationemail(string email, string username)
        {
            var fromAddress = new MailAddress("djlyriz@gmail.com", "Dj Lyriz");
            var toAddress = new MailAddress(email, username);
            const string fromPassword = "charmed01";
            const string subject = "One Step left to register for TubeRip!";
            string body = "Hello " + username + "!" + Environment.NewLine + "There is one remaining step to register for TubeRip! Please click the link below and your profile will be complete!" + Environment.NewLine + "http://djlyriz.com/tuberipverify.php?username=" + username;
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }
            MessageBox.Show("An email has been sent to: " + email + "." + Environment.NewLine + "Please follow the instructions provided in the email to verify your account.");
            this.Close();
        }
    }
}
