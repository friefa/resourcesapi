using System;
using System.Collections.Generic;
using System.Text;

namespace ResourcesAPI.Models.Factories
{
    public class Factory
    {
        public ushort FactoryID { get; private set; }

        public string FactoryName { get; private set; }

        public ushort ProductID { get; private set; }

        public int BaseUpgradeCost { get; private set; }

        public ushort MinimumUserLevel { get; private set; }

        public FactoryUpgradeElement[] UpgradeBaseElements { get; private set; }

        public Factory(ushort factoryId, string factoryName, ushort productId, int baseUpgradeCost, ushort minimumUserLevel, params FactoryUpgradeElement[] factoryUpgradeBaseElements)
        {
            this.FactoryID = factoryId;
            this.FactoryName = factoryName;
            this.ProductID = productId;
            this.BaseUpgradeCost = baseUpgradeCost;
            this.MinimumUserLevel = minimumUserLevel;
            this.UpgradeBaseElements = factoryUpgradeBaseElements;
        }
    }
}
