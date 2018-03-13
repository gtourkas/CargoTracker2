using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.Monitoring.WeightShiftMonitor
{

    public class Shift
    {
        public Percentage FrontSide { get; }

        public Percentage RearSide { get; }

        public Percentage RightSide { get; }

        public Percentage LeftSide { get; }


        public Directions DirOfLargestShift { get; }

        public Percentage PercOfLargestShift { get; }

        public Shift(Percentage frontSide, Percentage rearSide, Percentage rightSide, Percentage leftSide)
        {
            FrontSide = frontSide;
            RearSide = rearSide;
            RightSide = rightSide;
            LeftSide = leftSide;

            DirOfLargestShift = Directions.Front;
            PercOfLargestShift = frontSide;

            var shifts = new List<Percentage> { rearSide, rightSide, leftSide };
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
