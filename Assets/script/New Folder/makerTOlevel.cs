using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class PositionData
{
    public float x;
    public float y;
    public float z;
}

[System.Serializable]
public class TileData
{
    public PositionData position;
    public TileBaseData tileBase;
}

[System.Serializable]
public class TileBaseData
{
    public int instanceID;
}

[System.Serializable]
public class ItemData
{
    public string name;
    public List<TileData> tiles;
}

[System.Serializable]
public class TileMapData
{
    public List<ItemData> Items;
}

public class makerTOlevel : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject playerPrefab;
    public GameObject boxPrefab;
    public GameObject icePrefab;
    public GameObject targetPrefab;
    public GameObject wallPrefab;
    public GameObject groundPrefab;

    [Header("Settings")]
    public string mapName;
    public string jsonFileName = "test.json";

    private Dictionary<string, GameObject> prefabMap = new Dictionary<string, GameObject>();

    void Start()
    {
        jsonFileName = PlayerPrefs.GetString("DefaultName") + ".json";
        InitializePrefabMap();
        LoadLevelFromJson();
    }

    void InitializePrefabMap()
    {
        prefabMap["player"] = playerPrefab;
        prefabMap["box"] = boxPrefab;
        prefabMap["ice"] = icePrefab;
        prefabMap["target"] = targetPrefab;
        prefabMap["wall"] = wallPrefab;
        prefabMap["ground"] = groundPrefab;
        prefabMap["default"] = wallPrefab; // 将 default 映射到 wall
    }

    void LoadLevelFromJson()
    {
        string filePath = Path.Combine(Application.persistentDataPath, jsonFileName);
        
        if (File.Exists(filePath))
        {
            string jsonContent = File.ReadAllText(filePath);
            TileMapData mapData = JsonUtility.FromJson<TileMapData>(jsonContent);
            
            if (mapData != null)
            {
                Debug.Log($"Loaded tile map data with {mapData.Items.Count} items");
                CreateLevelObjects(mapData);
            }
            else
            {
                Debug.LogError("Failed to parse JSON data");
            }
        }
        else
        {
            Debug.LogError($"JSON file not found: {filePath}");
            CreateDefaultLevel();
        }
    }

    void CreateLevelObjects(TileMapData mapData)
    {
        foreach (ItemData item in mapData.Items)
        {
            string itemName = item.name.ToLower();
            
            if (prefabMap.ContainsKey(itemName))
            {
                GameObject prefab = prefabMap[itemName];
                if (prefab != null)
                {
                    foreach (TileData tile in item.tiles)
                    {
                        Vector3 position = new Vector3(
                            tile.position.x,
                            tile.position.y,
                            tile.position.z
                        );
                        
                        GameObject obj = Instantiate(prefab, position, Quaternion.identity);
                        obj.name = itemName;
                        Debug.Log($"Created {itemName} at {position}");
                    }
                }
                else
                {
                    Debug.LogError($"Prefab not assigned for: {itemName}");
                }
            }
            else
            {
                // 忽略未知的项目类型（如 previewMap、default、地图背景）
                Debug.Log($"Skipping unknown item type: {itemName}");
            }
        }
    }

    void CreateDefaultLevel()
    {
        Debug.Log("Creating default level");
        
        // 创建默认地面
        if (groundPrefab != null)
        {
            Instantiate(groundPrefab, Vector3.zero, Quaternion.identity);
        }
        
        // 创建默认玩家
        if (playerPrefab != null)
        {
            Instantiate(playerPrefab, new Vector3(0, 1, 0), Quaternion.identity);
        }
    }
}