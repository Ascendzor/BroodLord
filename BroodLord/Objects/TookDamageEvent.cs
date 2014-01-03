using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Objects
{
    [Serializable()]
    public class TookDamage : Event
    {
        public int DamageTaken;

        public TookDamage(Guid id, int damageTaken)
        {
            this.Id = id;
            this.DamageTaken = damageTaken;
        }
    }
}
