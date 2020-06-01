using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;

public class MeshSplit : MonoBehaviour
{
    private readonly bool drawGrid = false;

    private Mesh baseMesh;
    private MeshRenderer baseRenderer;

    private Vector3[] baseVerticles;
    private int[] baseTriangles;
    private Vector2[] baseUvs;
    private Vector3[] baseNormals;
    
    private Dictionary<string, List<int>> trisDictionary;
    
    [HideInInspector]
    public List<GameObject> children = new List<GameObject>();

    private string getRegionName(int i)
    {
        if (baseVerticles[baseTriangles[i + 0]].y < 0.03 ||
            baseVerticles[baseTriangles[i + 1]].y < 0.03 ||
            baseVerticles[baseTriangles[i + 2]].y < 0.03)
        {
            return "Water";
        }
        else if (baseVerticles[baseTriangles[i + 0]].y < 0.2 ||
                 baseVerticles[baseTriangles[i + 1]].y < 0.2 ||
                 baseVerticles[baseTriangles[i + 2]].y < 0.2)
        {
            return "Sand";
        }
        else if (baseVerticles[baseTriangles[i + 0]].y < 4.5 ||
                 baseVerticles[baseTriangles[i + 1]].y < 4.5 ||
                 baseVerticles[baseTriangles[i + 2]].y < 4.5)
        {
            return "Forest";
        }
        else
        {
            return "Rock";
        }
        
    }
    
    private void MapTrianglesToGridNodes()
    {
        trisDictionary = new Dictionary<string, List<int>>();
        string regionName;
        for (int i = 0; i < baseTriangles.Length; i += 3)
        {
            regionName = getRegionName(i);
            
            if (!trisDictionary.ContainsKey(regionName))
            {
                trisDictionary.Add(regionName, new List<int>());
            }
            
            trisDictionary[regionName].Add(baseTriangles[i]);
            trisDictionary[regionName].Add(baseTriangles[i + 1]);
            trisDictionary[regionName].Add(baseTriangles[i + 2]);
        }
    }

    public void Split()
    {
        DestroyChildren();

        if (GetComponent<MeshFilter>() == null)
        {
            Debug.LogError("Mesh Filter Component is missing.");
            return;
        }

        baseMesh = GetComponent<MeshFilter>().sharedMesh;

        baseRenderer = GetComponent<MeshRenderer>();
        if (baseRenderer)
        {
            baseRenderer.enabled = false;
        }

        baseVerticles = baseMesh.vertices;
        baseTriangles = baseMesh.triangles;
        baseUvs = baseMesh.uv;
        baseNormals = baseMesh.normals;
        
        MapTrianglesToGridNodes();
        
        foreach (var item in trisDictionary.Keys)
        {
            CreateMesh(item, trisDictionary[item]);
        }
        GetComponent<NavMeshSurface>().BuildNavMesh();
    }

    public void CreateMesh(string regionName, List<int> dictionaryTriangles)
    {
        Debug.Log(dictionaryTriangles.Count);
        GameObject newObject = new GameObject();
        newObject.name = "SubMesh " + regionName;
        newObject.transform.SetParent(transform);
        newObject.transform.localPosition = Vector3.zero;
        newObject.transform.localScale = Vector3.one;
        newObject.transform.localRotation = transform.localRotation;
        newObject.AddComponent<MeshFilter>();
        newObject.AddComponent<MeshRenderer>();

        MeshRenderer newRenderer = newObject.GetComponent<MeshRenderer>();
        newRenderer.sharedMaterial = GetComponent<MeshRenderer>().sharedMaterial;

        List<Vector3> verts = new List<Vector3>();
        List<int> tris = new List<int>();
        List<Vector2> uvs = new List<Vector2>();
        List<Vector3> normals = new List<Vector3>();

        for (int i = 0; i < dictionaryTriangles.Count; i += 3)
        {

            verts.Add(baseVerticles[dictionaryTriangles[i]]);
            verts.Add(baseVerticles[dictionaryTriangles[i + 1]]);
            verts.Add(baseVerticles[dictionaryTriangles[i + 2]]);

            tris.Add(i);
            tris.Add(i + 1);
            tris.Add(i + 2);

            uvs.Add(baseUvs[dictionaryTriangles[i]]);
            uvs.Add(baseUvs[dictionaryTriangles[i + 1]]);
            uvs.Add(baseUvs[dictionaryTriangles[i + 2]]);

            normals.Add(baseNormals[dictionaryTriangles[i]]);
            normals.Add(baseNormals[dictionaryTriangles[i + 1]]);
            normals.Add(baseNormals[dictionaryTriangles[i + 2]]);

        }

        if (regionName == "Water")
        {
            newObject.layer = 4;
        }

        
        
        children.Add(newObject);

        Mesh mesh = new Mesh();
        mesh.name = regionName;
        
        mesh.vertices = verts.ToArray();
        mesh.triangles = tris.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.normals = normals.ToArray();

        if (regionName == "Forest")
        {
            var go = newObject.AddComponent<ObjectPositionGenerator>();
            go.Generate(mesh.vertices);
        }
        
        MeshFilter newMeshFilter = newObject.GetComponent<MeshFilter>();
        newMeshFilter.mesh = mesh;
    }
    

    public void Clear()
    {

        DestroyChildren();

        GetComponent<MeshRenderer>().enabled = true;

    }

    private void DestroyChildren()
    {
        for (int i = 0; i < children.Count; i++)
        {
            DestroyImmediate(children[i]);
        }
        children.Clear();
    }
}
