using UnityEngine;

namespace UnityEditor
{
    internal class PlayerPrefsSettings
    {
        [MenuItem("Window/Clear")]
        static void ClearPlayerPrefs()
        {
            if (EditorUtility.DisplayDialog("Clear All PlayerPrefs",
                "Are you sure you want to clear all PlayerPrefs? " +
                "This action cannot be undone.", "Yes", "No"))
            {
                PlayerPrefs.DeleteAll();
            }
        }
    }
}