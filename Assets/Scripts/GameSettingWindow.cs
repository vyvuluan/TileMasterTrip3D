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
    private MapConfigSO mapConfigSO;
    private TileConfigSO tileConfigSO;
    private List<MapConfig> myMapConfigs;
    private List<TileConfig> myTileConfigs;

    private List<string> types;
    [MenuItem("Tools/GameSetting")]
    public static void ShowWindow()
    {
        GetWindow<GameSettingWindow>("GameSetting");
    }
    private void OnEnable()
    {
        ResetMapConfig();
        ResetTileTypeConfig();
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
        if (onClickTab2)
        {
            LoadContentConfigTile();
        }
    }
    private void ResetMapConfig()
    {
        mapConfigSO = AssetDatabase.LoadAssetAtPath<MapConfigSO>(Constanst.PathToScriptableObjectMap);
        myMapConfigs = Instantiate(mapConfigSO).Maps.ToList();

    }
    private void ResetTileTypeConfig()
    {
        tileConfigSO = AssetDatabase.LoadAssetAtPath<TileConfigSO>(Constanst.PathToScriptableObjectTileType);
        myTileConfigs = Instantiate(tileConfigSO).TileTypes.ToList();
        InitListStringType();
    }
    private void InitListStringType()
    {
        types = new();
        for (int i = 0; i < myTileConfigs.Count; i++)
        {
            types.Add($"Type {myTileConfigs[i].Type}");
        }
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
        if (tileConfigSO == null) Debug.Log("aaa");
        else Debug.Log("bbb");
        tileConfigSO = (TileConfigSO)EditorGUILayout.ObjectField("Tile Config SO", tileConfigSO, typeof(TileConfigSO), false);

        // Your content for Tab 2 goes here
        EditorGUILayout.LabelField("Tab 2 Content");
        onClickTab1 = false;
        onClickTab2 = true;
    }
    private void LoadContentConfigTile()
    {
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Save", GUILayout.Width(100)))
        {
            SaveTileConfig();
        }
        if (GUILayout.Button("Add", GUILayout.Width(100)))
        {
            TileConfig temp = new();
            myTileConfigs.Add(temp);

            myTileConfigs[^1].Sprite = null;
            myTileConfigs[^1].Type = myTileConfigs[^2].Type + 1;
            types.Add($"Type {myTileConfigs[^1].Type}");
        }
        if (GUILayout.Button("Reload", GUILayout.Width(100)))
        {
            ResetTileTypeConfig();
        }
        EditorGUILayout.EndHorizontal();


        // Draw the table headers
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Type", GUILayout.Width(100));
        EditorGUILayout.LabelField("Image", GUILayout.Width(100));
        EditorGUILayout.EndHorizontal();

        // Draw the table rows
        for (int i = 0; i < myTileConfigs.Count; i++)
        {
            TileConfig tileConfigTemp = myTileConfigs[i];
            EditorGUILayout.BeginHorizontal();
            tileConfigTemp.Type = EditorGUILayout.Popup(tileConfigTemp.Type, types.ToArray(), GUILayout.Width(100));
            tileConfigTemp.Sprite = (Sprite)EditorGUILayout.ObjectField(tileConfigTemp.Sprite, typeof(Sprite), false, GUILayout.Width(80), GUILayout.Height(80));
            if (GUILayout.Button("x", GUILayout.Width(30)))
            {
                myTileConfigs.Remove(myTileConfigs[i]);
            }
            EditorGUILayout.EndHorizontal();
        }
    }
    private void SaveLevelConfig()
    {
        mapConfigSO.Maps = myMapConfigs;
        EditorUtility.SetDirty(mapConfigSO);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        ResetMapConfig();
    }
    private void SaveTileConfig()
    {
        tileConfigSO.TileTypes = myTileConfigs;
        EditorUtility.SetDirty(mapConfigSO);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        ResetTileTypeConfig();
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
            myMapConfigs[index].MapDetails[^1].Type = 0;
            myMapConfigs[index].MapDetails[^1].Sprite = tileConfigSO.TileTypes[0].Sprite;
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
        InitListStringType();
        // Draw the table rows
        for (int i = 0; i < myMapConfigs[index].MapDetails.Count; i++)
        {
            MapDetail mapDetail = myMapConfigs[index].MapDetails[i];
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(mapDetail.Id.ToString(), GUILayout.Width(100));
            myMapConfigs[index].MapDetails[i].Type = EditorGUILayout.Popup((int)myMapConfigs[index].MapDetails[i].Type, types.ToArray(), GUILayout.Width(100));
            GUILayout.Box(tileConfigSO.TileTypes[myMapConfigs[index].MapDetails[i].Type].Sprite.texture, GUILayout.Width(100), GUILayout.Height(100));
            myMapConfigs[index].MapDetails[i].Sprite = tileConfigSO.TileTypes[myMapConfigs[index].MapDetails[i].Type].Sprite;
            myMapConfigs[index].MapDetails[i].Chance = EditorGUILayout.IntField(mapDetail.Chance, GUILayout.Width(80));
            if (GUILayout.Button("x", GUILayout.Width(30)))
            {
                myMapConfigs[index].MapDetails.Remove(myMapConfigs[index].MapDetails[i]);
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}
