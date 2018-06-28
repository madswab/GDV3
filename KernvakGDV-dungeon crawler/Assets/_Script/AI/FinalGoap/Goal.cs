using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VaalsGOAP
{
    public abstract class Goal : ScriptableObject
    {
        
        public int priority;

        internal List<EffectState> conditions;
        internal List<EffectState> effects;
        internal Stack<Action> actionStack = new Stack<Action>();

        protected GOAPAgent agent;

        public abstract void Initialize(GOAPAgent agent);
        public abstract bool IsViable(State state);
        public Goal Clone() { return MemberwiseClone() as Goal; }

    }
}

