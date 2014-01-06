using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Objects
{
    [Serializable()]
    public class TookDamageEvent : Event
    {
        public double DamageTaken;

        public TookDamageEvent(Guid id, double damageTaken)
        {
            this.Id = id;
            this.DamageTaken = damageTaken;
        }
    }
}
