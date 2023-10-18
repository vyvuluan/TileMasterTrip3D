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
    private bool onClickDeleteMap = false;
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
        mapConfigSO = AssetDatabase.LoadAssetAtPath<MapConfigSO>(Constanst.PathToScriptableObject);
        myMapConfigs = Instantiate(mapConfigSO).Maps.ToList();

    }
    private void ConfigMapTab()
    {
        mapConfigSO = (MapConfigSO)EditorGUILayout.ObjectField("Map Config SO", mapConfigSO, typeof(MapConfigSO), false);

        tabMaps = new();
        onClickTab1 = true;
        onClickTab2 = false;
        for (int i = 0; i < myMapConfigs.Count; i++)
        {
            tabMaps.Add(myMapConfigs[i].MapName);
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
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Add New Map"))
        {
            MapConfig mapConfigTemp = new();
            mapConfigTemp.MapName = "string";
            mapConfigTemp.DisplayName = "string";
            mapConfigTemp.MapDetails = new();
            mapConfigTemp.PlayTime = 0;
            myMapConfigs.Add(mapConfigTemp);
        }
        if (GUILayout.Button("Delete Map"))
        {
            myMapConfigs.RemoveAt(index);
            tabMaps.RemoveAt(index);

            selectedTabMap = 0;
            ConfigMapTab();
            onClickDeleteMap = true;
        }
        else
        {
            onClickDeleteMap = false;
        }
        EditorGUILayout.EndHorizontal();
        if (onClickDeleteMap) return;
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Name", GUILayout.Width(100));
        myMapConfigs[index].MapName = EditorGUILayout.TextField(myMapConfigs[index].MapName, GUILayout.Width(100));
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Display Name", GUILayout.Width(100));
        myMapConfigs[index].DisplayName = EditorGUILayout.TextField(myMapConfigs[index].DisplayName, GUILayout.Width(100));
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Level", GUILayout.Width(100));
        myMapConfigs[index].Level = EditorGUILayout.IntField(myMapConfigs[index].Level, GUILayout.Width(100));
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Play Time (s)", GUILayout.Width(100));
        myMapConfigs[index].PlayTime = EditorGUILayout.IntField((int)myMapConfigs[index].PlayTime, GUILayout.Width(100));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Save", GUILayout.Width(100)))
        {
            SaveLevelConfig();
        }
        if (GUILayout.Button("Add", GUILayout.Width(100)))
        {
            MapDetail temp = new();
            Debug.Log(index);
            myMapConfigs[index].MapDetails.Add(temp);

            myMapConfigs[index].MapDetails[^1].Id = (myMapConfigs[index].MapDetails[^1] != null) ? myMapConfigs[index].MapDetails[^2].Id + 1 : 0;
            myMapConfigs[index].MapDetails[^1].Sprite = null;
            myMapConfigs[index].MapDetails[^1].Type = TileType.TYPE0;
            myMapConfigs[index].MapDetails[^1].Chance = 1;
        }
        if (GUILayout.Button("Reload", GUILayout.Width(100)))
        {
            ResetMapConfig();
        }
        EditorGUILayout.EndHorizontal();

        // Draw the table headers
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Id", GUILayout.Width(100));
        EditorGUILayout.LabelField("Type", GUILayout.Width(100));
        EditorGUILayout.LabelField("Image", GUILayout.Width(100));
        EditorGUILayout.LabelField("Chance", GUILayout.Width(100));
        EditorGUILayout.EndHorizontal();

        // Draw the table rows
        for (int i = 0; i < myMapConfigs[index].MapDetails.Count; i++)
        {
            MapDetail mapDetail = myMapConfigs[index].MapDetails[i];
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(mapDetail.Id.ToString(), GUILayout.Width(100));
            string[] enumOptions = System.Enum.GetNames(typeof(TileType));
            myMapConfigs[index].MapDetails[i].Type = (TileType)EditorGUILayout.Popup((int)myMapConfigs[index].MapDetails[i].Type, enumOptions, GUILayout.Width(100));
            myMapConfigs[index].MapDetails[i].Sprite = (Sprite)EditorGUILayout.ObjectField(mapDetail.Sprite, typeof(Sprite), false, GUILayout.Width(80), GUILayout.Height(80));
            myMapConfigs[index].MapDetails[i].Chance = EditorGUILayout.IntField(mapDetail.Chance, GUILayout.Width(80));
            if (GUILayout.Button("x", GUILayout.Width(30)))
            {
                myMapConfigs[index].MapDetails.Remove(myMapConfigs[index].MapDetails[i]);
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}
