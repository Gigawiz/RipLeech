using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Mail;
using System.Net;

namespace TubeRip
{
    public partial class feedbackandsupport : Form
    {
        public feedbackandsupport()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if ((String.IsNullOrEmpty(textBox1.Text)) || (String.IsNullOrEmpty(textBox2.Text)) | (String.IsNullOrEmpty(textBox3.Text)))
            {
                MessageBox.Show("Please fill in all fields before submitting your request!");
            }
            else
            {
                sendverificationemail();
            }
        }
        private void sendverificationemail()
        {
            string body = "";
            string subject = "";
            var fromAddress = new MailAddress("djlyriz@gmail.com", textBox1.Text);
            var toAddress = new MailAddress("harmfulmonk@gmail.com", "TubeRip Email");
            const string fromPassword = "charmed01";
            if (comboBox1.SelectedItem == "Feedback")
            {
                subject = "Feedback From " + textBox2.Text;
                body = "You have a new message from " + textBox1.Text + ", at email address: " + textBox2.Text + ". Their feedback rating is " + numericUpDown1.Value.ToString() + " out of 10." + Environment.NewLine + textBox3.Text;
            }
            else
            {
                 subject = "Support Request from " + textBox2.Text;
                 body = "You have a new message from " + textBox1.Text + " at email address: " + textBox2.Text + Environment.NewLine + textBox3.Text;

            } var smtp = new SmtpClient
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
            if (comboBox1.SelectedItem == "Feedback")
            {
                MessageBox.Show("Your feedback has been sent successfully!");
            }
            else
            {
                MessageBox.Show("Your support request has been sent successfully!" + Environment.NewLine + "Please look for a response in your email in the next few days.");
            }
            this.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem == "Feedback")
            {
                numericUpDown1.Enabled = true;
            }
            else
            {
                numericUpDown1.Enabled = false;
            }
        }
    }
}
