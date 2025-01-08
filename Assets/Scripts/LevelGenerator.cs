using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public GameObject blockPrefab;
    public GameObject sawPrefab;
    public GameObject keyPrefab;
    public GameObject lockedBlockPrefab;
    public GameObject endHolePrefab;
    public GameObject playerPrefab;
    public GameObject spikePrefab;

    private string jsonLayout = @"
{
  ""levelWidth"": 18,
  ""levelHeight"": 10,
  ""objects"": [
    { ""type"": ""player"", ""position"": { ""x"": 1, ""y"": 1 } }, 
    { ""type"": ""block"", ""position"": { ""x"": 0, ""y"": 0 } }, 
    { ""type"": ""block"", ""position"": { ""x"": 1, ""y"": 0 } }, 
    { ""type"": ""block"", ""position"": { ""x"": 2, ""y"": 0 } },
    { ""type"": ""block"", ""position"": { ""x"": 3, ""y"": 0 } },
    { ""type"": ""block"", ""position"": { ""x"": 4, ""y"": 0 } },
    { ""type"": ""block"", ""position"": { ""x"": 5, ""y"": 0 } },
    { ""type"": ""block"", ""position"": { ""x"": 6, ""y"": 0 } },
    { ""type"": ""block"", ""position"": { ""x"": 7, ""y"": 0 } },
    { ""type"": ""block"", ""position"": { ""x"": 8, ""y"": 0 } },
    { ""type"": ""block"", ""position"": { ""x"": 9, ""y"": 0 } },
    { ""type"": ""block"", ""position"": { ""x"": 10, ""y"": 0 } },
    { ""type"": ""block"", ""position"": { ""x"": 11, ""y"": 0 } },
    { ""type"": ""block"", ""position"": { ""x"": 12, ""y"": 0 } },
    { ""type"": ""block"", ""position"": { ""x"": 13, ""y"": 0 } },
    { ""type"": ""block"", ""position"": { ""x"": 14, ""y"": 0 } },
    { ""type"": ""block"", ""position"": { ""x"": 15, ""y"": 0 } },
    { ""type"": ""block"", ""position"": { ""x"": 16, ""y"": 0 } },
    { ""type"": ""block"", ""position"": { ""x"": 17, ""y"": 0 } },

    { ""type"": ""block"", ""position"": { ""x"": 0, ""y"": 9 } },
    { ""type"": ""block"", ""position"": { ""x"": 17, ""y"": 9 } },
    
    { ""type"": ""key"", ""position"": { ""x"": 10, ""y"": 2 } }, 
    { ""type"": ""lockedBlock"", ""position"": { ""x"": 12, ""y"": 2 } },

    { ""type"": ""saw"", ""position"": { ""x"": 6, ""y"": 3 } },
    { ""type"": ""spike"", ""position"": { ""x"": 7, ""y"": 3 } },

    { ""type"": ""endHole"", ""position"": { ""x"": 15, ""y"": 8 } }
  ]
}";



    void Start()
    {
        // JSON'u LevelDataJ sýnýfýna dönüþtür
        LevelDataJ levelDataJ = JsonUtility.FromJson<LevelDataJ>(jsonLayout);

        // Her bir nesneyi oyun dünyasýna yerleþtir
        foreach (LevelObjectJ obj in levelDataJ.objects)
        {
            Vector3 position = new Vector3(obj.position.x, obj.position.y, 0);

            switch (obj.type)
            {
                case "player":
                    Instantiate(playerPrefab, position, Quaternion.identity);
                    break;
                case "block":
                    PlaceBlock(obj, position);
                    break;
                case "key":
                    Instantiate(keyPrefab, position, Quaternion.identity);
                    break;
                case "lockedBlock":
                    Instantiate(lockedBlockPrefab, position, Quaternion.identity);
                    break;
                case "saw":
                    Instantiate(sawPrefab, position, Quaternion.identity);
                    break;
                case "spike":
                    Instantiate(spikePrefab, position, Quaternion.identity);
                    break;
                case "endHole":
                    Instantiate(endHolePrefab, position, Quaternion.identity);
                    break;
                default:
                    Debug.LogWarning("Unknown object type: " + obj.type);
                    break;
            }
        }
    }

    // Bloklarý yerleþtirme iþlemi
    void PlaceBlock(LevelObjectJ obj, Vector3 position)
    {
        GameObject block = Instantiate(blockPrefab, position, Quaternion.identity);
        if (obj.size != null)
        {
            block.transform.localScale = new Vector3(obj.size.width, obj.size.height, 1);
        }
    }
}

[System.Serializable]
public class PositionJ
{
    public float x;
    public float y;
}

[System.Serializable]
public class SizeJ
{
    public float width;
    public float height;
}

[System.Serializable]
public class LevelObjectJ
{
    public string type;
    public PositionJ position;
    public SizeJ size; // Bazý nesneler için boyut bilgisi eklenebilir
}

[System.Serializable]
public class LevelDataJ
{
    public int levelWidth;
    public int levelHeight;
    public List<LevelObjectJ> objects;
}
