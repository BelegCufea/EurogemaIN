using System;
using System.Windows.Forms;
using ddPlugin;
using System.Globalization;
using System.Text;
using System.Collections.Generic;
using System.Linq;

namespace EurogemaIN
{
    public partial class DochazkaForm : Form
    {
        private Int32 ID;
        private IHelios Helios;
        private Int32 FocusTyden;
        private Int32 FocusRadek;
        private List<Double> Hodiny;
        private List<String> Strediska;
        private List<String> Zakazky;
        private List<String> Cinnosti;
        private List<String> DoplnkoveCinnosti;
        private struct CinnostStruct
        {
            public String Cislo;
            public Double? Hodiny;
            public Boolean Smena;
        }
        private List<CinnostStruct> CinnostiHodiny;
        private struct KombinaceStruct
        {
            public String Stredisko;
            public String Zakazka;
        }
        private List<KombinaceStruct> Kombinace;
        private ChybovaHlaseni Chyby;
        private ChybovaHlaseni Informace;
        private Progress Postup;
        private struct DochazkaStruct
        {
            public Int32 CisloZam;
            public DateTime Datum;
            public String Stredisko;
            public String Zakazka;
            public String Cinnost;
            public Double Hodiny;
        }
        private List<DochazkaStruct> DochazkaDny;
        private class ZamestnanciItem
        {
            public String Zamestnanec;
            public Int32 Cislo;
            public ZamestnanciItem(String zamestnanec, Int32 cislo)
            {
                Zamestnanec = zamestnanec;
                Cislo = cislo;
            }
            public override string ToString()
            {
                return Zamestnanec;
            }
        }
        private class DochazkaUprava
        {
            private DateTime _datum;
            private String _stredisko;
            private String _zakazka;
            private String _cinnost;
            private Double _hodiny;
            private Boolean _svatek;
            private Boolean _prace;

            public DateTime Datum
            {
                get
                {
                    return _datum;
                }
                set
                {
                    _datum = value;
                }
            }

            public Int32 Den
            {
                get
                {
                    return _datum.Day;
                }
            }

            public String Stredisko
            {
                get
                {
                    return _stredisko;
                }
                set
                {
                    _stredisko = value;
                }
            }

            public String Zakazka
            {
                get
                {
                    return _zakazka;
                }
                set
                {
                    _zakazka = value;
                }
            }

            public String Cinnost
            {
                get
                {
                    return _cinnost;
                }
                set
                {
                    _cinnost = value;
                }
            }

            public Double Hodiny
            {
                get
                {
                    return _hodiny;
                }
                set
                {
                    _hodiny = value;
                }
            }

            public Boolean Prace
            {
                get
                {
                    return _prace;
                }
                set
                {
                    _prace = value;
                }
            }

            public Int32 DenTydne
            {
                get
                {
                    switch (_datum.DayOfWeek)
                    {
                        case DayOfWeek.Friday:
                            return 5;
                        case DayOfWeek.Monday:
                            return 1;
                        case DayOfWeek.Saturday:
                            return 6;
                        case DayOfWeek.Sunday:
                            return 7;
                        case DayOfWeek.Thursday:
                            return 4;
                        case DayOfWeek.Tuesday:
                            return 2;
                        case DayOfWeek.Wednesday:
                            return 3;
                        default:
                            return 0;
                    }
                }
            }

            public Boolean Svatek
            {
                get
                {
                    return _svatek;
                }
            }

            public DochazkaUprava(DateTime Datum, String Stredisko, String Zakazka, String Cinnost, Double Hodiny, IHelios HeliosHandler)
            {
                _datum = Datum;
                _stredisko = Stredisko;
                _zakazka = Zakazka;
                _cinnost = Cinnost;
                _hodiny = Hodiny;

                String SQL = "SELECT KD.Datum_D FROM TabMzKalendar AS K INNER JOIN TabMzKalendarDny AS KD ON KD.IDKalendar = K.ID WHERE K.Cislo = '001' AND KD.Svatek = 1 AND KD.Datum_Y = " + Datum.Year + " AND KD.Datum_M = " + Datum.Month + " AND KD.Datum_D = " + Datum.Day;
                IHeQuery JeSvatek = HeliosHandler.OpenSQL(SQL);
                if (JeSvatek.RecordCount() == 0)
                    _svatek = false;
                else
                    _svatek = true;

                SQL = "SELECT Cislo FROM TabEGDochazkaCinnosti WHERE Cislo = '" + Cinnost + "'";
                IHeQuery JePrace = HeliosHandler.OpenSQL(SQL);
                if (JePrace.RecordCount() == 0)
                    _prace = true;
                else
                    _prace = false;
                
            }
        }

        private List<DochazkaUprava> DochazkaUpravy;
        private Boolean Ulozit;
        private Boolean Zavedeni;
        private Decimal _Mesic;
        private Decimal _Rok;
        private Boolean Uzamceno;
        private System.Threading.Timer timer;
        private Boolean Vyhledavam;

