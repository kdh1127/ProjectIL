using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public static class DataUtility
{
	private static readonly string filePath = Application.persistentDataPath + "/data.json";
    private static readonly string encryptionKey =  "1234567890abcdef";
    private static readonly string encryptionIV =   "1234567890abcdef";

    private static string EncryptData(string data)
    {
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = Encoding.UTF8.GetBytes(encryptionKey);
            aesAlg.IV = Encoding.UTF8.GetBytes(encryptionIV);

            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
            using (MemoryStream msEncrypt = new())
            {
                using (CryptoStream csEncrypt = new(msEncrypt, encryptor, CryptoStreamMode.Write))
                using (StreamWriter swEncrypt = new(csEncrypt))
                {
                    swEncrypt.Write(data);
                }
                return Convert.ToBase64String(msEncrypt.ToArray());
            }
        }
    }

    private static string DecryptData(string encryptedData)
    {
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = Encoding.UTF8.GetBytes(encryptionKey);
            aesAlg.IV = Encoding.UTF8.GetBytes(encryptionIV);

            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
            using (MemoryStream msDecrypt = new(Convert.FromBase64String(encryptedData)))
            {
                using (CryptoStream csDecrypt = new(msDecrypt, decryptor, CryptoStreamMode.Read))
                using (StreamReader srDecrypt = new(csDecrypt))
                {
                    return srDecrypt.ReadToEnd();
                }
            }
        }
    }

    public static void Save<T>(string path, T data)
    {
        try
        {
            string jsonData = JsonConvert.SerializeObject(data);
            string encryptedData = EncryptData(jsonData);

            File.WriteAllText(Application.persistentDataPath + path, encryptedData);
        }
        catch (IOException ex)
        {
            Debug.LogError("파일 저장 중 오류 발생: " + ex.Message);
        }
    }

    public static T Load<T>(string path, T defaultValue = default)
    {
        try
        {
            if (File.Exists(path))
            {
                string encryptedData = File.ReadAllText(Application.persistentDataPath + path);
                string jsonData = DecryptData(encryptedData);

                return JsonConvert.DeserializeObject<T>(jsonData);
            }
            else
            {
                Debug.LogWarning("DataUtility: The file does not exist: " + path);
                return defaultValue;
            }
        }
        catch (IOException ex)
        {
            Debug.LogError("DataUtility: An error occurred while loading the file: " + ex.Message);
            return defaultValue;
        }
        catch (Exception ex)
        {
            Debug.LogError("DataUtility: An unknown error occurred: " + ex.Message);
            return defaultValue;
        }
    }
}
