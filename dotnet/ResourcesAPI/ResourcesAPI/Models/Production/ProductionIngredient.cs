using System;
using System.Collections.Generic;
using System.Text;

namespace ResourcesAPI.Models.Production
{
    public class ProductionIngredient
    {
        public ushort ItemID { get; private set; }

        public int QuantityPerCycle { get; private set; }

        public ProductionIngredient(ushort id, int quantityPerCylce)
        {
            this.ItemID = id;
            this.QuantityPerCycle = quantityPerCylce;
        }
    }
}
