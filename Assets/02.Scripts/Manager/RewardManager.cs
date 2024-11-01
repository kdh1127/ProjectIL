using System.Numerics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ThreeRabbitPackage.DesignPattern;

public class Reward
{
	public EnumList.ERewardType type;
	public BigInteger amount;
}
public class RewardManager : TRSingleton<RewardManager>
{

}
