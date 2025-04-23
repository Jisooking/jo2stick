using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPGTemplate.Animation
{
    internal class Animation
    {
        public string[][] frames { get; set; } = Array.Empty<string[]>();
        public int[] x { get; set; } = Array.Empty<int>();
        public int[] y { get; set; } = Array.Empty<int>();
        public int frameDurationMs { get; set; } = 100;
        public Action? OnComplete { get; set; } = null;
    }
}
