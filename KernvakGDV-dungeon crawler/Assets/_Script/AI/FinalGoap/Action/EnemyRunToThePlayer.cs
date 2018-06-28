using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace VaalsGOAP {
    [CreateAssetMenu(menuName = "Action/EnemyRunToThePlayer")]
    public class EnemyRunToThePlayer : Action{

        public float offSetFromObstacle = 5f;
        private Vector3 startPos;

        public override void Initialize(GOAPAgent act)
        {
            actor = act;
            conditions = new List<EffectState>() { new EffectState(Effect.SeePlayer, true)};
            effects = new List<EffectState>() { new EffectState(Effect.RunToThePlayer, true)};
        }

        public override bool IsViable()
        {
            if (actor.playerInRange)
            {
                return true;
            }

            return false;
        }

        public override void OnEnterAction()
        {
            //Debug.Log("Run To");
            actor.agent.speed = 5f;
            startPos = actor.transform.position;
            actor.agent.stoppingDistance = 1.1f;
        }

        public override void OnUpdateAction()
        {
            actor.transform.LookAt(actor.playerPos);
            actor.agent.SetDestination(actor.playerPos);

            if (actor.playerInRange == false || Vector3.Distance(actor.playerPos, startPos) > 10f)
            {
                actor.agent.SetDestination(startPos);
                if (!actor.agent.pathPending && actor.agent.remainingDistance < actor.agent.stoppingDistance)
                {
                    actor.playerInRange = false;
                    OnExitAction();
                }
            }

            if (!actor.agent.pathPending && actor.agent.remainingDistance < actor.agent.stoppingDistance)
            {
                OnActionCompleted();
            }

        }
        public override void OnExitAction()
        {
            //Debug.Log("walkTo exit");
            if (actor.playerInRange == false)
            {
                actor.state.RemoveEffect(Effect.RunToThePlayer);
                actor.state.RemoveEffect(Effect.SeePlayer);
                actor.state.RemoveEffect(Effect.Walk);
            }
        }
        public override void OnActionCompleted()
        {
            //Debug.Log("walkTo Action Completed!");
            actor.OnActionCompleted(this);

        }

        public override void OnActionFailed()
        {
            //Debug.Log("walkTo Action Failed!");
            actor.OnActionFailed(this);
        }


    }
}
