using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Objects
{
    [Serializable()]
    public class SpawnToonEvent : Event
    {
        public SpawnToonEvent(Guid id)
        {
            this.Id = id;
        }
    }
}
