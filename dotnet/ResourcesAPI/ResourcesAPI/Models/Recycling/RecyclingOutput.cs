namespace ResourcesAPI.Models.Recycling
{
    public class RecyclingOutput
    {
        public ushort ItemID { get; private set; }

        public int Quantity { get; private set; }

        public RecyclingOutput(ushort itemId, int quantity)
        {
            this.ItemID = itemId;
            this.Quantity = quantity;
        }
    }
}
