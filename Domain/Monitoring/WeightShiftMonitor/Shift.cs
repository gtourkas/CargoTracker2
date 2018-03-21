using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.Monitoring.WeightShiftMonitor
{

    public class Shift
    {
        public PercentageOffset FrontSide { get; }

        public PercentageOffset RearSide { get; }

        public PercentageOffset RightSide { get; }

        public PercentageOffset LeftSide { get; }


        public Directions DirOfLargestShift { get; }

        public PercentageOffset PercOfLargestShift { get; }

        public Shift(PercentageOffset frontSide, PercentageOffset rearSide, PercentageOffset rightSide, PercentageOffset leftSide)
        {
            FrontSide = frontSide;
            RearSide = rearSide;
            RightSide = rightSide;
            LeftSide = leftSide;

            DirOfLargestShift = Directions.Front;
            PercOfLargestShift = frontSide;

            var shifts = new List<PercentageOffset> { rearSide, rightSide, leftSide };
            for (var i = 1; i < shifts.Count; i++)
            {
                var s = shifts[i];

                if (s.GreaterThan(PercOfLargestShift))
                {
                    PercOfLargestShift = s;
                    DirOfLargestShift = (Directions)i;
                }
            }
        }
     }

}
