using ARC_Itecture;
using ARC_Itecture.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Windows;

[System.Serializable]
public class Door
{
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("isFrontDoor")]
    public bool IsFrontDoor { get; set; }

    [JsonProperty("start")]
    public List<float> Start { get; set; }

    [JsonProperty("stop")]
    public List<float> Stop { get; set; }

    public static int NbDoor = 0;

    public Door(string name, List<float> start, List<float> stop)
    {
        this.Name = name;
        this.Start = start;
        this.Stop = stop;
        this.IsFrontDoor = false;
    }

    public Tuple<Segment, Segment> BreakDoor(Segment s)
    {
        /*List<PointF> list = new List<PointF>() { new PointF(s.Start[0], s.Start[1]), 
                                                new PointF(s.Stop[0], s.Stop[1]), 
                                                new PointF(this.Start[0], this.Start[1]), 
                                                new PointF(this.Stop[0], this.Stop[1])};
        List<PointF> l = list.OrderBy(p => p.Y).ThenBy(p => p.X).ToList();

        Segment s1 = new Segment("test", new List<float>() { l[0].X, l[0].Y }, new List<float>() { l[1].X, l[1].Y });
        Segment s2 = new Segment("test", new List<float>() { l[2].X, l[2].Y }, new List<float>() { l[3].X, l[3].Y });

        HouseWindow w = s.Window;
        if (w != null)
        {
            // Check if the segment is vertical or horizontal
            if(Math.Floor(s.Start[1]) == Math.Floor(s.Stop[1]))
            {
                if (w.Start[0] > s1.Start[0] && w.Stop[0] < s1.Stop[0])
                {
                    s1.Window = w;
                }
                else if (w.Start[0] > s2.Start[0] && w.Stop[0] < s2.Stop[0])
                {
                    s2.Window = w;
                }
            }
            else
            {
                if (w.Start[1] > s1.Start[1] && w.Stop[1] < s1.Stop[1])
                {
                    s1.Window = w;
                }
                else if (w.Start[1] > s2.Start[1] && w.Stop[1] < s2.Stop[1])
                {
                    s2.Window = w;
                }
            }
        }
        return new Tuple<Segment, Segment>(s1, s2);*/
        return null;
    }
}