        public DochazkaForm(IHelios MainHelios, Int32 RecordID)
        {
            Zavedeni = true;
            Ulozit = true;
            Uzamceno = true;
            Vyhledavam = false;
            this.InitializeComponent();
            Helios = MainHelios;
            ID = RecordID;
            PripravValidaci();
            NaplnDatumy();
            NaplnStromZamestnancu();
            PripravFormularMesic();
            NajdiZamestnance();
            Zavedeni = false;
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

        private void NaplnDatumy()
        {
            String SQL = "SELECT Rok, Mesic FROM TabMzdObd WHERE Stav = 1";
            IHeQuery AktualniObdobi = Helios.OpenSQL(SQL);
            if (AktualniObdobi.EOF())
            {
                numericUpDown_Mesic.Value = DateTime.Now.Month - 1;
                numericUpDown_Rok.Value = DateTime.Now.Month == 1 ? DateTime.Now.Year - 1 : DateTime.Now.Year;
            }
            else
            {
                numericUpDown_Mesic.Value = AktualniObdobi.FieldValues(1);
                numericUpDown_Rok.Value = AktualniObdobi.FieldValues(0);
            }
        }

        private void NaplnStromZamestnancu()
        {
            String SQL;
            String Stredisko = "";
            String Zakazka = "";
            TreeNode StrediskoNode;
            TreeNode ZakazkaNode;


            SQL = "SELECT * FROM dbo.EGDochazkaSeznam(" + numericUpDown_Rok.Value + ", " + numericUpDown_Mesic.Value + ")";
            IHeQuery Zamestnanci = Helios.OpenSQL(SQL);
            StrediskoNode = null;
            ZakazkaNode = null;
            treeView_Zamestnanci.Nodes.Clear();
            treeView_Zamestnanci.BeginUpdate();
            TreeNode Root = treeView_Zamestnanci.Nodes.Add("EUROGEMA", "EUROGEMA");
            List<ZamestnanciItem> ZamestnanciSeznam = new List<ZamestnanciItem>();
            while (!Zamestnanci.EOF())
            {
                if (Stredisko != (String)(Zamestnanci.FieldValues(2)))
                {
                    Stredisko = (String)(Zamestnanci.FieldValues(2));
                    StrediskoNode = Root.Nodes.Add("S" + Stredisko, Stredisko);
                    Zakazka = "";
                }
                if (Zakazka != (String)(Zamestnanci.FieldValues(3)))
                {
                    Zakazka = (String)(Zamestnanci.FieldValues(3));
                    ZakazkaNode = StrediskoNode.Nodes.Add("Z" + Zakazka, Zakazka + " (" + (String)(Zamestnanci.FieldValues(4)) + ")");
                }
                ZakazkaNode.Nodes.Add(((Int32)(Zamestnanci.FieldValues(0))).ToString(),(String)(Zamestnanci.FieldValues(1)));
                ZamestnanciSeznam.Add(new ZamestnanciItem((String)(Zamestnanci.FieldValues(1)), (Int32)(Zamestnanci.FieldValues(0))));
                Zamestnanci.Next();
            }
            ZamestnanciSeznam.Sort((a, b) => a.Zamestnanec.CompareTo(b.Zamestnanec));
            comboBox_Kopirovani.Items.Clear();
            foreach (ZamestnanciItem item in ZamestnanciSeznam)
            {
                comboBox_Kopirovani.Items.Add(item); 
            }
            treeView_Zamestnanci.EndUpdate();
            treeView_Zamestnanci.Refresh();
        }

        private void numericUpDown_Rok_ValueChanged(object sender, EventArgs e)
        {
            if (!Ulozit)
                return;
            Boolean Provest = true;

            if (treeView_Zamestnanci.SelectedNode == null)
                return;
            if (!Uzamceno && !Zavedeni)
            {
                switch (tabControlZadavani.SelectedIndex)
                {
                    case 1:
                        if (Helios.YesNo("Uložit doplňkové informace?", true))
                        {
                            if (!UlozitAkce())
                            {
                                Helios.Error("Doplňkové informace neuloženy");
                                Provest = false;
                                Ulozit = false;
                                numericUpDown_Rok.Value = _Rok;
                                Ulozit = true;
                            }
                        }
                        break;
                    case 0:
                        if (Helios.YesNo("Uložit rozpracovanou docházku?", true))
                        {
                            if (!UlozitDochazku())
                            {
                                Helios.Error("Docházka neuložena");
                                Provest = false;
                                Ulozit = false;
                                numericUpDown_Rok.Value = _Rok;
                                Ulozit = true;
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
            if (Provest)
            {
                _Rok = numericUpDown_Rok.Value;
                String Zamestnanec = treeView_Zamestnanci.SelectedNode == null ? "" : treeView_Zamestnanci.SelectedNode.Text;
                NaplnStromZamestnancu();
                //PripravFormularMesic();
                textBox_Vyhledavani.Text = Zamestnanec;
                VyhledejZamestnance();
            }
        }

        private void numericUpDown_Mesic_ValueChanged(object sender, EventArgs e)
        {
            if (!Ulozit)
                return;
            Boolean Provest = true;

            if (treeView_Zamestnanci.SelectedNode == null)
                return;
            if (!Uzamceno && !Zavedeni)
            {
                switch (tabControlZadavani.SelectedIndex)
                {
                    case 1:
                        if (Helios.YesNo("Uložit doplňkové informace?", true))
                        {
                            if (!UlozitAkce())
                            {
                                Helios.Error("Doplňkové informace neuloženy");
                                Provest = false;
                                Ulozit = false;
                                numericUpDown_Mesic.Value = _Mesic;
                                Ulozit = true;
                            }
                        }
                        break;
                    case 0:
                        if (Helios.YesNo("Uložit rozpracovanou docházku?", true))
                        {
                            if (!UlozitDochazku())
                            {
                                Helios.Error("Docházka neuložena");
                                Provest = false;
                                Ulozit = false;
                                numericUpDown_Mesic.Value = _Mesic;
                                Ulozit = true;
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
            if (Provest)
            {
                _Mesic = numericUpDown_Mesic.Value;
                String Zamestnanec = treeView_Zamestnanci.SelectedNode == null ? "" : treeView_Zamestnanci.SelectedNode.Text;
                NaplnStromZamestnancu();
                //PripravFormularMesic();
                textBox_Vyhledavani.Text = Zamestnanec;
                VyhledejZamestnance();
            }
        }

        private void PripravFormularMesic()
        {
            panel_Formular.Controls.Clear();

            Int16 Rok = (Int16)numericUpDown_Rok.Value;
            Int16 Mesic = (Int16)numericUpDown_Mesic.Value;
            UzamciUzavreneObdobi(Rok, Mesic);
            Calendar Kalendar = new CultureInfo("cs-CZ").Calendar;
            Int32 PrvniTyden = Kalendar.GetWeekOfYear(new DateTime(Rok, Mesic, 1), CalendarWeekRule.FirstDay, DayOfWeek.Monday);
            Int32 PosledniTyden = Kalendar.GetWeekOfYear(new DateTime(Rok, Mesic, DateTime.DaysInMonth(Rok, Mesic)), CalendarWeekRule.FirstDay, DayOfWeek.Monday);
            TableLayoutPanel Formular = new TableLayoutPanel();
            Formular.AutoSize = true;
            for (int i = PrvniTyden; i <= PosledniTyden; i++)
            {
                Formular.RowCount++;
                Formular.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            }
            Int32 Tyden;
            Int32 AktalniTyden = 0;
            Int32 DenTydne;
            Label Popisek;
            Button Tlacitko;
            TextBox Text;
            ToolTip toolTip_CtrlEnter;
            String SQL = "SELECT D.Datum_D, D.TypDne, D.Svatek FROM TabMzKalendar AS K INNER JOIN TabMzKalendarDny AS D ON D.IDKalendar = K.ID WHERE K.Rok = " + Rok + " AND K.Cislo = '001' AND D.Datum_M = " + Mesic;
            IHeQuery Svatky = Helios.OpenSQL(SQL);
            for (int Den = 1; Den <= DateTime.DaysInMonth(Rok, Mesic); Den++)
            {
                DateTime Datum = new DateTime(Rok, Mesic, Den);
                Tyden = Kalendar.GetWeekOfYear(Datum, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
                if (AktalniTyden != Tyden)
                {
                    TableLayoutPanel TydenTable = new TableLayoutPanel();
                    TydenTable.AutoSize = true;
                    TydenTable.Tag = Tyden;
                    for (int Radek = 0; Radek < 5; Radek++)
                    {
                        TydenTable.RowCount++;
                        TydenTable.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                        TableLayoutPanel DenTable = new TableLayoutPanel();
                        DenTable.AutoSize = true;
                        DenTable.Margin = new Padding(0);
                        DenTable.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
                        DenTable.Tag = 1;
                        DenTable.RowCount++;
                        DenTable.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                        for (int d = 0; d < 9; d++)
                        {
                            DenTable.ColumnCount++;
                            DenTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 70));
                        }
                        switch (Radek)
                        {
                            case 0:
                                DenTable.BackColor = System.Drawing.Color.Beige;
                                Popisek = new Label();
                                Popisek.Text = "Týden " + Tyden;
                                Popisek.Dock = DockStyle.Top;
                                Popisek.Height = 16;
                                Popisek.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                                DenTable.Controls.Add(Popisek, 0, 0);
                                DenTable.SetColumnSpan(Popisek, 2);
                                break;
                            case 1:
                                Popisek = new Label();
                                Popisek.Text = "Úsek";
                                Popisek.Dock = DockStyle.Top;
                                Popisek.Height = 16;
                                Popisek.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                                DenTable.Controls.Add(Popisek, 0, 0);
                                Popisek = new Label();
                                Popisek.Text = "Zakázka";
                                Popisek.Dock = DockStyle.Top;
                                Popisek.Height = 16;
                                Popisek.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                                DenTable.Controls.Add(Popisek, 1, 0);
                                Popisek = new Label();
                                Popisek.Text = "Činnost";
                                Popisek.Dock = DockStyle.Top;
                                Popisek.Height = 16;
                                Popisek.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                                DenTable.Controls.Add(Popisek, 2, 0);
                                DenTable.SetColumnSpan(Popisek, 7);
                                break;
                            case 2:
                                toolTip_CtrlEnter = new ToolTip();
                                Text = new TextBox();
                                Text.Dock = DockStyle.Top;
                                Text.Height = 16;
                                Text.TextAlign = HorizontalAlignment.Center;
                                Text.DoubleClick += VyberStrediska_DblClick;
                                Text.PreviewKeyDown += AllowKeys;
                                Text.KeyDown += VyberStredisko_CtrlEnter;
                                Text.KeyDown += ZpracujKlavesy;
                                Text.Validating += KontrolaStrediska_Validating;
                                Text.Enter += PodbarviFocusRadku_Enter;
                                Text.Tag = ("S" + Tyden + "_1");
                                toolTip_CtrlEnter.SetToolTip(Text, "CTRL+Enter = výběr úseku");
                                DenTable.Controls.Add(Text, 0, 0);
                                Text = new TextBox();
                                Text.Dock = DockStyle.Top;
                                Text.Height = 16;
                                Text.TextAlign = HorizontalAlignment.Center;
                                Text.DoubleClick += VyberZakazky_DblClick;
                                Text.PreviewKeyDown += AllowKeys;
                                Text.KeyDown += VyberZakazky_CtrlEnter;
                                Text.KeyDown += ZpracujKlavesy;
                                Text.Validating += KontrolaZakazky_Validating;
                                Text.Enter += PodbarviFocusRadku_Enter;
                                Text.Tag = ("Z" + Tyden + "_1");
                                toolTip_CtrlEnter.SetToolTip(Text, "CTRL+Enter = výběr zakázky");
                                DenTable.Controls.Add(Text, 1, 0);
                                break;
                            case 3:
                                Popisek = new Label();
                                Popisek.Text = "Hodiny";
                                Popisek.Dock = DockStyle.Top;
                                Popisek.Height = 16;
                                Popisek.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                                DenTable.Controls.Add(Popisek);
                                DenTable.SetColumnSpan(Popisek, 9);
                                break;
                            case 4:
                                Tlacitko = new Button();
                                Tlacitko.Text = "+";
                                Tlacitko.Width = 24;
                                Tlacitko.Height = 16;
                                Tlacitko.TabStop = false;
                                Tlacitko.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                                Tlacitko.Click += PridejRadek_Click;
                                Tlacitko.Tag = ("B" + Tyden + "_1");
                                DenTable.Controls.Add(Tlacitko, 0, 0);
                                break;
                            default:
                                break;
                        }
                        TydenTable.Controls.Add(DenTable, 0, Radek);
                    }
                    Panel TydenPanel = new Panel();
                    TydenPanel.BorderStyle = BorderStyle.Fixed3D;
                    TydenPanel.AutoSize = true;
                    TydenPanel.Controls.Add(TydenTable);
                    TydenPanel.Tag = Tyden;
                    Formular.Controls.Add(TydenPanel, 0, Tyden - PrvniTyden);
                    AktalniTyden = Tyden;
                }
                DenTydne = (Int32)Kalendar.GetDayOfWeek(Datum);
                DenTydne = DenTydne == 0 ? 8 : DenTydne + 1;
                Popisek = new Label();
                Popisek.Text = Datum.Day.ToString();
                Popisek.Dock = DockStyle.Top;
                Popisek.Height = 16;
                Popisek.Tag = Datum.Day;
                Popisek.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                if (!Svatky.EOF())
                {
                    if (Svatky.FieldValues(1) == 1)
                        Popisek.BackColor = System.Drawing.Color.Cyan;
                    if (Svatky.FieldValues(2))
                        Popisek.BackColor = System.Drawing.Color.Red;
                    Svatky.Next();
                }
                ((TableLayoutPanel)((TableLayoutPanel)((Panel)(Formular.GetControlFromPosition(0, AktalniTyden - PrvniTyden))).Controls[0]).GetControlFromPosition(0, 0)).Controls.Add(Popisek, DenTydne, 0);
                Text = new TextBox();
                Text.Dock = DockStyle.Top;
                Text.Height = 16;
                Text.TextAlign = HorizontalAlignment.Center;
                toolTip_CtrlEnter = new ToolTip();
                Text.DoubleClick += VyberCinnost_DblClick;
                Text.PreviewKeyDown += AllowKeys;
                Text.KeyDown += VyberCinnost_CtrlEnter;
                Text.KeyDown += ZpracujKlavesy;
                Text.Enter += PodbarviFocusRadku_Enter;
                Text.Validating += KontrolaCinnosti_Validating;
                toolTip_CtrlEnter.SetToolTip(Text, "CTRL+Enter = výběr činnosti");
                Text.Tag = ("C" + Den + "_1");
                ((TableLayoutPanel)((TableLayoutPanel)((Panel)(Formular.GetControlFromPosition(0, AktalniTyden - PrvniTyden))).Controls[0]).GetControlFromPosition(0, 2)).Controls.Add(Text, DenTydne, 0);
                Text = new TextBox();
                Text.Dock = DockStyle.Top;
                Text.Height = 16;
                Text.TextAlign = HorizontalAlignment.Center;
                Text.Tag = ("H" + Den + "_1");
                Text.PreviewKeyDown += AllowKeys;
                Text.KeyDown += ZpracujKlavesy;
                Text.Enter += PodbarviFocusRadku_Enter;
                Text.Validating += KontrolaHodin_Validating;
                ((TableLayoutPanel)((TableLayoutPanel)((Panel)(Formular.GetControlFromPosition(0, AktalniTyden - PrvniTyden))).Controls[0]).GetControlFromPosition(0, 4)).Controls.Add(Text, DenTydne, 0);
            }

            panel_Formular.Controls.Add(Formular);
            FocusTyden = 0;
            FocusRadek = 0;
        }

        private void ZpracujKlavesy(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Right)
            {
                SendKeys.Send("{TAB}");
            }
            if (e.KeyCode == Keys.Left)
            {
                SendKeys.Send("+{TAB}");
            }
            if (e.Control && (e.KeyCode == Keys.Add || e.KeyCode == Keys.Oemplus))
            {
                PridejRadek(sender);
            }
            if (e.Control && (e.KeyCode == Keys.Subtract || e.KeyCode == Keys.OemMinus))
            {
                ZrusRadek(sender);
            }
        }

        private void UzamciUzavreneObdobi(Int16 Rok, Int16 Mesic)
        {
            String SQL = "SELECT * FROM TabMzdObd WHERE Uzavreno < 3 AND Rok = " + Rok + " AND Mesic = " + Mesic;
            IHeQuery SQLKalendar = Helios.OpenSQL(SQL);
            if (SQLKalendar.EOF())
            {
                button_Ulozit.Enabled = false;
                button_Kopirovani.Enabled = false;
                button_UlozitAkce.Enabled = false;
                button_Smaz.Enabled = false;
                Uzamceno = true;
            }
            else
            {
                button_Ulozit.Enabled = true;
                button_Kopirovani.Enabled = true;
                button_UlozitAkce.Enabled = true;
                button_Smaz.Enabled = true;
                Uzamceno = false;
            }
        }

        private void VyberText_Enter(object sender, EventArgs e)
        {
            ((TextBox)sender).SelectAll();
        }

        private void KontrolaCinnosti_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            TextBox Cinnost = (TextBox)sender;
            if (Cinnost.Text == "")
                return;
            if (!Cinnosti.Contains(Cinnost.Text))
            {
                String CinnostU = Cinnost.Text.ToUpper();
                if (!Cinnosti.Contains(CinnostU))
                {
                    e.Cancel = true;
                    Cinnost.SelectAll();
                    Helios.Error("Vyberte validní činnost");
                    return;
                }
                else
                {
                    Cinnost.Text = CinnostU;
                }
            }
        }

        private void KontrolaZakazky_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            TextBox Zakazka = (TextBox)sender;
            if (Zakazka.Text == "")
                return;
            if (!Zakazky.Contains(Zakazka.Text))
            {
                e.Cancel = true;
                Zakazka.SelectAll();
                Helios.Error("Vyberte validní zakázku");
                return;
            }
            TextBox Stredisko;
            if (Zakazka.Name == "textBox_Zakazka")
                Stredisko = textBox_Stredisko;
            else
                Stredisko = (TextBox)((TableLayoutPanel)(Zakazka.Parent)).GetControlFromPosition(0, ZjistiRadek(Zakazka) - 1);
            if (Stredisko.Text.Length > 0)
            {
                /*if (Zakazka.Text.Substring(0, 1) != "0")
                {
                    Int32 Pozice = Zakazka.Text.IndexOf(".");
                    Pozice = Pozice == -1 ? Zakazka.Text.Length - 1 : Pozice - 1;
                    if ((Zakazka.Text.Substring(Pozice, 1)) != (Stredisko.Text.Substring(Stredisko.Text.Length - 1)))
                    {
                        //e.Cancel = true;
                        //Zakazka.SelectAll();
                        Helios.Info("Zařazení zakázky neodpovídá úseku");
                        //return;
                    }
                }*/
                KombinaceStruct Polozka = new KombinaceStruct();
                Polozka.Stredisko = Stredisko.Text;
                Polozka.Zakazka = Zakazka.Text;
                if (!Kombinace.Contains(Polozka))
                {
                    e.Cancel = true;
                    Zakazka.SelectAll();
                    Helios.Error("Zadejte existující kombinaci úseku a zakázky");
                    return;
                }
            }

        }

        private void KontrolaStrediska_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            TextBox Stredisko = (TextBox)sender;
            if (Stredisko.Text == "")
                return;
            if (Stredisko.Text.Length == 2)
                Stredisko.Text = "001000" + Stredisko.Text;
            if (!Strediska.Contains(Stredisko.Text))
            {
                e.Cancel = true;
                Stredisko.SelectAll();
                Helios.Error("Vyberte validní úsek");
                return;
            }
            TextBox Zakazka;
            if (Stredisko.Name == "textBox_Stredisko")
                Zakazka = textBox_Zakazka;
            else
                Zakazka = (TextBox)((TableLayoutPanel)(Stredisko.Parent)).GetControlFromPosition(1, ZjistiRadek(Stredisko) - 1);
            if (Zakazka.Text.Length > 0)
            {
                /*if (Zakazka.Text.Substring(0, 1) != "0")
                {
                    Int32 Pozice = Zakazka.Text.IndexOf(".");
                    Pozice = Pozice == -1 ? Zakazka.Text.Length - 1 : Pozice - 1;
                    if ((Zakazka.Text.Substring(Pozice, 1)) != (Stredisko.Text.Substring(Stredisko.Text.Length - 1)))
                    {
                        //e.Cancel = true;
                        //Stredisko.SelectAll();
                        Helios.Info("Zařazení zakázky neodpovídá úseku");
                        //return;
                    }
                }*/
                KombinaceStruct Polozka = new KombinaceStruct();
                Polozka.Stredisko = Stredisko.Text;
                Polozka.Zakazka = Zakazka.Text;
                if (!Kombinace.Contains(Polozka))
                {
                    e.Cancel = true;
                    Zakazka.SelectAll();
                    Helios.Error("Zadejte existující kombinaci úseku a zakázky");
                    return;
                }
            }
        }

        private void KontrolaHodin_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            TextBox Text = (TextBox)sender;
            if (Text.Text != "")
            {
                Double Delka;
                if (!Double.TryParse(Text.Text, out Delka))
                {
                    e.Cancel = true;
                    Text.SelectAll();
                    Helios.Error("Hodiny musí být číslo");
                    return;
                }
                if (!Hodiny.Contains(Delka))
                {
                    e.Cancel = true;
                    Text.SelectAll();
                    Helios.Error("Hodiny musí být v rozmezí " + Hodiny[0] + " - " + Hodiny[Hodiny.Count - 1]);
                    return;
                }
            }
            SectiHodiny(Text);
        }

        private void SectiHodiny(TextBox Text)
        {
            TableLayoutPanel DenTable = (TableLayoutPanel)Text.Parent;
            Int32 Sloupec = ZjistiSloupec(Text);
            Double HodinyD;
            Double HodinyC = 0;
            for (int i = 0; i < DenTable.RowCount; i++)
            {
                Text = (TextBox)DenTable.GetControlFromPosition(Sloupec, i);
                if (!Double.TryParse(Text.Text, out HodinyD)) HodinyD = 0;
                HodinyC += HodinyD;
            }
            DenTable = (TableLayoutPanel)((TableLayoutPanel)DenTable.Parent).GetControlFromPosition(0, 0);
            Label Popisek = (Label)DenTable.GetControlFromPosition(Sloupec, 0);
            Popisek.Text = Popisek.Tag.ToString();
            Popisek.ForeColor = System.Drawing.Color.Black;
            if (HodinyC != 0)
            {
                Popisek.Text = Popisek.Text + " (" + HodinyC.ToString() + ")";
                if (Sloupec < 7)
                {
                    if (HodinyC < 8)
                        Popisek.ForeColor = System.Drawing.Color.Red;
                    if (HodinyC == 8)
                        Popisek.ForeColor = System.Drawing.Color.Green;
                    if (HodinyC > 8)
                        Popisek.ForeColor = System.Drawing.Color.Blue;
                }
                else
                {
                    Popisek.ForeColor = System.Drawing.Color.Blue;
                }
            }
        }

        private Int32 ZjistiSloupec(Control control)
        {
            Int32 Radek = ZjistiRadek(control);
            TableLayoutPanel DenTable = (TableLayoutPanel)control.Parent;
            for (int i = 0; i < 9; i++)
            {
                if (DenTable.GetControlFromPosition(i, Radek - 1) != null && (DenTable.GetControlFromPosition(i, Radek - 1).Equals(control)))
                    return i;
            }
            return -1;
        }

        private void VyberCinnost_CtrlEnter(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.Enter)
            {
                VyberCinnost_DblClick(sender, e);
            }
        }

        private void VyberCinnost_DblClick(object sender, EventArgs e)
        {
            Object Cinnost = ((TextBox)sender).Text;
            Helios.Prenos(100128, "Cislo", ref Cinnost, "", "Vyberte činnost", true);
            ((TextBox)sender).Text = Cinnost.ToString();

        }

        private Int32 ZjistiDen(Control control)
        {
            String tag = control.Tag.ToString();
            Int32 Pozice = tag.IndexOf("_");
            return Int32.Parse(tag.Substring(1, Pozice - 1));
        }

        private Int32 ZjistiTyden(Control control)
        {
            return (Int32)(control.Parent.Parent.Tag);
        }
        
        private Int32 ZjistiRadek(Control control)
        {
            String tag = control.Tag.ToString();
            Int32 Pozice = tag.IndexOf("_");
            return Int32.Parse(tag.Substring(Pozice + 1, tag.Length - Pozice - 1));
        }

        private void ZapisRadek(Control control, Int32 radek)
        {
            String tag = control.Tag.ToString();
            control.Tag = tag.Substring(0, tag.IndexOf("_") + 1) + radek;
        }

        private void PridejRadek_Click(object sender, EventArgs e)
        {
            PridejRadek(sender);
        }

        private void PridejRadek(object sender)
        {
            Control control = (Control)sender;
            TableLayoutPanel DenTable = (TableLayoutPanel)(control.Parent);
            TableLayoutPanel TydenTable = (TableLayoutPanel)(DenTable.Parent);
            Int32 Tyden = (Int32)(TydenTable.Tag);
            Int32 Radek = PridejRadek(Tyden, "", "", null);
            ((TableLayoutPanel)TydenTable.GetControlFromPosition(0, 2)).GetControlFromPosition(0, Radek).Focus();
            //for (int i = 2; i < 9; i++)
            //{
            //    if (DenTable.GetControlFromPosition(i, Radek) != null)
            //    {
            //        DenTable.GetControlFromPosition(i, Radek).Focus();
            //        break;
            //    }
            //}
        }

        private Int32 PridejRadek(Int32 Tyden, String Stredisko, String Zakazka, Int32? DenTydne)
        {
            TableLayoutPanel Formular;
            Panel TydenPanel = null;
            TableLayoutPanel TydenTable;
            TableLayoutPanel DenTable;
            Button Tlacitko;
            Control control;
            TextBox Text;
            Int32 Radek = 0;
            ToolTip toolTip_CtrlEnter;

            Boolean skryto = !panel_Formular.Visible;
            if (!skryto)
                panel_Formular.Visible = false;
            PodbarviFocusRadku(0, 0);
            Formular = (TableLayoutPanel)(panel_Formular.Controls[0]);
            for (int i = 0; i < Formular.RowCount; i++)
            {
                if (Int32.Parse(((Panel)(Formular.GetControlFromPosition(0, i))).Tag.ToString()) == Tyden)
                    TydenPanel = (Panel)(Formular.GetControlFromPosition(0, i));
            }
            TydenTable = (TableLayoutPanel)(TydenPanel.Controls[0]);
            DenTable = (TableLayoutPanel)(TydenTable.GetControlFromPosition(0, 2));
            for (int i = 0; i < DenTable.RowCount; i++)
            {
                if (((TextBox)DenTable.GetControlFromPosition(0, i)).Text == "" && ((TextBox)DenTable.GetControlFromPosition(1, i)).Text == "")
                {
                    ((TextBox)DenTable.GetControlFromPosition(0, i)).Text = Stredisko;
                    ((TextBox)DenTable.GetControlFromPosition(1, i)).Text = Zakazka;
                    if (!skryto)
                        panel_Formular.Visible = true;
                    return i;
                }
                if (((TextBox)DenTable.GetControlFromPosition(0, i)).Text == Stredisko && ((TextBox)DenTable.GetControlFromPosition(1, i)).Text == Zakazka)
                {
                    if (DenTydne == null || (((TextBox)DenTable.GetControlFromPosition((Int32)DenTydne, i)).Text == ""))
                    {
                        if (!skryto)
                            panel_Formular.Visible = true;
                        return i;
                    }
                }
            }

            for (int j = 1; j <= 2; j++)
            {
                DenTable = (TableLayoutPanel)(TydenTable.GetControlFromPosition(0, j * 2));
                Radek = (Int32)(DenTable.Tag);
                DenTable.RowCount++;
                switch (j)
                {
                    case 1:
                        toolTip_CtrlEnter = new ToolTip();
                        Text = new TextBox();
                        Text.Dock = DockStyle.Top;
                        Text.Height = 16;
                        Text.TextAlign = HorizontalAlignment.Center;
                        Text.DoubleClick += VyberStrediska_DblClick;
                        Text.PreviewKeyDown += AllowKeys;
                        Text.KeyDown += VyberStredisko_CtrlEnter;
                        Text.KeyDown += ZpracujKlavesy;
                        Text.Tag = ("S" + Tyden + "_" + (Radek + 1));
                        Text.Validating += KontrolaStrediska_Validating;
                        Text.Enter += PodbarviFocusRadku_Enter;
                        toolTip_CtrlEnter.SetToolTip(Text, "CTRL+Enter = výběr úseku");
                        Text.Text = Stredisko;
                        DenTable.Controls.Add(Text, 0, Radek);
                        Text = new TextBox();
                        Text.Dock = DockStyle.Top;
                        Text.Height = 16;
                        Text.TextAlign = HorizontalAlignment.Center;
                        Text.DoubleClick += VyberZakazky_DblClick;
                        Text.PreviewKeyDown += AllowKeys;
                        Text.KeyDown += VyberZakazky_CtrlEnter;
                        Text.KeyDown += ZpracujKlavesy;
                        Text.Validating += KontrolaZakazky_Validating;
                        Text.Enter += PodbarviFocusRadku_Enter;
                        Text.Tag = ("Z" + Tyden + "_" + (Radek + 1));
                        toolTip_CtrlEnter.SetToolTip(Text, "CTRL+Enter = výběr zakázky");
                        Text.Text = Zakazka;
                        DenTable.Controls.Add(Text, 1, Radek);
                        break;
                    case 2:
                        Tlacitko = new Button();
                        Tlacitko.Text = "-";
                        Tlacitko.Width = 24;
                        Tlacitko.Height = 16;
                        Tlacitko.TabStop = false;
                        Tlacitko.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                        Tlacitko.Click += ZrusRadek_Click;
                        Tlacitko.Tag = ("B" + Tyden + "_" + (Radek + 1));
                        DenTable.Controls.Add(Tlacitko, 0, Radek);
                        break;
                    default:
                        break;
                }

                for (int i = 2; i < 9; i++)
                {
                    control = DenTable.GetControlFromPosition(i, 0);
                    if (control != null)
                    {
                        Text = new TextBox();
                        Text.Dock = DockStyle.Top;
                        Text.Height = 16;
                        Text.TextAlign = HorizontalAlignment.Center;
                        Text.Tag = (control.Tag.ToString().Substring(0, control.Tag.ToString().IndexOf("_") + 1) + (Radek + 1));
                        Text.PreviewKeyDown += AllowKeys;
                        if (j == 1)
                        {
                            toolTip_CtrlEnter = new ToolTip();
                            Text.DoubleClick += VyberCinnost_DblClick;
                            Text.KeyDown += VyberCinnost_CtrlEnter;
                            Text.Validating += KontrolaCinnosti_Validating;
                            toolTip_CtrlEnter.SetToolTip(Text, "CTRL+Enter = výběr činnosti");
                        }
                        if (j == 2)
                        {
                            Text.Validating += KontrolaHodin_Validating;
                        }
                        Text.KeyDown += ZpracujKlavesy;
                        Text.Enter += PodbarviFocusRadku_Enter;
                        DenTable.Controls.Add(Text, i, Radek);
                    }

                }
                DenTable.Tag = (Radek + 1);
            }
            if (!skryto)
                panel_Formular.Visible = true;
            return Radek;
        }

        private void ZrusRadek_Click(object sender, EventArgs e)
        {
            ZrusRadek(sender);
        }

        private void ZrusRadek(object sender)
        {
            Control control = (Control)sender;
            TableLayoutPanel DenTable = (TableLayoutPanel)(control.Parent);
            TableLayoutPanel TydenTable = (TableLayoutPanel)(DenTable.Parent);
            Int32 Tyden = (Int32)(TydenTable.Tag);
            Int32 Radek = ZjistiRadek(control);
            if (Radek == 1)
                return;
            ZrusRadek(Tyden, Radek);
            ((TableLayoutPanel)TydenTable.GetControlFromPosition(0, 2)).GetControlFromPosition(0, Radek - 2).Focus();

        }

        private void ZrusRadek(Int32 Tyden, Int32 Radek)
        {
            TableLayoutPanel Formular;
            Panel TydenPanel = null;
            TableLayoutPanel TydenTable;
            TableLayoutPanel DenTable;
            Control control;

            panel_Formular.Visible = false;
            PodbarviFocusRadku(0, 0);
            Formular = (TableLayoutPanel)(panel_Formular.Controls[0]);
            for (int i = 0; i < Formular.RowCount; i++)
            {
                if (Int32.Parse(((Panel)(Formular.GetControlFromPosition(0, i))).Tag.ToString()) == Tyden)
                    TydenPanel = (Panel)(Formular.GetControlFromPosition(0, i));
            }
            TydenTable = (TableLayoutPanel)(TydenPanel.Controls[0]);

            for (int j = 1; j <= 2; j++)
            {
                DenTable = (TableLayoutPanel)(TydenTable.GetControlFromPosition(0, j * 2));
                Int32 PocetRadku = (Int32)(DenTable.Tag);
                for (int i = 0; i < 9; i++)
                {
                    DenTable.Controls.Remove(DenTable.GetControlFromPosition(i, Radek - 1));
                }
                for (int r = Radek; r < PocetRadku; r++)
                {
                    for (int i = 0; i < 9; i++)
                    {
                        control = DenTable.GetControlFromPosition(i, r);
                        if (control != null)
                        {
                            DenTable.SetRow(DenTable.GetControlFromPosition(i, r), r - 1);
                            ZapisRadek(control, r);
                        }
                    }
                }
                DenTable.Tag = (PocetRadku - 1);
                DenTable.RowCount--;
            }
            panel_Formular.Visible = true;
        }

        private void AllowKeys(object sender, PreviewKeyDownEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    e.IsInputKey = true;
                    break;
                case Keys.Right:
                    e.IsInputKey = true;
                    break;
                case Keys.Left:
                    e.IsInputKey = true;
                    break;
                case Keys.Up:
                    e.IsInputKey = true;
                    break;
                case Keys.Down:
                    e.IsInputKey = true;
                    break;
            }
        }

        private void VyberStredisko_CtrlEnter(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.Enter)
            {
                VyberStrediska_DblClick(sender, e);
            }
        }

