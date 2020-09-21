namespace ResourcesAPI.Models.Basic
{
    public abstract class UpgradeElement
    {
        public ushort ItemID { get; private set; }

        public int Quantity { get; private set; }

        public UpgradeElement(ushort itemId, int quantity)
        {
            this.ItemID = itemId;
            this.Quantity = quantity;
        }
    }
}
