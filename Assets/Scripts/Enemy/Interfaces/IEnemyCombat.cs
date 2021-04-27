using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy;

namespace Enemy
{
    public interface IEnemyCombat
    {
        void OnAttackEnds();
        void PerformAttackBehavior(int behavior);
        attackType GetAttackType();
        float GetAttackRange();
        bool GetInAttackState();
        void AttackReset();
        void Attack();

        void DeactivateComponent();
    }
}