        private void VyberStrediska_DblClick(object sender, EventArgs e)
        {
            TextBox StrediskoTB = (TextBox)sender;
            TextBox ZakazkaTB;
            if (StrediskoTB.Name == "textBox_Stredisko")
                ZakazkaTB = textBox_Zakazka;
            else
                ZakazkaTB = (TextBox)((TableLayoutPanel)StrediskoTB.Parent).GetControlFromPosition(1,ZjistiRadek(StrediskoTB) - 1);
            String WHERE = ZakazkaTB.Text == "" ? "TabStrom.Cislo LIKE N'0010%'" : "TabStrom.Cislo IN (SELECT DISTINCT Stredisko FROM TabBKOSchvalovaniRozpis WHERE CisloZakazky = N'" + ZakazkaTB.Text + "')";
            Object Stredisko = StrediskoTB.Text;
            Helios.Prenos(4, "Cislo",ref Stredisko, WHERE, "Vyberte úsek", true);
            ((TextBox)sender).Text = Stredisko.ToString();
        }


        private void VyberZakazky_CtrlEnter(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.Enter)
            {
                VyberZakazky_DblClick(sender, e);
            }
        }

        private void VyberZakazky_DblClick(object sender, EventArgs e)
        {
            TextBox ZakazkaTB = (TextBox)sender;
            TextBox StrediskoTB;
            if (ZakazkaTB.Name == "textBox_Zakazka")
                StrediskoTB = textBox_Stredisko;
            else
                StrediskoTB = (TextBox)((TableLayoutPanel)ZakazkaTB.Parent).GetControlFromPosition(0, ZjistiRadek(ZakazkaTB) - 1);
            String WHERE = StrediskoTB.Text == "" ? "TabZakazka.NadrizenaZak IS NULL" : "TabZakazka.NadrizenaZak IS NULL AND TabZakazka.CisloZakazky IN (SELECT DISTINCT CisloZakazky FROM TabBKOSchvalovaniRozpis WHERE Stredisko = N'" + StrediskoTB.Text + "')";
            Object Zakazka = ((TextBox)sender).Text;
            Helios.Prenos(59, "CisloZakazky", ref Zakazka, WHERE, "Vyberte zakázku", true);
            ((TextBox)sender).Text = Zakazka.ToString();
        }

