using System;
using ddPlugin;

namespace EurogemaIN
{
    public class SchvalovaniFaktury : IHePlugin2
    {
        public string PartnerIdentification()
        {
            return @"HEIQ0100-23351";
        }

        public void Run(IHelios Helios)
        {
            Int32 ID = Helios.CurrentRecordID();
            SchvalovaniFakturyForm SchvalovaniFaktury = new SchvalovaniFakturyForm(Helios, ID);
            SchvalovaniFaktury.ShowDialog();
            SchvalovaniFaktury.Dispose();
        }
    }
}
