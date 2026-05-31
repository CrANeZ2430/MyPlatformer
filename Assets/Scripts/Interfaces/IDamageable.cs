using System;

public interface IDamageable
{
    void Damage(int damage, Action onDamage = null);
}
