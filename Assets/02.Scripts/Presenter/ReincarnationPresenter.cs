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
                .SetTitle("ȯ���ϱ�")
                //.SetItemImage("�����̹��� �ְ�")
                .SetMessage($"ȯ���Ͻðٽ��ϰ�?\n ���� ŉ�淮: {model.GetReward()}")
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
