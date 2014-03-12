using System;
using ddPlugin;

namespace EurogemaIN
{
    public class ImportOutlook : IHePlugin2
    {
        public string PartnerIdentification()
        {
            return @"HEIQ0100-23351";
        }

        public void Run(IHelios Helios)
        {
            Int32 ID = Helios.CurrentRecordID();
            ImportOutlookForm Import = new ImportOutlookForm(Helios, ID);
            Import.ShowDialog();
            Import.Dispose();
        }
    }
}
