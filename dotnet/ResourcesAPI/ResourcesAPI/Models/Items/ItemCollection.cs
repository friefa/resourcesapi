using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ResourcesAPI.Models.Items
{
    public class ItemCollection : ICollection<Item>, ISaveable
    {
        private Item[] items;
        
        public ItemCollection(Item[] items = default)
        {
            this.items = items;
        }

        public int Count => this.items.Length;

        public bool IsReadOnly => true;

        public void Add(Item item)
        {
            if (this.items != null && this.items.Length > 0)
            {
                Item[] buffer = items;

                this.items = new Item[buffer.Length + 1];

                for (int i = 0; i < buffer.Length; i++)
                {
                    this.items[i] = buffer[i];
                }

                this.items[items.Length] = item;
            }
            else
            {
                this.items = new Item[] { item };
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

        public bool Contains(Item item)
        {
            if (this.items != null & this.items.Length > 0)
            {
                for (int i = 0; i < this.items.Length; i++)
                {
                    if (this.items[i].ItemId == item.ItemId) return true;
                }
            }

            return false;
        }

        public void CopyTo(Item[] array, int arrayIndex)
        {
            this.items.CopyTo(array, arrayIndex);
        }

        public IEnumerator<Item> GetEnumerator()
        {
            for (int i = 0; i < this.items.Length; i++)
            {
                yield return this.items[i];
            }
        }

        public bool Remove(Item item)
        {
            Item[] buffer = new Item[this.items.Length - 1];
            bool result = false;

            int counter = 0;

            for (int i = this.items.Length; i >= 0; i--)
            {
                if (this.items[i].ItemId == item.ItemId)
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

            foreach (Item item in this.items)
            {
                bin.Write(item.ItemId);
                bin.Write(item.Name);
                bin.Write(item.IconUrl);
            }

            bin.Close();
            stream.Close();
        }

        public void Load(string filename)
        {
            FileStream stream = new FileStream(filename, FileMode.Open, FileAccess.Read);
            BinaryReader bin = new BinaryReader(stream, Encoding.UTF8);

            if (bin.ReadString().Equals(typeof(ItemCollection).FullName))
            {
                int number = bin.ReadInt32();

                Item[] buffer = new Item[number];

                for (int i = 0; i < number; i++)
                {
                    ushort id = bin.ReadUInt16();
                    string name = bin.ReadString();
                    string icon_url = bin.ReadString();

                    buffer[i] = new Item(id, name, icon_url);
                }

                this.items = buffer;
            }
        }
    }
}