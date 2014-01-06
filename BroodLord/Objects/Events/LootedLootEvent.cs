using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Objects
{
    [Serializable()]
    public class LootedLootEvent : Event
    {
        public Guid item;
        public LootedLootEvent(Guid id, Guid item)
        {
            this.Id = id;
            this.item = item;
        }
    }
}
