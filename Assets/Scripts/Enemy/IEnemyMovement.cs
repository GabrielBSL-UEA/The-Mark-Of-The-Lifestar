using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public interface IEnemyMovement
    {
        float GetFacingRight();
        void DetectionsInterpreter(bool playerDetected, bool obstacleDetected, bool inAttackRange, Transform playerPosition, bool inAttackState);
    }
}
