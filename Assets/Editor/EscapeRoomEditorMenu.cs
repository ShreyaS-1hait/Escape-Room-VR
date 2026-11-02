using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

/// <summary>
/// Unity Editor utility forwarding menu commands to EscapeRoomSceneBuilder.
/// Automatically verifies and opens EscapeRoom.unity on load.
/// </summary>
[InitializeOnLoad]
public static class EscapeRoomEditorMenu
{
    static EscapeRoomEditorMenu()
    {
        EditorApplication.delayCall += OnEditorLoaded;
    }

    private static void OnEditorLoaded()
    {
        const string scenePath = "Assets/Scenes/EscapeRoom.unity";
        var activeScene = EditorSceneManager.GetActiveScene();
        if (activeScene == null || string.IsNullOrEmpty(activeScene.path) || activeScene.path != scenePath)
        {
            if (System.IO.File.Exists(scenePath))
            {
                EditorSceneManager.OpenScene(scenePath);
                Debug.Log($"<color=green><b>[VR Escape Room]</b> Auto-loaded active scene: {scenePath}</color>");
            }
        }
        AddSceneToBuildSettings();
    }

    [MenuItem("VR Escape Room/Generate & Save Escape Room Scene", false, 10)]
    public static void GenerateAndSaveScene()
    {
        EscapeRoomSceneBuilder.BuildAndSaveScene(!Application.isBatchMode);
    }

    [MenuItem("VR Escape Room/Add EscapeRoom Scene to Build Settings", false, 11)]
    public static void AddSceneToBuildSettings()
    {
        EscapeRoomSceneBuilder.AddSceneToBuildSettings();
    }

    [MenuItem("VR Escape Room/Open EscapeRoom Scene", false, 12)]
    public static void OpenEscapeRoomScene()
    {
        EscapeRoomSceneBuilder.OpenScene();
    }
}

