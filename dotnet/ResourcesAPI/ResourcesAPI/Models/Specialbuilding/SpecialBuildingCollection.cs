using ResourcesAPI.Models.Interfaces;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ResourcesAPI.Models.Specialbuilding
{
    public class SpecialBuildingCollection : ICollection<Specialbuilding>, ISaveable
    {
        private Specialbuilding[] items;

        public SpecialBuildingCollection(Specialbuilding[] items = default)
        {
            this.items = items;
        }

        public int Count => this.items.Length;

        public bool IsReadOnly => true;

        public void Add(Specialbuilding item)
        {
            if (this.items != null && this.items.Length > 0)
            {
                Specialbuilding[] buffer = items;

                this.items = new Specialbuilding[buffer.Length + 1];

                for (int i = 0; i < buffer.Length; i++)
                {
                    this.items[i] = buffer[i];
                }

                this.items[items.Length] = item;
            }
            else
            {
                this.items = new Specialbuilding[] { item };
            }
        }

        public void Clear()
        {
            for (int i = 0; i < this.items.Length; i++)
            {
                this.items[i] = null;
            }

            this.items = null;
        }

        public Specialbuilding GetItem(ushort id)
        {
            foreach (Specialbuilding item in this.items)
            {
                if (item.ItemID == id) return item;
            }

            return default;
        }

        public bool Contains(ushort id)
        {
            return this.Contains(new Specialbuilding(id, default, default, default));
        }

        public bool Contains(Specialbuilding item)
        {
            if (this.items != null & this.items.Length > 0)
            {
                for (int i = 0; i < this.items.Length; i++)
                {
                    if (this.items[i].ItemID == item.ItemID) return true;
                }
            }

            return false;
        }

        public void CopyTo(Specialbuilding[] array, int arrayIndex)
        {
            this.items.CopyTo(array, arrayIndex);
        }

        public IEnumerator<Specialbuilding> GetEnumerator()
        {
            for (int i = 0; i < this.items.Length; i++)
            {
                yield return this.items[i];
            }
        }

        public bool Remove(Specialbuilding item)
        {
            Specialbuilding[] buffer = new Specialbuilding[this.items.Length - 1];
            bool result = false;

            int counter = 0;

            for (int i = this.items.Length; i >= 0; i--)
            {
                if (this.items[i].ItemID == item.ItemID)
                {
                    result = true;
                    i--;
                }

                buffer[counter] = this.items[i];

                counter++;
            }

            this.items = buffer;
            return result;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (this.items != null) ? this.items.GetEnumerator() : default;
        }

        public void Save(string filename)
        {
            FileStream stream = new FileStream(filename, FileMode.Create, FileAccess.Write);
            BinaryWriter bin = new BinaryWriter(stream, Encoding.UTF8);

            bin.Write(this.GetType().FullName);
            bin.Write(this.items.Length);

            foreach (Specialbuilding item in this.items)
            {
                bin.Write(item.ItemID);
                bin.Write(item.BuildingName);
                bin.Write(item.BaseUpgradeCost);

                int count = item.SpecialbuildingUpgradeElements.Length;

                bin.Write(count);

                for (int i = 0; i < count; i++)
                {
                    bin.Write(typeof(SpecialbuildingUpgradeElement).FullName);
                    bin.Write(item.SpecialbuildingUpgradeElements[i].ItemID);
                    bin.Write(item.SpecialbuildingUpgradeElements[i].Quantity);
                }
            }

            bin.Close();
            stream.Close();
        }

        public void Load(string filename)
        {
            FileStream stream = new FileStream(filename, FileMode.Open, FileAccess.Read);
            BinaryReader bin = new BinaryReader(stream, Encoding.UTF8);

            if (bin.ReadString().Equals(typeof(Specialbuilding).FullName))
            {
                int number = bin.ReadInt32();

                Specialbuilding[] buffer = new Specialbuilding[number];

                for (int i = 0; i < number; i++)
                {
                    ushort itemId = bin.ReadUInt16();
                    string buildingName = bin.ReadString();
                    int baseUpgradeCost = bin.ReadInt32();

                    int count = bin.ReadInt32();

                    SpecialbuildingUpgradeElement[] upgradeElements = new SpecialbuildingUpgradeElement[count];

                    for (int c = 0; c < count; c++)
                    {
                        if (bin.ReadString().Equals(typeof(SpecialbuildingUpgradeElement).FullName))
                        {
                            ushort _itemId = bin.ReadUInt16();
                            int quantity = bin.ReadInt32();

                            upgradeElements[c] = new SpecialbuildingUpgradeElement(_itemId, quantity);
                        }
                    }

                    buffer[i] = new Specialbuilding(itemId, buildingName, baseUpgradeCost, upgradeElements);
                }

                this.items = buffer;
            }
        }
    }
}
