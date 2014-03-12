namespace EurogemaIN
{
    partial class ImportOutlookForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImportOutlookForm));
            this.OutlookSplitContainer = new System.Windows.Forms.SplitContainer();
            this.FolderTreeView = new System.Windows.Forms.TreeView();
            this.MessageListView = new System.Windows.Forms.ListView();
            this.EntryIDColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SenderColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SubjectColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SizeColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.RecieveTimeColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.OpenButton = new System.Windows.Forms.Button();
            this.ImportButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.OutlookSplitContainer)).BeginInit();
            this.OutlookSplitContainer.Panel1.SuspendLayout();
            this.OutlookSplitContainer.Panel2.SuspendLayout();
            this.OutlookSplitContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // OutlookSplitContainer
            // 
            this.OutlookSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.OutlookSplitContainer.Location = new System.Drawing.Point(0, 0);
            this.OutlookSplitContainer.Name = "OutlookSplitContainer";
            // 
            // OutlookSplitContainer.Panel1
            // 
            this.OutlookSplitContainer.Panel1.Controls.Add(this.FolderTreeView);
            // 
            // OutlookSplitContainer.Panel2
            // 
            this.OutlookSplitContainer.Panel2.Controls.Add(this.MessageListView);
            this.OutlookSplitContainer.Panel2.Controls.Add(this.OpenButton);
            this.OutlookSplitContainer.Panel2.Controls.Add(this.ImportButton);
            this.OutlookSplitContainer.Size = new System.Drawing.Size(984, 561);
            this.OutlookSplitContainer.SplitterDistance = 234;
            this.OutlookSplitContainer.TabIndex = 0;
            // 
            // FolderTreeView
            // 
            this.FolderTreeView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FolderTreeView.Location = new System.Drawing.Point(0, 0);
            this.FolderTreeView.Name = "FolderTreeView";
            this.FolderTreeView.Size = new System.Drawing.Size(231, 561);
            this.FolderTreeView.TabIndex = 0;
            this.FolderTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.FolderTreeView_AfterSelect);
            // 
            // MessageListView
            // 
            this.MessageListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MessageListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.EntryIDColumnHeader,
            this.SenderColumnHeader,
            this.SubjectColumnHeader,
            this.SizeColumnHeader,
            this.RecieveTimeColumnHeader});
            this.MessageListView.FullRowSelect = true;
            this.MessageListView.Location = new System.Drawing.Point(0, 34);
            this.MessageListView.MultiSelect = false;
            this.MessageListView.Name = "MessageListView";
            this.MessageListView.Size = new System.Drawing.Size(746, 527);
            this.MessageListView.TabIndex = 2;
            this.MessageListView.UseCompatibleStateImageBehavior = false;
            this.MessageListView.View = System.Windows.Forms.View.Details;
            // 
            // EntryIDColumnHeader
            // 
            this.EntryIDColumnHeader.Text = "EntryID";
            this.EntryIDColumnHeader.Width = 0;
            // 
            // SenderColumnHeader
            // 
            this.SenderColumnHeader.Text = "Od";
            this.SenderColumnHeader.Width = 150;
            // 
            // SubjectColumnHeader
            // 
            this.SubjectColumnHeader.Text = "Předmět";
            this.SubjectColumnHeader.Width = 380;
            // 
            // SizeColumnHeader
            // 
            this.SizeColumnHeader.Text = "Velikost";
            this.SizeColumnHeader.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.SizeColumnHeader.Width = 80;
            // 
            // RecieveTimeColumnHeader
            // 
            this.RecieveTimeColumnHeader.Text = "Přijato";
            this.RecieveTimeColumnHeader.Width = 130;
            // 
            // OpenButton
            // 
            this.OpenButton.AutoSize = true;
            this.OpenButton.Location = new System.Drawing.Point(115, 4);
            this.OpenButton.Name = "OpenButton";
            this.OpenButton.Size = new System.Drawing.Size(86, 23);
            this.OpenButton.TabIndex = 1;
            this.OpenButton.Text = "Otevřít zprávu";
            this.OpenButton.UseVisualStyleBackColor = true;
            this.OpenButton.Click += new System.EventHandler(this.OpenButton_Click);
            // 
            // ImportButton
            // 
            this.ImportButton.AutoSize = true;
            this.ImportButton.Location = new System.Drawing.Point(4, 4);
            this.ImportButton.Name = "ImportButton";
            this.ImportButton.Size = new System.Drawing.Size(104, 23);
            this.ImportButton.TabIndex = 0;
            this.ImportButton.Text = "Přenos do Heliosu";
            this.ImportButton.UseVisualStyleBackColor = true;
            this.ImportButton.Click += new System.EventHandler(this.ImportButton_Click);
            // 
            // ImportOutlookForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 561);
            this.Controls.Add(this.OutlookSplitContainer);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ImportOutlookForm";
            this.Text = "Pošta";
            this.OutlookSplitContainer.Panel1.ResumeLayout(false);
            this.OutlookSplitContainer.Panel2.ResumeLayout(false);
            this.OutlookSplitContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.OutlookSplitContainer)).EndInit();
            this.OutlookSplitContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer OutlookSplitContainer;
        private System.Windows.Forms.TreeView FolderTreeView;
        private System.Windows.Forms.Button OpenButton;
        private System.Windows.Forms.Button ImportButton;
        private System.Windows.Forms.ListView MessageListView;
        private System.Windows.Forms.ColumnHeader EntryIDColumnHeader;
        private System.Windows.Forms.ColumnHeader SenderColumnHeader;
        private System.Windows.Forms.ColumnHeader SubjectColumnHeader;
        private System.Windows.Forms.ColumnHeader SizeColumnHeader;
        private System.Windows.Forms.ColumnHeader RecieveTimeColumnHeader;
    }
}