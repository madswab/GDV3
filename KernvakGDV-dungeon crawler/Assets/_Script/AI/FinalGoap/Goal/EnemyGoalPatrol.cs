using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VaalsGOAP
{
    [CreateAssetMenu(menuName = "Goal/EnemyGoalPatrol")]
    public class EnemyGoalPatrol : Goal
    {
        public override void Initialize(GOAPAgent agent)
        {
            this.agent = agent;
            conditions = new List<EffectState>() { new EffectState(Effect.Walk, false), new EffectState(Effect.SeePlayer, false) };
            effects = new List<EffectState>() { new EffectState(Effect.Walk, true) };
        }

        public override bool IsViable(State state)
        {
            return state.CheckIfEffectsArePresent(conditions);
        }

    }
}
