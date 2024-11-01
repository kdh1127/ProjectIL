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
        SubscribeCharacter();
    }

	private void Start()
	{
        battleState = new InitState();
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

    private void SubscribeCharacter()
    {
        characterView.AttackSubject.Subscribe(monster =>
        {
            var attackInfo = characterModel.Attack();
            MonsterManager.Instance.GetTargetMonster().TakeDamage(attackInfo);
        }).AddTo(characterView.gameObject);
    }

    private void SubscribeMonster(MonsterModel model, MonsterView view)
	{
        model.DeathSubject.Subscribe(_ =>
        {
            view.Animator.SetTrigger("Death");
            MonsterManager.Instance.IncreaseIndex();
            Destroy(view.gameObject);
        }).AddTo(view.gameObject);

        model.hp.Subscribe(hp =>
        {
            view.UpdateHpBar(hp, model.maxHp);
        }).AddTo(view.gameObject);

        model.AttackInfoSubject.Subscribe(attackInfo =>
        {
            view.ShowDamage(attackInfo);
        }).AddTo(view.gameObject);
    }

    public void Lull()
	{
        battleState = new LullState();
    }

    public void Battle()
    {
        battleState = new BattleState();
    }

    public void Clear()
    {
        battleState = new ClearState();
    }
    public void Init()
    {
        battleState = new InitState();
    }
}
