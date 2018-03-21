using System;
using System.ComponentModel;

namespace Domain.Monitoring.WeightShiftMonitor
{
    public class Reading
    {
        public Weight FrontSide { get; }

        public Weight RearSide { get; }

        public Weight RightSide { get; }

        public Weight LeftSide { get; }

        // convenience property for looping through weights
        public Weight[] Weights { get; }

        public DateTime Timestamp { get; }

        public Reading(Weight frontSide
            , Weight rearSide
            , Weight leftSide
            , Weight rightSide
            , DateTime timestamp)
        {
            FrontSide = frontSide ?? throw new ArgumentNullException(nameof(frontSide));
            RearSide = rearSide ?? throw new ArgumentNullException(nameof(rearSide));
            RightSide = rightSide ?? throw new ArgumentNullException(nameof(rightSide));
            LeftSide = leftSide ?? throw new ArgumentNullException(nameof(leftSide));

            Weights = new Weight[4];
            Weights[(int)Directions.Front] = FrontSide;
            Weights[(int)Directions.Rear] = RearSide;
            Weights[(int)Directions.Right] = RightSide;
            Weights[(int)Directions.Left] = LeftSide;

            Timestamp = timestamp;
        }

        public Shift CalcShift(Reading reading)
        {
            return new Shift( 
                FrontSide.CalcPercDiff(reading.FrontSide)
                , RearSide.CalcPercDiff(reading.RearSide)
                , RightSide.CalcPercDiff(reading.RightSide)
                , LeftSide.CalcPercDiff(reading.LeftSide)
            );
        }
    }
}
