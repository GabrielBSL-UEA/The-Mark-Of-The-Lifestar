using UnityEngine;

namespace Interactable
{
    public interface IHittable
    {
        bool IsAlive { get; }
        void RegisterHit(float damage, float stun, Transform aggressor);
    }
}
