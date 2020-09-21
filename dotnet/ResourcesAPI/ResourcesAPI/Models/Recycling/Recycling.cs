namespace ResourcesAPI.Models.Recycling
{
    public class Recycling
    {
        public ushort ItemID { get; private set; }

        public string ItemName { get; private set; }

        public int InputQuantity { get; private set; }

        public RecyclingOutput[] RecyclingOutputs { get; private set; }

        public Recycling(ushort itemId, string itemName, int inputQuantity, params RecyclingOutput[] recyclingOutputs)
        {
            this.ItemID = itemId;
            this.ItemName = itemName;
            this.InputQuantity = inputQuantity;
            this.RecyclingOutputs = recyclingOutputs;
        }
    }
}
