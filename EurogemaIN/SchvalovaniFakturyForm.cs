using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ddPlugin;

namespace EurogemaIN
{
    public partial class SchvalovaniFakturyForm : Form
    {
        private Int32 ID;
        private IHelios Helios;
        private bool Zavedeni;

        private Int32? _IDFaktury;
        private Int32? IDFaktury
        {
            get
            {
                return _IDFaktury;
            }
            set
            {
                _IDFaktury = value;

                if (_IDFaktury != null)
                {
                    string SQL;
                    SQL = "SELECT CAST(DZ.RadaDokladu as NVARCHAR) + '-' + CAST(DZ.PoradoveCislo as NVARCHAR) + ': ' + CO.Nazev + ' (' + CAST(DZ.CisloOrg AS NVARCHAR) + ')' FROM	TabDokladyZbozi AS DZ INNER JOIN TabCisOrg AS CO ON CO.CisloOrg = DZ.CisloOrg";
                    SQL += " WHERE DZ.ID = " + _IDFaktury;
                    IHeQuery Faktura = Helios.OpenSQL(SQL);
                    if (!Faktura.EOF())
                        this.Text = Faktura.FieldValues(0);
                }
                else
                    this.Text = "Schvalování";
            }
        }

        
        private Int32? _IDPolozkySmlouvy;
        private Int32? IDPolozkySmlouvy
        {
            get
            {
                return _IDPolozkySmlouvy;
            }
            set
            {
                _IDPolozkySmlouvy = value;
                textBox_Smlouva.Text = "";
                textBox_Smlouva_View.Text = "";
                textBox_Smlouva_Zbyva.Text = "";
                textBox_Stredisko.ReadOnly = false;
                textBox_Stredisko.Tag = "E";
                button_Stredisko.Enabled = true;
                button_Stredisko.Tag = "E";
                textBox_Zakazka.ReadOnly = false;
                textBox_Zakazka.Tag = "E";
                button_Zakazka.Enabled = true;
                button_Zakazka.Tag = "E";
                textBox_NakladovyOkruh.ReadOnly = false;
                textBox_NakladovyOkruh.Tag = "E";
                button_NakladovyOkruh.Enabled = true;
                button_NakladovyOkruh.Tag = "E";

                if (_IDPolozkySmlouvy != null)
                {
                    string SQL;
                    SQL = "SELECT S.CisloSmlouvy, S.nazev, ISNULL(PS.StredNaklad, S.StredNaklad) AS Stredisko, ISNULL(PS.CisloZakazky, S.CisloZakazky) AS Zakazka, ISNULL(PS.NOkruhCislo, S.NOkruhCislo) AS NakladovyOkruh FROM TabBKOPolozkySmlouvy AS PS INNER JOIN TabBKOSmlouvy AS S ON S.id = PS.idSmlouvy";
                    SQL += " WHERE PS.ID = " + _IDPolozkySmlouvy;
                    IHeQuery Smlouva = Helios.OpenSQL(SQL);
                    if (!Smlouva.EOF())
                    {
                        textBox_Smlouva.Text = Smlouva.FieldValues(0);
                        textBox_Smlouva_View.Text = Smlouva.FieldValues(1);
                        textBox_Stredisko.ReadOnly = true;
                        textBox_Stredisko.Text = Smlouva.FieldValues(2);
                        textBox_Stredisko.Tag = "";
                        button_Stredisko.Enabled = false;
                        button_Stredisko.Tag = "";
                        textBox_Zakazka.ReadOnly = true;
                        textBox_Zakazka.Text = Smlouva.FieldValues(3);
                        textBox_Zakazka.Tag = "";
                        button_Zakazka.Enabled = false;
                        button_Zakazka.Tag = "";
                        textBox_NakladovyOkruh.ReadOnly = true;
                        textBox_NakladovyOkruh.Text = Smlouva.FieldValues(4);
                        textBox_NakladovyOkruh.Tag = "";
                        button_NakladovyOkruh.Enabled = false;
                        button_NakladovyOkruh.Tag = "";
                    }
                    textBox_Rozpocet_Zbyva.Text = ZbyvaZRozpoctu();
                    textBox_Smlouva_Zbyva.Text = ZbyvaZeSmlouvy();
                }
            }
        }

