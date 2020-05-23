using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DefaultNamespace
{
    public static class StandardDeviation
    {
        public static float GetStd(this IEnumerable<float> values)
        {
            float standardDeviation = 0;
            var enumerable = values as float[] ?? values.ToArray();
            var count = enumerable.Length;
            if (count > 1)
            {
                var avg = enumerable.Average();
                var sum = enumerable.Sum(d => (d - avg) * (d - avg));
                standardDeviation = Mathf.Sqrt(sum / count);
            }
            return standardDeviation;
        }
    }
}