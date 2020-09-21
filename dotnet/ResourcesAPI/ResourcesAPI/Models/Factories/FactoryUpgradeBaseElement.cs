namespace ResourcesAPI.Models.Factories
{
    public class FactoryUpgradeBaseElement
    {
        public ushort UpgradeID { get; private set; }

        public int Quantity { get; private set; }

        public FactoryUpgradeBaseElement(ushort upgradeId, int quantity)
        {
            this.UpgradeID = upgradeId;
            this.Quantity = quantity;
        }
    }
}
