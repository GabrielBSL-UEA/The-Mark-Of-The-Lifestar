using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHitable
{
    void RegisterHit(float damage, float stun, float direction);
    bool GetIsAlive();
}