        private string ZbyvaZeSmlouvy()
        {

            if (IDPolozkySmlouvy == null)
                return "";
            Decimal Castka = 0;
            if ((Decimal.TryParse(textBox_Castka.Text, out Castka)) || (textBox_Castka.Text == ""))
            {
                string SQL;
                SQL = "SELECT CASE WHEN DZ.RadaDokladu LIKE '%1' THEN -1 WHEN DZ.RadaDokladu LIKE '%2' THEN 0 ELSE 1 END, dbo.EGSFZbyvaZeSmlouvy(" + IDPolozkySmlouvy.ToString() + ", " + ID.ToString() + ") FROM TabDokladyZbozi AS DZ WHERE DZ.ID = " + IDFaktury.ToString();
                IHeQuery CastkaPolozkySmlouvy = Helios.OpenSQL(SQL);
                if (CastkaPolozkySmlouvy.EOF())
                    return "";
                Castka = ((Decimal)(CastkaPolozkySmlouvy.FieldValues(0))) * Castka;
                return (((Decimal)(CastkaPolozkySmlouvy.FieldValues(1))) - Castka).ToString("N");
            }
            else
                return "";
        }

        public SchvalovaniFakturyForm(IHelios MainHelios, Int32 RecordID)
        {
            InitializeComponent();
            Helios = MainHelios;
            ID = RecordID;
            textBox_Stredisko.Leave += textBox_Rozpocet_Zbyva_Enter;
            textBox_Zakazka.Leave += textBox_Rozpocet_Zbyva_Enter;
            textBox_NakladovyOkruh.Leave += textBox_Rozpocet_Zbyva_Enter;
            textBox_Castka.Leave += textBox_Rozpocet_Zbyva_Enter;
            Zavedeni = true; 
            NaplnPole();
            Zavedeni = false;
        }

        private void NaplnPole()
        {
            string SQL;

            SQL = "SELECT SF.idFaktury, SF.CisloZam, SF.Stredisko, SF.CisloZakazky, SF.Castka, SF.Ukod, SF.NakladovyOkruh, SF.IDPolozkySmlouvy, SF.HodDodavatele, SF.StavbaSchvalil, SF.StavbaSchvalilAutor, SF.StavbaSchvalilCas, SF.StavbaPoznamka, SF.StavbaPozastavkaDoKdy, SF.StavbaPozastavkaCastka, SF.StavbaPozastavka, SF.StrediskoSchvalil, SF.StrediskoSchvalilAutor, SF.StrediskoSchvalilCas, SF.StrediskoPoznamka, SF.StrediskoPozastavka, ISNULL(UCA.FullName, SF.autor), SF.DatPorizeni, ISNULL(UCZ.FullName, SF.zmenil), SF.DatZmeny FROM TabBKOSchvalovaniFaktury AS SF LEFT OUTER JOIN TabUserCfg AS UCA ON UCA.LoginName = SF.autor LEFT OUTER JOIN TabUserCfg AS UCZ ON UCZ.LoginName = SF.zmenil";
            SQL += " WHERE SF.ID = " + ID.ToString();
            IHeQuery Radek = Helios.OpenSQL(SQL);
            if (Radek.EOF())
                return;
            IDFaktury = Radek.FieldValues(0);
            textBox_Zamestnanec.Text = ((int)(Radek.FieldValues(1))).ToString();
            textBox_Stredisko.Text = Radek.FieldValues(2);
            textBox_Zakazka.Text = Radek.FieldValues(3);
            textBox_Castka.Text = ((decimal)(Radek.FieldValues(4))).ToString();
            textBox_KodUctovani.Text = (Radek.FieldValues(5) is DBNull) ? "" : ((int)(Radek.FieldValues(5))).ToString();
            textBox_NakladovyOkruh.Text = Radek.FieldValues(6);
            IDPolozkySmlouvy = (Radek.FieldValues(7) is DBNull) ? null : Radek.FieldValues(7);
            switch ((Radek.FieldValues(8) is DBNull) ? -1 : (int)(Radek.FieldValues(8)))
            {   
                case 0: radioButton_Znamka_1.Checked = true; break;
                case 1: radioButton_Znamka_2.Checked = true; break;
                case 2: radioButton_Znamka_3.Checked = true; break;
                case 3: radioButton_Znamka_4.Checked = true; break;
                case 4: radioButton_Znamka_5.Checked = true; break;
            }
            if (Radek.FieldValues(9))
            {
                checkBox_Kontrola.Checked = true;
                label_Kontrola_View.Text = (Radek.FieldValues(10) is DBNull) ? "" : Radek.FieldValues(10);
                label_Kontrola_View.Text += (Radek.FieldValues(11) is DBNull) ? "" : " (" + ((DateTime)(Radek.FieldValues(11))).ToString() + ")";
            }
            else
                label_Kontrola_View.Text = "";
            textBox_Kontrola_Poznamka.Text = (Radek.FieldValues(12) is DBNull) ? "" : Radek.FieldValues(12);
            maskedTextBox_Kontrola_Pozastavky_Do.Text = (Radek.FieldValues(13) is DBNull) ? "" : ((DateTime)(Radek.FieldValues(13))).ToString("dd.MM.yyyy");
            textBox_Kontrola_Pozastavky_Castka.Text = (Radek.FieldValues(14) is DBNull) ? "" : ((decimal)(Radek.FieldValues(14))).ToString();
            textBox_Kontrola_Pozastavky_Poznamka.Text = (Radek.FieldValues(15) is DBNull) ? "" : Radek.FieldValues(15);
            if (Radek.FieldValues(16))
            {
                checkBox_Schvaleni.Checked = true;
                label_Schvaleni_View.Text = (Radek.FieldValues(17) is DBNull) ? "" : Radek.FieldValues(17);
                label_Schvaleni_View.Text += (Radek.FieldValues(18) is DBNull) ? "" : " (" + ((DateTime)(Radek.FieldValues(18))).ToString() + ")";
            }
            else
                label_Schvaleni_View.Text = "";
            textBox_Schvaleni_Poznamka.Text = (Radek.FieldValues(19) is DBNull) ? "" : Radek.FieldValues(19);
            textBox_Schvaleni_Pozastavky_Poznamka.Text = (Radek.FieldValues(20) is DBNull) ? "" : Radek.FieldValues(20);
            label_Rozepsal_View.Text = (Radek.FieldValues(21) is DBNull) ? "" : Radek.FieldValues(21);
            label_Rozepsal_View.Text += (Radek.FieldValues(22) is DBNull) ? "" : " (" + Radek.FieldValues(22) + ")";
            label_Zmenil_View.Text = (Radek.FieldValues(23) is DBNull) ? "" : Radek.FieldValues(23);
            label_Zmenil_View.Text += (Radek.FieldValues(24) is DBNull) ? "" : " (" + Radek.FieldValues(24) + ")";

            SQL = "SELECT COUNT(*) FROM TabDokumVazba";
            SQL += " WHERE IdentVazby = 9 AND IdTab = " + IDFaktury.ToString();
            IHeQuery PocetDokumentu = Helios.OpenSQL(SQL);
            if (PocetDokumentu.EOF())
                button_Zobraz_ALL.Text = "Zobraz skeny (0)";
            else
                button_Zobraz_ALL.Text = "Zobraz skeny (" + ((int)(PocetDokumentu.FieldValues(0))).ToString() + ")";
            if (IDPolozkySmlouvy == null)
                textBox_Rozpocet_Zbyva.Text = ZbyvaZRozpoctu();
        }

