using Parking_System.Forms;
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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {        
            string allowedchar = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";         
                if (textBox1.Text == "" || textBox2.Text == "")
                {
                    MessageBox.Show("You must fill in all fields");
                }
                else if (!textBox2.Text.All(allowedchar.Contains))
                {
                    MessageBox.Show("Check the password");
                }
                else
                {
                    korisnik k = new korisnik();
                    k.korisnickoIme = textBox1.Text;
                    k.sifra = textBox2.Text;
                    DataTable dt = Base.Login(k);

                    try
                    {
                        if (k.proveraKorisnika())
                        {
                            if (dt.Rows.Count == 1)
                            {
                                switch (dt.Rows[0][5] as string)
                                {
                                    case "A":
                                        {
                                            k.korisnickoIme = dt.Rows[0][4].ToString();
                                            this.Hide();
                                            Admin a = new Admin();
                                            a.Show();
                                            break;
                                        }
                                    case "R":
                                        {
                                            k.korisnickoIme = dt.Rows[0][4].ToString();
                                            User u = new User();
                                            u.Show();
                                            break;
                                        }
                                    default:
                                        {
                                            MessageBox.Show("You entered your username or password incorrectly.");
                                            break;
                                        }
                                }
                            }
                            else
                            {
                                MessageBox.Show("You entered your username or password incorrectly.");
                            }
                        }
                        else
                        {
                            MessageBox.Show("There is no user with the username entered.");
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
        }


        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            textBox2.PasswordChar = checkBox1.Checked ? '\0' : '*';
        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
        }

        private void textBox2_Click(object sender, EventArgs e)
        {
            textBox2.Clear();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            takeover t = new takeover();
            t.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            ChangePassword c = new ChangePassword();
            c.Show();
        }
    }
}
