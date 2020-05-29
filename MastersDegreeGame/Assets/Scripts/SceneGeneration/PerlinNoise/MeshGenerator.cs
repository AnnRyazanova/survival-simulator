using System.Collections;
using UnityEngine;

namespace SceneGeneration.PerlinNoise
{
    public static class MeshGenerator
    {
        public static MeshData GenerateTerrainMesh(float[,] heightMap, float heightMultiplier,
            AnimationCurve heightCurve, int levelOfDetail) {
            var myheightCurve = new AnimationCurve(heightCurve.keys);

            var width = heightMap.GetLength(0);
            var height = heightMap.GetLength(1);
            var topLeftX = (width - 1) / -2f;
            var topLeftZ = (height - 1) / 2f;

            var meshSimplificationIncrement = (levelOfDetail == 0) ? 1 : levelOfDetail * 2;
            var verticesPerLine = (width - 1) / meshSimplificationIncrement + 1;

            var meshData = new MeshData(verticesPerLine, verticesPerLine);
            var vertexIndex = 0;

            for (var y = 0; y < height; y += meshSimplificationIncrement) {
                for (var x = 0; x < width; x += meshSimplificationIncrement) {
                    meshData.Vertices[vertexIndex] = new Vector3(topLeftX + x,
                        myheightCurve.Evaluate(heightMap[x, y]) * heightMultiplier, topLeftZ - y);
                    meshData.Uvs[vertexIndex] = new Vector2(x / (float) width, y / (float) height);

                    if (x < width - 1 && y < height - 1) {
                        meshData.AddTriangle(vertexIndex, vertexIndex + verticesPerLine + 1,
                            vertexIndex + verticesPerLine);
                        meshData.AddTriangle(vertexIndex + verticesPerLine + 1, vertexIndex, vertexIndex + 1);
                    }

                    vertexIndex++;
                }
            }
            return meshData;
        }
    }

    public class MeshData
    {
        public Vector3[] Vertices;
        public int[] Triangles;
        public Vector2[] Uvs;

        private int _triangleIndex;

        public MeshData(int meshWidth, int meshHeight) {
            Vertices = new Vector3[meshWidth * meshHeight];
            Uvs = new Vector2[meshWidth * meshHeight];
            Triangles = new int[(meshWidth - 1) * (meshHeight - 1) * 6];
        }

        public void AddTriangle(int a, int b, int c) {
            Triangles[_triangleIndex] = a;
            Triangles[_triangleIndex + 1] = b;
            Triangles[_triangleIndex + 2] = c;
            _triangleIndex += 3;
        }

        public Mesh CreateMesh() {
            var mesh = new Mesh();
            mesh.vertices = Vertices;
            mesh.triangles = Triangles;
            mesh.uv = Uvs;
            mesh.RecalculateNormals();
            return mesh;
        }
    }
}