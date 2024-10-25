using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ThreeRabbitPackage.DesignPattern;

public partial class BattlePresenter : TRSingleton<BattlePresenter>
{
    public class BattleState : TRState<BattlePresenter>
    {
        public override TRState<BattlePresenter> InputHandle(BattlePresenter battlePresenter)
        {
            throw new System.NotImplementedException();
        }

        public override void Enter(BattlePresenter battlePresenter)
        {
            base.Enter(battlePresenter);
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

    public class LullState : TRState<BattlePresenter>
    {
        public override TRState<BattlePresenter> InputHandle(BattlePresenter battlePresenter)
        {
            return this;
        }

        public override void Enter(BattlePresenter battlePresenter)
        {
            base.Enter(battlePresenter);
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
