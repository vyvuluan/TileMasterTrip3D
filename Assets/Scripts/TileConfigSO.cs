using System;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "TileType", menuName = "ScriptableObjects/TileType")]
public class TileConfigSO : ScriptableObject
{
    [SerializeField] private List<TileConfig> tileTypes;
    public TileConfigSO(List<TileConfig> tileTypes)
    {
        this.tileTypes = tileTypes;
    }
    public List<TileConfig> TileTypes { get => tileTypes; set => tileTypes = value; }
}
[Serializable]
public class TileConfig
{
    [SerializeField] private int type;
    [SerializeField] private Sprite sprite;

    public int Type { get => type; set => type = value; }
    public Sprite Sprite { get => sprite; set => sprite = value; }
}
