using System;
using System.Collections;
using System.Collections.Generic;

namespace ResourcesAPI.Models.Items
{
    public class ItemCollection : ICollection<Item>
    {
        private Item[] items;
        
        public ItemCollection(Item[] items)
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
            return this.items.GetEnumerator() as IEnumerator<Item>;
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
            return this.items.GetEnumerator();
        }
    }
}