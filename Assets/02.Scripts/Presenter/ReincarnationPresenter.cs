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
        //SubscribeToReincarnationButton();
        SubscribeToReincarnationButton2();
        SubscribeToReincarnationPopupView();
        SubscribeToModel();
    }

    private void SubscribeToReincarnationButton()
    {
        reincarnationButton.button.OnClickAsObservable()
            .Subscribe(_ =>
            {
                reincarnationPopupView = reincarnationButton.OnPopup().GetComponent<ReincarnationPopupView>();
            }).AddTo(reincarnationButton.gameObject);
    }

    private void SubscribeToReincarnationButton2()
    {

        reincarnationButton.button.OnClickAsObservable()
            .Subscribe(_ =>
            {
                TRCommonPopup.Instantiate(PopupManager.Instance.transform)
                .SetTitle("환생하기")
                //.SetItemImage("열쇠이미지 넣고")
                .SetMessage($"환생하시겟습니가?\n 열쇠 흭득량: {model.GetReward()}")
                .SetConfirm(confirmAction: obj =>
                {
                    model.Reincarnation(1);
                    questModel.Reset();
                    weaponModel.Reset();
                    BattlePresenter.Instance.battleState = new BattlePresenter.ResetState();
                    GameObject.Destroy(obj);
                }, confirmText: "환생하기")
                .SetCancel(cancelAction: obj =>
                {
                    GameObject.Destroy(obj);
                }, cancelText: "취소하기")
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
