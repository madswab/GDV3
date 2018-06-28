using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace VaalsGOAP
{
    //[CreateAssetMenu(fileName = "Action", menuName = "Create Action", order = 1)]
    public abstract class Action : ScriptableObject
    {
        public List<EffectState> conditions;
        public List<EffectState> effects;

        public int cost;
        protected GOAPAgent actor;

        public abstract void Initialize(GOAPAgent actor);
        public abstract bool IsViable();

        public abstract void OnEnterAction();
        public abstract void OnUpdateAction();
        public abstract void OnExitAction();

        public abstract void OnActionCompleted();
        public abstract void OnActionFailed();
        public Action Clone() { Action other = (Action)this.MemberwiseClone(); return other; }
    }

}

