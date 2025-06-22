using System;
using System.Collections.Generic;

namespace MORT.ColorThief
{
    class VBoxCountComparer : IComparer<VBox>
    {
        public int Compare(VBox? x, VBox? y)
        {
            ArgumentNullException.ThrowIfNull(x);
            ArgumentNullException.ThrowIfNull(y);
            var a = x.Count(false);
            var b = y.Count(false);
            return a < b ? -1 : (a > b ? 1 : 0);
        }
    }
}
