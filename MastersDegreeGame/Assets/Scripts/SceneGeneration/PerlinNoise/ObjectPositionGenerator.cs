using System.Collections;
using System.Collections.Generic;
using SceneGeneration.PerlinNoise;
using UnityEngine;

public class ObjectPositionGenerator : MonoBehaviour {

    public int forestSize = 131; // Overall size of the forest (a square of forestSize X forestSize).
    public int elementSpacing = 15; // The spacing between element placements. Basically grid size.

    public Element[] elements;

    public void Generate(Vector3[] meshVertices) {
        //var _MapGenerator = FindObjectOfType<MapGenerator>();
        Debug.Log("begin forest");
        
        
        for (int i = 0; i < meshVertices.Length; i++)
        {
            for (int j = 0; j < elements.Length; j++)
            {
                Element element = elements[j];
                Vector3 position = meshVertices[i];
                Vector3 offset = new Vector3(Random.Range(-0.75f, 0.75f), 0f, Random.Range(-0.75f, 0.75f));
                Vector3 rotation = new Vector3(Random.Range(0, 5f), Random.Range(0, 360f), Random.Range(0, 5f));
                Vector3 scale = Vector3.one * Random.Range(0.75f, 1.25f);
                var @params = new PrefabsCreator.PrefabParams {
                    scale = new Vector3(1,1,1),
                    position = position,
                    parent = transform,
                        
                };
                PrefabsCreator.Get.LoadPrefab("Player/Player", @params);
                // Instantiate and place element in world.
                GameObject newElement = Instantiate(());
                newElement.transform.SetParent(transform);
                newElement.transform.position = position + offset;
                newElement.transform.eulerAngles = rotation;
                newElement.transform.localScale = scale;
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
        Debug.Log("end");
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

}