using System;
using ddPlugin;

namespace EurogemaIN
{
    public class Dochazka : IHePlugin2
    {
        public string PartnerIdentification()
        {
            return @"HEIQ0100-23351";
        }

        public void Run(IHelios Helios)
        {
            Int32 ID = Helios.CurrentRecordID();
            DochazkaForm DochazkaDialog = new DochazkaForm(Helios, ID);
            DochazkaDialog.ShowDialog();
            DochazkaDialog.Dispose();
        }
    }
}
