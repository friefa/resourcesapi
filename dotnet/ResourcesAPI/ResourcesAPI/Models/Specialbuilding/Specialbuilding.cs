namespace ResourcesAPI.Models.Specialbuilding
{
    public class Specialbuilding
    {
        public ushort ItemID { get; private set; }

        public string BuildingName { get; private set; }

        public int BaseUpgradeCost { get; private set; }

        public SpecialbuildingUpgradeElement[] SpecialbuildingUpgradeElements { get; private set; }

        public Specialbuilding(ushort itemId, string buildingName, int baseUpgradeCost, params SpecialbuildingUpgradeElement[] specialbuildingUpgradeElements)
        {
            this.ItemID = itemId;
            this.BuildingName = buildingName;
            this.BaseUpgradeCost = baseUpgradeCost;
        }
    }
}
