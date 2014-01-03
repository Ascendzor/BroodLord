using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Objects
{
    [Serializable()]
    public class ChopEvent : Event
    {
        public ChopEvent(Guid id)
        {
            this.Id = id;
        }
    }
}
