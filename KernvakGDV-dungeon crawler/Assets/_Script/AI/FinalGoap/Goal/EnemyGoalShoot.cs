using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VaalsGOAP
{
    [CreateAssetMenu(menuName = "Goal/EnemyGoalShoot")]
    public class EnemyGoalShoot : Goal
    {
        public override void Initialize(GOAPAgent agent)
        {
            this.agent = agent;
            conditions = new List<EffectState>() { new EffectState(Effect.Reload, false) };
            effects = new List<EffectState>() { new EffectState(Effect.Reload, true) };
        }

        public override bool IsViable(State state)
        {
            return state.CheckIfEffectsArePresent(conditions);
        }

    }
}
