using System;
using System.Numerics;
using System.Collections.Generic;

public abstract class IRCurrencyBase : ICurrency<BigInteger>, IObservable<BigInteger>
{
	private BigInteger m_amount;
	List<IObserver<BigInteger>> m_observerList = new List<IObserver<BigInteger>>();

	public BigInteger Amount => m_amount;

	public IDisposable Subscribe(IObserver<BigInteger> observer)
	{
		m_observerList.Add(observer);
		m_observerList.ForEach(observer => observer.OnNext(m_amount));
		return new Unsubscriber(() => m_observerList = null);
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
		m_observerList.ForEach(observer => observer.OnNext(m_amount));
	}

	public bool IsPositive(BigInteger delta)
	{
		return m_amount - BigInteger.Abs(delta) >= 0;
	}

	public bool Sub(BigInteger value)
	{
		if (!IsPositive(value)) return false;

		m_amount -= value;
		m_observerList.ForEach(observer => observer.OnNext(m_amount));
		return true;
	}
}
public class Gold : IRCurrencyBase { }
public class Dia : IRCurrencyBase { }
public class Key : IRCurrencyBase { }
