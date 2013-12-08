using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Objects
{
    [Serializable()]
    public class Event
    {
        public Guid id;
        public object value;

        public Event(Guid id, object value)
        {
            this.id = id;
            this.value = value;
        }
    }
}
