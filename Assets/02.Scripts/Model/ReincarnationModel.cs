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
        var curStage = StageManager.Instance.CurStage.Value;
        return curStage + (curStage * curStage / 100);
    }

    public void Reincarnation(int rate)
    {
        key.Add(GetReward()*rate);
    }
}
