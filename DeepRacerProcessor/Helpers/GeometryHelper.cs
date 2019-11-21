using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeepRacerProcessor.Helpers
{
    public static class GeometryHelper
    {
        public static double GetDirectionBetweenTwoPoints(double[] p1, double[] p2)
        {
            var radians = Math.Atan2(p2[1] - p1[1], p2[0] - p1[0]);
            var degrees = (180 / Math.PI) * radians;
            return degrees;
        }
    }
}
