using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace VaalsGOAP {
    [CreateAssetMenu(menuName = "Action/Idle")]
    public class EnemyIdle : Action{

        private Vector3 startPos;
        private float rotationTime;
        private float maxTimeInThisAction;
        private float timeInThisAction;
        private float rot;

        public override void Initialize(GOAPAgent act)
        {
            actor = act;
            conditions = new List<EffectState>() { new EffectState(Effect.Spin, false), new EffectState(Effect.SeePlayer, false) };
            effects = new List<EffectState>() { new EffectState(Effect.Spin, true) };
        }

        public override bool IsViable()
        {
            if (actor.playerInRange == false && actor.AIInRange == false)
            {
                return true;

            }
            return false;

        }

        public override void OnActionCompleted()
        {
            //actor.transform.position = (startPos);
            //Debug.Log("Idle Action Completed!");
            actor.OnActionCompleted(this);
        }

        public override void OnActionFailed()
        {
            //Debug.Log("Idle Action Failed!");
            actor.OnActionFailed(this);
        }

        public override void OnEnterAction()
        {
            //Debug.Log("idle");
            actor.agent.speed = 1f;
            startPos = actor.transform.position;
            maxTimeInThisAction = UnityEngine.Random.Range(2, 6);
            actor.agent.stoppingDistance = 0.1f;
            timeInThisAction = 0;
            actor.state.RemoveEffect(Effect.Walk);

        }

        public override void OnExitAction()
        {
            if (actor.playerInRange)
            {
                //actor.state.RemoveEffect(Effect.Alerted);
                actor.state.RemoveEffect(Effect.Spin);
                actor.state.AddEffect(Effect.Walk);
            }
        }

        public override void OnUpdateAction()
        {
            if (actor != null && actor.playerInRange == false)
            {
                rot++;
                actor.transform.Rotate(new Vector3(0,0.5f,0), rot);
                timeInThisAction += Time.deltaTime;

                if (timeInThisAction > maxTimeInThisAction)
                {               
                    OnActionCompleted();
                }
            }
            else
            {
                OnActionCompleted();

            }


        }

    }
}
