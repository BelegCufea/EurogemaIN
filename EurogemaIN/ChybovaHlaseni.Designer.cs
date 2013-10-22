namespace EurogemaIN
{
    partial class ChybovaHlaseni
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
            this.textBox_Chyby = new System.Windows.Forms.TextBox();
            this.button_OK = new System.Windows.Forms.Button();
            this.label_Nadpis = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // textBox_Chyby
            // 
            this.textBox_Chyby.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_Chyby.Location = new System.Drawing.Point(12, 33);
            this.textBox_Chyby.Multiline = true;
            this.textBox_Chyby.Name = "textBox_Chyby";
            this.textBox_Chyby.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_Chyby.Size = new System.Drawing.Size(356, 484);
            this.textBox_Chyby.TabIndex = 2;
            this.textBox_Chyby.WordWrap = false;
            // 
            // button_OK
            // 
            this.button_OK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_OK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button_OK.Location = new System.Drawing.Point(293, 523);
            this.button_OK.Name = "button_OK";
            this.button_OK.Size = new System.Drawing.Size(75, 23);
            this.button_OK.TabIndex = 0;
            this.button_OK.Text = "OK";
            this.button_OK.UseVisualStyleBackColor = true;
            this.button_OK.Click += new System.EventHandler(this.button_OK_Click);
            // 
            // label_Nadpis
            // 
            this.label_Nadpis.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label_Nadpis.Location = new System.Drawing.Point(13, 13);
            this.label_Nadpis.Name = "label_Nadpis";
            this.label_Nadpis.Size = new System.Drawing.Size(355, 17);
            this.label_Nadpis.TabIndex = 1;
            this.label_Nadpis.Text = "Akce nedokončena";
            // 
            // ChybovaHlaseni
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(380, 558);
            this.Controls.Add(this.label_Nadpis);
            this.Controls.Add(this.button_OK);
            this.Controls.Add(this.textBox_Chyby);
            this.Name = "ChybovaHlaseni";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Chybová hlášení";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_OK;
        public System.Windows.Forms.TextBox textBox_Chyby;
        public System.Windows.Forms.Label label_Nadpis;
    }
}