using ThreeRabbitPackage.Util;
using System.Numerics;
using Zenject;

public class ReincarnationModel
{
    private readonly CurrencyModel.Key key;
    [Inject]
    public ReincarnationModel(CurrencyModel.Key key)
    {
        this.key = key;
    }

    public void Init()
    {
    }

    public BigInteger GetReward()
    {
        return (StageManager.Instance.stageBaseHp / 100) * StageManager.Instance.CurStage.Value / 10;
    }

    public void Reincarnation(int rate)
    {
        key.Add(GetReward()*rate);
    }
}
