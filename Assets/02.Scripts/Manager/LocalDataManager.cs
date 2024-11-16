using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ThreeRabbitPackage.DesignPattern;
public class LocalDataManager<T> : TRSingleton<LocalDataManager<T>> 
{
	public void RegisterLocalData(T localData)
	{

	}

	public void Save(string path, T localData)
	{
		DataUtility.Save<T>(path, localData);
	}

	public T Load(string path, T defalutValue = default)
	{
		var data = DataUtility.Load<T>(path, defalutValue);
		return data;
	}
}
