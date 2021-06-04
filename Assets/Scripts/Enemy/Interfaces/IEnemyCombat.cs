namespace Enemy
{
    public interface IEnemyCombat
    {
        bool InAttackState { get; }

        void OnAttackEnds();
        void PerformAttackBehavior(int behavior);
        AttackType GetAttackType();
        float GetAttackRange();
        void AttackReset();
        void Attack();

        void DeactivateComponent();
    }
}
