namespace ResourcesAPI.Models.Production
{
    public class Production
    {
        public ushort ItemID { get; private set; }

        public string ItemName { get; private set; }

        public ushort FactoryID { get; private set; }

        public string FactoryName { get; private set; }

        public int BaseOutputPerHour { get; private set; }

        public int OutputPerCycle { get; private set; }

        public int CreditsPerCycle { get; private set; }

        public ProductionIngredient[] Ingredients { get; private set; }

        public Production(ushort itemId, string itemName, ushort factoryId, string factoryName, int baseOutputPerHour, int outputPerCylce, int creditsPerCycle, params ProductionIngredient[] ingredients)
        {
            this.ItemID = itemId;
            this.ItemName = itemName;
            this.FactoryID = factoryId;
            this.FactoryName = factoryName;
            this.BaseOutputPerHour = baseOutputPerHour;
            this.OutputPerCycle = outputPerCylce;
            this.CreditsPerCycle = creditsPerCycle;
            this.Ingredients = ingredients;
        }
    }
}
