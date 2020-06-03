using System.Collections;
using System.Collections.Generic;
using SceneGeneration.PerlinNoise;
using UnityEngine;

public class ObjectPositionGenerator : MonoBehaviour
{
    public void Generate(Vector3[] meshVertices)
    {
        string[] elements = { "Carrot", "Branch_5", "Mushroom", "Branch_5", "Rock Type2 02", "Cover", "Spider"};
        string[] elements1 = { "Carrot", "Branch_5", "Mushroom", "Branch_5", "Rock Type2 02", "Cover",};
        Debug.Log("begin forest");
        string elem;
        int count_spider = 0;
        for (int i = 0; i < meshVertices.Length; i+=50)
        {
            
            elem = elements[Random.Range(1, elements.Length)];
            if (elem == "Spider")
            {
                count_spider++;
            }

            if (count_spider > 5)
            {
                elem = elements1[Random.Range(1, elements1.Length)];
            }
            var @params = new PrefabsCreator.PrefabParams
            {
                scale = elem == "Spider"? new Vector3(0.35f, .35f, .35f): Vector3.one,
                position = meshVertices[i],
                parent = transform
                
            };
            PrefabsCreator.Get.LoadPrefab("Environment/" + elem, @params);

        }
    }
}



/*
// Loop through all the positions within our forest boundary.
for (int x = 0; x < forestSize; x += elementSpacing) {
    for (int z = 0; z < forestSize; z += elementSpacing) {
        if (_MapGenerator.getMapData().HeightMap[x, z] > 0.1)
        {
            for (int i = 0; i < elements.Length; i++) {

                // Get the current element.
                Element element = elements[i];
                
                // Check if the element can be placed.
                if (element.CanPlace()) {
                    Debug.Log("if element can place");
                    // Add random elements to element placement.
                    Vector3 position = new Vector3(x, _MapGenerator.getMapData().HeightMap[x, z], z);
                    Vector3 offset = new Vector3(Random.Range(-0.75f, 0.75f), 0f, Random.Range(-0.75f, 0.75f));
                    Vector3 rotation = new Vector3(Random.Range(0, 5f), Random.Range(0, 360f), Random.Range(0, 5f));
                    Vector3 scale = Vector3.one * Random.Range(0.75f, 1.25f);

                    // Instantiate and place element in world.
                    GameObject newElement = Instantiate(element.GetRandom());
                    newElement.transform.SetParent(transform);
                    newElement.transform.position = position + offset;
                    newElement.transform.eulerAngles = rotation;
                    newElement.transform.localScale = scale;

                    // Break out of this for loop to ensure we don't place another element at this position.
                    break;

                }

            }
        }

    }
}*/
      /*  Debug.Log("end");
    }

}

[System.Serializable]
public class Element {

    public string name;
    [Range(1, 10)]
    public int density;

    public GameObject[] prefabs;

    public bool CanPlace () {

        // Validation check to see if element can be placed. More detailed calculations can go here, such as checking perlin noise.
        
        if (Random.Range(0, 10) < density)
            return true;
        else
            return false;

    }

    public GameObject GetRandom() {

        // Return a random GameObject prefab from the prefabs array.

        return prefabs[Random.Range(0, prefabs.Length)];

    }

}*/