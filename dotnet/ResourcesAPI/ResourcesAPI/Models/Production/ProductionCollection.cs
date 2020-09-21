using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ResourcesAPI.Models.Production
{
    public class ProductionCollection : ICollection<Production>, ISaveable
    {
        private Production[] items;

        public ProductionCollection(Production[] items = default)
        {
            this.items = items;
        }

        public int Count => this.items.Length;

        public bool IsReadOnly => true;

        public void Add(Production item)
        {
            if (this.items != null && this.items.Length > 0)
            {
                Production[] buffer = items;

                this.items = new Production[buffer.Length + 1];

                for (int i = 0; i < buffer.Length; i++)
                {
                    this.items[i] = buffer[i];
                }

                this.items[items.Length] = item;
            }
            else
            {
                this.items = new Production[] { item };
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

        public Production GetItem(ushort id)
        {
            foreach (Production item in this.items)
            {
                if (item.ItemID == id) return item;
            }

            return default;
        }

        public bool Contains(ushort id)
        {
            return this.Contains(new Production(id, default, default, default, default, default, default, default));
        }

        public bool Contains(Production item)
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

        public void CopyTo(Production[] array, int arrayIndex)
        {
            this.items.CopyTo(array, arrayIndex);
        }

        public IEnumerator<Production> GetEnumerator()
        {
            for (int i = 0; i < this.items.Length; i++)
            {
                yield return this.items[i];
            }
        }

        public bool Remove(Production item)
        {
            Production[] buffer = new Production[this.items.Length - 1];
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

            foreach (Production item in this.items)
            {
                bin.Write(item.ItemID);
                bin.Write(item.ItemName);
                bin.Write(item.FactoryID);
                bin.Write(item.FactoryName);
                bin.Write(item.BaseOutputPerHour);
                bin.Write(item.OutputPerCycle);
                bin.Write(item.CreditsPerCycle);

                int count = item.Ingredients.Length;

                bin.Write(count);

                for (int i = 0; i < count; i++)
                {
                    bin.Write(typeof(ProductionIngredient).FullName);
                    bin.Write(item.Ingredients[i].ItemID);
                    bin.Write(item.Ingredients[i].QuantityPerCycle);
                }                
            }

            bin.Close();
            stream.Close();
        }

        public void Load(string filename)
        {
            FileStream stream = new FileStream(filename, FileMode.Open, FileAccess.Read);
            BinaryReader bin = new BinaryReader(stream, Encoding.UTF8);

            if (bin.ReadString().Equals(typeof(ProductionCollection).FullName))
            {
                int number = bin.ReadInt32();

                Production[] buffer = new Production[number];

                for (int i = 0; i < number; i++)
                {
                    ushort itemId = bin.ReadUInt16();
                    string itemName = bin.ReadString();
                    ushort factoryId = bin.ReadUInt16();
                    string factoryName = bin.ReadString();
                    int baseOutputPerHour = bin.ReadInt32();
                    int outputPerCycle = bin.ReadInt32();
                    int creditsPerCycle = bin.ReadInt32();

                    int count = bin.ReadInt32();

                    ProductionIngredient[] ingredients = new ProductionIngredient[count];

                    for (int c = 0; c < count; c++)
                    {
                        if (bin.ReadString().Equals(typeof(ProductionIngredient).FullName))
                        {
                            ushort ingredientItemId = bin.ReadUInt16();
                            int ingredientQuantityPerCycle = bin.ReadInt32();

                            ingredients[c] = new ProductionIngredient(ingredientItemId, ingredientQuantityPerCycle);
                        }                        
                    }

                    buffer[i] = new Production(itemId, itemName, factoryId, factoryName, baseOutputPerHour, outputPerCycle, creditsPerCycle, ingredients);
                }

                this.items = buffer;
            }
        }
    }
}
