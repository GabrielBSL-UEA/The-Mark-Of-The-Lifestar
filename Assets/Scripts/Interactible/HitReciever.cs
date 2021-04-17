using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interactible
{
    public class HitReciever : MonoBehaviour
    {
        public void RecieveHit(float damage, float stun, float direction)
        {
            if (GetComponent<IHitable>() != null) GetComponent<IHitable>().RegisterHit(damage, stun, direction);
        }
    }
}