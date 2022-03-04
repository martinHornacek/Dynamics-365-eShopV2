using System.Collections.Generic;

namespace Item.API.DTOs
{
    public class ItemPayload
    {
        public List<ItemReadDto> Items { get; set; }
        public int Count { get; set; }

        public ItemPayload(List<ItemReadDto> Items, int Count)
        {
            this.Items = Items;
            this.Count = Count;
        }
    }
}
