using UnityEditor;
using I2.Loc;
using System.IO;

public class PostBuild
{
    [InitializeOnLoadMethod]
    static void OnPreprocessBuild()
    {
        string path = "Assets/Plugins/Android/res/values/strings.xml";

        if (File.Exists(path))
        {
            string content = File.ReadAllText(path);
            content = content.Replace("app_name", LocalizationManager.GetTranslation("AppName"));
            File.WriteAllText(path, content);
        }
    }

}
