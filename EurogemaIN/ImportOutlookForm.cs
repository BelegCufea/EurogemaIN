using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ddPlugin;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace EurogemaIN
{
    public partial class ImportOutlookForm : Form
    {
        private Int32 ID;
        private Int32 BrowseID;
        private IHelios Helios;
        private Progress Postup;

        public ImportOutlookForm(IHelios MainHelios, Int32 RecordID)
        {
            InitializeComponent();
            Helios = MainHelios;
            ID = RecordID;
            BrowseID = Helios.BrowseID();
            EnumerateStores();
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

        private void EnumerateFolders(Outlook.Folder folder, TreeNode folderNode)
        {
            Outlook.Folders childFolders =
                folder.Folders;
            if (childFolders.Count > 0)
            {
                foreach (Outlook.Folder childFolder in childFolders)
                {
                    if (childFolder.DefaultItemType == Outlook.OlItemType.olMailItem)
                    {
                        TreeNode childNode = new TreeNode(childFolder.Name); //FolderPath
                        EnumerateFolders(childFolder, childNode);
                        folderNode.Nodes.Add(childNode);
                    }
                }
            }
        }

        private void EnumerateStores()
        {

            if (Postup == null)
                Postup = new Progress();
            Postup.Show();
            Postup.label_Popis.Text = "Načítají se poštovní schránky";
            Postup.label_Prvek.Text = "";
            Postup.progressBar_Postup.Value = 0;
            Postup.Show();

            FolderTreeView.Nodes.Clear();
            Outlook.Application oApp = new Outlook.Application();
            foreach (Outlook.Store store in oApp.Session.Stores)
            {
                Outlook.Folder folder = store.GetRootFolder() as Outlook.Folder;
                Postup.label_Prvek.Text = folder.Name;
                Postup.Refresh();
                TreeNode folderNode = new TreeNode(folder.Name);
                EnumerateFolders(folder, folderNode);
                FolderTreeView.Nodes.Add(folderNode);
            }
            FolderTreeView.Sort();
            Postup.Hide();
        }

        private void FolderTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            MessageListView.BeginUpdate();
            MessageListView.Items.Clear();

            if (Postup == null)
                Postup = new Progress();
            Postup.Show();
            Postup.label_Popis.Text = "Načítají se zprávy poštovní schránky " + e.Node.FullPath;
            Postup.label_Prvek.Text = "";
            Postup.progressBar_Postup.Value = 0;
            Postup.Show();

            Outlook.Folder mailFolder;
            string folderPath = e.Node.FullPath;

            string backslash = @"\";
            if (folderPath.StartsWith(@"\\"))
            {
                folderPath = folderPath.Remove(0, 2);
            }
            String[] folders = folderPath.Split(backslash.ToCharArray());
            Outlook.Application oApp = new Outlook.Application();
            mailFolder = oApp.Session.Folders[folders[0]] as Outlook.Folder;
            if (mailFolder != null)
            {
                for (int i = 1; i <= folders.GetUpperBound(0); i++)
                {
                    Outlook.Folders subFolders = mailFolder.Folders;
                    mailFolder = subFolders[folders[i]] as Outlook.Folder;
                }
            }

            Outlook.Table mailsTable = mailFolder.GetTable(Type.Missing, Outlook.OlTableContents.olUserItems);
            mailsTable.Columns.RemoveAll();
            mailsTable.Columns.Add("EntryID");
            mailsTable.Columns.Add("SenderName");
            mailsTable.Columns.Add("Subject");
            mailsTable.Columns.Add("Size");
            mailsTable.Columns.Add("ReceivedTime");
            mailsTable.Sort("ReceivedTime", Outlook.OlSortOrder.olDescending);

            Int32 Celkem = mailsTable.GetRowCount();
            Int32 Krok = 1;
            Postup.progressBar_Postup.Maximum = Celkem;

            while (!mailsTable.EndOfTable)
            {
                Outlook.Row nextRow = mailsTable.GetNextRow();
                Postup.label_Prvek.Text = Krok++.ToString() + "/" + Celkem.ToString();
                Postup.progressBar_Postup.PerformStep();
                Postup.Refresh();
                ListViewItem item = new ListViewItem(nextRow["EntryID"]);
                item.SubItems.Add(nextRow["SenderName"] ?? "");
                item.SubItems.Add(nextRow["Subject"] ?? "");
                item.SubItems.Add(((Double)(nextRow["Size"] / 1024)).ToString("#,0 kB"));
                item.SubItems.Add(nextRow["ReceivedTime"].ToString());
                MessageListView.Items.Add(item);
            }

            Postup.Hide();

            MessageListView.EndUpdate();
        }

        private void OpenButton_Click(object sender, EventArgs e)
        {
            if (MessageListView.SelectedItems.Count > 0)
            {
                Outlook.Application oApp = new Outlook.Application();
                Outlook.MailItem mail = oApp.Session.GetItemFromID(MessageListView.SelectedItems[0].Text);
                mail.Display(false);
            }
        }

        private void ImportToHelios()
        {
            if (MessageListView.SelectedItems.Count > 0)
            {
                Outlook.Application oApp = new Outlook.Application();
                Outlook.MailItem mail = oApp.Session.GetItemFromID(MessageListView.SelectedItems[0].Text);

                string SQL = @"SELECT IdentVazby, Cesta, Prefix, idTab FROM BKO_mini_autoscan_settings WHERE CisloPrehledu = " + BrowseID;

                IHeQuery SettingsQuery = Helios.OpenSQL(SQL);

                SQL = @"SELECT " + (string)SettingsQuery.FieldValues(2) + ", " + (string)SettingsQuery.FieldValues(3) + ", SUSER_SNAME() FROM " + Helios.MainBrowseTable() + " WHERE ID = " + ID;

                IHeQuery FileNameQuery = Helios.OpenSQL(SQL);

                string msgfilepath = (string)SettingsQuery.FieldValues(1);
                string msgfilename = (string)FileNameQuery.FieldValues(0) + "-" + ((string)(FileNameQuery.FieldValues(2))).Substring(((string)(FileNameQuery.FieldValues(2))).LastIndexOf(@"\") + 1);
                string msgfileextension = @".msg";
                Int32 poradi = 2;
                Int32 poradicur;

                if (File.Exists(msgfilepath + @"\" + msgfilename + msgfileextension))
                {
                    msgfilename += @"-";

                    string[] filePaths = Directory.GetFiles(msgfilepath, msgfilename + @"*" + msgfileextension);

                    if (filePaths.Count() >= 1)
                    {


                        foreach (string filepath in filePaths)
                        {
                            string filename = Path.GetFileNameWithoutExtension(filepath);
                            string poradistr = filename.Substring(filename.LastIndexOf(@"-") + 1);
                            poradicur = 0;
                            if (Int32.TryParse(poradistr, out poradicur))
                                poradi = poradicur >= poradi ? poradicur + 1 : poradi;
                        }

                    }

                    msgfilename += poradi.ToString();

                }

                string msgpathname = msgfilepath + @"\" + msgfilename + msgfileextension;
                string msgdescription = mail.Subject.Length <= 255 ? mail.Subject : mail.Subject.Substring(0,255);

                mail.SaveAs(msgpathname, Outlook.OlSaveAsType.olMSG);

                SQL = @"INSERT INTO TabDokumenty (Popis, JmenoACesta) VALUES ('" + msgdescription + "', '" + msgpathname + "')";
                Helios.ExecSQL(SQL);

                SQL = "SELECT ID FROM TabDokumenty WHERE JmenoACesta = '" + msgpathname + "'";
                IHeQuery DokumentQuery = Helios.OpenSQL(SQL);

                SQL = "INSERT INTO TabDokumVazba (IdentVazby, IdTab, IdDok) VALUES (" + SettingsQuery.FieldValues(0) + ", " + FileNameQuery.FieldValues(1) + ", " + DokumentQuery.FieldValues(0) + ")";
                Helios.ExecSQL(SQL);

                Helios.Info("Import dokončen");


            }
        }

        private void ImportButton_Click(object sender, EventArgs e)
        {
            ImportToHelios();
        }

    }
}
