using System.Numerics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterModel : MonoBehaviour
{
    public BigInteger hp;

    public void TakeDamage(BigInteger damage)
    {
        hp -= damage;
    }
}