        private void button_PridejRadky_Click(object sender, EventArgs e)
        {
            Int16 Rok = (Int16)numericUpDown_Rok.Value;
            Int16 Mesic = (Int16)numericUpDown_Mesic.Value;
            Calendar Kalendar = new CultureInfo("cs-CZ").Calendar;
            Int32 PrvniTyden = Kalendar.GetWeekOfYear(new DateTime(Rok, Mesic, 1), CalendarWeekRule.FirstDay, DayOfWeek.Monday);
            Int32 PosledniTyden = Kalendar.GetWeekOfYear(new DateTime(Rok, Mesic, DateTime.DaysInMonth(Rok, Mesic)), CalendarWeekRule.FirstDay, DayOfWeek.Monday);
            String Stredisko = textBox_Stredisko.Text;
            String Zakazka = textBox_Zakazka.Text;
            panel_Formular.Visible = false;
            for (int i = PrvniTyden; i <= PosledniTyden; i++)
            {
                PridejRadek(i, Stredisko, Zakazka, null);
            }
            panel_Formular.Visible = true;
        }
        
        private void PodbarviFocusRadku_Enter(object sender, EventArgs e)
        {
            TextBox Text = (TextBox)sender;
            Text.SelectAll();
            Int32 Radek = ZjistiRadek(Text);
            Int32 Tyden = ZjistiTyden(Text);
            PodbarviFocusRadku(Tyden, Radek);
        }
        
        private void PodbarviFocusRadku(Int32 Tyden, Int32 Radek)
        {
            if ((Tyden == FocusRadek) && (Radek == FocusRadek))
                return;

            TableLayoutPanel Formular;
            Panel TydenPanel = null;
            TableLayoutPanel TydenTable;
            TableLayoutPanel DenTable;
            Control control;

            if ((FocusTyden != 0) && (FocusRadek != 0))
            {
                Formular = (TableLayoutPanel)(panel_Formular.Controls[0]);
                for (int i = 0; i < Formular.RowCount; i++)
                {
                    if (Int32.Parse(((Panel)(Formular.GetControlFromPosition(0, i))).Tag.ToString()) == FocusTyden)
                        TydenPanel = (Panel)(Formular.GetControlFromPosition(0, i));
                }
                TydenTable = (TableLayoutPanel)(TydenPanel.Controls[0]);
                for (int j = 1; j <= 2; j++)
                {
                    DenTable = (TableLayoutPanel)(TydenTable.GetControlFromPosition(0, j * 2));
                    for (int i = 0; i < 9; i++)
                    {
                        if (j != 2 || i > 1)
                        {
                            control = DenTable.GetControlFromPosition(i, FocusRadek - 1);
                            if (control != null)
                                control.BackColor = System.Drawing.SystemColors.Window;
                        }
                    }
                }
            }
            if ((Tyden != 0) && (Radek != 0))
            {
                Formular = (TableLayoutPanel)(panel_Formular.Controls[0]);
                for (int i = 0; i < Formular.RowCount; i++)
                {
                    if (Int32.Parse(((Panel)(Formular.GetControlFromPosition(0, i))).Tag.ToString()) == Tyden)
                        TydenPanel = (Panel)(Formular.GetControlFromPosition(0, i));
                }
                TydenTable = (TableLayoutPanel)(TydenPanel.Controls[0]);
                for (int j = 1; j <= 2; j++)
                {
                    DenTable = (TableLayoutPanel)(TydenTable.GetControlFromPosition(0, j * 2));
                    for (int i = 0; i < 9; i++)
                    {
                        if (j != 2 || i > 1)
                        {
                            control = DenTable.GetControlFromPosition(i, Radek - 1);
                            if (control != null)
                                control.BackColor = System.Drawing.Color.PaleGreen;
                        }
                    }
                }
            }
            FocusTyden = Tyden;
            FocusRadek = Radek;
        }

        private void textBox_Vyhledavani_TextChanged(object sender, EventArgs e)
        {
            if (Vyhledavam)
                timer.Dispose();
            timer = new System.Threading.Timer(obj =>
            {
                timer.Dispose();
                Vyhledavam = false; 
                VyhledejZamestnance();
            }, null, 1000, System.Threading.Timeout.Infinite);
            Vyhledavam = true;
        }

        private void VyhledejZamestnance()
        {

            String Hledej = textBox_Vyhledavani.Text.ToLower();

            if (Hledej == "")
                return;

            treeView_Zamestnanci.CollapseAll();
            foreach (TreeNode Root in treeView_Zamestnanci.Nodes)
            {
                foreach (TreeNode Stredisko in Root.Nodes)
                {
                    if (Stredisko.Text.ToLower().Contains(Hledej))
                    {
                        treeView_Zamestnanci.HideSelection = false;
                        treeView_Zamestnanci.SelectedNode = Stredisko;
                        return;
                    }
                    foreach (TreeNode Zakazka in Stredisko.Nodes)
                    {
                        if (Zakazka.Text.ToLower().Contains(Hledej))
                        {
                            treeView_Zamestnanci.HideSelection = false;
                            treeView_Zamestnanci.SelectedNode = Zakazka;
                            return;
                        }
                        foreach (TreeNode Zamestnanec in Zakazka.Nodes)
                        {
                            if (Zamestnanec.Text.ToLower().Contains(Hledej))
                            {
                                treeView_Zamestnanci.HideSelection = false;
                                treeView_Zamestnanci.SelectedNode = Zamestnanec;
                                return;
                            }
                        }
                    }
                }
            }
        }

        private void NajdiZamestnance()
        {
            String SQL = "SELECT PrijmeniJmeno FROM TabCisZam WHERE ID = " + ID;
            IHeQuery Zamestnanec = Helios.OpenSQL(SQL);
            textBox_Vyhledavani.Text = Zamestnanec.FieldValues(0);
        }

        private void PripravValidaci()
        {
            String SQL;

            Hodiny = new List<Double>();
            for (Double i = 1; i <= 24; i++)
            {
                Hodiny.Add(i / 2);
            }

            Strediska = new List<String>();
            SQL = "SELECT Cislo FROM TabStrom WHERE Cislo LIKE N'0010%'";
            IHeQuery SQLStrediska = Helios.OpenSQL(SQL);
            while (!SQLStrediska.EOF())
            {
                Strediska.Add(SQLStrediska.FieldValues(0));
                SQLStrediska.Next();
            }

            Zakazky = new List<String>();
            SQL = "SELECT CisloZakazky FROM TabZakazka";
            IHeQuery SQLZakazky = Helios.OpenSQL(SQL);
            while (!SQLZakazky.EOF())
            {
                Zakazky.Add(SQLZakazky.FieldValues(0));
                SQLZakazky.Next();
            }

            Cinnosti = new List<String>();
            CinnostStruct Cinnost = new CinnostStruct();
            CinnostiHodiny = new List<CinnostStruct>();
            SQL = "SELECT Cislo, NULL AS Hodiny, 0 AS Smena FROM TabNakladovyOkruh UNION SELECT Cislo, Hodiny, Smena FROM TabEGDochazkaCinnosti";
            IHeQuery SQLCinnosti = Helios.OpenSQL(SQL);
            while (!SQLCinnosti.EOF())
            {
                Cinnost.Cislo = SQLCinnosti.FieldValues(0);
                Cinnost.Hodiny = (SQLCinnosti.FieldValues(1) is DBNull) ? null : SQLCinnosti.FieldValues(1);
                Cinnost.Smena = SQLCinnosti.FieldValues(2) == 0 ? false : true;
                Cinnosti.Add(Cinnost.Cislo);
                //if (Cinnost.Hodiny != null)
                    CinnostiHodiny.Add(Cinnost);
                SQLCinnosti.Next();
            }

            DoplnkoveCinnosti = new List<string>();
            SQL = "SELECT Cislo FROM TabEGDochazkaCinnosti";
            IHeQuery SQLDoplnkoveCinnosti = Helios.OpenSQL(SQL);
            while (!SQLDoplnkoveCinnosti.EOF())
            {
                DoplnkoveCinnosti.Add(SQLDoplnkoveCinnosti.FieldValues(0));
                SQLDoplnkoveCinnosti.Next();
            }

            Kombinace = new List<KombinaceStruct>();
            KombinaceStruct Polozka = new KombinaceStruct();
            SQL = "SELECT Stredisko, CisloZakazky FROM TabBKOSchvalovaniRozpis";
            IHeQuery SQLKombinace = Helios.OpenSQL(SQL);
            while (!SQLKombinace.EOF())
            {
                Polozka.Stredisko = SQLKombinace.FieldValues(0);
                Polozka.Zakazka = SQLKombinace.FieldValues(1);
                Kombinace.Add(Polozka);
                SQLKombinace.Next();
            }

        }

        private void button_Ulozit_Click(object sender, EventArgs e)
        {
           if (UlozitDochazku())
               Helios.Info("Docházka uložena");
           else
               Helios.Error("Opravte chyby");
        }

