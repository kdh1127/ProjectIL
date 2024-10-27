using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ThreeRabbitPackage.DesignPattern;
using UniRx;

public partial class BattlePresenter : TRSingleton<BattlePresenter>
{
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
}
