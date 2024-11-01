using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using ThreeRabbitPackage.DesignPattern;

public class MonsterManager : TRSingleton<MonsterManager>
{
    [SerializeField] private List<Transform> monsterPositionList_tr;
    public List<MonsterModel> monsterModelList = new();

    public int monsterIndex = 0;

    public MonsterModel CreateMonster(StageTable table, bool isBoss)
    {
        // 타입 지정
        EnumList.EMonsterType monsterType = isBoss ? EnumList.EMonsterType.BOSS : EnumList.EMonsterType.NORMAL;
        var monsterName = isBoss ? table.MonsterBossName : table.MonsterNormalName;

        // 모델 생성
        MonsterModel monsterModel = new();
        monsterModel.Init(table, monsterType);

        monsterModelList.Add(monsterModel);
        return monsterModel;
    }

    public Transform GetSpawnPosition(int index)
	{
        return monsterPositionList_tr[index];
	}

    public void IncreaseIndex()
    {
        monsterIndex++;

        if (monsterIndex > 4) monsterIndex = 0;
    }

    public MonsterModel GetTargetMonster()
	{
        return monsterModelList[monsterIndex];
	}
}
