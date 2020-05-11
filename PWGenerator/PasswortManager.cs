using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace PWGenerator
{
    public class PasswortManager
    {
        public string Sonderzeichen { get { return _sonderzeichen; } }
        string _sonderzeichen = "äöüß[]*♥!=(){}\"\\~%?.-_₀₁₂₃₄₅₆₇₈₉±¾↑&↓·←↗¤¼→™½¢ªµ´¯⁰¹²³⁴⁵⁶⁷⁸⁹♀¬°£$♂†…¥÷®º€¸§¶¨∞©";
        public string init
        {
            get
            {
                string x = "";
                for(int i = 0; i <= 10; i++)
                {
                    x += PasswortGenerieren() + "\n";
                }
                return x;
            }
        }

        public PasswortManager()
        {

        }

        public string PasswortGenerieren()
        {
            //Ein Passwort generieren
            PasswortArt art = new PasswortArt() { anzahlZeichen = 15, buchstaben = BuchstabenArt.großKlein, sonderzeichen = Sonderzeichen, zahlen = true };
            return NeuesPasswort(art);
        }

        public string PasswortGenerieren(PasswortArt art)
        {
            //Ein Passwort nach bestimmten Kriterien generieren
            return NeuesPasswort(art);
        }

        private string NeuesPasswort(PasswortArt art)
        {
            //String hält mögliche Zeichen
            string moeglicheZeichen = "";
            //Alphabet speichern für einfachen Zugriff
            string alphabetKlein = "abcdefghijklmnopqrstuvwxyz";
            //Welche Art an Buchstabendarstellung wurde übergeben?
            switch (art.buchstaben)
            {
                case BuchstabenArt.nurKlein:
                    moeglicheZeichen += alphabetKlein;
                    break;
                case BuchstabenArt.nurGroß:
                    moeglicheZeichen += alphabetKlein.ToUpper();
                    break;
                case BuchstabenArt.großKlein:
                    moeglicheZeichen += alphabetKlein + alphabetKlein.ToUpper();
                    break;
                case BuchstabenArt.keine:
                    break;
            }
            //Wurden zahlen erlaubt?
            if (art.zahlen)
                moeglicheZeichen += "1234567890";
            //Sonderzeichen anfügen
            moeglicheZeichen += art.sonderzeichen;
            //Neue Zufallszahl erstellen
            Random zufallsZahl = new Random();
            //Übergabestring erstellen und zufällig füllen, dann übergeben
            string returnString = "";
            try
            {
                for (int i = 0; i < art.anzahlZeichen; i++)
                    returnString += moeglicheZeichen[zufallsZahl.Next(moeglicheZeichen.Length)];
            }
            catch
            {
                for (int i = 0; i < art.anzahlZeichen; i++)
                    returnString += "*";
            }
            return returnString;
        }

        public struct PasswortArt
        {
            //Hier die verschiedenen Passwortarten / Kombinationen
            public BuchstabenArt buchstaben;
            public bool zahlen;
            public string sonderzeichen;
            public int anzahlZeichen;

            PasswortArt(BuchstabenArt bart, bool zahl, string sonder, int anzahl)
            {
                buchstaben = bart;
                zahlen = zahl;
                sonderzeichen = sonder;
                anzahlZeichen = anzahl;
            }
        }

        public enum BuchstabenArt
        {
            großKlein,
            nurKlein,
            nurGroß,
            keine,
        }
    }
}
