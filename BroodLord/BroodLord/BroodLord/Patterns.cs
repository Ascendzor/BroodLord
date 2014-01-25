using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BroodLord
{
    public static class Patterns
    {
        public static List<Pattern> patterns;



        public static void Initilize()
        {
            patterns.Add(new Pattern(new List<String>(new String[]{"Rock", "Club"})));
        }
    }
}
