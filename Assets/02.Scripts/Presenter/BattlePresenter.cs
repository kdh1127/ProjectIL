using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ThreeRabbitPackage.DesignPattern;

public partial class BattlePresenter : MonoBehaviour
{
    public CharacterView characterView;
    private CharacterModel characterModel;
    public TRState<BattlePresenter> battleState;

    private void Awake()
    {
        battleState = new LullState();
    }

    private void Update()
    {
        FSM();
    }

    private void FSM()
    {
        TRState<BattlePresenter> currentState = battleState.InputHandle(this);
        battleState.action(this);

        if(!currentState.Equals(battleState))
            battleState = currentState;
    }
}