using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ThreeRabbitPackage.DesignPattern;
using UniRx;

public partial class BattlePresenter : TRSingleton<BattlePresenter>
{
    public class InitState : TRState<BattlePresenter>
    {

        public override TRState<BattlePresenter> InputHandle(BattlePresenter battlePresenter)
        {
            return this;
        }

        public override void Enter(BattlePresenter battlePresenter)
        {
            base.Enter(battlePresenter);

			var table = StageManager.Instance.GetCurStageTable();
			var monsterResource = TRScriptableManager.Instance.GameObject["Monster"].gameObjectDictionary;

			for (int i = 0; i < 5; i++)
			{
				// 몬스터 타입 판별
				var isBoss = i == 4;
				var monsterName = isBoss ? table.MonsterBossName : table.MonsterNormalName;
				var monsterObj = Instantiate(monsterResource[monsterName], MonsterManager.Instance.GetSpawnPosition(i));
				var model = MonsterManager.Instance.CreateMonster(table, isBoss);
				var view = monsterObj.GetComponent<MonsterView>();

                battlePresenter.SubscribeMonster(model, view);
			}
		}

        public override void Update(BattlePresenter battlePresenter)
        {
            base.Update(battlePresenter);

        }

        public override void Exit(BattlePresenter battlePresenter)
        {
            base.Exit(battlePresenter);
            // 페이드 아웃
        }
    }

    public class BattleState : TRState<BattlePresenter>
    {

        public override TRState<BattlePresenter> InputHandle(BattlePresenter battlePresenter)
        {
            return battlePresenter.characterView.isCollision ? this : new LullState();

        }

        public override void Enter(BattlePresenter battlePresenter)
        {
            base.Enter(battlePresenter);
            battlePresenter.characterView.Animator.SetTrigger("Attack");
        }

        public override void Update(BattlePresenter battlePresenter)
        {
            base.Update(battlePresenter);

            battlePresenter.characterView.Animator.SetTrigger("Attack");
        }

        public override void Exit(BattlePresenter battlePresenter)
        {
            base.Exit(battlePresenter);
        }
    }

    public class LullState : TRState<BattlePresenter>
    {
        public override TRState<BattlePresenter> InputHandle(BattlePresenter battlePresenter)
        {
            return battlePresenter.characterView.isCollision ? new BattleState() : this;
        }

        public override void Enter(BattlePresenter battlePresenter)
        {
            base.Enter(battlePresenter);
            battlePresenter.characterView.Animator.SetTrigger("Run");
        }

        public override void Update(BattlePresenter battlePresenter)
        {
            base.Update(battlePresenter);

            battlePresenter.characterView.Animator.SetTrigger("Run");
            battlePresenter.characterView.Move(battlePresenter.characterModel.moveSpeed);
            Camera.main.transform.Translate(battlePresenter.characterModel.moveSpeed * Time.smoothDeltaTime * Vector2.right);
        }

        public override void Exit(BattlePresenter battlePresenter)
        {
            base.Exit(battlePresenter);
        }
    }

    public class ClearState : TRState<BattlePresenter>
    {

        public override TRState<BattlePresenter> InputHandle(BattlePresenter battlePresenter)
        {
            return this;
        }

        public override void Enter(BattlePresenter battlePresenter)
        {
            base.Enter(battlePresenter);
            // 페이드인
            // 스테이지 넘버 증가
            
        }

        public override void Update(BattlePresenter battlePresenter)
        {
            base.Update(battlePresenter);

        }

        public override void Exit(BattlePresenter battlePresenter)
        {
            base.Exit(battlePresenter);
        }
    }
}
