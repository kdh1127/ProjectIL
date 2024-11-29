using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ThreeRabbitPackage.DesignPattern;
using UniRx;

public partial class BattlePresenter : TRSingleton<BattlePresenter>
{
    public class InitState : TRState<BattlePresenter>
    {
        bool isComplete = false;

        public override TRState<BattlePresenter> InputHandle(BattlePresenter battlePresenter)
        {
            if (isComplete) return new LullState();
            return this;
        }

        public override void Enter(BattlePresenter battlePresenter)
        {
            base.Enter(battlePresenter);
            
            var table = StageManager.Instance.GetCurStageTable();
			var monsterResource = TRScriptableManager.Instance.GetGameObject("MonsterResources").gameObjectDictionary;
            var monsterManager = MonsterManager.Instance;

            // 캐릭터 위치 초기화
            CharacterManager.Instance.Init();

            battlePresenter.pixelCamera.transform.position = new Vector3(0, 0, -10);

            StageManager.Instance.SetStageBaseHp();
            // 몬스터 생성
            for (int i = 0; i < 5; i++)
			{
				// 몬스터 타입 판별
				var isBoss = i == 4;
				var monsterName = isBoss ? table.MonsterBossName : table.MonsterNormalName;
				var monsterObj = Instantiate(monsterResource[monsterName], monsterManager.GetSpawnPosition(i));
				var model = monsterManager.CreateMonster(table, isBoss);
				var view = monsterObj.GetComponent<MonsterView>();

                battlePresenter.SubscribeMonster(model, view);
			}

            battlePresenter.fadeScreenView.FadeIn(0.5f, () => isComplete = true);
            battlePresenter.state = EBattleState.Init;
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
            battlePresenter.state = EBattleState.Battle;
        }

        public override void Update(BattlePresenter battlePresenter)
        {
            base.Update(battlePresenter);

            battlePresenter.characterView.Animator.SetTrigger("Attack");
            battlePresenter.characterView.SetAttackSpeed(battlePresenter.characterModel.AttackSpeed);
        }

        public override void Exit(BattlePresenter battlePresenter)
        {
            base.Exit(battlePresenter);
        }
    }

    public class LullState : TRState<BattlePresenter>
    {
        bool isClear = false;
        public override TRState<BattlePresenter> InputHandle(BattlePresenter battlePresenter)
        {
            if (isClear) return new ClearState();
            return battlePresenter.characterView.isCollision ? new BattleState() : this;
        }

        public override void Enter(BattlePresenter battlePresenter)
        {
            base.Enter(battlePresenter);
            battlePresenter.characterView.SetRunSpeed(battlePresenter.characterModel.RunSpeed);
            battlePresenter.characterView.Animator.SetTrigger("Run");
            battlePresenter.state = EBattleState.Lull;
        }

        public override void Update(BattlePresenter battlePresenter)
        {
            base.Update(battlePresenter);

            battlePresenter.characterView.SetRunSpeed(battlePresenter.characterModel.RunSpeed);
            battlePresenter.characterView.Animator.SetTrigger("Run");
            battlePresenter.characterView.Move(battlePresenter.characterModel.RunSpeed);


            if (CharacterManager.Instance.CharacterPositionX > 3 && CharacterManager.Instance.CharacterPositionX < 22)
                battlePresenter.pixelCamera.transform.Translate(battlePresenter.characterModel.RunSpeed * Time.smoothDeltaTime * Vector2.right);
            if (CharacterManager.Instance.CharacterPositionX >= 26)
                isClear = true;

        }
    }

    public class ClearState : TRState<BattlePresenter>
    {
        bool isComplete = false;
        public override TRState<BattlePresenter> InputHandle(BattlePresenter battlePresenter)
        {
            if (isComplete) return new InitState();
            return this;
        }

        public override void Enter(BattlePresenter battlePresenter)
        {
            base.Enter(battlePresenter);
            battlePresenter.fadeScreenView.FadeOut(0.5f, () =>
            {
                StageManager.Instance.IncreaseStage();
                MonsterManager.Instance.ClearAllMonster();
                UserDataManager.Instance.SaveAll();
                isComplete = true;
            });
            battlePresenter.state = EBattleState.Clear;
        }
	}

    public class ResetState : TRState<BattlePresenter>
    {
        bool isComplete = false;
        public override TRState<BattlePresenter> InputHandle(BattlePresenter battlePresenter)
        {
            if (isComplete) return new InitState();
            return this;
        }

        public override void Enter(BattlePresenter battlePresenter)
        {
            base.Enter(battlePresenter);
            battlePresenter.state = EBattleState.Reset;
            battlePresenter.characterView.Animator.SetTrigger("Run");
            battlePresenter.gold.Subtract(battlePresenter.gold.Amount);
            StageManager.Instance.ResetStage();
            MonsterManager.Instance.ClearAllMonster();
            battlePresenter.fadeScreenView.FadeOut(0.5f, () =>
            {
                UserDataManager.Instance.SaveAll();
                isComplete = true;
            });
        }
    }
}
