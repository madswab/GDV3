﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace VaalsGOAP {
    [CreateAssetMenu(menuName = "Action/EnemyWalkAround")]
    public class EnemyWalkAround : Action{

        private float timeInThisAction = 0;
        private Vector3 endPos;

        public override void Initialize(GOAPAgent act)
        {
            actor = act;
            conditions = new List<EffectState>() { new EffectState(Effect.Walk, false), new EffectState(Effect.Spin, true), new EffectState(Effect.SeePlayer, false) };
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
            endPos = actor.gameObject.transform.position + new Vector3(rndDir.x, 0, rndDir.y).normalized * UnityEngine.Random.Range(1, 5f);
            timeInThisAction = 0;
            actor.agent.stoppingDistance = 0.1f;

            actor.agent.SetDestination(endPos);
            actor.transform.LookAt(endPos);
        }

        public override void OnUpdateAction()
        {
            if (actor != null && actor.playerInRange == false)
            {
                timeInThisAction += Time.deltaTime;
                if (actor.agent.remainingDistance < actor.agent.stoppingDistance || timeInThisAction > 5f )
                {
                    OnActionCompleted();
                }
            }
            else
            {
                OnActionCompleted();
            }

        }
        public override void OnExitAction()
        {
            actor.state.RemoveEffect(Effect.Spin);
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