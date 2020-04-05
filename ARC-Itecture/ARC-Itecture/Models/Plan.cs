using ARC_Itecture;
using ARC_Itecture.DrawCommand;
using ARC_Itecture.DrawCommand.Commands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using Point = System.Windows.Point;

[System.Serializable]
public class Plan
{
    [JsonProperty("wallHeight")]
    public float WallHeight { get; set; }

    [JsonProperty("wallWidth")]
    public float WallWidth { get; set; }

    [JsonProperty("windowH1")]
    public float WindowH1 { get; set; }

    [JsonProperty("windowH2")]
    public float WindowH2 { get; set; }

    [JsonProperty("doorH2")]
    public float DoorH2 { get; set; }

    public List<float> entryPoint;
    public List<Segment> segments;
    public List<Area> areas;
    public List<Door> doors;

    private const string SEGMENT_STRING = "seg";

    public Plan()
    {
        entryPoint = new List<float>();
        segments = new List<Segment>();
        areas = new List<Area>();
        doors = new List<Door>();
    }

    public void AddComponent(Point point1, Point point2, ComponentType componentType)
    {
        if (componentType == ComponentType.Area)
        {
            List<List<float>> corners = new List<List<float>>
            {
                new List<float>() { (float)point1.X, (float)point1.Y },
                new List<float>() { (float)point2.X, (float)point1.Y },
                new List<float>() { (float)point2.X, (float)point2.Y },
                new List<float>() { (float)point1.X, (float)point2.Y }
            };

            for(int i = 0; i < corners.Count; i++)
            {
                List<float> start;
                List<float> stop;

                if(i != corners.Count - 1)
                {
                    start = corners[i];
                    stop = corners[i + 1];
                }
                else
                {
                    start = corners[i];
                    stop = corners[0];
                }
                segments.Add(new Segment(SEGMENT_STRING + Segment.nbSegment, start, stop));
            }

            areas.Add(new Area("Test", corners));
        }
    }

    public void AddComponent(Point p, ComponentType componentType)
    {
        if(componentType == ComponentType.Camera)
        {
            entryPoint.Add((float) p.X);
            entryPoint.Add((float) p.Y);
        }
    }

    public void ImportDraw(Receiver receiver, Invoker invoker)
    {
        // Entry points
        invoker.Command = new CameraCommand(receiver);
        invoker.Invoke(new Point(entryPoint[0], entryPoint[1]));

        // Area
        invoker.Command = new AreaCommand(receiver);
        foreach(Area area in areas)
        {
            Tuple<PointF, PointF> minMaxPoints = area.GetMinMaxPoints();
            invoker.Invoke(new Point(minMaxPoints.Item1.X, minMaxPoints.Item1.Y));
            invoker.Invoke(new Point(minMaxPoints.Item2.X, minMaxPoints.Item2.Y));
        }
    }
}
