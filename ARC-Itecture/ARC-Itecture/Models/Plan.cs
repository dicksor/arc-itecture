using ARC_Itecture;
using ARC_Itecture.DrawCommand;
using ARC_Itecture.DrawCommand.Commands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows;
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

    public IDrawComponent AddComponent(Point point1, Point point2, ComponentType componentType)
    {
        IDrawComponent component = null;
        if (componentType == ComponentType.Area)
        {
            List<List<float>> corners = new List<List<float>>
            {
                new List<float>() { (float)point1.X, (float)point1.Y },
                new List<float>() { (float)point2.X, (float)point1.Y },
                new List<float>() { (float)point2.X, (float)point2.Y },
                new List<float>() { (float)point1.X, (float)point2.Y }
            };
            component = new Area("Test", corners);
            areas.Add((Area)component);
        }
        else if(componentType == ComponentType.Wall)
        {
            Debug.WriteLine("point");

            Segment.nbSegment++;
            List<float> start = new List<float>() { (float)point1.X, (float)point1.Y };
            List<float> stop = new List<float>() { (float)point2.X, (float)point2.Y };

            component = new Segment(SEGMENT_STRING + Segment.nbSegment, start, stop);
            segments.Add((Segment)component);
        }
        return component;
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
        invoker.DrawCommand = new CameraCommand(receiver);
        if(entryPoint.Count != 0)
        {
            invoker.InvokeClick(new Point(entryPoint[0], entryPoint[1]));
        }


        // Areas
        invoker.DrawCommand = new AreaCommand(receiver);
        foreach(Area area in areas)
        {
            Tuple<PointF, PointF> minMaxPoints = area.GetMinMaxPoints();
            invoker.InvokeClick(new Point(minMaxPoints.Item1.X, minMaxPoints.Item1.Y));
            invoker.InvokeClick(new Point(minMaxPoints.Item2.X, minMaxPoints.Item2.Y));
        }

        // Segments
        foreach(Segment segment in segments)
        {
            invoker.DrawCommand = new WallCommand(receiver);
            invoker.InvokeClick(new Point(segment.Start[0], segment.Start[1]));
            invoker.InvokeClick(new Point(segment.Stop[0], segment.Stop[1]));
        }
    }
}
