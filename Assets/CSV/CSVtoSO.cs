using UnityEngine;
using UnityEditor;
using System.IO;

public class CSVtoSO
{
    private static string enemyCSVPath = "/CSV/Monster.csv";
    [MenuItem("Utilities/Generate Enemies")]
    public static void GenerateEnemies()
    {
        string[] allLines = File.ReadAllLines(Application.dataPath + enemyCSVPath);

        foreach (string allLine in allLines)
        {
            string[] splitData = allLine.Split(',');

            if (splitData.Length != 4)
            {
                Debug.Log(allLine + " Does not have 4 values");
            }

            T_Enemy enemy = ScriptableObject.CreateInstance<T_Enemy>();
            enemy.enemyName = splitData[0];
            enemy.hp = int.Parse(splitData[1]);
            enemy.strength = int.Parse(splitData[2]);
            enemy.xpReward = int.Parse(splitData[3]);

            AssetDatabase.CreateAsset(enemy, $"Assets/CSV/{enemy.enemyName}.asset");
        }

        AssetDatabase.SaveAssets();
    }
}
