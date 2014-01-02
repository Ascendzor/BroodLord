using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Objects
{
    [Serializable()]
    public class Event
    {
        public Guid Id;
        public string Type;
        public object Value;

        public Event(Guid id, string type, object value)
        {
            this.Id = id;
            this.Type = type;
            this.Value = value;
        }
    }
}
