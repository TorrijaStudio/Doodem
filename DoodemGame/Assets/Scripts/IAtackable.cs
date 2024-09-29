using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAtackable
{
    public void Attack();
    public float Attacked(float enemyDamage);
}
