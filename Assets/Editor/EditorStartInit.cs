using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityToolbarExtender;

[InitializeOnLoad]
public class CustomToolbarButton
{
    static CustomToolbarButton()
    {
        ToolbarExtender.RightToolbarGUI.Add(OnToolbarGUI);
    }

    static void OnToolbarGUI()
    {
        GUILayout.FlexibleSpace();

        if (GUILayout.Button(new GUIContent("�������")))
        {
            EditorSceneManager.playModeStartScene = null;
            UnityEditor.EditorApplication.isPlaying = true;
        }

        if (GUILayout.Button(new GUIContent("�����ξ�")))
        {
            var pathOfMainMenuScene = "Assets/Scenes/Main Scene.unity"; // Main Menu Scene�� ��θ� ��Ȯ�ϰ� �Է����ּ���.
            var sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(pathOfMainMenuScene);
            EditorSceneManager.playModeStartScene = sceneAsset;
            UnityEditor.EditorApplication.isPlaying = true;
        }
    }
}