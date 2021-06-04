using UnityEngine;

namespace Interactable
{
    public class HitReceiver : MonoBehaviour
    {
        private bool _canReceivedHit = true;

        public void ReceivedHit(float damage, float stun, Transform aggressor)
        {
            if (GetComponent<IHittable>() != null) GetComponent<IHittable>().RegisterHit(damage, stun, aggressor);
        }

        //-----------------------------------------------------------------
        //**********                Get Functions                **********
        //-----------------------------------------------------------------

        public bool GetCanReceivedHit()
        {
            return _canReceivedHit;
        }

        public void SetCanReceivedHit(bool value)
        {
            _canReceivedHit = value;
        }
    }
}