using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VaalsGOAP
{
    [System.Serializable]
    public class State
    {
        //[BitMask(typeof(Effect))]
        public Effect state;

        public State()
        {

        }

        public void AddEffectsToState(List<EffectState> effects)
        {
            foreach (EffectState efState in effects)
            {
                if (efState.isActive)
                {
                    AddEffect(efState.effect);
                }
                else
                {
                    RemoveEffect(efState.effect);
                }
            }
        }

        public void AddEffect(Effect effect)
        {
            state |= effect;
        }

        public void RemoveEffect(Effect effect)
        {
            state &= ~effect; 
        }

        public bool CheckIfEffectsArePresent(List<EffectState> effects)
        {
            bool res = true;
            foreach(EffectState efState in effects)
            {
                res &= efState.isActive? CheckIfEffectIsPresent(efState.effect) : CheckIfEffectIsNotPresent(efState.effect);
            }
            return res;
        }

        public bool CheckIfEffectIsPresent(Effect effect)
        {
            return (state & effect) != 0;
        }

        public bool CheckIfEffectIsNotPresent(Effect effect)
        {
            return !((state & effect) != 0);
        }

        public bool CompareState(State otherState)
        {
            return (state == otherState.state);
        }

    }

}

