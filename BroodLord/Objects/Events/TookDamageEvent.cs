using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Objects
{
    [Serializable()]
    public class TookDamage : Event
    {
        public double DamageTaken;

        public TookDamage(Guid id, double damageTaken)
        {
            this.Id = id;
            this.DamageTaken = damageTaken;
        }
    }
}
