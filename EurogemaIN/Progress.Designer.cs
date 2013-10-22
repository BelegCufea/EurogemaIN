namespace EurogemaIN
{
    partial class Progress
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
            this.label_Popis = new System.Windows.Forms.Label();
            this.label_Prvek = new System.Windows.Forms.Label();
            this.progressBar_Postup = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // label_Popis
            // 
            this.label_Popis.AutoSize = true;
            this.label_Popis.Location = new System.Drawing.Point(12, 9);
            this.label_Popis.Name = "label_Popis";
            this.label_Popis.Size = new System.Drawing.Size(35, 13);
            this.label_Popis.TabIndex = 0;
            this.label_Popis.Text = "label1";
            // 
            // label_Prvek
            // 
            this.label_Prvek.AutoSize = true;
            this.label_Prvek.Location = new System.Drawing.Point(12, 31);
            this.label_Prvek.Name = "label_Prvek";
            this.label_Prvek.Size = new System.Drawing.Size(35, 13);
            this.label_Prvek.TabIndex = 1;
            this.label_Prvek.Text = "label1";
            // 
            // progressBar_Postup
            // 
            this.progressBar_Postup.Location = new System.Drawing.Point(15, 48);
            this.progressBar_Postup.Name = "progressBar_Postup";
            this.progressBar_Postup.Size = new System.Drawing.Size(426, 23);
            this.progressBar_Postup.Step = 1;
            this.progressBar_Postup.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar_Postup.TabIndex = 2;
            // 
            // Progress
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(453, 80);
            this.Controls.Add(this.progressBar_Postup);
            this.Controls.Add(this.label_Prvek);
            this.Controls.Add(this.label_Popis);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Progress";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Probíhá akce ...";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Label label_Popis;
        public System.Windows.Forms.Label label_Prvek;
        public System.Windows.Forms.ProgressBar progressBar_Postup;

    }
}