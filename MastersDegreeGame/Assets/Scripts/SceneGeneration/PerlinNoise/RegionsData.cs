using System.Collections.Generic;
 
 namespace SceneGeneration.PerlinNoise
 {
     [UnityEngine.CreateAssetMenu(fileName = "New regions", menuName = "Map generator/Regions")]
     public class RegionsData : UnityEngine.ScriptableObject
     {
         public List<TerrainType> regions = new List<TerrainType>();
     }
 }