using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VaalsGOAP
{
    public class Player : MonoBehaviour {

        public int radius = 5;
        public int castTime = 3;
        public LayerMask findLayer;

        void Start() {
            StartCoroutine(SphireCast());
        }


        void Update() {



        }

        private IEnumerator SphireCast()
        {
            while (gameObject.activeInHierarchy)
            {
                Collider[] hitcol = Physics.OverlapSphere(transform.position, radius, findLayer);
                foreach (var item in hitcol)
                {
                    if (item.GetComponent<GOAPAgent>())
                    {
                        item.GetComponent<GOAPAgent>().CheckPlayerInRange(transform.position);
                    }
                }
                yield return new WaitForSeconds(castTime);
            }
        }

        private void OnDrawGizmos()
        {
            //Gizmos.color = Color.yellow;
            //Gizmos.DrawSphere(transform.position, radius);
        }
    }
}