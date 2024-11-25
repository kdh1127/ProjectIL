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
        TRLog.Green($"¿­¼è: {key.Amount}");
    }

    public BigInteger GetReward()
    {
        return (StageManager.Instance.stageBaseHp / 100) * StageManager.Instance.CurStage.Value;
    }

    public void Reincarnation(int rate)
    {
        key.Add(GetReward()*rate);
    }
}