        private Boolean UlozitDochazku()
        {
            if (Uzamceno)
                return false;
            Boolean Result;
            TableLayoutPanel Formular = (TableLayoutPanel)(panel_Formular.Controls[0]);
            TableLayoutPanel TydenTable;
            TableLayoutPanel CinnostiTable;
            TableLayoutPanel HodinyTable;
            Boolean Ulozit;
            Boolean Preruseno = false;
            Int32 Rok = (Int32)numericUpDown_Rok.Value;
            Int32 Mesic = (Int32)numericUpDown_Mesic.Value;
            Int32 Tyden;
            Int32 Den;
            Double Hodiny;
            TextBox Stredisko;
            TextBox Zakazka;
            Control control;
            TextBox Cinnost;
            TextBox Delka;
            String Error;
            List<String> Errors = new List<String>();
            String Info;
            List<String> Infos = new List<String>();
            Boolean ChybaTyden;
            DochazkaStruct DochazkaPolozka = new DochazkaStruct();
            Int32 CisloZam = 0;
            String SQL;
            Double HodinyCelkem;
            Boolean Smena;

            Result = false;
            if (DochazkaDny == null)
                DochazkaDny = new List<DochazkaStruct>();
            DochazkaDny.Clear();
            if ((treeView_Zamestnanci.SelectedNode != null) && (!Int32.TryParse(treeView_Zamestnanci.SelectedNode.Name, out CisloZam)))
            {
                Helios.Error("Vyberte zaměstnance");
                return Result;
            }
            foreach (Control TydenPanel in Formular.Controls)
            {
                TydenTable = (TableLayoutPanel)(((Panel)(TydenPanel)).Controls[0]);
                Tyden = (Int32)(TydenTable.Tag);
                ChybaTyden = false;
                CinnostiTable = (TableLayoutPanel)(TydenTable.GetControlFromPosition(0, 2));
                HodinyTable = (TableLayoutPanel)(TydenTable.GetControlFromPosition(0, 4));
                for (int j = 2; j < 9; j++)
                {
                    HodinyCelkem = 0;
                    Smena = false;
                    Den = CinnostiTable.GetControlFromPosition(j, 0) == null ? 0 : ZjistiDen(CinnostiTable.GetControlFromPosition(j, 0));
                    for (int i = 0; i < CinnostiTable.RowCount; i++)
                    {
                        Stredisko = (TextBox)(CinnostiTable.GetControlFromPosition(0, i));
                        Zakazka = (TextBox)(CinnostiTable.GetControlFromPosition(1, i));
                        control = CinnostiTable.GetControlFromPosition(j, i);
                        //Den = control == null ? 0 : ZjistiDen(control);
                        if (control != null)
                        {
                            Ulozit = true;
                            Cinnost = (TextBox)control;
                            Delka = (TextBox)(HodinyTable.GetControlFromPosition(j, i));
                            if ((Cinnost.Text.Length != 0) ^ (Delka.Text.Length != 0))
                            {
                                if (Cinnost.Text.Length == 0)
                                {
                                    Error = "Pro " + Den + "." + Mesic + "." + Rok + " není zadána činnost";
                                    Errors.Add(Error);
                                    Ulozit = false;
                                    Preruseno = true;
                                }
                                else if (ZjistiHodiny(Cinnost.Text, out Hodiny))
                                {
                                    Delka.Text = Hodiny.ToString();
                                    Info = "Pro " + Den + "." + Mesic + "." + Rok + " byly pro činnost " + Cinnost.Text + " nastaveny hodiny na " + Hodiny;
                                    Infos.Add(Info);
                                    Errors.Add(Info);
                                }
                                else
                                {
                                    Error = "Pro " + Den + "." + Mesic + "." + Rok + " nejsou zadány hodiny";
                                    Errors.Add(Error);
                                    Ulozit = false;
                                    Preruseno = true;
                                }
                            }
                            if ((Cinnost.Text.Length != 0))
                            {
                                if ((Stredisko.Text.Length == 0) && (!ChybaTyden))
                                {
                                    Error = "Pro " + Tyden + ". týden není zadán úsek";
                                    Errors.Add(Error);
                                    Ulozit = false;
                                    Preruseno = true;
                                    ChybaTyden = true;
                                }
                                if ((Zakazka.Text.Length == 0) && (!ChybaTyden))
                                {
                                    Error = "Pro " + Tyden + ". týden není zadána zakázka";
                                    Errors.Add(Error);
                                    Ulozit = false;
                                    Preruseno = true;
                                    ChybaTyden = true;
                                }
                                if (Ulozit)
                                {
                                    if (ZjistiHodiny(Cinnost.Text, out Hodiny))
                                    {
                                        if (Delka.Text != Hodiny.ToString())
                                        {
                                            Delka.Text = Hodiny.ToString();
                                            Info = "Pro " + Den + "." + Mesic + "." + Rok + " byly pro činnost " + Cinnost.Text + " nastaveny hodiny na " + Hodiny;
                                            Infos.Add(Info);
                                            Errors.Add(Info);
                                        }
                                    }
                                    DochazkaPolozka.CisloZam = CisloZam;
                                    DochazkaPolozka.Datum = new DateTime(Rok, Mesic, Den);
                                    DochazkaPolozka.Stredisko = Stredisko.Text;
                                    DochazkaPolozka.Zakazka = Zakazka.Text;
                                    DochazkaPolozka.Cinnost = Cinnost.Text;
                                    DochazkaPolozka.Hodiny = Double.Parse(Delka.Text);
                                    DochazkaDny.Add(DochazkaPolozka);
                                    HodinyCelkem += DochazkaPolozka.Hodiny;
                                    if (!Smena && ZjistiSmenu(DochazkaPolozka.Cinnost))
                                        Smena = true;
                                }
                            }
                        }
                    }
                    if (j < 7 && Den != 0)
                    {
                        if (HodinyCelkem == 0)
                        {
                            Info = "Pro " + Den + "." + Mesic + "." + Rok + " není zadána docházka";
                            Infos.Add(Info);
                        }
                        else if (HodinyCelkem < 8)
                        {
                            Error = "Pro " + Den + "." + Mesic + "." + Rok + " je zadáno méně jak 8 hodin";
                            Errors.Add(Error);
                            Preruseno = true;
                        }
                    }
                    if (Smena && HodinyCelkem != 8)
                    {
                        Error = "Pro " + Den + "." + Mesic + "." + Rok + " musí být zadáno přesně 8 hodin";
                        Errors.Add(Error);
                        Preruseno = true;
                    }
                }
            }
            if (Preruseno)
            {
                if (Chyby == null)
                {
                    Chyby = new ChybovaHlaseni();
                    Chyby.Text = "Docházka neuložena";
                    Chyby.label_Nadpis.Text = "Seznam chyb:";
                }
                Chyby.textBox_Chyby.Clear();
                foreach (String item in Errors)
                {
                    Chyby.textBox_Chyby.Text += item + Environment.NewLine;
                }
                Chyby.ShowDialog();
                Result = false;
            }
            else
            {
                if (Infos.Count > 1)
                {
                    if (Informace == null)
                    {
                        Informace = new ChybovaHlaseni();
                        Informace.Text = "Docházka uložena";
                        Informace.label_Nadpis.Text = "Seznam varování:";
                    }
                    Informace.textBox_Chyby.Clear();
                    foreach (String item in Infos)
                    {
                        Informace.textBox_Chyby.Text += item + Environment.NewLine;
                    }
                    Informace.ShowDialog();
                }
                SQL = "DELETE FROM TabEGDochazkaDny WHERE CisloZam = " + CisloZam + " AND YEAR(Datum) = " + Rok + " AND Month(Datum) = " + Mesic;
                Helios.ExecSQL(SQL);
                foreach (DochazkaStruct item in DochazkaDny)
                {
                    SQL = "INSERT INTO TabEGDochazkaDny (CisloZam, Datum, Stredisko, Zakazka, Cinnost, Hodiny) VALUES (";
                    SQL += item.CisloZam + ", '";
                    SQL += item.Datum.Year + "-" + item.Datum.Day + "-" + item.Datum.Month + "', '";
                    SQL += item.Stredisko + "', '";
                    SQL += item.Zakazka + "', '";
                    SQL += item.Cinnost + "', ";
                    SQL += item.Hodiny.ToString().Replace(",", ".") + ")";
                    Helios.ExecSQL(SQL);
                }
                SQL = "EXEC dbo.EGDochazkaMesicAktualizuj " + CisloZam + ", " + Rok + ", " + Mesic;
                Helios.ExecSQL(SQL);
                NaplnUpravenouDochazku(CisloZam, Rok, Mesic);
                Result = true;
            }
            return Result;
        }

        private Boolean ZjistiHodiny(String Cinnost, out Double Hodiny)
        {
            Boolean Result = false;
            Hodiny = -1;
            foreach (CinnostStruct item in CinnostiHodiny)
            {
                if (item.Cislo == Cinnost)
                {
                    if (item.Hodiny != null)
                    {
                        Hodiny = (Double)item.Hodiny;
                        Result = true;
                    }
                    return Result;
                }
            }
            return Result;
        }

        private Boolean ZjistiSmenu(String Cinnost)
        {
            Boolean Result = false;
            foreach (CinnostStruct item in CinnostiHodiny)
            {
                if (item.Cislo == Cinnost)
                {
                    Result = item.Smena;
                    return Result;
                }
            }
            return Result;
        }
        
        private void treeView_Zamestnanci_AfterSelect(object sender, TreeViewEventArgs e)
        {
            //Int32 CisloZam;
            //if (!Int32.TryParse(treeView_Zamestnanci.SelectedNode.Name, out CisloZam))
            //    return;
            //textBox_Vyhledavani.Text = treeView_Zamestnanci.SelectedNode.Text;
            switch (tabControlZadavani.SelectedIndex)
            {
                case 0:
                    NactiDochazku();
                    break;
                case 1:
                    NaplnZakazky();
                    break;
                default:
                    break;
            }
        }

