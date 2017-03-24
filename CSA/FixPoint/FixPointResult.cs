using System;
using System.Collections.Immutable;

namespace CSA.FixPoint
{
    class FixPointResult
    {
        public FixPointResult()
        {
            Gen = ImmutableHashSet<string>.Empty;
            Kill = ImmutableHashSet<string>.Empty;
            In = ImmutableHashSet<string>.Empty;
            Out = ImmutableHashSet<string>.Empty;
        }

        public ImmutableHashSet<string> Gen { get; set; }
        public ImmutableHashSet<string> Kill { get; set; }
        public ImmutableHashSet<string> In { get; set; }
        public ImmutableHashSet<string> Out { get; set; }

        public override string ToString()
        {
            var str = "";
            str += "Gen: " + string.Join(", ", Gen) + Environment.NewLine;
            str += "Kill: " + string.Join(", ", Kill) + Environment.NewLine;
            str += "In: " + string.Join(", ", In) + Environment.NewLine;
            str += "Out: " + string.Join(", ", Out) + Environment.NewLine;
            return str;
        }
    }
}