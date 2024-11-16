using System;

public interface ICurrency<T> : IObservable<T>
{
	public T Amount { get; }
	public bool IsPositive(T delta);
	public void Add(T value);
	public bool Sub(T value);
}