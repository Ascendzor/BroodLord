using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Objects
{
    [Serializable()]
    public class TreeRipEvent : Event
    {
        public TreeRipEvent(Guid id)
        {
            this.Id = id;
        }
    }
}
