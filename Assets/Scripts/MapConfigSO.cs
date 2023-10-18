using System;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Map", menuName = "ScriptableObjects/Map")]
public class MapConfigSO : ScriptableObject
{
    [SerializeField] private List<MapConfig> maps;
    public MapConfigSO(List<MapConfig> maps)
    {
        this.maps = maps;
    }

    public List<MapConfig> Maps { get => maps; set => maps = value; }
}
[Serializable]
public class MapConfig
{
    [SerializeField] private string mapName;
    [SerializeField] private string displayName;
    [SerializeField] private int level;
    [SerializeField] private float playTime;
    [SerializeField] private List<MapDetail> mapDetails;

    public string MapName { get => mapName; set => mapName = value; }
    public string DisplayName { get => displayName; set => displayName = value; }
    public int Level { get => level; set => level = value; }
    public float PlayTime { get => playTime; set => playTime = value; }
    public List<MapDetail> MapDetails { get => mapDetails; set => mapDetails = value; }
}
[Serializable]
public class MapDetail
{
    [SerializeField] private int id;
    [SerializeField] private TileType type;
    [SerializeField] private Sprite sprite;
    [SerializeField] private int chance;

    public int Id { get => id; set => id = value; }
    public TileType Type { get => type; set => type = value; }
    public Sprite Sprite { get => sprite; set => sprite = value; }
    public int Chance { get => chance; set => chance = value; }
}
