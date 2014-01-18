using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Objects
{
    [Serializable()]
    public class LootedLootEvent : Event
    {
        public LootedLootEvent(Guid id)
        {
            this.Id = id;
        }
    }
}