        private void NactiDochazku()
        {
            if (panel_Formular.Controls[0] == null)
                return;

            Int32 CisloZam = 0;
            if ((treeView_Zamestnanci.SelectedNode != null) && (!Int32.TryParse(treeView_Zamestnanci.SelectedNode.Name, out CisloZam)))
                return;
            //textBox_Vyhledavani.Text = treeView_Zamestnanci.SelectedNode.Text;
            this.Text = "Docházka - " + treeView_Zamestnanci.SelectedNode.Text;
            panel_Formular.Visible = false;
            PripravFormularMesic();
            String SQL = "SELECT CisloZam, Datum, Stredisko, Zakazka, Cinnost, SUM(Hodiny) FROM TabEGDochazkaDny WHERE CisloZam = " + CisloZam + " AND YEAR(Datum) = " + numericUpDown_Rok.Value + " AND Month(Datum) = " + numericUpDown_Mesic.Value + " GROUP BY CisloZam, Datum, Stredisko, Zakazka, Cinnost ORDER BY Datum, Stredisko, Zakazka, Cinnost";
            IHeQuery SQLDochazka = Helios.OpenSQL(SQL);
            if (DochazkaDny == null)
                DochazkaDny = new List<DochazkaStruct>();
            DochazkaDny.Clear();
            DochazkaStruct Polozka = new DochazkaStruct();
            while (!SQLDochazka.EOF())
            {
                Polozka.CisloZam = SQLDochazka.FieldValues(0);
                Polozka.Datum = SQLDochazka.FieldValues(1);
                Polozka.Stredisko = SQLDochazka.FieldValues(2);
                Polozka.Zakazka = SQLDochazka.FieldValues(3);
                Polozka.Cinnost = SQLDochazka.FieldValues(4);
                Polozka.Hodiny = SQLDochazka.FieldValues(5);
                DochazkaDny.Add(Polozka);
                SQLDochazka.Next();
            }
            String Stredisko = "";
            String Zakazka = "";
            Calendar Kalendar = new CultureInfo("cs-CZ").Calendar;
            Int32 PrvniTyden = Kalendar.GetWeekOfYear(new DateTime((int)numericUpDown_Rok.Value, (int)numericUpDown_Mesic.Value, 1), CalendarWeekRule.FirstDay, DayOfWeek.Monday);
            Int32 Tyden = 0;
            Int32 AktualniTyden;
            Int32 DenTydne;
            Int32 Radek = 0;
            TableLayoutPanel TydenTable = null;
            foreach (DochazkaStruct item in DochazkaDny)
            {
                //if (PrvniTyden == 0)
                //    PrvniTyden = Kalendar.GetWeekOfYear(item.Datum, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
                AktualniTyden = Kalendar.GetWeekOfYear(item.Datum, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
                DenTydne = (Int32)Kalendar.GetDayOfWeek(item.Datum);
                DenTydne = DenTydne == 0 ? 8 : DenTydne + 1; 
                if (Tyden != AktualniTyden)
                {
                    Stredisko = item.Stredisko;
                    Zakazka = item.Zakazka;
                    Tyden = AktualniTyden;
                    Radek = 0;
                    TydenTable = (TableLayoutPanel)(((Panel)(((TableLayoutPanel)(panel_Formular.Controls[0])).GetControlFromPosition(0, AktualniTyden - PrvniTyden))).Controls[0]);
                    ((TextBox)(((TableLayoutPanel)(TydenTable.GetControlFromPosition(0, 2))).GetControlFromPosition(0, Radek))).Text = item.Stredisko;
                    ((TextBox)(((TableLayoutPanel)(TydenTable.GetControlFromPosition(0, 2))).GetControlFromPosition(1, Radek))).Text = item.Zakazka;
                }
                Stredisko = item.Stredisko;
                Zakazka = item.Zakazka;
                Radek = PridejRadek(AktualniTyden, Stredisko, Zakazka, DenTydne);
                ((TextBox)(((TableLayoutPanel)(TydenTable.GetControlFromPosition(0, 2))).GetControlFromPosition(DenTydne, Radek))).Text = item.Cinnost;
                ((TextBox)(((TableLayoutPanel)(TydenTable.GetControlFromPosition(0, 4))).GetControlFromPosition(DenTydne, Radek))).Text = item.Hodiny.ToString();
                SectiHodiny(((TextBox)(((TableLayoutPanel)(TydenTable.GetControlFromPosition(0, 4))).GetControlFromPosition(DenTydne, Radek))));
            }
            panel_Formular.Visible = true;
        }

        private void button_Kopirovani_Click(object sender, EventArgs e)
        {
            if (comboBox_Kopirovani.SelectedItem == null)
            {
                return;
            }
            Int32 CisloZam;
            if (!Int32.TryParse(treeView_Zamestnanci.SelectedNode.Name, out CisloZam))
            {
                Helios.Error("Vyberte zaměstnance, do kterého budete kopírovat docházku");
                return;
            }
            Int16 Rok = (Int16)numericUpDown_Rok.Value;
            Int16 Mesic = (Int16)numericUpDown_Mesic.Value;
            Int32 CisloZamProKopirovani = ((ZamestnanciItem)comboBox_Kopirovani.SelectedItem).Cislo;
            String Zamestnanec = ((ZamestnanciItem)comboBox_Kopirovani.SelectedItem).Zamestnanec;
            if (!Helios.YesNo("Opravdu chcete zkopírovat docházku od zaměstnance: " + Zamestnanec + "?", true))
                return;
            String SQL = "DELETE FROM TabEGDochazkaDny WHERE YEAR(Datum) = " + Rok + " AND Month(Datum) = " + Mesic + " AND CisloZam = " + CisloZam;
            Helios.ExecSQL(SQL);
            SQL = "INSERT INTO TabEGDochazkaDny (CisloZam, Datum, Stredisko, Zakazka, Cinnost, Hodiny) SELECT " + CisloZam + ", Datum, Stredisko, Zakazka, Cinnost, Hodiny FROM TabEGDochazkaDny WHERE YEAR(Datum) = " + Rok + " AND Month(Datum) = " + Mesic + " AND CisloZam = " + CisloZamProKopirovani;
            Helios.ExecSQL(SQL);
            SQL = "EXEC dbo.EGDochazkaMesicAktualizuj " + CisloZam + ", " + Rok + ", " + Mesic;
            Helios.ExecSQL(SQL);
            NaplnUpravenouDochazku(CisloZam, Rok, Mesic);
            NactiDochazku();
        }

        private void NaplnZakazky()
        {
            Int16 Rok = (Int16)numericUpDown_Rok.Value;
            Int16 Mesic = (Int16)numericUpDown_Mesic.Value;
            UzamciUzavreneObdobi(Rok, Mesic);
            if (treeView_Zamestnanci.SelectedNode == null)
                return;
            String Node = treeView_Zamestnanci.SelectedNode.Name;
            this.Text = "Doplňkové informace - " + treeView_Zamestnanci.SelectedNode.Text;
            Int32 CisloZam = 0;
            String Stredisko = "";
            String Zakazka = "";

            switch (Node[0])
            {
                case 'E':
                    break;
                case 'S':
                    Stredisko = Node.Substring(1);
                    break;
                case 'Z':
                    Zakazka = Node.Substring(1);
                    break;
                default:
                    Int32.TryParse(Node, out CisloZam);
                    break;
            }
            String SQL = "SELECT Polozka FROM dbo.EGDochazkaZakazky(" + Rok + ", " + Mesic + ", " + CisloZam + ", '" + Stredisko + "', '" + Zakazka + "')";
            IHeQuery Polozky = Helios.OpenSQL(SQL);
            comboBox_Zakazky.Items.Clear();
            while (!Polozky.EOF())
            {
                comboBox_Zakazky.Items.Add(Polozky.FieldValues(0));
                Polozky.Next();
            }
            comboBox_Zakazky.SelectedIndex = 0;
            panel_FormAkce.Controls.Clear();

        }

        private void tabControlZadavani_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!Ulozit)
                return;
            Boolean Provest = true;
            switch (tabControlZadavani.SelectedIndex)
            {
                case 0:
                    if (!Uzamceno && Helios.YesNo("Uložit doplňkové informace?", true))
                    {
                        if (!UlozitAkce())
                        {
                            Helios.Error("Doplňkové informace neuloženy");
                            Provest = false;
                        }
                    }
                    if (!Provest)
                    {
                        Ulozit = false;
                        tabControlZadavani.SelectedIndex = 0;
                        Ulozit = true;
                    }
                    else
                        NactiDochazku();
                    break;
                case 1:
                    if (!Uzamceno && Helios.YesNo("Uložit rozpracovanou docházku?", true))
                    {
                        if (!UlozitDochazku())
                        {
                            Helios.Error("Docházka neuložena");
                            Provest = false;
                        }
                    }
                    if (!Provest)
                    {
                        Ulozit = false;
                        tabControlZadavani.SelectedIndex = 0;
                        Ulozit = true;
                    }
                    else
                        NaplnZakazky();
                    break;
                default:
                    break;
            }
        }

        private void NactiAkce()
        {
            if (Postup == null)
                Postup = new Progress();
            Postup.Show();
            Postup.label_Popis.Text = "Načítají se doplňkové informace";
            Postup.label_Prvek.Text = "";
            Postup.progressBar_Postup.Value = 0;
            Postup.Show();
            if (treeView_Zamestnanci.SelectedNode == null)
                return;
            if (comboBox_Zakazky.SelectedIndex == -1)
                return;
            Int16 Rok = (Int16)numericUpDown_Rok.Value;
            Int16 Mesic = (Int16)numericUpDown_Mesic.Value;
            String Node = treeView_Zamestnanci.SelectedNode.Name;
            Int32 CisloZam = 0;
            String Stredisko = "";
            String Zakazka = "";

            switch (Node[0])
            {
                case 'E':
                    break;
                case 'S':
                    Stredisko = Node.Substring(1);
                    break;
                case 'Z':
                    Zakazka = Node.Substring(1);
                    break;
                default:
                    Int32.TryParse(Node, out CisloZam);
                    break;
            }

            String StrediskoDochazka = "";
            String ZakazkaDochazka = "";
            String Oznaceni = comboBox_Zakazky.SelectedItem.ToString();
            if (Oznaceni[0] != '<')
            {
                StrediskoDochazka = Oznaceni.Substring(0, Oznaceni.IndexOf("-"));
                ZakazkaDochazka = Oznaceni.Substring(Oznaceni.IndexOf("-") + 1, Oznaceni.IndexOf(":") - Oznaceni.IndexOf("-") - 1);
            }

            panel_FormAkce.Controls.Clear();
            TableLayoutPanel AkceTable = new TableLayoutPanel();
            AkceTable.AutoSize = true;
            AkceTable.Height = 0;
            AkceTable.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            AkceTable.RowCount++;
            AkceTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 16));
            AkceTable.ColumnCount++;
            AkceTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100));
            AkceTable.ColumnCount++;
            AkceTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120));
            AkceTable.ColumnCount++;
            AkceTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 40));
            AkceTable.ColumnCount++;
            AkceTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 40));
            AkceTable.ColumnCount++;
            AkceTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 40));
            AkceTable.ColumnCount++;
            AkceTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 40));
            AkceTable.ColumnCount++;
            AkceTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 40));
            AkceTable.ColumnCount++;
            AkceTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 60));
            AkceTable.ColumnCount++;
            AkceTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 40));
            AkceTable.ColumnCount++;
            AkceTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 40));
            AkceTable.ColumnCount++;
            AkceTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 40));
            AkceTable.ColumnCount++;
            AkceTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 40));
            Label Popisek;
            ToolTip Tooltip;
            TextBox Text;
            Popisek = new Label();
            Popisek.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            Tooltip = new ToolTip();
            Tooltip.SetToolTip(Popisek, "Zaměstnanec");
            Popisek.Text = "Zaměstnanec";
            Popisek.Font = new System.Drawing.Font(Popisek.Font, System.Drawing.FontStyle.Bold);
            AkceTable.Controls.Add(Popisek, 0, 0);
            Popisek = new Label();
            Popisek.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            Tooltip = new ToolTip();
            Tooltip.SetToolTip(Popisek, "Středisko a zakázka");
            Popisek.Text = "Zakázka";
            Popisek.Font = new System.Drawing.Font(Popisek.Font, System.Drawing.FontStyle.Bold);
            AkceTable.Controls.Add(Popisek, 1, 0);
            Popisek = new Label();
            Popisek.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            Tooltip = new ToolTip();
            Tooltip.SetToolTip(Popisek, "Hodiny celkem");
            Popisek.Text = "H(C)";
            Popisek.Font = new System.Drawing.Font(Popisek.Font, System.Drawing.FontStyle.Bold);
            AkceTable.Controls.Add(Popisek, 2, 0);
            Popisek = new Label();
            Popisek.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            Tooltip = new ToolTip();
            Tooltip.SetToolTip(Popisek, "Hodiny na zakázce");
            Popisek.Text = "H(Z)";
            Popisek.Font = new System.Drawing.Font(Popisek.Font, System.Drawing.FontStyle.Bold);
            AkceTable.Controls.Add(Popisek, 3, 0);
            Popisek = new Label();
            Popisek.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            Tooltip = new ToolTip();
            Tooltip.SetToolTip(Popisek, "Přesčasy celkem");
            Popisek.Text = "P(C)";
            Popisek.Font = new System.Drawing.Font(Popisek.Font, System.Drawing.FontStyle.Bold);
            AkceTable.Controls.Add(Popisek, 4, 0);
            Popisek = new Label();
            Popisek.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            Tooltip = new ToolTip();
            Tooltip.SetToolTip(Popisek, "Přesčasy větší než 32 hodin");
            Popisek.Text = "P(32)";
            Popisek.Font = new System.Drawing.Font(Popisek.Font, System.Drawing.FontStyle.Bold);
            AkceTable.Controls.Add(Popisek, 5, 0);
            Popisek = new Label();
            Popisek.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            Tooltip = new ToolTip();
            Tooltip.SetToolTip(Popisek, "Procento přesčasů nad 32 hodin ze základní odpracované doby");
            Popisek.Text = "P(%)";
            Popisek.Font = new System.Drawing.Font(Popisek.Font, System.Drawing.FontStyle.Bold);
            AkceTable.Controls.Add(Popisek, 6, 0);
            Popisek = new Label();
            Popisek.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            Tooltip = new ToolTip();
            Tooltip.SetToolTip(Popisek, "Odměny v Kč");
            Popisek.Text = "O(Kč)";
            Popisek.Font = new System.Drawing.Font(Popisek.Font, System.Drawing.FontStyle.Bold);
            AkceTable.Controls.Add(Popisek, 7, 0);
            Popisek = new Label();
            Popisek.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            Tooltip = new ToolTip();
            Tooltip.SetToolTip(Popisek, "Diety ve dnech");
            Popisek.Text = "D(d)";
            Popisek.Font = new System.Drawing.Font(Popisek.Font, System.Drawing.FontStyle.Bold);
            AkceTable.Controls.Add(Popisek, 8, 0);
            Popisek = new Label();
            Popisek.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            Tooltip = new ToolTip();
            Tooltip.SetToolTip(Popisek, "Svařování v hodinách");
            Popisek.Text = "S(h)";
            Popisek.Font = new System.Drawing.Font(Popisek.Font, System.Drawing.FontStyle.Bold);
            AkceTable.Controls.Add(Popisek, 9, 0);
            Popisek = new Label();
            Popisek.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            Tooltip = new ToolTip();
            Tooltip.SetToolTip(Popisek, "Vibrace v hodinách");
            Popisek.Text = "V(h)";
            Popisek.Font = new System.Drawing.Font(Popisek.Font, System.Drawing.FontStyle.Bold);
            AkceTable.Controls.Add(Popisek, 10, 0);
            Popisek = new Label();
            Popisek.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            Tooltip = new ToolTip();
            Tooltip.SetToolTip(Popisek, "Noční hodiny");
            Popisek.Text = "N(h)";
            Popisek.Font = new System.Drawing.Font(Popisek.Font, System.Drawing.FontStyle.Bold);
            AkceTable.Controls.Add(Popisek, 11, 0);

            String SQL = "SELECT CisloZam, Zamestnanec, Oznaceni, HodinyCelkem, HodinyZakazka, PrescasyCelkem, Prescasy32, PrescasyProcento, Odmena, Diety, Svarovani, Vibrace, NocniHodiny FROM dbo.EGDochazkaAkce(" + Rok + ", " + Mesic + ", " + CisloZam + ", '" + Stredisko + "', '" + Zakazka + "', '" + StrediskoDochazka + "', '" + ZakazkaDochazka + "') ORDER BY Zamestnanec";
            IHeQuery Akce = Helios.OpenSQL(SQL);
            Int32 Celkem = Akce.RecordCount();
            Postup.progressBar_Postup.Maximum = Celkem;
            while (!Akce.EOF())
            {
                Postup.label_Prvek.Text = Akce.FieldValues(1) + "(" + Akce.FieldValues(2) + ")";
                Postup.progressBar_Postup.PerformStep();
                Postup.Refresh();
                AkceTable.RowCount++;
                AkceTable.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                Popisek = new Label();
                Popisek.Tag = ((Int32)Akce.FieldValues(0)).ToString();
                Popisek.Text = Akce.FieldValues(1);
                Popisek.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
                AkceTable.Controls.Add(Popisek, 0, AkceTable.RowCount - 1);
                Popisek = new Label();
                Popisek.Text = Akce.FieldValues(2);
                Popisek.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
                Popisek.AutoSize = true;
                Popisek.Dock = DockStyle.Fill;
                AkceTable.Controls.Add(Popisek, 1, AkceTable.RowCount - 1);
                Popisek = new Label();
                Popisek.Text = ((Single)Akce.FieldValues(3)).ToString();
                Popisek.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                AkceTable.Controls.Add(Popisek, 2, AkceTable.RowCount - 1);
                Popisek = new Label();
                Popisek.Text = ((Single)Akce.FieldValues(4)).ToString();
                Popisek.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                AkceTable.Controls.Add(Popisek, 3, AkceTable.RowCount - 1);
                Popisek = new Label();
                Popisek.Text = ((Single)Akce.FieldValues(5)).ToString();
                Popisek.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                AkceTable.Controls.Add(Popisek, 4, AkceTable.RowCount - 1);
                Popisek = new Label();
                Popisek.Text = ((Single)Akce.FieldValues(6)).ToString();
                Popisek.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                AkceTable.Controls.Add(Popisek, 5, AkceTable.RowCount - 1);
                Popisek = new Label();
                Popisek.Text = ((Single)Akce.FieldValues(7)).ToString();
                Popisek.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                AkceTable.Controls.Add(Popisek, 6, AkceTable.RowCount - 1);
                Text = new TextBox();
                Text.Text = ((Single)Akce.FieldValues(8)).ToString();
                Text.TextAlign = HorizontalAlignment.Center;
                Text.Enter += VyberText_Enter;
                Text.Validating += KontrolaCisla_Validating;
                Text.Tag = "O" + (AkceTable.RowCount - 1);
                AkceTable.Controls.Add(Text, 7, AkceTable.RowCount - 1);
                Text = new TextBox();
                Text.Text = ((Single)Akce.FieldValues(9)).ToString();
                Text.TextAlign = HorizontalAlignment.Center;
                Text.Enter += VyberText_Enter;
                Text.Validating += KontrolaCisla_Validating;
                Text.Tag = "D" + (AkceTable.RowCount - 1);
                AkceTable.Controls.Add(Text, 8, AkceTable.RowCount - 1);
                Text = new TextBox();
                Text.Text = ((Single)Akce.FieldValues(10)).ToString();
                Text.TextAlign = HorizontalAlignment.Center;
                Text.Enter += VyberText_Enter;
                Text.Validating += KontrolaCisla_Validating;
                Text.Tag = "S" + (AkceTable.RowCount - 1);
                AkceTable.Controls.Add(Text, 9, AkceTable.RowCount - 1);
                Text = new TextBox();
                Text.Text = ((Single)Akce.FieldValues(11)).ToString();
                Text.TextAlign = HorizontalAlignment.Center;
                Text.Enter += VyberText_Enter;
                Text.Validating += KontrolaCisla_Validating;
                Text.Tag = "V" + (AkceTable.RowCount - 1);
                AkceTable.Controls.Add(Text, 10, AkceTable.RowCount - 1);
                Text = new TextBox();
                Text.Text = ((Single)Akce.FieldValues(12)).ToString();
                Text.TextAlign = HorizontalAlignment.Center;
                Text.Enter += VyberText_Enter;
                Text.Validating += KontrolaCisla_Validating;
                Text.Tag = "N" + (AkceTable.RowCount - 1);
                AkceTable.Controls.Add(Text, 11, AkceTable.RowCount - 1);
                Akce.Next();
            }
            panel_FormAkce.Controls.Add(AkceTable);
            Postup.Hide();
        }

        private void KontrolaCisla_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            String Text = ((TextBox)sender).Text;
            Single Cislo;
            if (!Single.TryParse(Text, out Cislo))
            {
                Helios.Info("Zadejte číslo");
                e.Cancel = true;
            }
            //Text = ((TextBox)sender).Tag.ToString();
            //Int32 Radek;
            //Int32.TryParse(Text.Substring(1), out Radek);
            //TableLayoutPanel AkceTable = (TableLayoutPanel)panel_FormAkce.Controls[0];
            //Label Popisek;
            //Single Hodnota;
            //switch (Text[0])
            //{
            //    case 'O':
            //        Popisek = (Label)AkceTable.GetControlFromPosition(6, Radek);
            //        if (!Single.TryParse(Popisek.Text, out Hodnota)) Hodnota = 0;
            //        if ((Cislo != 0) && ((Hodnota + Cislo) > 10))
            //        {
            //            Helios.Info("Součet zadané odměny a procenta přesčasů nesmí být vyšší než 10 %");
            //            e.Cancel = true;
            //        }
            //        break;
            //    default:
            //        break;
            //}
        }

        private void button_UlozitAkce_Click(object sender, EventArgs e)
        {
            if (UlozitAkce())
                Helios.Info("Doplňkové inormace uloženy");
        }

        private Boolean UlozitAkce()
        {
            if (Uzamceno)
                return false;
            Int16 Rok = (Int16)numericUpDown_Rok.Value;
            Int16 Mesic = (Int16)numericUpDown_Mesic.Value;
            if (panel_FormAkce.Controls.Count == 0)
                return true;
            TableLayoutPanel AkceTable = (TableLayoutPanel)panel_FormAkce.Controls[0];

            Int32 StareCisloZam = 0;
            Int32 CisloZam = 0;
            String Oznaceni;
            String Stredisko;
            String Zakazka;
            Single Odmena;
            Single Diety;
            Single Svarovani;
            Single Vibrace;
            Single Nocni;
            String SQL;
            for (int i = 1; i < AkceTable.RowCount; i++)
            {
                Int32.TryParse(AkceTable.GetControlFromPosition(0, i).Tag.ToString(), out CisloZam);
                Oznaceni = AkceTable.GetControlFromPosition(1, i).Text;
                Stredisko = Oznaceni.Substring(0, Oznaceni.IndexOf("-"));
                Zakazka = Oznaceni.Substring(Oznaceni.IndexOf("-") + 1);
                if (!Single.TryParse(AkceTable.GetControlFromPosition(7, i).Text, out Odmena)) Odmena = 0;
                if (!Single.TryParse(AkceTable.GetControlFromPosition(8, i).Text, out Diety)) Diety = 0;
                if (!Single.TryParse(AkceTable.GetControlFromPosition(9, i).Text, out Svarovani)) Svarovani = 0;
                if (!Single.TryParse(AkceTable.GetControlFromPosition(10, i).Text, out Vibrace)) Vibrace = 0;
                if (!Single.TryParse(AkceTable.GetControlFromPosition(11, i).Text, out Nocni)) Nocni = 0;
                SQL = "UPDATE TabEGDochazkaMesic SET Odmena = " + Odmena.ToString().Replace(",", ".") + ", Diety = " + Diety.ToString().Replace(",", ".") + ", Svarovani = " + Svarovani.ToString().Replace(",", ".") + ", Vibrace = " + Vibrace.ToString().Replace(",", ".") + ", NocniHodiny = " + Nocni.ToString().Replace(",", ".") + " WHERE Rok = " + Rok + " AND Mesic = " + Mesic + " AND CisloZam = " + CisloZam + " AND Stredisko = '" + Stredisko + "' AND Zakazka = '" + Zakazka + "'";
                Helios.ExecSQL(SQL);
                if (StareCisloZam != CisloZam)
                {
                    StareCisloZam = CisloZam;
                    NaplnUpravenouDochazku(CisloZam, Rok, Mesic);
                }
            }
            return true;
        }

        private void NaplnUpravenouDochazku(Int32 CisloZam, Int32 Rok, Int32 Mesic)
        {
            Double PrescasyD = 0;
            Double PrescasyS = 0;
            Double PrescasyS8 = 0;
            Double PrescasyN = 0;
            Double PrescasySv = 0;

            if (DochazkaUpravy == null)
                DochazkaUpravy = new List<DochazkaUprava>();
            DochazkaUpravy.Clear();
            DochazkaUprava Polozka;
            DateTime Datum;
            String Stredisko;
            String Zakazka;
            String Cinnost;
            Double Hodiny;
            String SQL = "SELECT Datum, Stredisko, Zakazka, Cinnost, Hodiny FROM TabEGDochazkaDny WHERE CisloZam = " + CisloZam + " AND YEAR(Datum) = " + Rok + " AND Month(Datum) = " + Mesic + " ORDER BY Datum, Stredisko, Zakazka, Cinnost";
            IHeQuery Polozky = Helios.OpenSQL(SQL);
            if (Polozky.EOF())
            {
                SQL = "DELETE FROM TabEGDochazkaSichtovka WHERE CisloZam = " + CisloZam + " AND Rok = " + Rok + " AND Mesic = " + Mesic;
                Helios.ExecSQL(SQL);
                SQL = "DELETE FROM TabEGDochazkaPodklady WHERE CisloZam = " + CisloZam + " AND Rok = " + Rok + " AND Mesic = " + Mesic;
                Helios.ExecSQL(SQL);
                return;
            }
            while (!Polozky.EOF())
            {
                Datum = Polozky.FieldValues(0);
                Stredisko = Polozky.FieldValues(1);
                Zakazka = Polozky.FieldValues(2);
                Cinnost = Polozky.FieldValues(3);
                Hodiny = Polozky.FieldValues(4);
                Polozka = new DochazkaUprava(Datum, Stredisko, Zakazka, Cinnost, Hodiny, Helios);
                DochazkaUpravy.Add(Polozka);
                Polozky.Next();   
            }
            
            var DochazkaSoucty =
                (from p in DochazkaUpravy.Where(w => w.Prace == true).GroupBy(g => g.Den).Select(s => new { Den = s.Key, Hodiny = s.Sum(a => a.Hodiny) })
                 join d in DochazkaUpravy on p.Den equals d.Den
                 select new { Den = p.Den, p.Hodiny, d.DenTydne, d.Svatek }).Distinct();

            Double _PrescasyD = DochazkaSoucty.Where(w => w.Svatek == false && w.DenTydne != 6 && w.DenTydne != 7).Sum(s => s.Hodiny <= 8 ? 0 : s.Hodiny - 8);
            Double _PrescasyS = DochazkaSoucty.Where(w => w.DenTydne == 6 && w.Svatek == false).Sum(s => s.Hodiny);
            Double _PrescasyN = DochazkaSoucty.Where(w => w.DenTydne == 7 && w.Svatek == false).Sum(s => s.Hodiny);
            Double _PrescasySv = DochazkaSoucty.Where(w => w.Svatek == true).Sum(s => s.Hodiny);
            Double _PrescasyC = _PrescasyD + _PrescasyS + _PrescasyN + _PrescasySv;

            Double Zustatek = _PrescasyC;
            Double Odecti;

            //if (Zustatek > 32)
            //{
                Zustatek -= _PrescasySv;
                PrescasySv = _PrescasyC - Zustatek;
                _PrescasySv -= PrescasySv;
                _PrescasyC = Zustatek;
                Odecti = PrescasySv;
                while (Odecti != 0)
                {
                    foreach (var item in DochazkaSoucty)
                    {
                        if (item.Svatek)
                        {
                            Polozka =
                                (from d in DochazkaUpravy
                                 where d.Den == item.Den && d.Prace
                                 orderby d.Hodiny descending
                                 select d).First();
                            Polozka.Hodiny -= 0.5;
                            Odecti -= 0.5;
                            if (Odecti == 0)
                                break;
                        }
                    }
                    DochazkaSoucty =
                        (from p in DochazkaUpravy.Where(w => w.Prace == true).GroupBy(g => g.Den).Select(s => new { Den = s.Key, Hodiny = s.Sum(a => a.Hodiny) })
                         join d in DochazkaUpravy on p.Den equals d.Den
                         select new { Den = p.Den, p.Hodiny, d.DenTydne, d.Svatek }).Distinct();
                }

            //}
            //if (Zustatek > 32)
            //{
                Zustatek -= _PrescasyN;
                PrescasyN = _PrescasyC - Zustatek;
                _PrescasyN -= PrescasyN;
                _PrescasyC = Zustatek;
                Odecti = PrescasyN;
                while (Odecti != 0)
                {
                    foreach (var item in DochazkaSoucty)
                    {
                        if (item.DenTydne == 7 && !item.Svatek)
                        {
                            Polozka =
                                (from d in DochazkaUpravy
                                 where d.Den == item.Den && d.Prace
                                 orderby d.Hodiny descending
                                 select d).First();
                            Polozka.Hodiny -= 0.5;
                            Odecti -= 0.5;
                            if (Odecti == 0)
                                break;
                        }
                    }
                    DochazkaSoucty =
                        (from p in DochazkaUpravy.Where(w => w.Prace == true).GroupBy(g => g.Den).Select(s => new { Den = s.Key, Hodiny = s.Sum(a => a.Hodiny) })
                         join d in DochazkaUpravy on p.Den equals d.Den
                         select new { Den = p.Den, p.Hodiny, d.DenTydne, d.Svatek }).Distinct();
                }

            //}
            foreach (var item in DochazkaSoucty)
            {
                if (item.DenTydne == 6 && !item.Svatek)
                {
                    Polozka =
                        (from d in DochazkaUpravy
                         where d.Den == item.Den && d.Prace
                         orderby d.Hodiny descending
                         select d).First();
                    if (Polozka.Hodiny > 8)
                    {
                        PrescasyS8 += Polozka.Hodiny - 8;
                        Polozka.Hodiny = 8;
                    }
                }
            }
            Zustatek -= PrescasyS8;
            _PrescasyS -= PrescasyS8;
            _PrescasyC = Zustatek;
            if (Zustatek > 32)
            {
                if (Zustatek - _PrescasyD < 32)
                    Zustatek = 32;
                else
                    Zustatek -= _PrescasyD;
                PrescasyD = _PrescasyC - Zustatek;
                _PrescasyD -= PrescasyD;
                _PrescasyC = Zustatek;
                Odecti = PrescasyD;
                while (Odecti != 0)
                {
                    foreach (var item in DochazkaSoucty)
                    {
                        if (item.DenTydne < 6 && !item.Svatek && item.Hodiny > 8)
                        {
                            Polozka =
                                (from d in DochazkaUpravy
                                where d.Den == item.Den && d.Prace
                                orderby d.Hodiny descending
                                select d).First();
                            Polozka.Hodiny -= 0.5;
                            Odecti -= 0.5;
                            if (Odecti == 0)
                                break;
                        }
                    }
                    DochazkaSoucty =
                        (from p in DochazkaUpravy.Where(w => w.Prace == true).GroupBy(g => g.Den).Select(s => new { Den = s.Key, Hodiny = s.Sum(a => a.Hodiny) })
                         join d in DochazkaUpravy on p.Den equals d.Den
                         select new { Den = p.Den, p.Hodiny, d.DenTydne, d.Svatek }).Distinct();
                }
                
            }
            if (Zustatek > 32)
            {
                if (Zustatek - _PrescasyS < 32)
                    Zustatek = 32;
                else
                    Zustatek -= _PrescasyS;
                PrescasyS = _PrescasyC - Zustatek;
                _PrescasyS -= PrescasyS;
                _PrescasyC = Zustatek;
                Odecti = PrescasyS;
                while (Odecti != 0)
                {
                    foreach (var item in DochazkaSoucty)
                    {
                        if (item.DenTydne == 6 && !item.Svatek)
                        {
                            Polozka =
                                (from d in DochazkaUpravy
                                 where d.Den == item.Den && d.Prace
                                 orderby d.Hodiny descending
                                 select d).First();
                            Polozka.Hodiny -= 0.5;
                            Odecti -= 0.5;
                            if (Odecti == 0)
                                break;
                        }
                    }
                    DochazkaSoucty =
                        (from p in DochazkaUpravy.Where(w => w.Prace == true).GroupBy(g => g.Den).Select(s => new { Den = s.Key, Hodiny = s.Sum(a => a.Hodiny) })
                         join d in DochazkaUpravy on p.Den equals d.Den
                         select new { Den = p.Den, p.Hodiny, d.DenTydne, d.Svatek }).Distinct();
                }
            }


            SQL = "DELETE FROM TabEGDochazkaSichtovka WHERE CisloZam = " + CisloZam + " AND Rok = " + Rok + " AND Mesic = " + Mesic;
            Helios.ExecSQL(SQL);
            foreach (DochazkaUprava item in DochazkaUpravy)
            {
                SQL = "UPDATE TabEGDochazkaDny SET Prepocteno = " + item.Hodiny.ToString().Replace(",", ".") + " WHERE CisloZam = " + CisloZam + " AND YEAR(Datum) = " + Rok + " AND Month(Datum) = " + Mesic + " AND Day(Datum) = " + item.Den + " AND Stredisko LIKE '" + item.Stredisko + "' AND Zakazka LIKE '" + item.Zakazka + "' AND Cinnost LIKE '" + item.Cinnost + "'";
                Helios.ExecSQL(SQL); 
                SQL = "EXEC dbo.EGDochazkaSichtovkaAktualizuj " + CisloZam + ", " + Rok + ", " + Mesic + ", " + item.Den + ", N'" + item.Stredisko + "', N'" + item.Zakazka + "', N'" + item.Cinnost + "', " + item.Hodiny.ToString().Replace(",", ".");
                Helios.ExecSQL(SQL);
            }

            Double Odmena = 0;
            Double Diety = 0;
            Double Svarovani = 0;
            Double Vibrace = 0;
            Double NocniHodiny = 0;

            SQL = "SELECT COUNT(*) FROM TabEGDochazkaMesic WHERE CisloZam = " + CisloZam + " AND Rok = " + Rok + " AND Mesic = " + Mesic;
            IHeQuery AkcePocet = Helios.OpenSQL(SQL);
            if (AkcePocet.RecordCount() > 0)
            {
                SQL = "SELECT SUM(Odmena), SUM(Diety), SUM(Svarovani), SUM(Vibrace), SUM(NocniHodiny) FROM TabEGDochazkaMesic WHERE CisloZam = " + CisloZam + " AND Rok = " + Rok + " AND Mesic = " + Mesic;
                IHeQuery Akce = Helios.OpenSQL(SQL);
                Odmena = Akce.FieldValues(0);
                Diety = Akce.FieldValues(1);
                Svarovani = Akce.FieldValues(2);
                Vibrace = Akce.FieldValues(3);
                NocniHodiny = Akce.FieldValues(4);
            }

            PrescasyS += PrescasyS8;
            Double HodinyC = DochazkaUpravy.Where(w => w.Prace == true).Sum(s => s.Hodiny);
            Double SmenyC = DochazkaUpravy.Where(w => w.Prace == true || w.Cinnost == "Sk").GroupBy(g => g.Den).Select(s => new { Hodiny = s.Sum(i => i.Hodiny) }).Where(w => w.Hodiny >= 4).Count();
            SQL = "SELECT ZM.ZakladniPlat FROM TabZamMzd AS ZM INNER JOIN TabCisZam AS CZ ON CZ.ID = ZM.ZamestnanecId INNER JOIN TabMzdObd AS MO ON MO.IdObdobi = ZM.IdObdobi WHERE CZ.Cislo = " + CisloZam + " AND MO.Rok = " + Rok + " AND MO.Mesic = " + Mesic;
            IHeQuery Hodinovky = Helios.OpenSQL(SQL);
            Double Hodinovka = Hodinovky.EOF() ? 0 : Hodinovky.FieldValues(0);
            Double Premie = (Double)(PrescasyD * 1 + PrescasyS * 1.5 + PrescasyN * 1.75 + PrescasySv * 2) * Hodinovka;
            Premie += Odmena;
            Premie = 5 * (int)Math.Round(Premie / 5.0);
            Double Svatky = DochazkaUpravy.Where(w => w.Cinnost == "S").Count();
            Double Paragraf = DochazkaUpravy.Where(w => w.Cinnost == "P").Sum(s => s.Hodiny) / 8;
            Double Skoleni = DochazkaUpravy.Where(w => w.Cinnost == "Sk" || w.Cinnost == "St").Sum(s => s.Hodiny) / 8;
            Double Dovolena = DochazkaUpravy.Where(w => w.Cinnost == "D").Count() + (Double)DochazkaUpravy.Where(w => w.Cinnost == "D/4").Count() / 2;
            Double Nemoc = DochazkaUpravy.Where(w => w.Cinnost == "N").Count();
            Double Tickety = SmenyC - Diety;

            SQL = "DELETE FROM TabEGDochazkaPodklady WHERE CisloZam = " + CisloZam + " AND Rok = " + Rok + " AND Mesic = " + Mesic;
            Helios.ExecSQL(SQL);

            SQL = "INSERT INTO dbo.TabEGDochazkaPodklady (CisloZam, Rok, Mesic, Hodiny, Smeny, Premie, PrescasyD, PrescasyS, PrescasyN, PrescasySv, Svatek, Paragraf, Skoleni, Dovolena, Nemoc, Svareni, Vibrace, Nocni, Diety, Tickety) VALUES (" + CisloZam + ", " + Rok + ", " + Mesic + ", " + HodinyC.ToString().Replace(",", ".") + ", " + SmenyC.ToString().Replace(",", ".") + ", " + Premie.ToString().Replace(",", ".") + ", " + _PrescasyD.ToString().Replace(",", ".") + ", " + _PrescasyS.ToString().Replace(",", ".") + ", " + _PrescasyN.ToString().Replace(",", ".") + ", " + _PrescasySv.ToString().Replace(",", ".") + ", " + Svatky.ToString().Replace(",", ".") + ", " + Paragraf.ToString().Replace(",", ".") + ", " + Skoleni.ToString().Replace(",", ".") + ", " + Dovolena.ToString().Replace(",", ".") + ", " + Nemoc.ToString().Replace(",", ".") + ", " + Svarovani.ToString().Replace(",", ".") + ", " + Vibrace.ToString().Replace(",", ".") + ", " + NocniHodiny.ToString().Replace(",", ".") + ", " + Diety.ToString().Replace(",", ".") + ", " + Tickety.ToString().Replace(",", ".") + ")";
            Helios.ExecSQL(SQL);
        }

        private void button_TiskniPodklady_Click(object sender, EventArgs e)
        {
            if (treeView_Zamestnanci.SelectedNode == null)
                return;
            if (comboBox_Zakazky.SelectedIndex == -1)
                return;
            Int16 Rok = (Int16)numericUpDown_Rok.Value;
            Int16 Mesic = (Int16)numericUpDown_Mesic.Value;
            String Node = treeView_Zamestnanci.SelectedNode.Name;
            Int32 CisloZam = 0;
            String Stredisko = "";
            String Zakazka = "";

            switch (Node[0])
            {
                case 'E':
                    break;
                case 'S':
                    Stredisko = Node.Substring(1);
                    break;
                case 'Z':
                    Zakazka = Node.Substring(1);
                    break;
                default:
                    Int32.TryParse(Node, out CisloZam);
                    break;
            }

            String StrediskoDochazka = "";
            String ZakazkaDochazka = "";
            String Oznaceni = comboBox_Zakazky.SelectedItem.ToString();
            if (Oznaceni[0] != '<')
            {
                StrediskoDochazka = Oznaceni.Substring(0, Oznaceni.IndexOf("-"));
                ZakazkaDochazka = Oznaceni.Substring(Oznaceni.IndexOf("-") + 1, Oznaceni.IndexOf(":") - Oznaceni.IndexOf("-") - 1);
            }

            String SQL = "SELECT DISTINCT CisloZam FROM dbo.EGDochazkaAkce(" + Rok + ", " + Mesic + ", " + CisloZam + ", '" + Stredisko + "', '" + Zakazka + "', '" + StrediskoDochazka + "', '" + ZakazkaDochazka + "')";
            IHeQuery Zamestnanci = Helios.OpenSQL(SQL);
            String Seznam = "";
            while (!Zamestnanci.EOF())
            {
                if (Seznam.Length == 0)
                    Seznam = "CisloZam IN (" + Zamestnanci.FieldValues(0);
                else
                    Seznam += ", " + Zamestnanci.FieldValues(0);
                Zamestnanci.Next();
            }
            if (Seznam.Length == 0)
                return;
            Seznam += ")";

            String User = Helios.LoginName();

            SQL = "SELECT ID FROM TabEGDochazkaPodklady WHERE Rok = " + Rok + " AND Mesic = " + Mesic + " AND " + Seznam;
            //SQL = "SELECT P.ID FROM TabEGDochazkaPodklady AS P INNER JOIN TabCisZam AS CZ ON CZ.Cislo = P.CisloZam WHERE Rok = " + Rok + " AND Mesic = " + Mesic + " AND " + Seznam + " ORDER BY P.Rok, P.Mesic, CZ.PrijmeniJmeno";
            IHeQuery Podklady = Helios.OpenSQL(SQL);
            SQL = "DELETE FROM TabEGDochazkaPodkladyTisk WHERE Autor = '" + User + "'";
            Helios.ExecSQL(SQL);
            while (!Podklady.EOF())
            {
                SQL = "INSERT INTO TabEGDochazkaPodkladyTisk (PodkladyID, Autor) VALUES (" + Podklady.FieldValues(0) + ", '" + User + "')";
                Helios.ExecSQL(SQL);
                Podklady.Next();
            }
            Helios.PrintForm3(100135, 77, "hvw_384511A644604C409154C0CF18D7C4C3.Autor = '" + User + "'");

            SQL = "SELECT ID FROM TabEGDochazkaSichtovka WHERE Rok = " + Rok + " AND Mesic = " + Mesic + " AND " + Seznam;
            IHeQuery Sichtovka = Helios.OpenSQL(SQL);
            SQL = "DELETE FROM TabEGDochazkaSichtovkaTisk WHERE Autor = '" + User + "'";
            Helios.ExecSQL(SQL);
            while (!Sichtovka.EOF())
            {
                SQL = "INSERT INTO TabEGDochazkaSichtovkaTisk (SichtovkaID, Autor) VALUES (" + Sichtovka.FieldValues(0) + ", '" + User + "')";
                Helios.ExecSQL(SQL);
                Sichtovka.Next();
            }
            Helios.PrintForm3(100136, 105, "hvw_365235CAB8A843CBB8E32370C411DDA1.Autor = '" + User + "'");

        }

        private void button_Nacti_Click(object sender, EventArgs e)
        {
            NactiAkce();
        }

        private void button_Smaz_Click(object sender, EventArgs e)
        {
            Int32 CisloZam;
            if (!Int32.TryParse(treeView_Zamestnanci.SelectedNode.Name, out CisloZam))
            {
                Helios.Error("Vyberte zaměstnance, kterému chcete vymazat docázku");
                return;
            }
            Int16 Rok = (Int16)numericUpDown_Rok.Value;
            Int16 Mesic = (Int16)numericUpDown_Mesic.Value;
            String Zamestnanec = treeView_Zamestnanci.SelectedNode.Text;
            if (!Helios.YesNo("Opravdu chcete vymazat docházku zaměstnance: " + Zamestnanec + "?", true))
                return;
            String SQL = "DELETE FROM TabEGDochazkaDny WHERE YEAR(Datum) = " + Rok + " AND Month(Datum) = " + Mesic + " AND CisloZam = " + CisloZam;
            Helios.ExecSQL(SQL);
            SQL = "EXEC dbo.EGDochazkaMesicAktualizuj " + CisloZam + ", " + Rok + ", " + Mesic;
            Helios.ExecSQL(SQL);
            NaplnUpravenouDochazku(CisloZam, Rok, Mesic);
            NactiDochazku();
        }

        private void treeView_Zamestnanci_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if (treeView_Zamestnanci.SelectedNode == null)
                return;
            if (!Uzamceno && !Zavedeni)
            {
                switch (tabControlZadavani.SelectedIndex)
                {
                    //case 1:
                    //    if (Helios.YesNo("Uložit doplňkové informace?", true))
                    //    {
                    //        if (!UlozitAkce())
                    //        {
                    //            Helios.Error("Doplňkové informace neuloženy");
                    //            e.Cancel = true;
                    //        }
                    //    }
                    //    break;
                    case 0:
                        if (Helios.YesNo("Uložit rozpracovanou docházku?", true))
                        {
                            if (!UlozitDochazku())
                            {
                                Helios.Error("Docházka neuložena");
                                e.Cancel = true;
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        private void DochazkaForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (treeView_Zamestnanci.SelectedNode == null)
                return;
            if (!Uzamceno && !Zavedeni)
            {
                switch (tabControlZadavani.SelectedIndex)
                {
                    case 1:
                        if (Helios.YesNo("Uložit doplňkové informace?", true))
                        {
                            if (!UlozitAkce())
                            {
                                Helios.Error("Doplňkové informace neuloženy");
                                e.Cancel = true;
                            }
                        }
                        break;
                    case 0:
                        if (Helios.YesNo("Uložit rozpracovanou docházku?", true))
                        {
                            if (!UlozitDochazku())
                            {
                                Helios.Error("Docházka neuložena");
                                e.Cancel = true;
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
