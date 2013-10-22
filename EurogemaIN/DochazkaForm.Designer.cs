namespace EurogemaIN
{
    partial class DochazkaForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DochazkaForm));
            this.textBox_Vyhledavani = new System.Windows.Forms.TextBox();
            this.treeView_Zamestnanci = new System.Windows.Forms.TreeView();
            this.numericUpDown_Rok = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown_Mesic = new System.Windows.Forms.NumericUpDown();
            this.splitContainer_Okno = new System.Windows.Forms.SplitContainer();
            this.panel_Vyhledavani = new System.Windows.Forms.Panel();
            this.groupBox_Obdobi = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox_Vyhledavani = new System.Windows.Forms.GroupBox();
            this.tabControlZadavani = new System.Windows.Forms.TabControl();
            this.tabPage_Dochazka = new System.Windows.Forms.TabPage();
            this.panel_Hlavicka = new System.Windows.Forms.Panel();
            this.button_Smaz = new System.Windows.Forms.Button();
            this.groupBox_Kopirovani = new System.Windows.Forms.GroupBox();
            this.button_Kopirovani = new System.Windows.Forms.Button();
            this.comboBox_Kopirovani = new System.Windows.Forms.ComboBox();
            this.button_Ulozit = new System.Windows.Forms.Button();
            this.groupBox_PridatRadky = new System.Windows.Forms.GroupBox();
            this.button_PridejRadky = new System.Windows.Forms.Button();
            this.textBox_Zakazka = new System.Windows.Forms.TextBox();
            this.textBox_Stredisko = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.panel_Formular = new System.Windows.Forms.Panel();
            this.tabPage_Akce = new System.Windows.Forms.TabPage();
            this.panel_FormAkce = new System.Windows.Forms.Panel();
            this.panel_HlavickaAkce = new System.Windows.Forms.Panel();
            this.button_TiskniPodklady = new System.Windows.Forms.Button();
            this.button_UlozitAkce = new System.Windows.Forms.Button();
            this.groupBox_Zakazky = new System.Windows.Forms.GroupBox();
            this.button_Nacti = new System.Windows.Forms.Button();
            this.comboBox_Zakazky = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_Rok)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_Mesic)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_Okno)).BeginInit();
            this.splitContainer_Okno.Panel1.SuspendLayout();
            this.splitContainer_Okno.Panel2.SuspendLayout();
            this.splitContainer_Okno.SuspendLayout();
            this.panel_Vyhledavani.SuspendLayout();
            this.groupBox_Obdobi.SuspendLayout();
            this.groupBox_Vyhledavani.SuspendLayout();
            this.tabControlZadavani.SuspendLayout();
            this.tabPage_Dochazka.SuspendLayout();
            this.panel_Hlavicka.SuspendLayout();
            this.groupBox_Kopirovani.SuspendLayout();
            this.groupBox_PridatRadky.SuspendLayout();
            this.tabPage_Akce.SuspendLayout();
            this.panel_HlavickaAkce.SuspendLayout();
            this.groupBox_Zakazky.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox_Vyhledavani
            // 
            this.textBox_Vyhledavani.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_Vyhledavani.Location = new System.Drawing.Point(7, 17);
            this.textBox_Vyhledavani.Name = "textBox_Vyhledavani";
            this.textBox_Vyhledavani.Size = new System.Drawing.Size(182, 20);
            this.textBox_Vyhledavani.TabIndex = 0;
            this.textBox_Vyhledavani.TextChanged += new System.EventHandler(this.textBox_Vyhledavani_TextChanged);
            // 
            // treeView_Zamestnanci
            // 
            this.treeView_Zamestnanci.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeView_Zamestnanci.Location = new System.Drawing.Point(0, 88);
            this.treeView_Zamestnanci.Name = "treeView_Zamestnanci";
            this.treeView_Zamestnanci.Size = new System.Drawing.Size(200, 473);
            this.treeView_Zamestnanci.TabIndex = 1;
            this.treeView_Zamestnanci.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeView_Zamestnanci_BeforeSelect);
            this.treeView_Zamestnanci.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView_Zamestnanci_AfterSelect);
            // 
            // numericUpDown_Rok
            // 
            this.numericUpDown_Rok.Location = new System.Drawing.Point(130, 14);
            this.numericUpDown_Rok.Maximum = new decimal(new int[] {
            3099,
            0,
            0,
            0});
            this.numericUpDown_Rok.Minimum = new decimal(new int[] {
            1900,
            0,
            0,
            0});
            this.numericUpDown_Rok.Name = "numericUpDown_Rok";
            this.numericUpDown_Rok.Size = new System.Drawing.Size(61, 20);
            this.numericUpDown_Rok.TabIndex = 4;
            this.numericUpDown_Rok.Value = new decimal(new int[] {
            1900,
            0,
            0,
            0});
            this.numericUpDown_Rok.ValueChanged += new System.EventHandler(this.numericUpDown_Rok_ValueChanged);
            // 
            // numericUpDown_Mesic
            // 
            this.numericUpDown_Mesic.Location = new System.Drawing.Point(45, 14);
            this.numericUpDown_Mesic.Maximum = new decimal(new int[] {
            12,
            0,
            0,
            0});
            this.numericUpDown_Mesic.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown_Mesic.Name = "numericUpDown_Mesic";
            this.numericUpDown_Mesic.Size = new System.Drawing.Size(44, 20);
            this.numericUpDown_Mesic.TabIndex = 2;
            this.numericUpDown_Mesic.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown_Mesic.ValueChanged += new System.EventHandler(this.numericUpDown_Mesic_ValueChanged);
            // 
            // splitContainer_Okno
            // 
            this.splitContainer_Okno.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer_Okno.Location = new System.Drawing.Point(0, 0);
            this.splitContainer_Okno.Name = "splitContainer_Okno";
            // 
            // splitContainer_Okno.Panel1
            // 
            this.splitContainer_Okno.Panel1.Controls.Add(this.panel_Vyhledavani);
            this.splitContainer_Okno.Panel1.Controls.Add(this.treeView_Zamestnanci);
            // 
            // splitContainer_Okno.Panel2
            // 
            this.splitContainer_Okno.Panel2.Controls.Add(this.tabControlZadavani);
            this.splitContainer_Okno.Size = new System.Drawing.Size(884, 561);
            this.splitContainer_Okno.SplitterDistance = 200;
            this.splitContainer_Okno.TabIndex = 3;
            this.splitContainer_Okno.TabStop = false;
            // 
            // panel_Vyhledavani
            // 
            this.panel_Vyhledavani.Controls.Add(this.groupBox_Obdobi);
            this.panel_Vyhledavani.Controls.Add(this.groupBox_Vyhledavani);
            this.panel_Vyhledavani.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel_Vyhledavani.Location = new System.Drawing.Point(0, 0);
            this.panel_Vyhledavani.Name = "panel_Vyhledavani";
            this.panel_Vyhledavani.Size = new System.Drawing.Size(200, 85);
            this.panel_Vyhledavani.TabIndex = 1;
            // 
            // groupBox_Obdobi
            // 
            this.groupBox_Obdobi.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox_Obdobi.Controls.Add(this.label1);
            this.groupBox_Obdobi.Controls.Add(this.numericUpDown_Mesic);
            this.groupBox_Obdobi.Controls.Add(this.label2);
            this.groupBox_Obdobi.Controls.Add(this.numericUpDown_Rok);
            this.groupBox_Obdobi.Location = new System.Drawing.Point(4, 4);
            this.groupBox_Obdobi.Name = "groupBox_Obdobi";
            this.groupBox_Obdobi.Size = new System.Drawing.Size(194, 40);
            this.groupBox_Obdobi.TabIndex = 1;
            this.groupBox_Obdobi.TabStop = false;
            this.groupBox_Obdobi.Text = "Období";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Měsíc";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(101, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(27, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Rok";
            // 
            // groupBox_Vyhledavani
            // 
            this.groupBox_Vyhledavani.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox_Vyhledavani.Controls.Add(this.textBox_Vyhledavani);
            this.groupBox_Vyhledavani.Location = new System.Drawing.Point(3, 45);
            this.groupBox_Vyhledavani.Name = "groupBox_Vyhledavani";
            this.groupBox_Vyhledavani.Size = new System.Drawing.Size(195, 40);
            this.groupBox_Vyhledavani.TabIndex = 0;
            this.groupBox_Vyhledavani.TabStop = false;
            this.groupBox_Vyhledavani.Text = "Zaměstnanci";
            // 
            // tabControlZadavani
            // 
            this.tabControlZadavani.CausesValidation = false;
            this.tabControlZadavani.Controls.Add(this.tabPage_Dochazka);
            this.tabControlZadavani.Controls.Add(this.tabPage_Akce);
            this.tabControlZadavani.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlZadavani.Location = new System.Drawing.Point(0, 0);
            this.tabControlZadavani.Name = "tabControlZadavani";
            this.tabControlZadavani.SelectedIndex = 0;
            this.tabControlZadavani.Size = new System.Drawing.Size(680, 561);
            this.tabControlZadavani.TabIndex = 2;
            this.tabControlZadavani.SelectedIndexChanged += new System.EventHandler(this.tabControlZadavani_SelectedIndexChanged);
            // 
            // tabPage_Dochazka
            // 
            this.tabPage_Dochazka.Controls.Add(this.panel_Hlavicka);
            this.tabPage_Dochazka.Controls.Add(this.panel_Formular);
            this.tabPage_Dochazka.Location = new System.Drawing.Point(4, 22);
            this.tabPage_Dochazka.Name = "tabPage_Dochazka";
            this.tabPage_Dochazka.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_Dochazka.Size = new System.Drawing.Size(672, 535);
            this.tabPage_Dochazka.TabIndex = 0;
            this.tabPage_Dochazka.Text = "Docházka";
            this.tabPage_Dochazka.UseVisualStyleBackColor = true;
            // 
            // panel_Hlavicka
            // 
            this.panel_Hlavicka.Controls.Add(this.button_Smaz);
            this.panel_Hlavicka.Controls.Add(this.groupBox_Kopirovani);
            this.panel_Hlavicka.Controls.Add(this.button_Ulozit);
            this.panel_Hlavicka.Controls.Add(this.groupBox_PridatRadky);
            this.panel_Hlavicka.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel_Hlavicka.Location = new System.Drawing.Point(3, 3);
            this.panel_Hlavicka.Name = "panel_Hlavicka";
            this.panel_Hlavicka.Size = new System.Drawing.Size(666, 60);
            this.panel_Hlavicka.TabIndex = 6;
            // 
            // button_Smaz
            // 
            this.button_Smaz.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Smaz.Location = new System.Drawing.Point(514, 26);
            this.button_Smaz.Name = "button_Smaz";
            this.button_Smaz.Size = new System.Drawing.Size(61, 27);
            this.button_Smaz.TabIndex = 8;
            this.button_Smaz.Text = "Vymaž";
            this.button_Smaz.UseVisualStyleBackColor = true;
            this.button_Smaz.Click += new System.EventHandler(this.button_Smaz_Click);
            // 
            // groupBox_Kopirovani
            // 
            this.groupBox_Kopirovani.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox_Kopirovani.Controls.Add(this.button_Kopirovani);
            this.groupBox_Kopirovani.Controls.Add(this.comboBox_Kopirovani);
            this.groupBox_Kopirovani.Location = new System.Drawing.Point(218, 4);
            this.groupBox_Kopirovani.Name = "groupBox_Kopirovani";
            this.groupBox_Kopirovani.Size = new System.Drawing.Size(290, 53);
            this.groupBox_Kopirovani.TabIndex = 7;
            this.groupBox_Kopirovani.TabStop = false;
            this.groupBox_Kopirovani.Text = "Kopírování od zaměstnance";
            // 
            // button_Kopirovani
            // 
            this.button_Kopirovani.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Kopirovani.Location = new System.Drawing.Point(227, 22);
            this.button_Kopirovani.Name = "button_Kopirovani";
            this.button_Kopirovani.Size = new System.Drawing.Size(57, 28);
            this.button_Kopirovani.TabIndex = 1;
            this.button_Kopirovani.Text = "Kopíruj";
            this.button_Kopirovani.UseVisualStyleBackColor = true;
            this.button_Kopirovani.Click += new System.EventHandler(this.button_Kopirovani_Click);
            // 
            // comboBox_Kopirovani
            // 
            this.comboBox_Kopirovani.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox_Kopirovani.FormattingEnabled = true;
            this.comboBox_Kopirovani.Location = new System.Drawing.Point(6, 29);
            this.comboBox_Kopirovani.Name = "comboBox_Kopirovani";
            this.comboBox_Kopirovani.Size = new System.Drawing.Size(215, 21);
            this.comboBox_Kopirovani.TabIndex = 0;
            this.comboBox_Kopirovani.TabStop = false;
            // 
            // button_Ulozit
            // 
            this.button_Ulozit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Ulozit.Image = global::EurogemaIN.Properties.Resources.Ulozit16;
            this.button_Ulozit.Location = new System.Drawing.Point(581, 26);
            this.button_Ulozit.Name = "button_Ulozit";
            this.button_Ulozit.Size = new System.Drawing.Size(80, 28);
            this.button_Ulozit.TabIndex = 6;
            this.button_Ulozit.Text = "Uložit";
            this.button_Ulozit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.button_Ulozit.UseVisualStyleBackColor = true;
            this.button_Ulozit.Click += new System.EventHandler(this.button_Ulozit_Click);
            // 
            // groupBox_PridatRadky
            // 
            this.groupBox_PridatRadky.Controls.Add(this.button_PridejRadky);
            this.groupBox_PridatRadky.Controls.Add(this.textBox_Zakazka);
            this.groupBox_PridatRadky.Controls.Add(this.textBox_Stredisko);
            this.groupBox_PridatRadky.Controls.Add(this.label3);
            this.groupBox_PridatRadky.Controls.Add(this.label4);
            this.groupBox_PridatRadky.Location = new System.Drawing.Point(4, 4);
            this.groupBox_PridatRadky.Name = "groupBox_PridatRadky";
            this.groupBox_PridatRadky.Size = new System.Drawing.Size(208, 53);
            this.groupBox_PridatRadky.TabIndex = 5;
            this.groupBox_PridatRadky.TabStop = false;
            this.groupBox_PridatRadky.Text = "Přidaní řádků do všech týdnů";
            // 
            // button_PridejRadky
            // 
            this.button_PridejRadky.Location = new System.Drawing.Point(146, 22);
            this.button_PridejRadky.Name = "button_PridejRadky";
            this.button_PridejRadky.Size = new System.Drawing.Size(56, 28);
            this.button_PridejRadky.TabIndex = 10;
            this.button_PridejRadky.TabStop = false;
            this.button_PridejRadky.Text = "Přidej";
            this.button_PridejRadky.UseVisualStyleBackColor = true;
            this.button_PridejRadky.Click += new System.EventHandler(this.button_PridejRadky_Click);
            // 
            // textBox_Zakazka
            // 
            this.textBox_Zakazka.Location = new System.Drawing.Point(77, 29);
            this.textBox_Zakazka.Name = "textBox_Zakazka";
            this.textBox_Zakazka.Size = new System.Drawing.Size(65, 20);
            this.textBox_Zakazka.TabIndex = 9;
            this.textBox_Zakazka.TabStop = false;
            this.textBox_Zakazka.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBox_Zakazka.DoubleClick += new System.EventHandler(this.VyberZakazky_DblClick);
            this.textBox_Zakazka.KeyDown += new System.Windows.Forms.KeyEventHandler(this.VyberZakazky_CtrlEnter);
            this.textBox_Zakazka.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.AllowKeys);
            this.textBox_Zakazka.Validating += new System.ComponentModel.CancelEventHandler(this.KontrolaZakazky_Validating);
            // 
            // textBox_Stredisko
            // 
            this.textBox_Stredisko.Location = new System.Drawing.Point(6, 29);
            this.textBox_Stredisko.Name = "textBox_Stredisko";
            this.textBox_Stredisko.Size = new System.Drawing.Size(65, 20);
            this.textBox_Stredisko.TabIndex = 8;
            this.textBox_Stredisko.TabStop = false;
            this.textBox_Stredisko.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBox_Stredisko.DoubleClick += new System.EventHandler(this.VyberStrediska_DblClick);
            this.textBox_Stredisko.KeyDown += new System.Windows.Forms.KeyEventHandler(this.VyberStredisko_CtrlEnter);
            this.textBox_Stredisko.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.AllowKeys);
            this.textBox_Stredisko.Validating += new System.ComponentModel.CancelEventHandler(this.KontrolaStrediska_Validating);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Úsek";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(79, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(49, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Zakázka";
            // 
            // panel_Formular
            // 
            this.panel_Formular.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_Formular.AutoScroll = true;
            this.panel_Formular.Location = new System.Drawing.Point(0, 63);
            this.panel_Formular.Name = "panel_Formular";
            this.panel_Formular.Size = new System.Drawing.Size(672, 472);
            this.panel_Formular.TabIndex = 11;
            // 
            // tabPage_Akce
            // 
            this.tabPage_Akce.Controls.Add(this.panel_FormAkce);
            this.tabPage_Akce.Controls.Add(this.panel_HlavickaAkce);
            this.tabPage_Akce.Location = new System.Drawing.Point(4, 22);
            this.tabPage_Akce.Name = "tabPage_Akce";
            this.tabPage_Akce.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_Akce.Size = new System.Drawing.Size(672, 535);
            this.tabPage_Akce.TabIndex = 1;
            this.tabPage_Akce.Text = "Doplňkové informace";
            this.tabPage_Akce.UseVisualStyleBackColor = true;
            // 
            // panel_FormAkce
            // 
            this.panel_FormAkce.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_FormAkce.AutoScroll = true;
            this.panel_FormAkce.Location = new System.Drawing.Point(0, 63);
            this.panel_FormAkce.Name = "panel_FormAkce";
            this.panel_FormAkce.Size = new System.Drawing.Size(672, 472);
            this.panel_FormAkce.TabIndex = 1;
            // 
            // panel_HlavickaAkce
            // 
            this.panel_HlavickaAkce.Controls.Add(this.button_TiskniPodklady);
            this.panel_HlavickaAkce.Controls.Add(this.button_UlozitAkce);
            this.panel_HlavickaAkce.Controls.Add(this.groupBox_Zakazky);
            this.panel_HlavickaAkce.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel_HlavickaAkce.Location = new System.Drawing.Point(3, 3);
            this.panel_HlavickaAkce.Name = "panel_HlavickaAkce";
            this.panel_HlavickaAkce.Size = new System.Drawing.Size(666, 60);
            this.panel_HlavickaAkce.TabIndex = 0;
            // 
            // button_TiskniPodklady
            // 
            this.button_TiskniPodklady.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_TiskniPodklady.Location = new System.Drawing.Point(495, 25);
            this.button_TiskniPodklady.Name = "button_TiskniPodklady";
            this.button_TiskniPodklady.Size = new System.Drawing.Size(80, 28);
            this.button_TiskniPodklady.TabIndex = 8;
            this.button_TiskniPodklady.Text = "Tisk";
            this.button_TiskniPodklady.UseVisualStyleBackColor = true;
            this.button_TiskniPodklady.Click += new System.EventHandler(this.button_TiskniPodklady_Click);
            // 
            // button_UlozitAkce
            // 
            this.button_UlozitAkce.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_UlozitAkce.Image = global::EurogemaIN.Properties.Resources.Ulozit16;
            this.button_UlozitAkce.Location = new System.Drawing.Point(581, 25);
            this.button_UlozitAkce.Name = "button_UlozitAkce";
            this.button_UlozitAkce.Size = new System.Drawing.Size(80, 28);
            this.button_UlozitAkce.TabIndex = 7;
            this.button_UlozitAkce.Text = "Uložit";
            this.button_UlozitAkce.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.button_UlozitAkce.UseVisualStyleBackColor = true;
            this.button_UlozitAkce.Click += new System.EventHandler(this.button_UlozitAkce_Click);
            // 
            // groupBox_Zakazky
            // 
            this.groupBox_Zakazky.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox_Zakazky.Controls.Add(this.button_Nacti);
            this.groupBox_Zakazky.Controls.Add(this.comboBox_Zakazky);
            this.groupBox_Zakazky.Location = new System.Drawing.Point(4, 4);
            this.groupBox_Zakazky.Name = "groupBox_Zakazky";
            this.groupBox_Zakazky.Size = new System.Drawing.Size(485, 53);
            this.groupBox_Zakazky.TabIndex = 0;
            this.groupBox_Zakazky.TabStop = false;
            this.groupBox_Zakazky.Text = "Výběr zakázky";
            // 
            // button_Nacti
            // 
            this.button_Nacti.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Nacti.Location = new System.Drawing.Point(404, 21);
            this.button_Nacti.Name = "button_Nacti";
            this.button_Nacti.Size = new System.Drawing.Size(75, 28);
            this.button_Nacti.TabIndex = 1;
            this.button_Nacti.Text = "Načtení";
            this.button_Nacti.UseVisualStyleBackColor = true;
            this.button_Nacti.Click += new System.EventHandler(this.button_Nacti_Click);
            // 
            // comboBox_Zakazky
            // 
            this.comboBox_Zakazky.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox_Zakazky.FormattingEnabled = true;
            this.comboBox_Zakazky.Location = new System.Drawing.Point(7, 26);
            this.comboBox_Zakazky.Name = "comboBox_Zakazky";
            this.comboBox_Zakazky.Size = new System.Drawing.Size(391, 21);
            this.comboBox_Zakazky.TabIndex = 0;
            // 
            // DochazkaForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 561);
            this.Controls.Add(this.splitContainer_Okno);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DochazkaForm";
            this.Text = "Docházka";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DochazkaForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_Rok)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_Mesic)).EndInit();
            this.splitContainer_Okno.Panel1.ResumeLayout(false);
            this.splitContainer_Okno.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_Okno)).EndInit();
            this.splitContainer_Okno.ResumeLayout(false);
            this.panel_Vyhledavani.ResumeLayout(false);
            this.groupBox_Obdobi.ResumeLayout(false);
            this.groupBox_Obdobi.PerformLayout();
            this.groupBox_Vyhledavani.ResumeLayout(false);
            this.groupBox_Vyhledavani.PerformLayout();
            this.tabControlZadavani.ResumeLayout(false);
            this.tabPage_Dochazka.ResumeLayout(false);
            this.panel_Hlavicka.ResumeLayout(false);
            this.groupBox_Kopirovani.ResumeLayout(false);
            this.groupBox_PridatRadky.ResumeLayout(false);
            this.groupBox_PridatRadky.PerformLayout();
            this.tabPage_Akce.ResumeLayout(false);
            this.panel_HlavickaAkce.ResumeLayout(false);
            this.groupBox_Zakazky.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeView_Zamestnanci;
        private System.Windows.Forms.NumericUpDown numericUpDown_Rok;
        private System.Windows.Forms.NumericUpDown numericUpDown_Mesic;
        private System.Windows.Forms.SplitContainer splitContainer_Okno;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel_Hlavicka;
        private System.Windows.Forms.Panel panel_Formular;
        private System.Windows.Forms.GroupBox groupBox_PridatRadky;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox_Zakazka;
        private System.Windows.Forms.TextBox textBox_Stredisko;
        private System.Windows.Forms.Button button_PridejRadky;
        private System.Windows.Forms.Panel panel_Vyhledavani;
        private System.Windows.Forms.GroupBox groupBox_Vyhledavani;
        private System.Windows.Forms.TextBox textBox_Vyhledavani;
        private System.Windows.Forms.Button button_Ulozit;
        private System.Windows.Forms.GroupBox groupBox_Kopirovani;
        private System.Windows.Forms.ComboBox comboBox_Kopirovani;
        private System.Windows.Forms.Button button_Kopirovani;
        private System.Windows.Forms.TabControl tabControlZadavani;
        private System.Windows.Forms.TabPage tabPage_Dochazka;
        private System.Windows.Forms.TabPage tabPage_Akce;
        private System.Windows.Forms.GroupBox groupBox_Obdobi;
        private System.Windows.Forms.Panel panel_HlavickaAkce;
        private System.Windows.Forms.GroupBox groupBox_Zakazky;
        private System.Windows.Forms.ComboBox comboBox_Zakazky;
        private System.Windows.Forms.Button button_UlozitAkce;
        private System.Windows.Forms.Panel panel_FormAkce;
        private System.Windows.Forms.Button button_TiskniPodklady;
        private System.Windows.Forms.Button button_Nacti;
        private System.Windows.Forms.Button button_Smaz;
    }
}