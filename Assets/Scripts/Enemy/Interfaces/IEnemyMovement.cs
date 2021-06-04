using UnityEngine;

namespace Enemy
{
    public interface IEnemyMovement
    {
        float FacingRight { get; }
        void DetectionsInterpreter(bool playerDetected, bool obstacleDetected, bool inAttackRange, Transform playerPosition, bool inAttackState);
        void DeactivateComponent();
    }
}
