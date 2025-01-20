using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditorInternal.VersionControl.ListControl;

namespace GOAP
{
    public class Action
    {
        public string Name { get; set; }
        public State Precond { get; set; }
        public State Effects { get; set; }
        public int Cost { get; set; }

        public Action(string name, State precond, State effects, int cost)
        {
            Name = name;
            Precond = precond;
            Effects = effects;
            Cost = cost;
        }

        public List<(string, object)> GetEffects()
        {
            return Effects.GetState();
        }

        public List<(string, object)> GetPreconds() { 
            return Precond.GetState();
        }
    }
}

