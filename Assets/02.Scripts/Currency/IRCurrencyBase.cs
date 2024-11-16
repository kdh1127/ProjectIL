using System.Numerics;
using UnityEngine;
using UniRx;
using System;

public abstract class IRCurrencyBase : ICurrency<BigInteger>
{
	private BigInteger m_amount;
	private IObserver<BigInteger> m_observer;

	public BigInteger Amount => m_amount;

	public IDisposable Subscribe(IObserver<BigInteger> observer)
	{
		m_observer = observer;
		return new Unsubscriber(() => m_observer = null);
	}

	private class Unsubscriber : IDisposable
	{
		private readonly Action m_unsubscribeAction;

		public Unsubscriber(Action unsubscribeAction)
		{
			m_unsubscribeAction = unsubscribeAction;
		}

		public void Dispose()
		{
			m_unsubscribeAction?.Invoke();
		}
	}

	public void Add(BigInteger value)
	{
		m_amount += value;
		m_observer?.OnNext(m_amount);
	}

	public bool IsPositive(BigInteger delta)
	{
		return m_amount - BigInteger.Abs(delta) >= 0;
	}

	public bool Sub(BigInteger value)
	{
		if (!IsPositive(value)) return false;

		m_amount -= value;
		m_observer?.OnNext(m_amount);
		return true;
	}
}
public class Gold : IRCurrencyBase { }
public class Dia : IRCurrencyBase { }
public class Key : IRCurrencyBase { }
