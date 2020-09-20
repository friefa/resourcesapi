namespace ResourcesAPI.Models.Items
{
    public class Item
    {
        /// <summary>
        /// The item id of the object.
        /// </summary>
        public ushort ItemId { get; private set; }

        /// <summary>
        /// The name of the item in its selected langauge.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The direct url to the icon of this item.
        /// </summary>
        public string IconUrl { get; private set; }

        /// <summary>
        /// This constructor is called when the object is created and initializes the object itself.
        /// </summary>
        /// <param name="id">The item id of the object</param>
        /// <param name="name">The name of the item in the selected language</param>
        /// <param name="url">The item icon url</param>
        public Item(ushort id, string name, string url)
        {
            this.ItemId = id;
            this.Name = name;
            this.IconUrl = url;
        }
    }
}
