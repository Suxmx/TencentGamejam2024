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
            // �ڴ˴�ִ�б����߼������籣�������޸ĵĶ���
            foreach (var obj in Object.FindObjectsByType<GameObject>(FindObjectsSortMode.None))
            {
                EditorUtility.SetDirty(obj);
            }
            AssetDatabase.SaveAssets();
        }
    }
}
