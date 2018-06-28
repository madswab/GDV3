using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace VaalsGOAP
{
    [CreateAssetMenu(menuName = "Action/Shoot")]
    public class EnemyShoot : Action
    {


        public override void Initialize(GOAPAgent act)
        {
            actor = act;
            conditions = new List<EffectState>() { new EffectState(Effect.SeePlayer, true) };
            effects = new List<EffectState>() { new EffectState(Effect.ShootPlayer, true) };
        }

        public override bool IsViable()
        {
            if (actor.playerInRange)
            {
                return true;
            }

            return false;
        }

        public override void OnActionCompleted()
        {
            //Debug.Log("shoot Action Completed!");
            actor.OnActionCompleted(this);
        }

        public override void OnActionFailed()
        {
            Debug.Log("shoot Action Failed!");
            actor.OnActionFailed(this);
        }

        public override void OnEnterAction()
        {
            //Debug.Log("shoot");
        }

        public override void OnExitAction()
        {

        }

        public override void OnUpdateAction()
        {
            //actor.transform.LookAt(actor.playerPos);

            if (actor.playerInRange)
            {
                Shoot();
            }
            else
            {
                OnActionCompleted();

            }

        }

        private void Shoot()
        {
            //Debug.Log("shot");
            GameObject b = Instantiate(actor.bullet, actor.gun.transform.position + Vector3.forward * 2, Quaternion.identity);
            b.GetComponent<Rigidbody>().AddForce(actor.gun.transform.forward * 50, ForceMode.Impulse);
            Destroy(b, 10f);
            OnActionCompleted();

        }

    }
}
