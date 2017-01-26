using ddPlugin;
using System;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace EurogemaIN
{
    public partial class ImportPilotyForm : Form
    {
        private IHelios Helios;
        private Int32 ID;
        public ImportPilotyForm(IHelios MainHelios, Int32 RecordID)
        {
            this.InitializeComponent();
            Helios = MainHelios;
            ID = RecordID;
        }

        private void buttonChooseFile_Click(object sender, EventArgs e)
        {
            openFileDialogPiloty.ShowDialog();
            textBoxFileName.Text = openFileDialogPiloty.FileName;
        }

        private void buttonImport_Click(object sender, EventArgs e)
        {
            String Polozka;
            String SQL;
            Int32 CisloRadku;
            Boolean DalsiRadek;
            String RegCis;
            String Upresneni;
            Double Mnozstvi;
            Double CC;
            Int32 Kusy;
            String Typ;

            Excel.Application xlApp;
            Excel.Workbook xlWorkBook;
            Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;

            xlApp = new Excel.Application();
            xlWorkBook = xlApp.Workbooks.Open(textBoxFileName.Text, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets["Helios"];

            Polozka = xlWorkSheet.Range["G1"].Value2.ToString();
            SQL = "UPDATE TabDokladyZbozi SET PopisDodavky = '" + Polozka + "' WHERE ID = " + ID;
            Helios.ExecSQL(SQL);

            SQL = "DELETE FROM TabPohybyZbozi WHERE IDDoklad = " + ID;
            Helios.ExecSQL(SQL);

            CisloRadku = 6;
            DalsiRadek = true;

            textBoxDebug.Text = "Importuji nabídkové položky:" + Environment.NewLine + Environment.NewLine;

            do
            {
                if (xlWorkSheet.Range["A" + CisloRadku, "A" + CisloRadku].Value2 != null)
                {
                    if ((xlWorkSheet.Range["G" + CisloRadku, "G" + CisloRadku].Value2 != null) && (xlWorkSheet.Range["G" + CisloRadku, "G" + CisloRadku].Value2 != 0))
                    {
                        RegCis = xlWorkSheet.Range["A" + CisloRadku, "A" + CisloRadku].Value2.ToString();
                        textBoxDebug.AppendText(RegCis);
                        textBoxDebug.AppendText(" ");
                        textBoxDebug.AppendText(xlWorkSheet.Range["B" + CisloRadku, "B" + CisloRadku].Value2.ToString());
                        textBoxDebug.AppendText(" ");
                        Upresneni = "";
                        if (xlWorkSheet.Range["C" + CisloRadku, "C" + CisloRadku].Value2 != null)
                        {
                            textBoxDebug.AppendText(" - ");
                            Upresneni = xlWorkSheet.Range["C" + CisloRadku, "C" + CisloRadku].Value2.ToString();
                            textBoxDebug.AppendText(Upresneni);
                        }
                        textBoxDebug.AppendText(Environment.NewLine);
                        Mnozstvi = xlWorkSheet.Range["D" + CisloRadku, "D" + CisloRadku].Value2;
                        CC = xlWorkSheet.Range["G" + CisloRadku, "G" + CisloRadku].Value2;
                        Kusy = Convert.ToInt32(xlWorkSheet.Range["H" + CisloRadku, "H" + CisloRadku].Value2);

                        SQL = "EXEC [dbo].[EGPohybyZboziNovy] '" + RegCis + "', '" + Upresneni + "', " + Mnozstvi.ToString().Replace(',', '.') + ", " + CC.ToString().Replace(',', '.') + ", " + Kusy.ToString() + ", " + ID.ToString();
                        Helios.ExecSQL(SQL);
                    }
                    CisloRadku++;
                }
                else
                    DalsiRadek = false;
            } while (DalsiRadek);

            CisloRadku = 45;
            DalsiRadek = true;

            textBoxDebug.AppendText(Environment.NewLine + Environment.NewLine + "Importuji nákladové položky:" + Environment.NewLine + Environment.NewLine);
            SQL = "DELETE FROM TabDVV4036079C7B2E40F3B9DC2CDCF4C7201C WHERE L_ID = " + ID.ToString();
            Helios.ExecSQL(SQL);

            do
            {
                if (xlWorkSheet.Range["B" + CisloRadku, "B" + CisloRadku].Value2 != null)
                {
                    if ((xlWorkSheet.Range["C" + CisloRadku, "C" + CisloRadku].Value2 != null) && (xlWorkSheet.Range["C" + CisloRadku, "C" + CisloRadku].Value2 != 0))
                    {
                        Typ = xlWorkSheet.Range["B" + CisloRadku, "B" + CisloRadku].Value2.ToString();
                        textBoxDebug.AppendText(Typ);
                        textBoxDebug.AppendText(Environment.NewLine);
                        CC = xlWorkSheet.Range["C" + CisloRadku, "C" + CisloRadku].Value2;
                        SQL = "INSERT INTO TabDVV4036079C7B2E40F3B9DC2CDCF4C7201C (L_ID, P_Typ, DVAtr_Pozadavek) VALUES (" + ID.ToString() + ",'" + Typ + "'," + CC.ToString().Replace(',', '.') + ")";
                        Helios.ExecSQL(SQL);
                    }
                    CisloRadku++;
                }
                else
                    DalsiRadek = false;
            } while (DalsiRadek);

            Helios.Refresh(false);

            xlWorkBook.Close(false, misValue, misValue);
            xlApp.Quit();

            releaseObject(xlWorkSheet);
            releaseObject(xlWorkBook);
            releaseObject(xlApp);
        }

        private void buttonKonec_Click(object sender, EventArgs e)
        {
            this.Close();
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
    }
}