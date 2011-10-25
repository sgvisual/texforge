using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace texforge
{
    public static class Noise
    {
        public static float Noise1(int x, int y)
        {
            int n = x + y * 57;
            n = (n<<13)^n;
            return ( 1f - ( (n * (n * n * 15731 + 789221) + 1376312589) & 0x7fffffff) / 1073741824f);   
        }

        public static float SmoothNoise(int x, int y)
        {
            float corners = (Noise1(x-1, y-1) + Noise1(x+1, y-1) + Noise1(x-1, y+1) + Noise1(x+1, y+1) ) / 16;
            float sides = (Noise1(x-1, y) + Noise1(x+1, y) + Noise1(x, y-1) + Noise1(x, y+1) ) / 8;
            float center = Noise1(x, y) / 4;

            return corners + sides + center;
        }
        
    }
}
