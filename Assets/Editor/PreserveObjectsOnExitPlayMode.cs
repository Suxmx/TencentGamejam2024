using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class PreserveChangesInPlayMode
{
    static PreserveChangesInPlayMode()
    {
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }

    private static void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.EnteredEditMode)
        {
            // 在此处执行保存逻辑，例如保存所有修改的对象
            foreach (var obj in Object.FindObjectsByType<GameObject>(FindObjectsSortMode.None))
            {
                EditorUtility.SetDirty(obj);
            }
            AssetDatabase.SaveAssets();
        }
    }
}
