using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LoadObj : MonoBehaviour
{
    public string objPath;

    void Start()
    {
        StartCoroutine(LoadOBJFromFile(objPath));
    }

    IEnumerator LoadOBJFromFile(string path)
    {
        var objImporter = new OBJImporter();
        yield return objImporter.Load(path);
        var mesh = objImporter.GetMesh();

        var meshFilter = GetComponent<MeshFilter>();
        if (meshFilter != null)
        {
            meshFilter.mesh = mesh;
        }
    }
}
