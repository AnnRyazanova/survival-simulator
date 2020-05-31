using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Terrain", menuName = "MapGenerator/TerrainData")]
public class TerrainData : UpdatableData
{
    public float uniformScale = 5f;
    public bool useFalloff;
    public float meshHeightMultiplier;
    public AnimationCurve meshHeightCurve;
}
