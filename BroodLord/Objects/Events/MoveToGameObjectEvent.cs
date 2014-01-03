using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Objects
{
    [Serializable()]
    public class MoveToGameObjectEvent : Event
    {
        public Guid GoalGameObject;

        public MoveToGameObjectEvent(Guid id, Guid goalGameObject)
        {
            this.Id = id;
            this.GoalGameObject = goalGameObject;
        }
    }
}
