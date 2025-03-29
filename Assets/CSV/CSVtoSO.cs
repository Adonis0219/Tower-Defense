using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public class CSVtoSO
{

    /*
    // csv 파일 경로
    private static string enemyCSVPath = "/CSV/Monster.csv";
    [MenuItem("Utilities/Generate Enemies")]
    public static void GenerateEnemies()
    {
        // 경로에 있는 파일 텍스트를 모두 읽어 한 줄 단위로 나누어 배열 형태로 내보내주는 함수
        string[] allLines = File.ReadAllLines(Application.dataPath + enemyCSVPath);

        foreach (string allLine in allLines)
        {
            // 문자열을 , 단위 
            string[] splitData = allLine.Split(',');

            if (splitData.Length != 4)
            {
                Debug.Log(allLine + " Does not have 4 values");
            }

            // 새로운 SO 만들어주기
            T_Enemy enemy = ScriptableObject.CreateInstance<T_Enemy>();
            enemy.enemyName = splitData[0];
            // int.Parse 문자열 -> 정수형
            enemy.hp = int.Parse(splitData[1]);
            enemy.strength = int.Parse(splitData[2]);
            enemy.xpReward = int.Parse(splitData[3]);

            AssetDatabase.CreateAsset(enemy, $"Assets/CSV/{enemy.enemyName}.asset");
        }
        AssetDatabase.SaveAssets();
    }


    const string REQ_TIME = "ReqTime";
    const string COST = "Cost";

    [MenuItem("Utilities/Import Level Datas")]
    public static void ImportLvDatas()
    {
        A("Damage", "A. Res 0 (Damage)");
        A("AtkSpeed", "A. Res 1 (Atk Speed)");
    }

    static void A(string csv, string path)
    {
        List<Dictionary<string, object>> datas = CSVReader.Read("ResearchCSV/" + csv);

        ResearchData rschData = (ResearchData)Resources.Load("Research Datas/" + path);

        for (int i = 0; i < datas.Count; i++)
        {
            rschData.reqTimes[i] = TimeToInt(datas[i][REQ_TIME].ToString());
            rschData.costs[i] = (int)datas[i][COST];
        }
    }

    static int TimeToInt(string time)
    {
        string[] reqTime = time.Split(" ");

        //50d, 5h, 52m

        int totalMin = 0;

        const int DAY = 1440;
        const int HOUR = 60;

        for (int i = 0; i < reqTime.Length; i++)
        {
            // 문자 제거
            reqTime[i] = reqTime[i].Remove(reqTime[i].Length - 1);
        }

        totalMin = int.Parse(reqTime[0]) * DAY + int.Parse(reqTime[1]) * HOUR 
            + int.Parse(reqTime[2]);

        return totalMin * 60;
    }
    */
}
