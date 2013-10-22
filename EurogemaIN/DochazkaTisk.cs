using System;
using ddPlugin;

namespace EurogemaIN
{
    public class DochazkaTisk : IHePlugin2
    {
        public string PartnerIdentification()
        {
            return @"HEIQ0100-23351";
        }

        public void Run(IHelios Helios)
        {
            Helios.DocasnaTabulkaZOznacenych("#TabOznaceneID", false);
            String SQL = "SELECT ID FROM #TabOznaceneID";
            String User = Helios.LoginName();
            if (Helios.BrowseID() == 100133)
            {
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
            if (Helios.BrowseID() == 100134)
            {
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
            }
            Helios.ExecSQL("DROP TABLE #TabOznaceneID");
        }
    }
}
