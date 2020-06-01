using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace SceneGeneration.PerlinNoise
{
    public class MapGenerator : MonoBehaviour
    {
        public enum DrawMode
        {
            NoiseMap,
            ColourMap,
            Mesh,
            FalloffMap
        };

        public DrawMode drawMode;

        public TerrainData terrainData;
        public NoiseData noiseData;

        public const int MapSize = 131;
        public const int editorPreviewLod = 0;
        
        public bool autoUpdate;

        public TerrainType[] regions;

        private float[,] _falloffMap;

        private Queue<MapThreadInfo<MapData>> _mapDataThreadInfoQueue = new Queue<MapThreadInfo<MapData>>();
        private Queue<MapThreadInfo<MeshData>> _meshDataThreadInfoQueue = new Queue<MapThreadInfo<MeshData>>();

        void OnValuesUpdated() {
            if (!Application.isPlaying) {
                DrawMapInEditor ();
            }
        }
        
        private void Awake() {
            _falloffMap = FalloffGenerator.GenerateFalloffMap(MapSize);
        }

        public void DrawMapInEditor() {
            var mapData = GenerateMapData(Vector2.zero);

            var display = FindObjectOfType<MapDisplay>();
            switch (drawMode) {
                case DrawMode.NoiseMap:
                    display.DrawTexture(TextureGenerator.TextureFromHeightMap(mapData.HeightMap));
                    break;
                case DrawMode.ColourMap:
                    display.DrawTexture(
                        TextureGenerator.TextureFromColourMap(mapData.ColourMap, MapSize, MapSize));
                    break;
                case DrawMode.Mesh:
                    display.DrawMesh(
                        MeshGenerator.GenerateTerrainMesh(mapData.HeightMap, terrainData.meshHeightMultiplier, terrainData.meshHeightCurve,
                            editorPreviewLod),
                        TextureGenerator.TextureFromColourMap(mapData.ColourMap, MapSize, MapSize));
                    break;
                case DrawMode.FalloffMap:
                    display.DrawTexture(
                        TextureGenerator.TextureFromHeightMap(FalloffGenerator.GenerateFalloffMap(MapSize)));
                    break;
            }
        }

        public void RequestMapData(Vector2 centre, Action<MapData> callback) {
            ThreadStart threadStart = delegate { MapDataThread(centre, callback); };

            new Thread(threadStart).Start();
        }

        private void MapDataThread(Vector2 centre, Action<MapData> callback) {
            var mapData = GenerateMapData(centre);
            lock (_mapDataThreadInfoQueue) {
                _mapDataThreadInfoQueue.Enqueue(new MapThreadInfo<MapData>(callback, mapData));
            }
        }

        public void RequestMeshData(MapData mapData, int lod, Action<MeshData> callback) {
            ThreadStart threadStart = delegate { MeshDataThread(mapData, lod, callback); };

            new Thread(threadStart).Start();
        }

        private void MeshDataThread(MapData mapData, int lod, Action<MeshData> callback) {
            var meshData =
                MeshGenerator.GenerateTerrainMesh(mapData.HeightMap, terrainData.meshHeightMultiplier, terrainData.meshHeightCurve, lod);
            lock (_meshDataThreadInfoQueue) {
                _meshDataThreadInfoQueue.Enqueue(new MapThreadInfo<MeshData>(callback, meshData));
            }
        }

        private void Update() {
            lock (_mapDataThreadInfoQueue) {
                if (_mapDataThreadInfoQueue.Count > 0) {
                    for (var i = 0; i < _mapDataThreadInfoQueue.Count; i++) {
                        var threadInfo = _mapDataThreadInfoQueue.Dequeue();
                        threadInfo.Callback(threadInfo.Parameter);
                    }
                }
            }

            if (_meshDataThreadInfoQueue.Count > 0) {
                for (var i = 0; i < _meshDataThreadInfoQueue.Count; i++) {
                    var threadInfo = _meshDataThreadInfoQueue.Dequeue();
                    threadInfo.Callback(threadInfo.Parameter);
                }
            }
        }

        private MapData GenerateMapData(Vector2 centre) {
            var noiseMap = Noise.GenerateNoiseMap(MapSize, MapSize, noiseData.seed, noiseData.noiseScale, noiseData.octaves, noiseData.persistance,
                noiseData.lacunarity, centre + noiseData.offset, noiseData.normalizeMode);

            var colourMap = new Color[MapSize * MapSize];
            for (var y = 0; y < MapSize; y++) {
                for (var x = 0; x < MapSize; x++) {
                    if (terrainData.useFalloff) {
                        noiseMap[x, y] = Mathf.Clamp01(noiseMap[x, y] - _falloffMap[x, y]);
                    }

                    var currentHeight = noiseMap[x, y];
                    for (var i = 0; i < regions.Length; i++) {
                        if (currentHeight >= regions[i].height) {
                            colourMap[y * MapSize + x] = regions[i].colour;
                        }
                        else {
                            break;
                        }
                    }
                }
            }


            return new MapData(noiseMap, colourMap);
        }

        private void OnValidate() {
            if (terrainData != null) {
                terrainData.OnValuesUpdated -= OnValuesUpdated;
                terrainData.OnValuesUpdated += OnValuesUpdated;
            }
            if (noiseData != null) {
                noiseData.OnValuesUpdated -= OnValuesUpdated;
                noiseData.OnValuesUpdated += OnValuesUpdated;
            }
            
            _falloffMap = FalloffGenerator.GenerateFalloffMap(MapSize);
        }

        private struct MapThreadInfo<T>
        {
            public readonly Action<T> Callback;
            public readonly T Parameter;

            public MapThreadInfo(Action<T> callback, T parameter) {
                this.Callback = callback;
                this.Parameter = parameter;
            }
        }
    }

    [System.Serializable]
    public struct TerrainType
    {
        public string name;
        public float height;
        public Color colour;
    }

    public struct MapData
    {
        public readonly float[,] HeightMap;
        public readonly Color[] ColourMap;

        public MapData(float[,] heightMap, Color[] colourMap) {
            this.HeightMap = heightMap;
            this.ColourMap = colourMap;
        }
    }
}