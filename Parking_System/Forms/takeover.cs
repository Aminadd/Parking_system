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

namespace Parking_System.Forms
{
    public partial class takeover : Form
    {
        private List<mesta> listamesta = new List<mesta>();
        public takeover()
        {
            InitializeComponent();
        }

        private void takeover_Load(object sender, EventArgs e)
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

                    if (orijentacija == 0)    //orijentacija 0 - za iscrtavanje horizontalno postavljenih mesta
                    {
                        btn.Location = new Point(m.x, m.y);
                        btn.Name = m.oznaka;
                        btn.Text = m.oznaka;
                        btn.Size = new Size(80, 45);
                        btn.BringToFront();
                        btn.Click += new EventHandler(this.button_click);
                    }
                    else                      //orijentacija 1 - za iscrtavanje vertikalno postavljenih mesta
                    {
                        btn.Location = new Point(m.x, m.y);
                        btn.Name = m.oznaka;
                        btn.Text = m.oznaka;
                        btn.Size = new Size(48, 80);
                        btn.BringToFront();
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

            if (btn.BackColor == Color.Red)
            {
                MessageBox.Show("The seat is taken");
            }
            else
            {
                btn.BackColor = Color.Red;
                SqlConnection abc = new SqlConnection("data source=AMINA;initial catalog=parking_servis;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework&quot;");
                SqlDataAdapter spo = new SqlDataAdapter("Select * From cene ORDER BY cena;", abc);
                DataTable xyz = new DataTable();
                spo.Fill(xyz);

                string cena = xyz.Rows[0][0].ToString();  //cena po satu

                SqlConnection aie = new SqlConnection("data source=AMINA;initial catalog=parking_servis;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework&quot;");
                try
                {
                    DateTime tr = DateTime.Now;
                    string vr = DateTime.Now.ToString(("MM/dd/yyyy  HH:mm:ss"));

                    aie.Open();
                    SqlCommand eia = new SqlCommand("Update parkingmesto set vremeDolaska = '" + vr + "' from parkingmesto where oznaka = '" + btn.Text + "' ", aie);
                    eia.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                SqlConnection con = new SqlConnection("data source=AMINA;initial catalog=parking_servis;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework&quot;");
                try
                {
                    con.Open();
                    DateTime trenutno = DateTime.Now;
                    SqlCommand sda = new SqlCommand("Update parkingmesto set trenutnoStanje = '1' from parkingmesto where oznaka = '" + btn.Text + "' ", con);

                    sda.ExecuteNonQuery();
                    MessageBox.Show("Time of arrival: " + trenutno.ToString("HH:mm:ss,  dd.MM.yyyy") + "\n" + "Price per hour: " + cena + " din" + "\n" + "Parking space: " + btn.Name);

                    DateTime tr = DateTime.Now;
                    string vreme = tr.ToString("HH:mm:ss,  dd.MM.yyyy");
                    string v = "Time of arrival: " + vreme;
                    string c = "Price per hour: " + cena + " RSD";
                    string pm = "Parking space: " + btn.Name;

                    PrintDocument p = new PrintDocument();
                    p.PrintPage += delegate (object sender1, PrintPageEventArgs e1)
                    {
                        e1.Graphics.DrawString(v + "\n" + c + "\n" + pm, new Font("Times New Roman", 12), new SolidBrush(Color.Black), new RectangleF(0, 0, p.DefaultPageSettings.PrintableArea.Width, p.DefaultPageSettings.PrintableArea.Height));
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
            }
        }
    }
}
