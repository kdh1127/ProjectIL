using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ThreeRabbitPackage.DesignPattern;

public class BattleManager : TRSingleton<BattleManager>
{
    public float monsterGap;

    private void Awake()
    {
        base.Awake();

    }


}
