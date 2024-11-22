using System;
using System.Numerics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ThreeRabbitPackage.DesignPattern;
using UniRx;
using System.Linq;
using Zenject;

public partial class BattlePresenter : TRSingleton<BattlePresenter>
{
    public TRState<BattlePresenter> battleState;

    [Inject]private readonly CharacterView characterView;
    private CharacterModel characterModel = new();

    public Camera pixelCamera;
    public FadeScreenView fadeScreenView;

    [Inject] private readonly CurrencyModel currencyModel;

    private new void Awake()
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
        // 나중에 오브젝트 풀로 변경하자
        model.DeathSubject.Subscribe(_ =>
        {
            view.Animator.SetTrigger("Death");
            view.GetComponent<BoxCollider2D>().enabled = false;
            MonsterManager.Instance.IncreaseIndex();

            model.GetReward().ToList().ForEach(reward =>
            {
                currencyModel.AddCurrency(reward.Key, reward.Value);
            });
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
