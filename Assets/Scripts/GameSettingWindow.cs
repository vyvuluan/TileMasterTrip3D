using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class GameSettingWindow : EditorWindow
{
    private int selectedTab = 0;
    private int selectedTabMap = 0;
    private string[] tabLabels = { "Map Config", "Tile Config" };
    private bool onClickTab1 = false;
    private bool onClickTab2 = false;
    private List<string> tabMaps;
    public MapConfigSO mapConfigSO;

    private List<MapConfig> myMapConfigs;
    [MenuItem("Tools/GameSetting")]
    public static void ShowWindow()
    {
        GetWindow<GameSettingWindow>("GameSetting");
    }
    private void OnEnable()
    {
        ResetMapConfig();
    }
    private void OnGUI()
    {
        selectedTab = GUILayout.Toolbar(selectedTab, tabLabels);
        switch (selectedTab)
        {
            case 0:
                // Content for Tab 1
                ConfigMapTab();
                break;
            case 1:
                // Content for Tab 2
                ConfigTileTab();
                break;
        }
        if (onClickTab1)
        {
            selectedTabMap = GUILayout.Toolbar(selectedTabMap, tabMaps.ToArray());
            LoadSOMap(selectedTabMap);
        }
    }
    private void ResetMapConfig()
    {
        mapConfigSO = mapConfigSO = AssetDatabase.LoadAssetAtPath<MapConfigSO>(Constanst.PathToScriptableObject);
        myMapConfigs = mapConfigSO.Maps.ToList();
    }
    private void ConfigMapTab()
    {
        mapConfigSO = (MapConfigSO)EditorGUILayout.ObjectField("Map Config SO", mapConfigSO, typeof(MapConfigSO), false);

        tabMaps = new();
        onClickTab1 = true;
        onClickTab2 = false;
        for (int i = 0; i < mapConfigSO.Maps.Count; i++)
        {
            tabMaps.Add(mapConfigSO.Maps[i].MapName);
        }

    }
    private void ConfigTileTab()
    {
        // Your content for Tab 2 goes here
        EditorGUILayout.LabelField("Tab 2 Content");
        onClickTab1 = false;
        onClickTab2 = true;
    }
    private void SaveLevelConfig()
    {
        mapConfigSO.Maps = myMapConfigs;
        EditorUtility.SetDirty(mapConfigSO);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        ResetMapConfig();
    }

    private void LoadSOMap(int index)
    {
        GUILayout.Label("Custom Table", EditorStyles.boldLabel);

        // Add a button to add a new row
        if (GUILayout.Button("Add Row"))
        {
            //data.Add(new CustomData());
        }

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Name", GUILayout.Width(100));
        myMapConfigs[index].MapName = EditorGUILayout.TextField(mapConfigSO.Maps[index].MapName, GUILayout.Width(100));
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Display Name", GUILayout.Width(100));
        myMapConfigs[index].DisplayName = EditorGUILayout.TextField(mapConfigSO.Maps[index].DisplayName, GUILayout.Width(100));
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Level", GUILayout.Width(100));
        myMapConfigs[index].Level = EditorGUILayout.IntField(mapConfigSO.Maps[index].Level, GUILayout.Width(100));
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Play Time (s)", GUILayout.Width(100));
        myMapConfigs[index].PlayTime = EditorGUILayout.IntField((int)mapConfigSO.Maps[index].PlayTime, GUILayout.Width(100));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Save", GUILayout.Width(100)))
        {
            SaveLevelConfig();
        }
        if (GUILayout.Button("Add", GUILayout.Width(100)))
        {
            MapDetail temp = new();
            temp.Id = myMapConfigs[index].MapDetails[^1].Id++;
            temp.Sprite = myMapConfigs[index].MapDetails[^1].Sprite;
            temp.Type = myMapConfigs[index].MapDetails[^1].Type;
            temp.Chance = myMapConfigs[index].MapDetails[^1].Chance;
            myMapConfigs[index].MapDetails.Add(temp);
        }
        EditorGUILayout.EndHorizontal();

        // Draw the table headers
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Id", GUILayout.Width(100));
        EditorGUILayout.LabelField("Type", GUILayout.Width(100));
        EditorGUILayout.LabelField("Image", GUILayout.Width(100));
        EditorGUILayout.LabelField("Quantity", GUILayout.Width(100));
        EditorGUILayout.EndHorizontal();

        // Draw the table rows
        for (int i = 0; i < myMapConfigs[index].MapDetails.Count; i++)
        {
            MapDetail mapDetail = myMapConfigs[index].MapDetails[i];
            EditorGUILayout.BeginHorizontal();
            //EditorGUILayout.TextField(mapDetail.Id.ToString(), GUILayout.Width(100));
            EditorGUILayout.LabelField(mapDetail.Id.ToString(), GUILayout.Width(100));
            string[] enumOptions = System.Enum.GetNames(typeof(TileType));
            myMapConfigs[index].MapDetails[i].Type = (TileType)EditorGUILayout.Popup((int)myMapConfigs[index].MapDetails[i].Type, enumOptions, GUILayout.Width(100));
            myMapConfigs[index].MapDetails[i].Sprite = (Sprite)EditorGUILayout.ObjectField(mapDetail.Sprite, typeof(Sprite), false, GUILayout.Width(80), GUILayout.Height(80));
            myMapConfigs[index].MapDetails[i].Chance = EditorGUILayout.IntField(mapDetail.Chance, GUILayout.Width(80));
            if (GUILayout.Button("Delete", GUILayout.Width(100)))
            {
                Debug.Log("Button clicked for row " + i);
                myMapConfigs[index].MapDetails.Remove(myMapConfigs[index].MapDetails[i]);
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}
