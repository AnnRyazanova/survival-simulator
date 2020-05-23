using UnityEngine;

namespace SceneGeneration.PerlinNoise
{
    public static class TextureGenerator {

        public static Texture2D TextureFromColourMap(Color[] colourMap, int width, int height) {
            var texture = new Texture2D(width, height)
            {
                filterMode = FilterMode.Point, wrapMode = TextureWrapMode.Clamp
            };
            texture.SetPixels (colourMap);
            texture.Apply ();
            return texture;
        }


        public static Texture2D TextureFromHeightMap(float[,] heightMap) {
            var width = heightMap.GetLength (0);
            var height = heightMap.GetLength (1);

            var colourMap = new Color[width * height];
            for (var y = 0; y < height; y++) {
                for (var x = 0; x < width; x++) {
                    colourMap [y * width + x] = Color.Lerp (Color.black, Color.white, heightMap [x, y]);
                }
            }

            return TextureFromColourMap (colourMap, width, height);
        }

    }
}