using System;
using System.Numerics;
using System.Collections.Generic;

public abstract class CurrencyBase : ICurrency<BigInteger>, IObservable<BigInteger>
{
	private BigInteger m_amount;
	public BigInteger Amount => m_amount;

	List<IObserver<BigInteger>> m_observerList = new ();

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

	public bool CanSubtract(BigInteger delta)
	{
		return m_amount - BigInteger.Abs(delta) >= 0;
	}

	public bool Subtract(BigInteger value)
	{
		if (!CanSubtract(value)) return false;

		m_amount -= value;
		m_observerList.ForEach(observer => observer.OnNext(m_amount));
		return true;
	}
}