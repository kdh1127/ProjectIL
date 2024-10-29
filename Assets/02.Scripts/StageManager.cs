using I2.Loc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ThreeRabbitPackage;
using ThreeRabbitPackage.DesignPattern;

public class StageManager : TRSingleton<StageManager>
{
	public TRGoogleSheet stageTable;

	public string GetLocalizationStage(int curStage)
    {
        var stageFormat = ScriptLocalization.Localization.Stage;
        var stageString = string.Format(stageFormat, curStage);
        return stageString;
    }
}
