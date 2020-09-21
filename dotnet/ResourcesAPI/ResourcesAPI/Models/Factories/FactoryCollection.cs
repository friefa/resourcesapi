using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ResourcesAPI.Models.Factories
{
    public class FactoryCollection : ICollection<Factory>, ISaveable
    {
        private Factory[] items;

        public FactoryCollection(Factory[] items = default)
        {
            this.items = items;
        }

        public int Count => this.items.Length;

        public bool IsReadOnly => true;

        public void Add(Factory item)
        {
            if (this.items != null && this.items.Length > 0)
            {
                Factory[] buffer = items;

                this.items = new Factory[buffer.Length + 1];

                for (int i = 0; i < buffer.Length; i++)
                {
                    this.items[i] = buffer[i];
                }

                this.items[items.Length] = item;
            }
            else
            {
                this.items = new Factory[] { item };
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

        public Factory GetItem(ushort id)
        {
            foreach (Factory item in this.items)
            {
                if (item.FactoryID == id) return item;
            }

            return default;
        }

        public bool Contains(ushort id)
        {
            return this.Contains(new Factory(id, default, default, default, default));
        }

        public bool Contains(Factory item)
        {
            if (this.items != null & this.items.Length > 0)
            {
                for (int i = 0; i < this.items.Length; i++)
                {
                    if (this.items[i].FactoryID == item.FactoryID) return true;
                }
            }

            return false;
        }

        public void CopyTo(Factory[] array, int arrayIndex)
        {
            this.items.CopyTo(array, arrayIndex);
        }

        public IEnumerator<Factory> GetEnumerator()
        {
            for (int i = 0; i < this.items.Length; i++)
            {
                yield return this.items[i];
            }
        }

        public bool Remove(Factory item)
        {
            Factory[] buffer = new Factory[this.items.Length - 1];
            bool result = false;

            int counter = 0;

            for (int i = this.items.Length; i >= 0; i--)
            {
                if (this.items[i].FactoryID == item.FactoryID)
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

            foreach (Factory item in this.items)
            {
                bin.Write(item.FactoryID);
                bin.Write(item.FactoryName);
                bin.Write(item.ProductID);
                bin.Write(item.BaseUpgradeCost);
                bin.Write(item.MinimumUserLevel);

                int count = item.UpgradeBaseElements.Length;

                bin.Write(count);

                for (int i = 0; i < count; i++)
                {
                    bin.Write(item.UpgradeBaseElements[i].GetType().FullName);
                    bin.Write(item.UpgradeBaseElements[i].UpgradeID);
                    bin.Write(item.UpgradeBaseElements[i].Quantity);
                }
            }

            bin.Close();
            stream.Close();
        }

        public void Load(string filename)
        {
            FileStream stream = new FileStream(filename, FileMode.Open, FileAccess.Read);
            BinaryReader bin = new BinaryReader(stream, Encoding.UTF8);

            if (bin.ReadString().Equals(typeof(FactoryCollection).FullName))
            {
                int number = bin.ReadInt32();

                Factory[] buffer = new Factory[number];

                for (int i = 0; i < number; i++)
                {
                    ushort id = bin.ReadUInt16();
                    string name = bin.ReadString();
                    ushort productId = bin.ReadUInt16();
                    int baseUpgradeCost = bin.ReadInt32();
                    ushort minimumUserLevel = bin.ReadUInt16();
                    int count = bin.ReadInt32();

                    FactoryUpgradeBaseElement[] elements = new FactoryUpgradeBaseElement[count];

                    for (int c = 0; c < count; c++)
                    {
                        if (bin.ReadString().Equals(typeof(FactoryUpgradeBaseElement).FullName))
                        {
                            ushort upgradeId = bin.ReadUInt16();
                            int quantity = bin.ReadInt32();

                            elements[c] = new FactoryUpgradeBaseElement(upgradeId, quantity);
                        }
                    }

                    buffer[i] = new Factory(id, name, productId, baseUpgradeCost, minimumUserLevel, elements);
                }

                this.items = buffer;
            }
        }
    }
}
