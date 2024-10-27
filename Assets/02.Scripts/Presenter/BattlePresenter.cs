using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ThreeRabbitPackage.DesignPattern;

public partial class BattlePresenter : MonoBehaviour
{
    public TRState<BattlePresenter> battleState;

    public CharacterView characterView;
    private CharacterModel characterModel = new();

    private void Awake()
    {
        battleState = new LullState();
    }

    private void Update()
    {
        FSM();
        Debug.Log(characterView.isCollision);
    }

    private void FSM()
    {
        TRState<BattlePresenter> currentState = battleState.InputHandle(this);
        battleState.action(this);

        if(!currentState.Equals(battleState))
            battleState = currentState;
    }
}
