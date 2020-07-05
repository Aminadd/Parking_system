//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Parking_System
{
    using System;
    using System.Collections.Generic;
    using System.Data;

    public partial class korisnik
    {
        public string ime { get; set; }
        public string prezime { get; set; }
        public string brTelefona { get; set; }
        public string sifra { get; set; }
        public string korisnickoIme { get; set; }
        public string tip { get; set; }

        public korisnik()
        {

            this.korisnickoIme = null;
            this.ime = null;
            this.prezime = null;
            this.brTelefona = null;
            this.sifra = "";
            this.tip = null;
        }
        public korisnik(String korisnickoIme, String ime, String prezime, String brTelefona, String sifra, String tip)
        {

            this.korisnickoIme = korisnickoIme;
            this.ime = ime;
            this.prezime = prezime;
            this.brTelefona = brTelefona;
            this.sifra = sifra;
            this.tip = tip;
        }
        public bool proveraKorisnika()
        {
            if (Base.ProveraKorisnika(this))
                return true;
            else return false;
        }

       
        public DataTable login()
        {
            DataTable dt = Base.Login(this);
            return dt;
        }
        public DataTable radniciBox()
        {
            DataTable dt = Base.radniciBox(this);
            return dt;
        }

        public bool promenaLozinke(string novasifra)
        {
            if (Base.resetLozinku(this, novasifra))
                return true;
            return false;
        }
    }
}