using System.Numerics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonClass : MonoBehaviour
{
	public struct AttackInfo
	{
		public BigInteger damage;
		public bool isCritical;

		public AttackInfo(BigInteger damage, bool isCritical)
		{
			this.damage = damage;
			this.isCritical = isCritical;
		}
	}
}
