using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Parking_System
{
    public partial class User : Form
    {
        private List<mesta> listamesta = new List<mesta>();

        public User()
        {
            InitializeComponent();
        }

        private void User_Load(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection("data source=AMINA;initial catalog=parking_servis;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework&quot;");
            SqlDataAdapter sda = new SqlDataAdapter("Select * From parkingmesto; ", con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                int i = 0;
                while (i < dt.Rows.Count)
                {
                    mesta m = new mesta();
                    m.oznaka = dt.Rows[i][0].ToString();
                    m.trenutnoStanje = int.Parse(dt.Rows[i][1].ToString());
                    m.id = int.Parse(dt.Rows[i][2].ToString());
                    m.x = int.Parse(dt.Rows[i][3].ToString());
                    m.y = int.Parse(dt.Rows[i][4].ToString());

                    int orijentacija = int.Parse(dt.Rows[i][6].ToString());

                    int x_pos = m.x;
                    int y_pos = m.y;

                    Button btn = new Button();

                    if (orijentacija == 0)
                    {
                        btn.Location = new Point(m.x, m.y);
                        btn.Name = m.oznaka;
                        btn.Text = m.oznaka;
                        btn.Size = new Size(80, 45);
                        btn.BringToFront();
                        btn.Click += new EventHandler(this.button_click);
                    }
                    else
                    {
                        btn.Location = new Point(m.x, m.y);
                        btn.Name = m.oznaka;
                        btn.Text = m.oznaka;
                        btn.Size = new Size(48, 80);
                        btn.BringToFront();
                        btn.BackColor = Color.Transparent;
                        btn.Click += new EventHandler(this.button_click);
                    }
                    if (m.trenutnoStanje == 0)
                    {
                        btn.BackColor = Color.Transparent;
                    }
                    else
                    {
                        btn.BackColor = Color.Red;
                    }

                    this.Controls.Add(btn);
                    listamesta.Add(m);
                    i++;
                }
            }
            else
            {
                MessageBox.Show("Error loading parking space");
            }
        }
        void button_click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn.BackColor == Color.Transparent)
            {
                MessageBox.Show("The place is free");
            }
            else
            {
                btn.BackColor = Color.Transparent;
                SqlConnection abc = new SqlConnection("data source=AMINA;initial catalog=parking_servis;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework&quot;");
                SqlDataAdapter spo = new SqlDataAdapter("Select * From cene ORDER BY cena;", abc);
                DataTable xyz = new DataTable();
                spo.Fill(xyz);

                int cenaPoDanu = int.Parse(xyz.Rows[1][0].ToString());
                int cenaPoMesecu = int.Parse(xyz.Rows[2][0].ToString());
                int cenaPoSatu = int.Parse(xyz.Rows[0][0].ToString());

                SqlConnection con2 = new SqlConnection("data source=AMINA;initial catalog=parking_servis;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework&quot;");
                SqlDataAdapter sdv = new SqlDataAdapter("Select * From parkingmesto where oznaka =  '" + btn.Text + "';", con2);
                DataTable dt = new DataTable();
                sdv.Fill(dt);

                string vremeDolaska = dt.Rows[0][5].ToString();    //stringove koristimo kod printanja

                DateTime vrDolaska = DateTime.Parse(vremeDolaska); //pretvaramo u DateTime format zbog racunanja razlike
                DateTime vrOdlaska = DateTime.Now;

                string vremeOdlaska = vrOdlaska.ToString();        //stringove koristimo kod printanja

                //odvajamo minute, sate, dane, godine
                var d = vrDolaska;
                var hmd = new DateTime(d.Year, d.Month, d.Day, d.Hour, d.Minute, 0);

                var o = vrOdlaska;
                var hmo = new DateTime(o.Year, o.Month, o.Day, o.Hour, o.Minute, 0);

                //postavljamo ih inicijalno na 0 pre racunanja razlike
                int mesec = 0;
                int sat = 0;
                int dan = 0;
                int racun = 0;



                //ako se nije promenio mesec i dan
                //ako dodjemo u 7:25, a odemo u 8:01, nije prosao sat ali ga naplati       
                //ili ako si dosao u 7:25  a otisao u 7:45, sat se nije promenio ali ga naplati                  
                if ((o.Month - d.Month) == 0 && (o.Day - d.Day) == 0 && (((o.Hour - d.Hour) > 0 && (o.Minute - d.Minute) < 0) || (o.Hour - d.Hour) == 0))
                {
                    racun = cenaPoSatu;
                }
                else
                {
                    //ako se promenio mesec
                    if ((o.Month - d.Month) > 0) //da li se promenio mesec ?
                    {
                        //ako se promenio mesec, ne znaci da je proslo 30 dana
                        //mozemo da dodjemo 29.09 a da odemo 1.10. --> nije proslo mesec dana
                        if ((o.Day - d.Day) >= 0)  //da li je proslo 30 dana?
                        {
                            //ako dodjemo 1.10 u 10h, a odemo 2.10 u 5h -> nije prosao dan
                            if ((o.Hour - d.Hour) < 0)
                            {
                                mesec = o.Month - d.Month;
                                racun = mesec * cenaPoMesecu;
                                dan = o.Day - d.Day - 1;
                                racun += dan * cenaPoDanu;
                            }
                            else if ((o.Hour - d.Hour) == 0)
                            {
                                //ako dodjemo 1.10 u 10:35, a odemo 2.10 u 10:05 -> nije prosao dan
                                if ((o.Minute - d.Minute) < 0)
                                {
                                    mesec = o.Month - d.Month;
                                    racun = mesec * cenaPoMesecu;
                                    dan = o.Day - d.Day - 1;
                                    racun += dan * cenaPoDanu;
                                }
                                else
                                {
                                    mesec = o.Month - d.Month;
                                    racun = mesec * cenaPoMesecu;
                                    dan = o.Day - d.Day;
                                    racun += dan * cenaPoDanu;
                                }
                            }
                            else
                            {
                                mesec = o.Month - d.Month;
                                racun = mesec * cenaPoMesecu;
                                dan = o.Day - d.Day;
                                racun += dan * cenaPoDanu;
                            }
                        }
                        else
                        {
                            mesec = o.Month - d.Month - 1;
                            racun = mesec * cenaPoMesecu;
                            //racunaj dane
                            if ((o.Hour - d.Hour) < 0)
                            {
                                dan = o.Day - d.Day + 30 - 1;
                                racun += dan * cenaPoDanu;
                            }
                            else if ((o.Hour - d.Hour) == 0)
                            {
                                if ((o.Minute - d.Minute) >= 0)     //da li je prosao pun sat?
                                {
                                    dan = o.Day - d.Day + 30;      // posto se mesec promenio +30 a dole posle ne
                                    racun += dan * cenaPoDanu;
                                }
                                else
                                {
                                    dan = o.Day - d.Day + 30 - 1;
                                    racun += dan * cenaPoDanu;
                                }
                            }
                            else
                            {
                                dan = o.Day - d.Day + 30;
                                racun += dan * cenaPoDanu;
                            }
                        }
                    }
                    else
                    {
                        if ((o.Day - d.Day) > 0)
                        {
                            //racunaj dane
                            if ((o.Hour - d.Hour) < 0)
                            {
                                dan = o.Day - d.Day - 1;
                                racun += dan * cenaPoDanu;
                            }
                            else if ((o.Hour - d.Hour) == 0)
                            {
                                if ((o.Minute - d.Minute) >= 0)     //da li je prosao pun sat?
                                {
                                    dan = o.Day - d.Day;      // posto se mesec promenio +30 a dole posle ne
                                    racun += dan * cenaPoDanu;
                                }
                                else
                                {
                                    dan = o.Day - d.Day - 1;
                                    racun += dan * cenaPoDanu;
                                }
                            }
                            else
                            {
                                dan = o.Day - d.Day;
                                racun += dan * cenaPoDanu;
                            }
                        }
                    }

                    if ((o.Day - d.Day) >= 0)
                    {
                        if ((o.Month - d.Month) == 0)
                        {
                            if ((o.Minute - d.Minute) >= 0)
                            {
                                sat = o.Hour - d.Hour;  //koliko je proslo sati
                                racun += sat * cenaPoSatu;
                            }
                            else
                            {
                                sat = o.Hour - d.Hour - 1;
                                racun += sat * cenaPoSatu;
                            }
                        }
                        else
                        {
                            if ((o.Hour - d.Hour) < 0)
                            {
                                if ((o.Minute - d.Minute) >= 0)
                                {
                                    sat = o.Hour - d.Hour + 24;  //koliko je proslo sati
                                    racun += sat * cenaPoSatu;
                                }
                                else
                                {
                                    sat = o.Hour - d.Hour - 1 + 24;
                                    racun += sat * cenaPoSatu;
                                }
                            }
                            else
                            {
                                if ((o.Minute - d.Minute) >= 0)
                                {
                                    sat = o.Hour - d.Hour;  //koliko je proslo sati
                                    racun += sat * cenaPoSatu;
                                }
                                else
                                {
                                    sat = o.Hour - d.Hour - 1;
                                    racun += sat * cenaPoSatu;
                                }
                            }
                        }
                    }
                    else
                    {
                        //racunaj sate
                        if ((o.Hour - d.Hour) < 0)
                        {
                            if ((o.Minute - d.Minute) >= 0)
                            {
                                sat = o.Hour - d.Hour + 24;  //koliko je proslo sati
                                racun += sat * cenaPoSatu;
                            }
                            else
                            {
                                sat = o.Hour - d.Hour - 1 + 24;
                                racun += sat * cenaPoSatu;
                            }
                        }
                        else
                        {
                            if ((o.Minute - d.Minute) >= 0)
                            {
                                sat = d.Hour - o.Hour + 24;
                                if (sat == 24)
                                {
                                    racun -= sat * cenaPoSatu;
                                    sat = 0;
                                    dan++;
                                    racun += cenaPoDanu;
                                }
                                else
                                {
                                    racun += sat * cenaPoSatu;
                                }
                            }
                            else
                            {
                                sat = d.Hour - o.Hour - 1 + 24;
                                racun += sat * cenaPoSatu;
                            }
                        }
                    }
                }
                //dodatno za neke slucajeve koji nisu najbolje radili
                //naplati prvi sat
                if ((o.Month - d.Month) == 0 && (o.Day - d.Day) == 0 && (((o.Hour - d.Hour) > 0 && (o.Minute - d.Minute) < 0) || (o.Hour - d.Hour) == 0))
                {
                    racun = cenaPoSatu;
                }
                else
                {
                    //slucaj koji je bio problematican, ispravka
                    if (((o.Month - d.Month) == 0) && ((o.Day - d.Day) > 0) && ((o.Hour - d.Hour) <= 0))
                    {
                        dan = o.Day - d.Day - 1;
                        if ((o.Minute - d.Minute) < 0)
                        {
                            sat = o.Hour - d.Hour + 24 - 1;
                        }
                        else
                        {
                            sat = o.Hour - d.Hour + 24;
                        }

                        racun = 0;                      //resetuj racun jer je prethodno pokupio neke vrednosti
                        racun += sat * cenaPoSatu;
                        racun += dan * cenaPoDanu;
                    }
                    if ((o.Month - d.Month) > 0 && (o.Day - d.Day) == 0 && (o.Hour - d.Hour) < 0)
                    {
                        mesec = o.Month - d.Month - 1;
                        dan = 30;
                        sat = o.Hour - d.Hour + 24;

                        racun = 0;
                        racun += sat * cenaPoSatu;
                        racun += dan * cenaPoDanu;
                        racun = mesec * cenaPoMesecu;
                    }
                    //korekcija
                    if ((o.Month - d.Month) > 0)
                    {
                        if ((o.Hour - d.Hour) > 0)
                        {
                            if ((o.Minute - d.Minute) >= 0)
                            {
                                racun = 0;
                                sat = o.Hour - d.Hour;
                                racun += sat * cenaPoSatu;
                                racun += mesec * cenaPoMesecu;
                                racun += dan * cenaPoDanu;
                            }
                            else
                            {
                                racun = 0;
                                sat = o.Hour - d.Hour - 1;
                                racun += sat * cenaPoSatu;
                                racun += mesec * cenaPoMesecu;
                                racun += dan * cenaPoDanu;
                            }
                        }
                    }
                }
                if ((o.Month - d.Month) == 0 && (o.Day - d.Day) == 0 && (((o.Hour - d.Hour) > 0 && (o.Minute - d.Minute) < 0) || (o.Hour - d.Hour) == 0))
                {
                    racun = cenaPoSatu;
                }
                else
                {
                    racun = 0;
                    racun += sat * cenaPoSatu;
                    racun += mesec * cenaPoMesecu;
                    racun += dan * cenaPoDanu;
                }

                MessageBox.Show("Time of arrival: " + vremeDolaska + "\n" + "Departure time: " + vremeOdlaska + "\n" + "Retention time: " + mesec + "months, " + dan + "days, " + sat + "hours " + "\n" + "Bill: " + racun + " RSD" + "\n" + "Parking space:" + btn.Name);
                SqlConnection con = new SqlConnection("data source=AMINA;initial catalog=parking_servis;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework&quot;");
                try
                {
                    con.Open();
                    DateTime trenutno = DateTime.Now;
                    SqlCommand sda = new SqlCommand("Update parkingmesto set trenutnoStanje = '0' from parkingmesto where oznaka = '" + btn.Text + "' ", con);
                    sda.ExecuteNonQuery();

                    PrintDocument p = new PrintDocument();
                    p.PrintPage += delegate (object sender1, PrintPageEventArgs e1)
                    {
                        e1.Graphics.DrawString("Time of arrival: " + vremeDolaska + "\n" + "Departure time: " + vremeOdlaska + "\n" + "Retention time: " + mesec + " months, " + dan + " days, " + sat + " hours " + "\n" + "Bill: " + racun + " RSD" + "\n" + "Parking space:" + btn.Name, new Font("Times New Roman", 12), new SolidBrush(Color.Black), new RectangleF(0, 0, p.DefaultPageSettings.PrintableArea.Width, p.DefaultPageSettings.PrintableArea.Height));

                    };
                    try
                    {
                        p.Print();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Exception Occured While Printing", ex);
                    }
                    con.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                SqlConnection con1 = new SqlConnection("data source=AMINA;initial catalog=parking_servis;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework&quot;");
                con1.Open();
                SqlCommand sda1 = new SqlCommand("Update parkingmesto set vremeDolaska = '0' from parkingmesto where oznaka = '" + btn.Text + "' ", con1);
                sda1.ExecuteNonQuery();
                con1.Close();
            }
        }

    }
}
