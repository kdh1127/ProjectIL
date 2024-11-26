using UnityEngine;
using Zenject;
using UniRx;

public class ReincarnationPresenter
{
    private readonly ReincarnationModel model;
    private readonly OpenPopupButtonView reincarnationButton;
    private readonly QuestModel questModel;
    private readonly WeaponModel weaponModel;
    public ReincarnationPopupView reincarnationPopupView;

    [Inject]
    public ReincarnationPresenter(ReincarnationModel model, OpenPopupButtonView reincarnationButton, QuestModel questModel, WeaponModel weaponModel)
    {
        this.model = model;
        this.reincarnationButton = reincarnationButton;
        this.questModel = questModel;
        this.weaponModel = weaponModel;
    }

    public void Subscribe()
    {
        SubscribeToReincarnationButton();
        SubscribeToReincarnationPopupView();
        SubscribeToModel();
    }

    private void SubscribeToReincarnationButton()
    {
        StageManager.Instance.CurStage.Subscribe(stageNo =>
        {
            var isOpen = stageNo >= 10;
            reincarnationButton.gameObject.SetActive(isOpen);
        }).AddTo(reincarnationButton.gameObject);
        reincarnationButton.button.OnClickAsObservable()
            .Subscribe(_ =>
            {
                TRCommonPopup.Instantiate(PopupManager.Instance.transform)
                .SetTitle("ȯ���ϱ�")
                .SetMessage($"������: {StageManager.Instance.CurStage.Value}\n���� ŉ�淮: {model.GetReward()}")
                .SetConfirm(confirmAction: obj =>
                {
                    model.Reincarnation(1);
                    questModel.Reset();
                    weaponModel.Reset();
                    BattlePresenter.Instance.battleState = new BattlePresenter.ResetState();
                    GameObject.Destroy(obj);
                }, confirmText: "ȯ���ϱ�")
                .SetCancel(cancelAction: obj =>
                {
                    GameObject.Destroy(obj);
                }, cancelText: "����ϱ�")
                .Build();

                
            }).AddTo(reincarnationButton.gameObject);
    }

    private void SubscribeToReincarnationPopupView()
    {

    }

    private void SubscribeToModel()
    {

    }
}
