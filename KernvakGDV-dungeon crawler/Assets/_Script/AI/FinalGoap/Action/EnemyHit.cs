using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace VaalsGOAP
{
    [CreateAssetMenu(menuName = "Action/EnemyHit")]
    public class EnemyHit : Action
    {
        private float timeInAction;
        private Color lerpedColor;
        private Color standardColor;
        private float pingPong;

        public override void Initialize(GOAPAgent act)
        {
            actor = act;
            conditions = new List<EffectState>() { new EffectState(Effect.SeePlayer, true), new EffectState(Effect.RunToThePlayer, true), new EffectState(Effect.Hit, false), };
            effects = new List<EffectState>() { new EffectState(Effect.Hit, true) };
        }

        public override bool IsViable()
        {
            
            return true;

        }

        public override void OnEnterAction()
        {
            //Debug.Log("reload");
            timeInAction = 0;
            standardColor = actor.GetComponent<MeshRenderer>().material.color;
        }

        public override void OnUpdateAction()
        {
            var pingPong = Mathf.PingPong(Time.time, 1f);
            lerpedColor = Color.Lerp(standardColor, new Color(0, 0, 0), pingPong);

            actor.GetComponent<MeshRenderer>().material.color = lerpedColor;
            actor.transform.LookAt(actor.playerPos);

            if (pingPong <= 0.1f)
            {
                OnActionCompleted();
            }

        }
        public override void OnExitAction()
        {
            //Debug.Log("reload exit");
            actor.GetComponent<MeshRenderer>().material.color = standardColor;
            actor.state.RemoveEffect(Effect.SeePlayer);
            actor.state.RemoveEffect(Effect.Hit);
            actor.state.RemoveEffect(Effect.RunToThePlayer);
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
