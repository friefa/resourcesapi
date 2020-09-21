using ResourcesAPI.Models.Basic;

namespace ResourcesAPI.Models.Production
{
    public class ProductionIngredient : UpgradeElement
    {
        public ProductionIngredient(ushort itemId, int quantityPerCylce) : base(itemId, quantityPerCylce) { }
    }
}
