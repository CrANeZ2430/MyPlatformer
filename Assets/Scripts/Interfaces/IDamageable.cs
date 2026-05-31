using System;

public interface IDamageable
{
    void Damage(Action onDamage = null);
}
