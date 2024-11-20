public interface ICurrency<T>
{
	public T Amount { get; }
	void Add(T amount);
	bool Subtract(T amount);
}