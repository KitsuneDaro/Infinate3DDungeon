using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;

public class MeshManager
{
    private string parentPath;
    private Dictionary<string, GameObject> meshResourceDictionary = new Dictionary<string, GameObject>();

    public MeshManager(string parentPath) {
        this.parentPath = parentPath;
    }

    public void AddMeshResource(string name, string relativePath) {
        string meshResourcePath = Path.Combine(parentPath, relativePath);
        
        GameObject meshResource = Resources.Load<GameObject>(meshResourcePath);
        meshResourceDictionary.Add(name, meshResource);
    }

    public void AddMeshResourceByDictionary(Dictionary<string, string> relativePathDictionary) {
        if (relativePathDictionary != null) {
            foreach (string name in relativePathDictionary.Keys) {
                AddMeshResource(name, relativePathDictionary[name]);
            }
        }
    }

    public GameObject GetMeshResource(string name) {
        return meshResourceDictionary[name];
    }
}
