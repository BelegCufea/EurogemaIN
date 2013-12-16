using System;
using ddPlugin;

namespace EurogemaIN
{
    public class ZobrazDokument : IHePlugin2
    {
        public string PartnerIdentification()
        {
            return @"HEIQ0100-23351";
        }

        public void Run(IHelios Helios)
        {
            Int32 ID = Helios.CurrentRecordID();
            Int32 IDB = Helios.BrowseID();
            //Zobrazí první připojený dokument na základě tabulky miniautoskenu
            String SQL = "SELECT TOP 1 D.JmenoACesta FROM TabDokumenty AS D INNER JOIN TabDokumVazba AS DV ON DV.IdDok = D.ID INNER JOIN BKO_mini_autoscan_settings AS MAS ON MAS.IdentVazby = DV.IdentVazby WHERE MAS.CisloPrehledu = " + IDB + " AND DV.IdTab = " + ID;
            IHeQuery Soubor = Helios.OpenSQL(SQL);
            if (Soubor.RecordCount() == 1)
            {
                System.Diagnostics.Process.Start((String)(Soubor.FieldValues(0)));
            }
        }
    }
}
