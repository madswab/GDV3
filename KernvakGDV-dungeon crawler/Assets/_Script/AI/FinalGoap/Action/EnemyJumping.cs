using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace VaalsGOAP {
    [CreateAssetMenu(menuName = "Action/Jumping")]
    public class EnemyJumping : Action{

        private float maxTimeInThisAction = 4f;
        private float timeInThisAction = 0;
        public float jumpTime = 1f;
        private Vector3 startPos;

        public override void Initialize(GOAPAgent act)
        {
            actor = act;
            conditions = new List<EffectState>() { new EffectState(Effect.Jumping, false), new EffectState(Effect.SeePlayer, false) };
            effects = new List<EffectState>() { new EffectState(Effect.Jumping, true) };
        }

        public override bool IsViable()
        {
            if (actor.AIInRange)
            {
                return false;

            }
            return true;
        }

        public override void OnEnterAction()
        {
            timeInThisAction = 0;
            startPos = actor.transform.position + new Vector3(0, 0.1f, 0);
            actor.transform.position = startPos;
            actor.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
        }

        public override void OnUpdateAction()
        {
            if (actor != null && actor.playerInRange == false)
            {
                timeInThisAction += Time.deltaTime;
                if (timeInThisAction % jumpTime <= 0.06f && timeInThisAction % jumpTime >= 0f && timeInThisAction < maxTimeInThisAction && Vector3.Distance(actor.agent.transform.position, startPos) <= 0.5f)
                {
                    Jump();
                }
                else if (timeInThisAction > maxTimeInThisAction && Vector3.Distance(actor.agent.transform.position, startPos) <= 0.5f)
                {
                     OnActionCompleted();                   
                }
            }
            else if(actor.playerInRange && Vector3.Distance(actor.agent.transform.position, startPos) <= 0.5f)
            {
                OnActionCompleted();
            }

        }
        public override void OnExitAction()
        {
            actor.transform.position = startPos;
            actor.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = true;

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

        private void Jump()
        {
            actor.GetComponent<Rigidbody>().AddForce(Vector3.up * 1.5f, ForceMode.Impulse);          
        }
    }
}
