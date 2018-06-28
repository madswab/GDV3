using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace VaalsGOAP
{
    [CreateAssetMenu(menuName = "Action/EnemyReload")]
    public class EnemyReload : Action
    {
        private float timeInAction;
        public float maxTimeInThisAction = 2f;

        public override void Initialize(GOAPAgent act)
        {
            actor = act;
            conditions = new List<EffectState>() { new EffectState(Effect.ShootPlayer, true), new EffectState(Effect.Reload, false) };
            effects = new List<EffectState>() { new EffectState(Effect.Reload, true) };
        }

        public override bool IsViable()
        {
            
            return true;

        }

        public override void OnEnterAction()
        {
            //Debug.Log("reload");
            timeInAction = 0;
        }

        public override void OnUpdateAction()
        {
            timeInAction += Time.deltaTime;
            if (timeInAction > maxTimeInThisAction)
            {
                OnActionCompleted();
            }

        }
        public override void OnExitAction()
        {
            //Debug.Log("reload exit");
            actor.state.RemoveEffect(Effect.SeePlayer);
            actor.state.RemoveEffect(Effect.ShootPlayer);
            actor.state.RemoveEffect(Effect.Reload);
            if (actor.playerInRange == false)
            {
                actor.state.RemoveEffect(Effect.Walk);
            }
        }
        public override void OnActionCompleted()
        {
            //Debug.Log("reload Action Completed!");
            actor.OnActionCompleted(this);

        }

        public override void OnActionFailed()
        {
            //Debug.Log("reload Action Failed!");
            actor.OnActionFailed(this);
        }


    }
}
