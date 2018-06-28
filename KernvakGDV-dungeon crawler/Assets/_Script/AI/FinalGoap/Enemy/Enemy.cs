using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VaalsGOAP
{

    public class Enemy : GOAPAgent
    {

        protected override void Start()
        {
            base.Start();

            InitializeActionsAndGoals();
        }

        protected override void Update()
        {
            base.Update();
        }
    }
}
