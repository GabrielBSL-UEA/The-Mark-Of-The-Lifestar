using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interactible
{
    public class HitReciever : MonoBehaviour
    {
        private bool canRecieveHit = true;

        public void RecieveHit(float damage, float stun, Transform agressor)
        {
            if (GetComponent<IHitable>() != null) GetComponent<IHitable>().RegisterHit(damage, stun, agressor);
        }

        //-----------------------------------------------------------------
        //**********                Get Functions                **********
        //-----------------------------------------------------------------

        public bool GetCanRecieveHit()
        {
            return canRecieveHit;
        }

        public void SetCanRecieveHit(bool value)
        {
            canRecieveHit = value;
        }
    }
}