using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace VaalsGOAP {
    [CreateAssetMenu(menuName = "Action/WalkBackAndForward")]
    public class EnemyWalkBackAndForward : Action{

        private Vector3 endPos;
        private Vector3 startPos;


        public override void Initialize(GOAPAgent act)
        {
            actor = act;
            conditions = new List<EffectState>() { new EffectState(Effect.Walk, false), new EffectState(Effect.SeePlayer, false), new EffectState(Effect.Jumping, true) };
            effects = new List<EffectState>() { new EffectState(Effect.Walk, true) };
        }

        public override bool IsViable()
        {

            //if (actor.playerInRange == false)
            //{
                return true;

            //}
            //return false;
        }

        public override void OnEnterAction()
        {
            actor.agent.speed = 1f;
            Vector3 rndDir = UnityEngine.Random.insideUnitCircle;
            startPos = actor.gameObject.transform.position;
            endPos = actor.gameObject.transform.position + new Vector3(rndDir.x, 0, rndDir.y).normalized * UnityEngine.Random.Range(1, 5f);
            actor.agent.stoppingDistance = 0.1f;

            actor.agent.SetDestination(endPos);
            actor.transform.LookAt(endPos);
        }

        public override void OnUpdateAction()
        {
            if (actor != null && actor.playerInRange == false)
            {
                if (Vector3.Distance(actor.agent.transform.position, startPos) <= 0.2f && actor.agent.remainingDistance < actor.agent.stoppingDistance)
                {
                    OnActionCompleted();
                }
                else if (!actor.agent.pathPending && actor.agent.remainingDistance < actor.agent.stoppingDistance && actor.agent.destination != startPos)
                {
                    actor.transform.LookAt(startPos);
                    actor.agent.destination = startPos;
                }
            }
            else
            {
                OnActionCompleted();
            }

        }
        public override void OnExitAction()
        {
            actor.agent.destination = actor.agent.transform.position;
            actor.state.RemoveEffect(Effect.Jumping);
            if (actor.playerInRange == false)
            {
                actor.state.RemoveEffect(Effect.Walk);
            }
        }
        public override void OnActionCompleted()
        {
            //actor.transform.position = (endPos);
            //Debug.Log("walk Action Completed!");
            actor.OnActionCompleted(this);
        }

        public override void OnActionFailed()
        {
            //Debug.Log("walk Action Failed!");
            actor.OnActionFailed(this);
        }


    }
}
