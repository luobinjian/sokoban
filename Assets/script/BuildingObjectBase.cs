using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum Type
{
    Wall,
    ground,
    player,
    box,
    target,
    ice,
}

[CreateAssetMenu(fileName = "Buildable", menuName = "BuildingObjectBase")]
public class BuildingObjectBase : ScriptableObject
{
    [SerializeField] public Type type;
    [SerializeField] public TileBase tileBase;

    public Type Type => type;
    public TileBase TileBase => tileBase;
}