        private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                MessageBox.Show("Unable to release the Object " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }

        private void textBox_Zamestnanec_TextChanged(object sender, EventArgs e)
        {
            Int32 CisloZamestnance;
            if (Int32.TryParse(textBox_Zamestnanec.Text, out CisloZamestnance))
            {
                string SQL;
                SQL = "SELECT PrijmeniJmeno FROM TabCisZam";
                SQL += " WHERE Cislo = " + textBox_Zamestnanec.Text;
                IHeQuery Zamestnanec = Helios.OpenSQL(SQL);
                if (Zamestnanec.EOF())
                    textBox_Zamestnanec_View.Text = "";
                else
                    textBox_Zamestnanec_View.Text = Zamestnanec.FieldValues(0);
            }
            else
                textBox_Zamestnanec_View.Text = "";

        }

        private void textBox_Stredisko_TextChanged(object sender, EventArgs e)
        {
            string SQL;
            SQL = "SELECT Nazev FROM TabStrom";
            SQL += " WHERE Cislo = '" + textBox_Stredisko.Text + "'";
            IHeQuery Stredisko = Helios.OpenSQL(SQL);
            if (Stredisko.EOF())
                textBox_Stredisko_View.Text = "";
            else
                textBox_Stredisko_View.Text = Stredisko.FieldValues(0);
        }

        private void textBox_Zakazka_TextChanged(object sender, EventArgs e)
        {
            string SQL;
            SQL = "SELECT Nazev FROM TabZakazka";
            SQL += " WHERE CisloZakazky = '" + textBox_Zakazka.Text + "'";
            IHeQuery Zakazka = Helios.OpenSQL(SQL);
            if (Zakazka.EOF())
                textBox_Zakazka_View.Text = "";
            else
                textBox_Zakazka_View.Text = Zakazka.FieldValues(0);
        }

