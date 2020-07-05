using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Parking_System
{
    public partial class ChangePassword : Form
    {
        public ChangePassword()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 f = new Form1();
            f.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {

            try
            {
                if (textBox1.Text == "" || textBox2.Text == "" || textBox5.Text == "" || textBox6.Text == "")
                {
                    MessageBox.Show("You must fill in all fields");
                }
                else
                {
                    korisnik k = new korisnik();
                    k.korisnickoIme = textBox5.Text;
                    k.sifra = textBox6.Text;
                    string allowedchar = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                    if (textBox1.Text.Equals(textBox2.Text))
                    {


                        if (!textBox1.Text.All(allowedchar.Contains))
                        {
                            MessageBox.Show("Check password");
                        }

                        else if (k.promenaLozinke(textBox1.Text))
                        {

                            MessageBox.Show("Password changed successfully");
                        }

                        else MessageBox.Show("Failed password change");


                    }
                    else MessageBox.Show("Incorrectly confirmed password");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void textBox5_Click(object sender, EventArgs e)
        {
            textBox5.Clear();
        }

        private void textBox6_Click(object sender, EventArgs e)
        {
            textBox6.Clear();
        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
        }

        private void textBox2_Click(object sender, EventArgs e)
        {
            textBox2.Clear();
        }
    }
}
