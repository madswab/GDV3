using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VaalsGOAP
{
    [CreateAssetMenu(menuName = "Action/EnemyFoundAI")]
    public class EnemyFoundAI : Action
    {

        //public float offSetFromObstacle = 5f;
        //private Vector3 startPos;

        public override void Initialize(GOAPAgent act)
        {
            actor = act;
            conditions = new List<EffectState>() { new EffectState(Effect.SeeAI, false), new EffectState(Effect.Walk, true) };
            effects = new List<EffectState>() { new EffectState(Effect.SeeAI, true) };
        }

        public override bool IsViable()
        {
            if (actor.AIInRange)
            {
                return true;
            }

            return false;
        }

        public override void OnEnterAction()
        {
            //Debug.Log("see");
            //startPos = actor.transform.position;
        }

        public override void OnUpdateAction()
        {
            //actor.transform.LookAt(actor.playerPos);
            //actor.agent.SetDestination(startPos);

            //if (actor.playerInRange == false)
            //{
            OnActionCompleted();
            //}
        }
        public override void OnExitAction()
        {
            //Debug.Log("see exit");
        }
        public override void OnActionCompleted()
        {
            //Debug.Log("seeplayer Action Completed!");
            actor.OnActionCompleted(this);

        }

        public override void OnActionFailed()
        {
            //Debug.Log("seeplayer Action Failed!");
            actor.OnActionFailed(this);
        }


    }
}
