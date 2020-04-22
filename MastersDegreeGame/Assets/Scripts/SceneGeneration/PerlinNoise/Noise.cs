using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Noise
{
    public static float[,] GenerateNoiseMap(int mapWidht, int mapHeight, float scale)
    {
        float[,] noiseMap = new float[mapWidht, mapHeight];

        if (scale <= 0)
        {
            scale = 0.0001f;
        }
        
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x= 0; x < mapWidht; x++)
            {
                float SampleX = x / scale;
                float SampleY = y / scale;

                float perlinValue = Mathf.PerlinNoise(SampleX, SampleY);
                noiseMap[x, y] = perlinValue;
            }
        }

        return noiseMap;
    }
}
