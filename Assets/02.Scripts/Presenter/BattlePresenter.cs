using System;
using System.Numerics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ThreeRabbitPackage.DesignPattern;
using UniRx;

public partial class BattlePresenter : TRSingleton<BattlePresenter>
{
    public TRState<BattlePresenter> battleState;

    public CharacterView characterView;
    private CharacterModel characterModel = new();

    private void Awake()
    {
        base.Awake();
        battleState = new LullState();

        AttakcSubscribe();
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

    private void AttakcSubscribe()
    {
        characterView.AttackSubject.Subscribe(monster =>
        {
            var damage = characterModel.Attack();
            Debug.Log($"데미지 {damage}");
            //monster.GetComponent<MonsterModel>().TakeDamage(damage);
        }).AddTo(characterView.gameObject);
    }
}
