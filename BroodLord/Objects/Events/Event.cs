using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Objects
{
    [Serializable()]
    public class Event
    {
        public int Type;
        public Guid Id;

        public virtual byte[] Serialize()
        {
            return null;
        }
    }
}