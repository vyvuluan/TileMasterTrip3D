
//using UnityEditor;
//using UnityEngine;
//public class GameSettingWindow : EditorWindow
//{
//    Color color;
//    public MapConfigSO myScriptableObject;
//    [SerializeField] private Sprite temp;
//    private Sprite selectedSprite;
//    [MenuItem("Window/GameSetting")]
//    public static void ShowWindow()
//    {
//        GetWindow<GameSettingWindow>("GameSetting");
//    }
//    private void OnEnable()
//    {
//        myScriptableObject = Resources.Load<MapConfigSO>("Map");
//        if (myScriptableObject == null)
//            Debug.Log("Ádas");
//    }
//    private void OnGUI()
//    {
//        //GUILayout.Label("MyScriptableObject:");

//        //// Hiển thị trường để chọn ScriptableObject
//        //myScriptableObject = (MapConfigSO)EditorGUILayout.ObjectField("MyScriptableObject", myScriptableObject, typeof(MapConfigSO), false);

//        //GUILayout.Label("Map", EditorStyles.boldLabel);
//        ////Rect objectFieldRect = GUILayoutUtility.GetRect(100, 100);
//        ////selectedSprite = (Sprite)EditorGUI.ObjectField(objectFieldRect, "Sprite", selectedSprite, typeof(Sprite), false);
//        //GUILayout.BeginHorizontal();
//        //GUILayout.Label("idx", EditorStyles.boldLabel);
//        //GUILayout.Label("Type", EditorStyles.boldLabel);
//        //GUILayout.Label("Sprite", EditorStyles.boldLabel);
//        //GUILayout.Label("Chance", EditorStyles.boldLabel);
//        //GUILayout.EndHorizontal();
//        //Rect objectFieldRect = GUILayoutUtility.GetRect(100, 100);
//        //selectedSprite = temp;
//        //selectedSprite = (Sprite)EditorGUI.ObjectField(objectFieldRect, "Sprite", selectedSprite, typeof(Sprite), false);

//        myScriptableObject = (MapConfigSO)EditorGUILayout.ObjectField("My ScriptableObject", myScriptableObject, typeof(MapConfigSO), false);

//        if (GUILayout.Button("Use Sprite from ScriptableObject"))
//        {
//            if (myScriptableObject != null)
//            {
//                selectedSprite = myScriptableObject.Maps[0].MapDetails[0].Sprite;
//            }
//            else
//            {
//                Debug.LogWarning("No ScriptableObject selected!");
//            }
//        }

//        // Hiển thị sprite trong EditorWindow
//        if (selectedSprite != null)
//        {
//            GUILayout.Label("Sprite from ScriptableObject:");
//            //Rect spriteRect = GUILayoutUtility.GetRect(100, 100);
//            //EditorGUI.DrawPreviewTexture(spriteRect, selectedSprite.texture);
//            Rect objectFieldRect = GUILayoutUtility.GetRect(100, 100);
//            selectedSprite = (Sprite)EditorGUI.ObjectField(objectFieldRect, "Sprite", selectedSprite, typeof(Sprite), false);
//        }
//    }
//}
