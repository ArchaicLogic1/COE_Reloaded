using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityToolbarExtender;
[InitializeOnLoad()]

public static class ToolbarExtensions
{
   


    static ToolbarExtensions()
    {
        ToolbarExtender.LeftToolbarGUI.Add(DrawLeftGUI);
        ToolbarExtender.RightToolbarGUI.Add(DrawRightGUI);
    }
    static void DrawLeftGUI()
    {

        Scene PreviousScene = SceneManager.GetSceneByPath("Assets/Scenes/MainMenu");
        GUILayout.FlexibleSpace();
        if (GUILayout.Button(" Start Game "))
        {
            UnityEditor.SceneManagement.EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
            SceneHelper.StartScene("GameBrain");
        }

    }
    static void DrawRightGUI()
    {
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("GameBrain"))
        {

            UnityEditor.SceneManagement.EditorSceneManager.OpenScene("Assets/Scenes/GameBrain.unity");


        }
        if (GUILayout.Button("Level1"))
        {

            UnityEditor.SceneManagement.EditorSceneManager.OpenScene("Assets/Scenes/Level_1.unity");


        }
        if (GUILayout.Button("Level2"))
        {

            UnityEditor.SceneManagement.EditorSceneManager.OpenScene("Assets/Scenes/SampleLevel 2.unity");


        }
        if (GUILayout.Button("Level3"))
        {

            UnityEditor.SceneManagement.EditorSceneManager.OpenScene("Assets/Scenes/SampleLevel 3.unity");


        }
    }


}
static class SceneHelper
{
    static string sceneToOpen;

    public static void StartScene(string sceneName)
    {
        if (EditorApplication.isPlaying)
        {
            EditorApplication.isPlaying = false;
        }

        sceneToOpen = sceneName;
        EditorApplication.update += OnUpdate;
        static void OnUpdate()
        {
            if (sceneToOpen == null ||
                EditorApplication.isPlaying || EditorApplication.isPaused ||
                EditorApplication.isCompiling || EditorApplication.isPlayingOrWillChangePlaymode)
            {
                return;
            }

            EditorApplication.update -= OnUpdate;

            if (UnityEditor.SceneManagement.EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                // need to get scene via search because the path to the scene
                // file contains the package version so it'll change over time
                string[] guids = AssetDatabase.FindAssets("t:scene " + sceneToOpen, null);
                if (guids.Length == 0)
                {
                    Debug.LogWarning("Couldn't find scene file");
                }
                else
                {
                    string scenePath = AssetDatabase.GUIDToAssetPath(guids[0]);
                    UnityEditor.SceneManagement.EditorSceneManager.OpenScene(scenePath);
                    EditorApplication.isPlaying = true;
                }
            }
            sceneToOpen = null;
        }
    }

}

#endif