using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interactible
{
    public interface IHitable
    {
        void RegisterHit(float damage, float stun, Transform agressor);
        bool GetIsAlive();
    }
}
