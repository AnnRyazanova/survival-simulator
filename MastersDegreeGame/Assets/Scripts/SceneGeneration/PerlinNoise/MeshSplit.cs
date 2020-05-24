using UnityEngine;
using System.Collections.Generic;
using SceneGeneration.PerlinNoise;

public class MeshSplit : MonoBehaviour
{
    // a struct which will act as a key in our dictionary.
    // a Vector3 could probably be used instead, but I wanted to use ints
    public struct GridCoordinates
    {
        private static readonly float precision = 100;

        public int x, y, z;

        public GridCoordinates(float x, float y, float z) {
            this.x = Mathf.RoundToInt(x * precision);
            this.y = Mathf.RoundToInt(y * precision);
            this.z = Mathf.RoundToInt(z * precision);
        }

        public static implicit operator GridCoordinates(Vector3 v) {
            return new GridCoordinates((int) v.x, (int) v.y, (int) v.z);
        }

        public static implicit operator Vector3(GridCoordinates i) {
            return new Vector3(i.x, i.y, i.z);
        }

        public override string ToString() => string.Format("({0},{1},{2})", (float) x / precision,
            (float) y / precision, (float) z / precision);
    }

    // draw debug grid when the object is selected
    private readonly bool drawGrid = false;

    private Mesh baseMesh;
    private MeshRenderer baseRenderer;

    [Range(0.1f, 256)] public float gridSize = 16;

    // which axes should the script use?
    public bool axisX = true;
    public bool axisY = true;
    public bool axisZ = true;

    public int renderLayerIndex = 0;
    public string renderLayerName = "Default";

    public bool useSortingLayerFromThisMesh = true;
    public bool useStaticSettingsFromThisMesh = true;

    // mesh data from this objects mesh filter
    private Vector3[] baseVerticles;
    private int[] baseTriangles;
    private Vector2[] baseUvs;
    private Vector3[] baseNormals;

    public RegionsData data;

    // this dictionary holds a list of triangle indices for every grid node
    private Dictionary<GridCoordinates, List<int>> triDictionary;
    private Dictionary<string, List<int>> triangles;

    // generated children are kept here, so the script knows what to delete on Split() or Clear()
    [HideInInspector] public List<GameObject> childen = new List<GameObject>();

    private void MapTrianglesToGridNodes() {
        var a = data.regions;
        /* Create a list of triangle indices from our mesh for every grid node */

        triDictionary = new Dictionary<GridCoordinates, List<int>>();

        for (int i = 0; i < baseTriangles.Length; i += 3) {
            // middle of the current triangle (average of its 3 verts).

            Vector3 currentPoint =
                (baseVerticles[baseTriangles[i]] +
                 baseVerticles[baseTriangles[i + 1]] +
                 baseVerticles[baseTriangles[i + 2]]) / 3;

            // calculate coordinates of the closest grid node.

            currentPoint.x = Mathf.Round(currentPoint.x / gridSize) * gridSize;
            currentPoint.y = Mathf.Round(currentPoint.y / gridSize) * gridSize;
            currentPoint.z = Mathf.Round(currentPoint.z / gridSize) * gridSize;

            // ignore an axis if its not enabled

            GridCoordinates gridPos = new GridCoordinates(
                axisX ? currentPoint.x : 0,
                axisY ? currentPoint.y : 0,
                axisZ ? currentPoint.z : 0
            );

            // check if the dictionary has a key (our grid position). Add it / create a list for it if it doesnt.

            if (!triDictionary.ContainsKey(gridPos)) {
                triDictionary.Add(gridPos, new List<int>());
            }

            // add these triangle indices to the list

            triDictionary[gridPos].Add(baseTriangles[i]);
            triDictionary[gridPos].Add(baseTriangles[i + 1]);
            triDictionary[gridPos].Add(baseTriangles[i + 2]);
        }
    }

    public void Split() {
        triangles = MeshGenerator.Triangles;
        if (triangles == null) {
            Debug.Log("sss");
            return;
        }
        else {
            Debug.Log(triangles.Count);
        }

        DestroyChildren();

        if (GetComponent<MeshFilter>() == null) {
            Debug.LogError("Mesh Filter Component is missing.");
            return;
        }

        if (GetUsedAxisCount() < 1) {
            Debug.LogError("You have to choose at least 1 axis.");
            return;
        }

        baseMesh = GetComponent<MeshFilter>().sharedMesh;

        baseRenderer = GetComponent<MeshRenderer>();
        if (baseRenderer) {
            baseRenderer.enabled = false;
        }

        baseVerticles = baseMesh.vertices;
        baseTriangles = baseMesh.triangles;
        baseUvs = baseMesh.uv;
        baseNormals = baseMesh.normals;

        // create a list of triangle indices for every grid node
        // MapTrianglesToGridNodes();

        // create a submesh for each list of triangle indices
        foreach (var item in triangles.Keys) {
            CreateMesh(item, triangles[item]);
        }
    }

