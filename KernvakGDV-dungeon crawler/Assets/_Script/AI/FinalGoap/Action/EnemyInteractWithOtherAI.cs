using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VaalsGOAP
{
    [CreateAssetMenu(menuName = "Action/InteractWithOtherAI")]
    public class EnemyInteractWithOtherAI : Action
    {

        private Vector3 startPos;
        private float rotationTime;
        private float maxTimeInThisAction;
        private float timeInThisAction;
        private float rot;

        public override void Initialize(GOAPAgent act)
        {
            actor = act;
            conditions = new List<EffectState>() { new EffectState(Effect.KillAI, false), new EffectState(Effect.SeePlayer, false), new EffectState(Effect.SeeAI, true) };
            effects = new List<EffectState>() { new EffectState(Effect.KillAI, true) };
        }

        public override bool IsViable()
        {
            if (actor.AIInRange)
            {
                return true;

            }
            return false;

        }

        public override void OnActionCompleted()
        {
            //actor.transform.position = (startPos);
            Debug.Log("InteractWithOtherAI Action Completed!");
            actor.OnActionCompleted(this);
        }

        public override void OnActionFailed()
        {
            //Debug.Log("Idle Action Failed!");
            actor.OnActionFailed(this);
        }

        public override void OnEnterAction()
        {
            Debug.Log("InteractWithOtherAI");
            actor.agent.speed = 1f;
            startPos = actor.transform.position;
            maxTimeInThisAction = UnityEngine.Random.Range(2, 6);
            actor.agent.stoppingDistance = 0.1f;
            timeInThisAction = 0;
            actor.state.RemoveEffect(Effect.Walk);

        }

        public override void OnExitAction()
        {

        }

        public override void OnUpdateAction()
        {
            if (actor.AIInRange == true)
            {
                Collider[] hitcol = Physics.OverlapSphere(actor.transform.position, 5);
                foreach (var item in hitcol)
                {
                    if (item.transform != actor.transform && item.GetComponent<GOAPAgent>())
                    {
                        Destroy(item.gameObject);
                        OnActionCompleted();
                    }
                }

            }
            else
            {
                OnActionCompleted();

            }


        }
    }
}
