using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public class SveManage : MonoBehaviour
{
    Dictionary<string, Tilemap> tilemaps = new Dictionary<string, Tilemap>();
    [SerializeField] BoundsInt boundsInt;
    [SerializeField] public string filename = "tilemapData.json";

    private void Start(){
        InitTilemaps();
        filename = PlayerPrefs.GetString("DefaultName");
        Debug.Log(filename);
    }

    private void InitTilemaps(){
        Tilemap[] maps = FindObjectsOfType<Tilemap>();
        foreach(Tilemap map in maps)
        {
            tilemaps.Add(map.name, map);
        }
    }

    public void onSave()
    {
        List<TilemapData> tilemapDatas = new List<TilemapData>();

        foreach(var mapObj in tilemaps)
        {
            TilemapData mapData = new TilemapData();
            mapData.name = mapObj.Key;

            for(int x = boundsInt.min.x; x < boundsInt.max.x; x++)
            {
                for(int y = boundsInt.min.y; y < boundsInt.max.y; y++)
                {
                    Vector3Int pos = new Vector3Int(x, y, 0);
                    TileBase tileBase = mapObj.Value.GetTile(pos);
                    if(tileBase != null)
                    {
                        TileInfo tileInfo = new TileInfo(pos, tileBase);
                        mapData.tiles.Add(tileInfo);
                    }
                    
                }
            }
            tilemapDatas.Add(mapData);
        }
        fileHandler.SaveToJSON(tilemapDatas, filename);
    }
    public void onLoad()
    {
        List<TilemapData> data = fileHandler.ReadListFromJSON<TilemapData>(filename);

        foreach(var mapData in data)
        {
            if(!tilemaps.ContainsKey(mapData.name))
            {
                Debug.LogError("No map with name: " + mapData.name);
                continue;
            }

            var map = tilemaps[mapData.name];
            map.ClearAllTiles();
            if(mapData.tiles != null && mapData.tiles.Count > 0)
            {
                foreach(var tileInfo in mapData.tiles)
                {
                    map.SetTile(tileInfo.position, tileInfo.tileBase);
                }
            }
        }
    }
}

[Serializable]
public class TilemapData
{
    public string name;
    public List<TileInfo> tiles = new List<TileInfo>();
}

[Serializable]
public class TileInfo{
    public Vector3Int position;
    public TileBase tileBase;

    public TileInfo(Vector3Int position, TileBase tileBase)
    {
        this.position = position;
        this.tileBase = tileBase;
    }
}