     public void CreateMesh(string gridCoordinates, List<int> dictionaryTriangles) {
        // create a new game object ...

        GameObject newObject = new GameObject();
        newObject.name = "SubMesh " + gridCoordinates;
        newObject.transform.SetParent(transform);
        newObject.transform.localPosition = Vector3.zero;
        newObject.transform.localScale = Vector3.one;
        newObject.transform.localRotation = transform.localRotation;
        newObject.AddComponent<MeshFilter>();
        newObject.AddComponent<MeshRenderer>();

        MeshRenderer newRenderer = newObject.GetComponent<MeshRenderer>();
        newRenderer.sharedMaterial = GetComponent<MeshRenderer>().sharedMaterial;

        // sorting order and layer name of the generated mesh renderer

        if (!useSortingLayerFromThisMesh) {
            newRenderer.sortingLayerName = renderLayerName;
            newRenderer.sortingOrder = renderLayerIndex;
        }
        else if (baseRenderer) {
            newRenderer.sortingLayerName = baseRenderer.sortingLayerName;
            newRenderer.sortingOrder = baseRenderer.sortingOrder;
        }

        // should the submesh also be static if the base object is static?

        if (useStaticSettingsFromThisMesh)
            newObject.isStatic = gameObject.isStatic;

        // mesh data lists for the new mesh

        List<Vector3> verts = new List<Vector3>();
        List<int> tris = new List<int>();
        List<Vector2> uvs = new List<Vector2>();
        List<Vector3> normals = new List<Vector3>();

        // these lists are filled in this loop

        for (int i = 0; i < dictionaryTriangles.Count; i += 3) {
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

        // add the newly created object to the list of children (submeshes)
        childen.Add(newObject);

        // Create a new mesh ...
        Mesh m = new Mesh();
        m.name = gridCoordinates;

        // fill it with data from our lists ...
        m.vertices = verts.ToArray();
        m.triangles = tris.ToArray();
        m.uv = uvs.ToArray();
        m.normals = normals.ToArray();

        UnityEditor.MeshUtility.Optimize(m);

        // assign the new mesh to this submeshes mesh filter

        MeshFilter newMeshFilter = newObject.GetComponent<MeshFilter>();
        newMeshFilter.mesh = m;
    }
     
    public void CreateMesh(GridCoordinates gridCoordinates, List<int> dictionaryTriangles) {
        // create a new game object ...

        GameObject newObject = new GameObject();
        newObject.name = "SubMesh " + gridCoordinates;
        newObject.transform.SetParent(transform);
        newObject.transform.localPosition = Vector3.zero;
        newObject.transform.localScale = Vector3.one;
        newObject.transform.localRotation = transform.localRotation;
        newObject.AddComponent<MeshFilter>();
        newObject.AddComponent<MeshRenderer>();

        MeshRenderer newRenderer = newObject.GetComponent<MeshRenderer>();
        newRenderer.sharedMaterial = GetComponent<MeshRenderer>().sharedMaterial;

        // sorting order and layer name of the generated mesh renderer

        if (!useSortingLayerFromThisMesh) {
            newRenderer.sortingLayerName = renderLayerName;
            newRenderer.sortingOrder = renderLayerIndex;
        }
        else if (baseRenderer) {
            newRenderer.sortingLayerName = baseRenderer.sortingLayerName;
            newRenderer.sortingOrder = baseRenderer.sortingOrder;
        }

        // should the submesh also be static if the base object is static?

        if (useStaticSettingsFromThisMesh)
            newObject.isStatic = gameObject.isStatic;

        // mesh data lists for the new mesh

        List<Vector3> verts = new List<Vector3>();
        List<int> tris = new List<int>();
        List<Vector2> uvs = new List<Vector2>();
        List<Vector3> normals = new List<Vector3>();

        // these lists are filled in this loop

        for (int i = 0; i < dictionaryTriangles.Count; i += 3) {
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

        // add the newly created object to the list of children (submeshes)
        childen.Add(newObject);

        // Create a new mesh ...
        Mesh m = new Mesh();
        m.name = gridCoordinates.ToString();

        // fill it with data from our lists ...
        m.vertices = verts.ToArray();
        m.triangles = tris.ToArray();
        m.uv = uvs.ToArray();
        m.normals = normals.ToArray();

        UnityEditor.MeshUtility.Optimize(m);

        // assign the new mesh to this submeshes mesh filter

        MeshFilter newMeshFilter = newObject.GetComponent<MeshFilter>();
        newMeshFilter.mesh = m;
    }

    private int GetUsedAxisCount() {
        return (axisX ? 1 : 0) + (axisY ? 1 : 0) + (axisZ ? 1 : 0);
    }

    public void Clear() {
        DestroyChildren();

        GetComponent<MeshRenderer>().enabled = true;
    }

    private void DestroyChildren() {
        for (int i = 0; i < childen.Count; i++) {
            DestroyImmediate(childen[i]);
        }

        childen.Clear();
    }

    void OnDrawGizmosSelected() {
        MeshFilter meshFilter = GetComponent<MeshFilter>();

        if (drawGrid && meshFilter && meshFilter.sharedMesh) {
            Bounds b = meshFilter.sharedMesh.bounds;

            float xSize = Mathf.Ceil(b.extents.x) + gridSize;
            float ySize = Mathf.Ceil(b.extents.y) + gridSize;
            float zSize = Mathf.Ceil(b.extents.z) + gridSize;

            for (float z = -zSize; z <= zSize; z += gridSize) {
                for (float y = -ySize; y <= ySize; y += gridSize) {
                    for (float x = -xSize; x <= xSize; x += gridSize) {
                        Vector3 position = transform.position + new Vector3(x, y, z);

                        Gizmos.DrawWireCube(position, gridSize * transform.localScale);
                    }
                }
            }
        }
    }
}