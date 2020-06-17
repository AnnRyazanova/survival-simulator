using System.Collections;
using System.Collections.Generic;
using SceneGeneration.PerlinNoise;
using UnityEngine;

public class ObjectPositionGenerator : MonoBehaviour
{

    private float neighborRadius = 2;
    public void GeneratePerlin(Vector3[] meshVertices)
    {
        var noiseMap = Noise.GenerateNoiseMap(100, 100, 
                                23, 10, 4, .5f, 2, 
                                new Vector2(0,0),  Noise.NormalizeMode.Local);

        float mapWidth = 100;
        float mapHeight = 100;
        int vertexIndex = 1;
        for (int x = 0; x < mapWidth; x++)
        {
            for (int z = 0; z < mapHeight; z++)
            {
                
                float objectValue = noiseMap [z, x];
                
                int neighborZBegin = (int)Mathf.Max (0, z - this.neighborRadius);
                int neighborZEnd = (int)Mathf.Min (mapHeight-1, z + this.neighborRadius);
                int neighborXBegin = (int)Mathf.Max (0, x - this.neighborRadius);
                int neighborXEnd = (int)Mathf.Min (mapWidth-1, x + this.neighborRadius);
                float maxValue = 0f;
                for (int neighborZ = neighborZBegin; neighborZ <= neighborZEnd; neighborZ++) {
                    for (int neighborX = neighborXBegin; neighborX <= neighborXEnd; neighborX++) {
                        float neighborValue = noiseMap [neighborZ, neighborX];
                        // saves the maximum tree noise value in the radius
                        if (neighborValue >= maxValue) {
                            maxValue = neighborValue;
                        }
                    }
                }
                if ((objectValue == maxValue)&&(vertexIndex <= meshVertices.Length)) {
                    var @params = new PrefabsCreator.PrefabParams
                    {
                        scale = new Vector3(50,50,50),
                        position = new Vector3(meshVertices[vertexIndex].x, meshVertices[vertexIndex].y + 1.5f, meshVertices[vertexIndex].z),
                        parent = transform
                    };
                    PrefabsCreator.Get.LoadPrefab("Environment/oak", @params);
                }
                vertexIndex+=3;
            }
        }
    }

    public void GenerateSpan(Vector3[] meshVertices)
    {
        for (int i = 0; i < meshVertices.Length; i+=300)
        {
            var @params = new PrefabsCreator.PrefabParams
            {
                scale = new Vector3(50,50,50),
                position = new Vector3(meshVertices[i].x, meshVertices[i].y + 1.5f, meshVertices[i].z),
                parent = transform
            };
            PrefabsCreator.Get.LoadPrefab("Environment/oak", @params);
        }
    }
}




