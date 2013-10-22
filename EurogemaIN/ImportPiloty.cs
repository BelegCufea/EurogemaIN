using System;
using ddPlugin;

namespace EurogemaIN
{
    public class ImportPiloty : IHePlugin2
    {
        public string PartnerIdentification()
        {
            return @"HEIQ0100-23351";
        }

        public void Run(IHelios Helios)
        {
            Int32 ID = Helios.CurrentRecordID();
            ImportPilotyForm Import = new ImportPilotyForm(Helios, ID);
            Import.ShowDialog();
            Import.Dispose();
        }
    }
}
