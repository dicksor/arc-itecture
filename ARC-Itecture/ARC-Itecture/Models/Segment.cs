using ARC_Itecture;
using System;
using System.Collections.Generic;
using System.Windows;

[System.Serializable]
public class Segment
{
    public static int nbSegment = 0;

    public string Name { get; set; }
    public List<float> Start { get; set; }
    public List<float> Stop { get; set; }
    public HouseWindow Window { get; set; }

    public Segment(string name, List<float> start, List<float> stop)
    {
        this.Name = name;
        this.Start = start;
        this.Stop = stop;
    }

    public Segment FindSegmentByCoord(Point p1, Point p2)
    {
        Segment s = null;
        if(Window == null)
        {
            if(Math.Floor(p1.X) == Math.Floor(Start[0]) && 
               Math.Floor(p1.Y) == Math.Floor(Start[1]) && 
               Math.Floor(p2.X) == Math.Floor(Stop[0]) && 
               Math.Floor(p2.Y) == Math.Floor(Stop[1]))
            {
                s = this;
            }
        }
        return s;
    }
}
