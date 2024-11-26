using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using UnityEngine;
using ThreeRabbitPackage.Settings;

namespace ThreeRabbitPackage
{
	public static class TRSaveLoad
	{
		static ThreeRabbitPackageSettings trpSettings = Resources.Load<ThreeRabbitPackageSettings>("TRP_Settings");

		public static void Save<T>(T data, string fileName)
		{
			string jsonFilePath = string.Format("{0}/{1}", Application.dataPath + trpSettings.jsonFilePath, fileName);
			//string jsonData = JsonConvert.SerializeObject(data);
			string jsonData = data.ToString();
			byte[] bytes = System.Text.Encoding.UTF8.GetBytes(jsonData);
			string format = System.Convert.ToBase64String(bytes);
			
			if (trpSettings.isEncryption) File.WriteAllText(jsonFilePath + ".json", format);
			else File.WriteAllText(jsonFilePath + ".json", jsonData);
		}

		public static T Load<T>(string fileName)
		{
			string jsonFilePath = string.Format("{0}/{1}", Application.dataPath + trpSettings.jsonFilePath, fileName);
			string jsonData = File.ReadAllText(jsonFilePath + ".json");

			if (trpSettings.isEncryption)
			{
				byte[] bytes = System.Convert.FromBase64String(jsonData);
				string reformat = System.Text.Encoding.UTF8.GetString(bytes);

				//return JsonConvert.DeserializeObject<T>(reformat);
			}
			//return JsonConvert.DeserializeObject<T>(jsonData);
			return default(T);
		}
	}
}