using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyMovement
{
    float GetFacingRight();
    void DetectionsInterpreter(bool playerDetected, bool obstacleDetected, bool inAttackRange, Vector2 playerPosition, bool inAttackState);
}
