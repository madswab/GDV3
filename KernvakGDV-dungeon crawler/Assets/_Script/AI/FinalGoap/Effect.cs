using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VaalsGOAP
{
    public enum Effect
    {
        Spin             = (1 << 0),
        Walk             = (1 << 1),
        SeePlayer        = (1 << 2),
        ShootPlayer      = (1 << 4),
        Reload           = (1 << 5),
        Jumping          = (1 << 6),
        Hit              = (1 << 7),
        RunToThePlayer   = (1 << 8),
        KillAI           = (1 << 9),
        SeeAI            = (1 << 10),

    }

    [System.Serializable]
    public struct EffectState
    {
        public EffectState(Effect effect, bool value)
        {
            this.effect = effect;
            this.isActive = value;
        }
        public Effect effect;
        public bool isActive;

    }
}

