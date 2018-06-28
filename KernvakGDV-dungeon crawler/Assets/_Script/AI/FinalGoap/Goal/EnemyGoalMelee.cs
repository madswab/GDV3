using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VaalsGOAP
{
    [CreateAssetMenu(menuName = "Goal/EnemyGoalMelee")]
    public class EnemyGoalMelee : Goal
    {
        public override void Initialize(GOAPAgent agent)
        {
            this.agent = agent;
            conditions = new List<EffectState>() { new EffectState(Effect.Hit, false) };
            effects = new List<EffectState>() { new EffectState(Effect.Hit, true) };
        }

        public override bool IsViable(State state)
        {
            return state.CheckIfEffectsArePresent(conditions);
        }

    }
}
