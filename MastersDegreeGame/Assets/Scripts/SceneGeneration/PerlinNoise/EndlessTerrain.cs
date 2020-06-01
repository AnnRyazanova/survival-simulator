/*using System.Collections.Generic;
using UnityEngine;

namespace SceneGeneration.PerlinNoise
{
    public class EndlessTerrain : MonoBehaviour
    {
        //private const float Scale = 5f;
        private const float ViewerMoveThresholdForChunkUpdate = 25f;

        private const float SqrViewerMoveThresholdForChunkUpdate =
            ViewerMoveThresholdForChunkUpdate * ViewerMoveThresholdForChunkUpdate;

        public LodInfo[] detailLevels;
        public static float MaxViewDst;

        public Transform viewer;
        public Material mapMaterial;

        public static Vector2 ViewerPosition;
        private Vector2 _viewerPositionOld;
        private static MapGenerator _mapGenerator;
        private int _chunkSize;
        private int _chunksVisibleInViewDst;

        private readonly Dictionary<Vector2, TerrainChunk> _terrainChunkDictionary = new Dictionary<Vector2, TerrainChunk>();
        private static readonly List<TerrainChunk> TerrainChunksVisibleLastUpdate = new List<TerrainChunk>();

        private void Start() {
            _mapGenerator = FindObjectOfType<MapGenerator>();

            MaxViewDst = detailLevels[detailLevels.Length - 1].visibleDstThreshold;
            _chunkSize = MapGenerator.MapSize - 1;
            _chunksVisibleInViewDst = Mathf.RoundToInt(MaxViewDst / _chunkSize);

            UpdateVisibleChunks();
        }

        private void Update() {
            var position = viewer.position;
            ViewerPosition = new Vector2(position.x, position.z) / _mapGenerator.terrainData.uniformScale;

            if ((_viewerPositionOld - ViewerPosition).sqrMagnitude > SqrViewerMoveThresholdForChunkUpdate) {
                _viewerPositionOld = ViewerPosition;
                UpdateVisibleChunks();
            }
        }

        private void UpdateVisibleChunks() {
            for (var i = 0; i < TerrainChunksVisibleLastUpdate.Count; i++) {
                TerrainChunksVisibleLastUpdate[i].SetVisible(false);
            }

            TerrainChunksVisibleLastUpdate.Clear();

            var currentChunkCoordX = Mathf.RoundToInt(ViewerPosition.x / _chunkSize);
            var currentChunkCoordY = Mathf.RoundToInt(ViewerPosition.y / _chunkSize);

            for (var yOffset = -_chunksVisibleInViewDst; yOffset <= _chunksVisibleInViewDst; yOffset++) {
                for (var xOffset = -_chunksVisibleInViewDst; xOffset <= _chunksVisibleInViewDst; xOffset++) {
                    var viewedChunkCoord = new Vector2(currentChunkCoordX + xOffset, currentChunkCoordY + yOffset);

                    if (_terrainChunkDictionary.ContainsKey(viewedChunkCoord)) {
                        _terrainChunkDictionary[viewedChunkCoord].UpdateTerrainChunk();
                    }
                    else {
                        _terrainChunkDictionary.Add(viewedChunkCoord,
                            new TerrainChunk(viewedChunkCoord, _chunkSize, detailLevels, transform, mapMaterial));
                    }
                }
            }
        }

        public class TerrainChunk
        {
            private readonly GameObject _meshObject;
            private readonly Vector2 _position;
            private Bounds _bounds;

            private readonly MeshRenderer _meshRenderer;
            private readonly MeshFilter _meshFilter;

            private readonly LodInfo[] _detailLevels;
            private readonly LodMesh[] _lodMeshes;

            private MapData _mapData;
            private bool _mapDataReceived;
            private int _previousLodIndex = -1;

            public TerrainChunk(Vector2 coord, int size, LodInfo[] detailLevels, Transform parent, Material material) {
                _detailLevels = detailLevels;

                _position = coord * size;
                _bounds = new Bounds(_position, Vector2.one * size);
                var positionV3 = new Vector3(_position.x, 0, _position.y);

                _meshObject = new GameObject("Terrain Chunk");
                _meshRenderer = _meshObject.AddComponent<MeshRenderer>();
                _meshFilter = _meshObject.AddComponent<MeshFilter>();
                _meshRenderer.material = material;

                _meshObject.transform.position = positionV3 * _mapGenerator.terrainData.uniformScale;
                _meshObject.transform.parent = parent;
                _meshObject.transform.localScale = Vector3.one * _mapGenerator.terrainData.uniformScale;
                SetVisible(false);

                _lodMeshes = new LodMesh[detailLevels.Length];
                for (var i = 0; i < detailLevels.Length; i++) {
                    _lodMeshes[i] = new LodMesh(detailLevels[i].lod, UpdateTerrainChunk);
                }

                _mapGenerator.RequestMapData(_position, OnMapDataReceived);
            }

            private void OnMapDataReceived(MapData mapData) {
                _mapData = mapData;
                _mapDataReceived = true;

                var texture = TextureGenerator.TextureFromColourMap(mapData.ColourMap, MapGenerator.MapSize,
                    MapGenerator.MapSize);
                _meshRenderer.material.mainTexture = texture;

                UpdateTerrainChunk();
            }


            public void UpdateTerrainChunk() {
                if (_mapDataReceived) {
                    var viewerDstFromNearestEdge = Mathf.Sqrt(_bounds.SqrDistance(ViewerPosition));
                    var visible = viewerDstFromNearestEdge <= MaxViewDst;

                    if (visible) {
                        var lodIndex = 0;

                        for (var i = 0; i < _detailLevels.Length - 1; i++) {
                            if (viewerDstFromNearestEdge > _detailLevels[i].visibleDstThreshold) {
                                lodIndex = i + 1;
                            }
                            else {
                                break;
                            }
                        }

                        if (lodIndex != _previousLodIndex) {
                            var lodMesh = _lodMeshes[lodIndex];
                            if (lodMesh.HasMesh) {
                                _previousLodIndex = lodIndex;
                                _meshFilter.mesh = lodMesh.Mesh;
                            }
                            else if (!lodMesh.HasRequestedMesh) {
                                lodMesh.RequestMesh(_mapData);
                            }
                        }

                        TerrainChunksVisibleLastUpdate.Add(this);
                    }

                    SetVisible(visible);
                }
            }

            public void SetVisible(bool visible) {
                _meshObject.SetActive(visible);
            }

            public bool IsVisible() {
                return _meshObject.activeSelf;
            }
        }

        private class LodMesh
        {
            public Mesh Mesh;
            public bool HasRequestedMesh;
            public bool HasMesh;
            private readonly int _lod;
            private readonly System.Action _updateCallback;

            public LodMesh(int lod, System.Action updateCallback) {
                _lod = lod;
                _updateCallback = updateCallback;
            }

            private void OnMeshDataReceived(MeshData meshData) {
                Mesh = meshData.CreateMesh();
                HasMesh = true;

                _updateCallback();
            }

            public void RequestMesh(MapData mapData) {
                HasRequestedMesh = true;
                _mapGenerator.RequestMeshData(mapData, _lod, OnMeshDataReceived);
            }
        }

        [System.Serializable]
        public struct LodInfo
        {
            public int lod;
            public float visibleDstThreshold;
        }
    }
}*/