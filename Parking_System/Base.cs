using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Parking_System
{
    class Base
    {
        private static string connectionString = @"data source =AMINA; initial catalog = parking_servis; integrated security = True; MultipleActiveResultSets = True; App = EntityFramework & quot;";
        private static SqlConnection connect = new SqlConnection(connectionString);


        //private static string hashLozinka(string lozinka)
        //{
        //    using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
        //    {
        //        UTF8Encoding utf8 = new UTF8Encoding();
        //        byte[] data = md5.ComputeHash(utf8.GetBytes(lozinka));
        //        return Convert.ToBase64String(data);
        //    }
        //}
        public static bool ProveraKorisnika(korisnik k)
        {
            string sql = "SELECT korisnickoIme FROM korisnik";
            SqlCommand cmd = new SqlCommand(sql, connect);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            connect.Open();
            DataTable dt = new DataTable();
            adapter.Fill(dt);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i][0].ToString() == k.korisnickoIme)
                {
                    connect.Close();
                    return true;
                }
            }
            connect.Close();
            return false;
        }

        public static DataTable Login(korisnik k)
        {
            DataTable dt = new DataTable();
            string sql = "SELECT * FROM korisnik WHERE korisnickoIme = @korisnickoIme AND sifra = @sifra ";
            SqlCommand cmd = new SqlCommand(sql, connect);
            cmd.Parameters.AddWithValue("@korisnickoIme", k.korisnickoIme);
            cmd.Parameters.AddWithValue("@sifra", k.sifra);

            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            connect.Open();
            adapter.Fill(dt);
            connect.Close();

            return dt;
        }

        public static void Promenicene(int cena1, int cena2, int cena3)
        {

            connect.Open();
            SqlCommand sda = new SqlCommand("Update cene set cena =" + cena1 + " where tip = 'S';Update cene set cena =" + cena2 + " where tip= 'D';Update cene set cena =" + cena3 + " where tip= 'M'; ", connect);

            sda.ExecuteNonQuery();
            connect.Close();


        }
        public static DataTable selectCene()
        {
            connect.Open();
            SqlDataAdapter sda = new SqlDataAdapter("Select * From cene ORDER BY cena;", connect);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            connect.Close();
            return dt;
        }

        public static void dodavanjeRadnika(string tbIme, string tbPrezime, string tbbrTel, string tbSifra, string tbKorisnickoIme)
        {
            connect.Open();
            SqlCommand sda = new SqlCommand("Insert into korisnik (ime, prezime, brTelefona, sifra, korisnickoIme, tip) values('" + tbIme + "','" + tbPrezime + "','" + tbbrTel + "','" +tbSifra + "', '" + tbKorisnickoIme + "', 'R');", connect);
            sda.ExecuteNonQuery();
            connect.Close();
        }
        public static void brisanjeRadnika(string tbKorisnickoIme, string tbIme, string tbPrezime)
        {
            connect.Open();
            SqlCommand sda = new SqlCommand("DELETE FROM korisnik where korisnickoIme ='" + tbKorisnickoIme + "';Delete from  korisnik where Ime='" + tbIme + "' and Prezime='" + tbPrezime + "' ", connect);
            sda.ExecuteNonQuery();
            connect.Close();
        }
        public static DataTable radniciBox(korisnik k)
        {
            connect.Open();
            SqlDataAdapter sda = new SqlDataAdapter("SELECT ime, prezime, brtelefona, korisnickoIme FROM korisnik WHERE tip='R'", connect);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            connect.Close();
            return dt;
        }

        public static List<korisnik> popuniTabelutermin()
        {
            parking_servisEntities1 vezasaBazom = new parking_servisEntities1();
            List<korisnik> korisnik = vezasaBazom.korisniks.Where(t => t.tip == "R").ToList();
            return korisnik;
        }

        private static bool proveraKorisnikaZaReset(korisnik k)
        {
            connect.Open();
            string sql = "SELECT * FROM korisnik WHERE korisnickoIme = @KorisnickoIme AND sifra = @sifra";
            SqlCommand cmd = new SqlCommand(sql, connect);
            cmd.Parameters.AddWithValue("@KorisnickoIme", k.korisnickoIme);
            cmd.Parameters.AddWithValue("@sifra", k.sifra);

            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                connect.Close();
                return true;
            }
            else
            {
                connect.Close();
                return false;
            }
        }
        public static bool resetLozinku(korisnik k, string novasifra)
        {
            if (proveraKorisnikaZaReset(k))
            {
                string sql = "UPDATE korisnik SET [sifra] = @sifra WHERE [korisnickoIme] = @KorisnickoIme";
                SqlCommand cmd = new SqlCommand(sql, connect);
                cmd.Parameters.Add("@sifra", SqlDbType.Char).Value = novasifra;
                cmd.Parameters.Add("@KorisnickoIme", SqlDbType.Char).Value = k.korisnickoIme;

                connect.Open();

                int rows = cmd.ExecuteNonQuery();

                connect.Close();

                if (rows > 0)
                    return true;
                return false;
            }
            else return false;
        }
    }
}