        private void textBox_Castka_TextChanged(object sender, EventArgs e)
        {
            Decimal Castka = 0;
            if ((Decimal.TryParse(textBox_Castka.Text, out Castka)) || (textBox_Castka.Text == ""))
            {
                string SQL;
                SQL = "SELECT MIN(ISNULL(DZE._castkaBezDPH, 0)) - SUM(ISNULL(SF.Castka, 0)) + (SELECT ISNULL(Castka, 0) FROM TabBKOSchvalovaniFaktury WHERE ID = " + ID.ToString() + ") FROM TabBKOSchvalovaniFaktury AS SF LEFT OUTER JOIN TabDokladyZbozi_EXT AS DZE ON DZE.ID = SF.idFaktury WHERE SF.idFaktury = " + IDFaktury.ToString();
                IHeQuery qCastka = Helios.OpenSQL(SQL);
                if (qCastka.EOF())
                    textBox_Castka_Zbyva.Text = "";
                else
                    textBox_Castka_Zbyva.Text = (((Decimal)(qCastka.FieldValues(0) is DBNull ? 0 : qCastka.FieldValues(0))) - Castka).ToString("N");
            }
            else
                textBox_Castka_Zbyva.Text = "";
            textBox_Smlouva_Zbyva.Text = ZbyvaZeSmlouvy();
        }

        private void textBox_NakladovyOkruh_TextChanged(object sender, EventArgs e)
        {
            string SQL;
            SQL = "SELECT Nazev FROM TabNakladovyOkruh";
            SQL += " WHERE Cislo = '" + textBox_NakladovyOkruh.Text + "'";
            IHeQuery NakladovyOkruh = Helios.OpenSQL(SQL);
            if (NakladovyOkruh.EOF())
                textBox_NakladovyOkruh_View.Text = "";
            else
                textBox_NakladovyOkruh_View.Text = NakladovyOkruh.FieldValues(0);
        }

        private string ZbyvaZRozpoctu()
        {
            if ((textBox_Stredisko.Text == "") || (textBox_Zakazka.Text == "") || (textBox_NakladovyOkruh.Text == "") || (textBox_Castka.Text == ""))
            {
                return "";
            }

            string SQL;
            Decimal Castka;

            SQL = "SELECT CASE WHEN DZ.RadaDokladu LIKE '%1' THEN -1 WHEN DZ.RadaDokladu LIKE '%2' THEN 0 ELSE 1 END, dbo.EGSFZbyvaZRozpoctu('" + textBox_Stredisko.Text + "', '" + textBox_Zakazka.Text + "', '" + textBox_NakladovyOkruh.Text + "', " + ID.ToString() + ")  FROM TabDokladyZbozi AS DZ WHERE DZ.ID = " + IDFaktury.ToString();
            IHeQuery qZbyva = Helios.OpenSQL(SQL);

            if (qZbyva.EOF())
                return "";
            else
            {
                if (Decimal.TryParse(textBox_Castka.Text, out Castka))
                {
                    Castka = ((Decimal)(qZbyva.FieldValues(0))) * Castka;
                    return (((Decimal)(qZbyva.FieldValues(1))) - Castka).ToString("N");
                }
                else
                    return "";
            }
        }

        private void textBox_Rozpocet_Zbyva_Enter(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            textBox_Rozpocet_Zbyva.Text = ZbyvaZRozpoctu();
            Cursor.Current = Cursors.Default;
        }

        private void textBox_Rozpocet_Zbyva_TextChanged(object sender, EventArgs e)
        {
            Decimal Castka = 0;
            if ((Decimal.TryParse(textBox_Rozpocet_Zbyva.Text, out Castka)))
            {
                if (Castka < 0)
                    textBox_Rozpocet_Zbyva.BackColor = Color.FromArgb(250, 115, 115);
                else
                    textBox_Rozpocet_Zbyva.BackColor = SystemColors.Control;
            }
            else
                textBox_Rozpocet_Zbyva.BackColor = SystemColors.Control;

        }

        private void textBox_Smlouva_Zbyva_TextChanged(object sender, EventArgs e)
        {
            Decimal Castka = 0;
            if ((Decimal.TryParse(textBox_Smlouva_Zbyva.Text, out Castka)))
            {
                if (Castka < 0)
                    textBox_Smlouva_Zbyva.BackColor = Color.FromArgb(250, 115, 115);
                else
                    textBox_Smlouva_Zbyva.BackColor = SystemColors.Control;
            }
            else
                textBox_Smlouva_Zbyva.BackColor = SystemColors.Control;
        }

