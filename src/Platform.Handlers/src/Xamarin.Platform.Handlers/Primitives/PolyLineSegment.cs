﻿namespace Xamarin.Forms
{
    public class PolyLineSegment : PathSegment
    {
        public PolyLineSegment()
        {
            Points = new PointCollection();
        }

        public PolyLineSegment(PointCollection points)
        {
            Points = points;
        }

        public PointCollection Points { get; set; }
    }
}