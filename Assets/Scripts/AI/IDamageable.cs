using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    public void DealDamage(int amount, string gunName, bool silent);
}
