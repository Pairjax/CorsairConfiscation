using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class MapManager : ScriptableSingleton<MapManager>
{
    private void OnDisable()
    {
        DeleteMap(FindMap());
    }
    
    public void SaveMapAsPrefab(GameObject map)
    {
        // Find map asset if it already exists.
        Object foundMap = FindMap();
        if (foundMap)
            DeleteMap(foundMap);

        if (!Directory.Exists("Assets/Prefabs/Map"))
            AssetDatabase.CreateFolder("Assets/Prefabs", "Map");
        string localPath = "Assets/Prefabs/Map/" + map.name + ".prefab";

        localPath = AssetDatabase.GenerateUniqueAssetPath(localPath);

        bool prefabSuccess;
        PrefabUtility.SaveAsPrefabAsset(map, localPath, out prefabSuccess);
        if (prefabSuccess == true)
            Debug.Log("Prefab was saved successfully");
        else
            Debug.Log("Prefab failed to save" + prefabSuccess);
    }

    public void LoadMapIntoScene()
    {
        Instantiate(FindMap());
    }

    public Object FindMap()
    {
        UnityEngine.Object prefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Map/SystemMap.prefab", typeof(MapGenerator));

        return prefab;
    }

    private void DeleteMap(Object map)
    {
        AssetDatabase.DeleteAsset("Assets/Prefabs/Map/SystemMap.prefab");
    }

    public void LoadMapScene()
    {
        SceneManager.LoadSceneAsync("MapScene");
    }
}
