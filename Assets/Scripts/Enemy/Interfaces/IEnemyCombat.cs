using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy;

namespace Enemy
{
    public interface IEnemyCombat
    {
        bool inAttackState { get; }

        void OnAttackEnds();
        void PerformAttackBehavior(int behavior);
        attackType GetAttackType();
        float GetAttackRange();
        void AttackReset();
        void Attack();

        void DeactivateComponent();
    }
}
