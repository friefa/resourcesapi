using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ResourcesAPI.Models.Recycling
{
    public class RecyclingCollection : ICollection<Recycling>, ISaveable
    {
        private Recycling[] items;

        public RecyclingCollection(Recycling[] items = default)
        {
            this.items = items;
        }

        public int Count => this.items.Length;

        public bool IsReadOnly => true;

        public void Add(Recycling item)
        {
            if (this.items != null && this.items.Length > 0)
            {
                Recycling[] buffer = items;

                this.items = new Recycling[buffer.Length + 1];

                for (int i = 0; i < buffer.Length; i++)
                {
                    this.items[i] = buffer[i];
                }

                this.items[items.Length] = item;
            }
            else
            {
                this.items = new Recycling[] { item };
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

        public Recycling GetItem(ushort id)
        {
            foreach (Recycling item in this.items)
            {
                if (item.ItemID == id) return item;
            }

            return default;
        }

        public bool Contains(ushort id)
        {
            return this.Contains(new Recycling(id, default, default, default));
        }

        public bool Contains(Recycling item)
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

        public void CopyTo(Recycling[] array, int arrayIndex)
        {
            this.items.CopyTo(array, arrayIndex);
        }

        public IEnumerator<Recycling> GetEnumerator()
        {
            for (int i = 0; i < this.items.Length; i++)
            {
                yield return this.items[i];
            }
        }

        public bool Remove(Recycling item)
        {
            Recycling[] buffer = new Recycling[this.items.Length - 1];
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

            foreach (Recycling item in this.items)
            {
                bin.Write(item.ItemID);
                bin.Write(item.ItemName);
                bin.Write(item.InputQuantity);

                int count = item.RecyclingOutputs.Length;

                bin.Write(count);

                for (int i = 0; i < count; i++)
                {
                    bin.Write(typeof(RecyclingOutput).FullName);
                    bin.Write(item.RecyclingOutputs[i].ItemID);
                    bin.Write(item.RecyclingOutputs[i].Quantity);
                }
            }

            bin.Close();
            stream.Close();
        }

        public void Load(string filename)
        {
            FileStream stream = new FileStream(filename, FileMode.Open, FileAccess.Read);
            BinaryReader bin = new BinaryReader(stream, Encoding.UTF8);

            if (bin.ReadString().Equals(typeof(RecyclingCollection).FullName))
            {
                int number = bin.ReadInt32();

                Recycling[] buffer = new Recycling[number];

                for (int i = 0; i < number; i++)
                {
                    ushort itemId = bin.ReadUInt16();
                    string itemName = bin.ReadString();
                    int inputQuantity = bin.ReadInt32();

                    int count = bin.ReadInt32();

                    RecyclingOutput[] outputs = new RecyclingOutput[count];

                    for (int c = 0; c < count; c++)
                    {
                        if (bin.ReadString().Equals(typeof(RecyclingOutput).FullName))
                        {
                            ushort outputItemId = bin.ReadUInt16();
                            int outputQuantity = bin.ReadInt32();

                            outputs[c] = new RecyclingOutput(outputItemId, outputQuantity);
                        }                        
                    }

                    buffer[i] = new Recycling(itemId, itemName, inputQuantity, outputs);
                }

                this.items = buffer;
            }
        }
    }
}
