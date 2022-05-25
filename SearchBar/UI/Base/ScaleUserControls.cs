using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchBar.UI.Base
{
    public static class ScaleUserControls
    {
        static readonly double NewsWidthScale = 0.083333333;
        static readonly double NewsHeightScale = 0.208605555;
        static readonly double NewsMaxAmoutScale = 0.003125;

        public static double GetNewsWidth(double width)
        {
            return width * NewsWidthScale;
        }

        public static double GetNewsHeight(double height)
        {
            return height * NewsHeightScale;
        }

        public static int GetNewsMaxAmount(double width)
        {
            return (int)Math.Round(NewsMaxAmoutScale * width);
        }
    }
}
