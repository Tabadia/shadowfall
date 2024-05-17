using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ObjectSaveManager : MonoBehaviour
{
    public HashSet<string> removedObjects;
    public List<GameObject> objectsInScene = new List<GameObject>();

    private void Awake()
    {
        LoadRemovedObjects();
        RemoveLoadedObjects();
    }

    public void LoadRemovedObjects()
    {
        removedObjects = new HashSet<string>();
        string json = PlayerPrefs.GetString("RemovedObjects", string.Empty);
        if (!string.IsNullOrEmpty(json))
        {
            RemovedObjectsData data = JsonUtility.FromJson<RemovedObjectsData>(json);
            removedObjects = new HashSet<string>(data.RemovedObjects);
        }
    }

    public void SaveRemovedObjects()
    {
        RemovedObjectsData data = new RemovedObjectsData { RemovedObjects = new List<string>(removedObjects) };
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString("RemovedObjects", json);
        PlayerPrefs.Save();
    }

    public void RemoveLoadedObjects()
    {
        for (int i = objectsInScene.Count - 1; i >= 0; i--)
        {
            if (removedObjects.Contains(objectsInScene[i].name))
            {
                Destroy(objectsInScene[i]);
                objectsInScene.RemoveAt(i);
            }
        }
    }

    public void RemoveObject(GameObject obj)
    {
        if (objectsInScene.Contains(obj))
        {
            objectsInScene.Remove(obj);
            removedObjects.Add(obj.name);
            SaveRemovedObjects();
            Destroy(obj);
        }
    }

    [ContextMenu("Populate Objects In Scene")]
    public void PopulateObjectsInScene()
    {
        objectsInScene.Clear();
        foreach (Transform child in transform)
        {
            objectsInScene.Add(child.gameObject);
        }
    }

    public void ClearSaveFile()
    {
        PlayerPrefs.DeleteKey("RemovedObjects");
        removedObjects.Clear();
    }

    [System.Serializable]
    public class RemovedObjectsData
    {
        public List<string> RemovedObjects;
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(ObjectSaveManager))]
    public class ObjectSaveManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            ObjectSaveManager myScript = (ObjectSaveManager)target;
            if (GUILayout.Button("Populate Objects In Scene"))
            {
                myScript.PopulateObjectsInScene();
            }

            if (GUILayout.Button("Clear Save File"))
            {
                myScript.ClearSaveFile();
            }
        }
    }
#endif
}
