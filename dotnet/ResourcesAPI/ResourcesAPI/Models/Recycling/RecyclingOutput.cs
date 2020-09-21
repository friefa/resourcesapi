using ResourcesAPI.Models.Basic;

namespace ResourcesAPI.Models.Recycling
{
    public class RecyclingOutput : UpgradeElement
    {
        public RecyclingOutput(ushort itemId, int quantity) : base(itemId, quantity) { }
    }
}
