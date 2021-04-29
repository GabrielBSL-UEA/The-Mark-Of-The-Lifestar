using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public interface IEnemyMovement
    {
        float facingRight { get; }
        void DetectionsInterpreter(bool playerDetected, bool obstacleDetected, bool inAttackRange, Transform playerPosition, bool inAttackState);
        void DeactivateComponent();
    }
}