        private void button_Smlouva_Zrus_Click(object sender, EventArgs e)
        {
            IDPolozkySmlouvy = null;
        }

        private void button_Smlouva_Click(object sender, EventArgs e)
        {
            string WHERE;
            WHERE = "CisloOrg = (SELECT CisloOrg FROM TabDokladyZbozi WHERE ID = " + IDFaktury.ToString() + ")";
            if (textBox_Stredisko.Text != "")
                WHERE += " AND Stredisko = " + textBox_Stredisko.Text;
            if (textBox_Zakazka.Text != "")
                WHERE += " AND Zakazka = " + textBox_Zakazka.Text;
            if (textBox_NakladovyOkruh.Text != "")
                WHERE += " AND NakladovyOkruh = " + textBox_NakladovyOkruh.Text;
            Object oIDPolozkySmlouvy = IDPolozkySmlouvy as Object;
            if (Helios.Prenos(100173, "ID", ref oIDPolozkySmlouvy, WHERE, "Výběr smlouvy", true))
                IDPolozkySmlouvy = oIDPolozkySmlouvy as Int32?;
        }

        private void checkBox_Kontrola_CheckedChanged(object sender, EventArgs e)
        {
            foreach (Control control in panel_Hlavicka.Controls)
            {
                if ((control.Tag != null) && (control.Tag.ToString() == "E"))
                    control.Enabled = !checkBox_Kontrola.Checked;
            }
            if (!checkBox_Kontrola.Checked)
                return;
            if (Zavedeni)
                return;

            string Nevyplneno = "Následující pole nejsou korektně vyplněny:";
            if (textBox_Stredisko.Text == "")
                Nevyplneno += " Středisko,";
            if (textBox_Zakazka.Text == "")
                Nevyplneno += " Zakázka,";
            if (textBox_NakladovyOkruh.Text == "")
                Nevyplneno += " Nákladový okruh,";
            
            Decimal Castka;
            Decimal.TryParse(textBox_Castka.Text, out Castka);
            if (Castka <= 0)
                Nevyplneno += " Částka musí být větší než 0,";
            
            string sZnamka = "";
            foreach (Control control in groupBox_Znamka.Controls)
            {
                if (((RadioButton)control).Checked)
                    sZnamka = control.Text;
            }
            if (sZnamka == "")
                Nevyplneno += " Hodnocení dodavatele,";

            if (Nevyplneno.EndsWith(","))
            {
                Nevyplneno = Nevyplneno.Substring(0, Nevyplneno.Length - 1) + ".";
                Helios.Error(Nevyplneno);
                checkBox_Kontrola.Checked = false;
                return;
            }

            if (!ExistujeKombinace())
            {
                Helios.Error("Neexistující kombinace střediska a zakázky!");
                checkBox_Kontrola.Checked = false;
                return;
            }

            bool Zaporne = false;
            Decimal.TryParse(textBox_Rozpocet_Zbyva.Text, out Castka);
            if (Castka < 0)
                Zaporne = true;
            Decimal.TryParse(textBox_Smlouva_Zbyva.Text, out Castka);
            if (Castka < 0)
                Zaporne = true;
            if (Zaporne && textBox_Kontrola_Poznamka.Text.Length < 50)
            {
                Helios.Error("V případě, že je zbytek rozpočtu nebo smlouvy záporný, je třeba vyplnit pole poznámka kontroly textem o délce alespoň 50 znaků!");
                checkBox_Kontrola.Checked = false;
                return;
            }

            Decimal.TryParse(textBox_Castka_Zbyva.Text, out Castka);
            if (Castka < 0)
                Helios.Info("Upozornění: Překročena celková částka pro schvalování!");
        }

        private bool ExistujeKombinace()
        {
            if ((textBox_Stredisko.Text == "") || (textBox_Zakazka.Text == ""))
                return true;

            string SQL;
            SQL = "SELECT COUNT(*) FROM TabBKOSchvalovaniRozpis AS SR";
            SQL += " WHERE SR.Stredisko = '" + textBox_Stredisko.Text + "' AND SR.CisloZakazky = '" + textBox_Zakazka.Text + "'";
            IHeQuery Kombinace = Helios.OpenSQL(SQL);
            if (Kombinace.EOF())
                return false;
            if (Kombinace.FieldValues(0) == 0)
                return false;
            return true;
        }

    }
}
