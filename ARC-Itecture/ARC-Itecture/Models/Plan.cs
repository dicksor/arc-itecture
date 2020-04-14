using ARC_Itecture;
using ARC_Itecture.DrawCommand;
using ARC_Itecture.DrawCommand.Commands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Shapes;
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
        entryPoint = new List<float>(); // camera
        segments = new List<Segment>(); // wall
        areas = new List<Area>(); // areas
        doors = new List<Door>(); // doors
    }

    private Segment FindSegmentByCoords(Point p1, Point p2)
    {
        Segment segment = null;
        foreach(Segment s in segments)
        {
            Segment currentOneTwo = s.FindSegmentByCoord(p1, p2);
            if(currentOneTwo != null)
            {
                segment = currentOneTwo;
            }

            Segment currentTwoOne = s.FindSegmentByCoord(p2, p1);
            if(currentTwoOne != null)
            {
                segment = currentTwoOne;
            }
        }
        return segment;
    }

    public HouseWindow AddWindow(Point window1, Point window2, Point wall1, Point wall2)
    {
        Segment wall = FindSegmentByCoords(wall1, wall2);
        HouseWindow houseWindow = new HouseWindow(new List<float>() { (float)window1.X, (float)window1.Y}, new List<float>() { (float)window2.X, (float)window2.Y});
        wall.Window = houseWindow;
        return houseWindow;
    }

    public Area AddArea(Point point1, Point point2, string areaTypeName)
    {
        List<List<float>> corners = new List<List<float>>
            {
                new List<float>() { (float)point1.X, (float)point1.Y },
                new List<float>() { (float)point2.X, (float)point1.Y },
                new List<float>() { (float)point2.X, (float)point2.Y },
                new List<float>() { (float)point1.X, (float)point2.Y }
            };
        Area area = new Area(areaTypeName, corners);
        areas.Add(area);
        return area;
    }

    public Segment AddWall(Point point1, Point point2)
    {
        Segment.nbSegment++;
        List<float> start = new List<float>() { (float)point1.X, (float)point1.Y };
        List<float> stop = new List<float>() { (float)point2.X, (float)point2.Y };

        Segment segment = new Segment(SEGMENT_STRING + Segment.nbSegment, start, stop);
        segments.Add(segment);
        return segment;
    }

    public void AddCamera(Point p)
    {
        entryPoint.Add((float) p.X);
        entryPoint.Add((float) p.Y);
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
        foreach (Area area in areas)
        {
            invoker.DrawCommand = new AreaCommand(receiver, area.Type);
            invoker.PreviewCommand = new PreviewAreaCommand(receiver);

            Tuple<PointF, PointF> minMaxPoints = area.GetMinMaxPoints();

            invoker.InvokeClick(new Point(minMaxPoints.Item1.X, minMaxPoints.Item1.Y));

            invoker.InvokeMouseMove(new Point(minMaxPoints.Item1.X, minMaxPoints.Item1.Y));
            invoker.InvokeMouseMove(new Point(minMaxPoints.Item2.X, minMaxPoints.Item2.Y));

            invoker.InvokeClick(new Point(minMaxPoints.Item2.X, minMaxPoints.Item2.Y));
        }

        // Segments
        foreach(Segment segment in segments)
        {
            invoker.DrawCommand = new WallCommand(receiver);
            invoker.InvokeClick(new Point(segment.Start[0], segment.Start[1]));
            invoker.InvokeClick(new Point(segment.Stop[0], segment.Stop[1]));

            if (segment.Window != null)
            {
                invoker.PreviewCommand = new PreviewWindowCommand(receiver);
                invoker.DrawCommand = new WindowCommand(receiver);

                invoker.InvokeClick(new Point(segment.Window.Start[0], segment.Window.Start[1]));

                invoker.InvokeMouseMove(new Point(segment.Window.Start[0], segment.Window.Start[1]));
                invoker.InvokeMouseMove(new Point(segment.Window.Stop[0], segment.Window.Stop[1]));
     
                invoker.InvokeClick(new Point(segment.Window.Stop[0], segment.Window.Stop[1]));
            }
        }
    }

    public void RemoveObject(Object obj)
    {
        switch(obj)
        {
            case Segment s:
                segments.Remove(s);
                Debug.WriteLine("Remove segment");
                break;
            case Area a:
                areas.Remove(a);
                Debug.WriteLine("Remove area");
                break;
            case HouseWindow w:
                this.RemoveWindow(w);
                Debug.WriteLine("Remove window");
                break;
            case System.Windows.Controls.Image i:
                CameraCommand.ResetIsAlreadyUsed();
                Debug.WriteLine("Remove camera");
                break;
            default:
                Debug.WriteLine("<unknown shape>");
                break;
            case null:
                throw new ArgumentNullException(nameof(obj));
        }
    }

    public void RemoveWindow(HouseWindow w)
    {
        List<HouseWindow> hw = ListWindows();
        
        int index = hw.IndexOf(w);
        segments[index].Window = null;

        Debug.WriteLine(index);
    }

    public List<HouseWindow> ListWindows()
    {
        List<HouseWindow> hw = new List<HouseWindow>();

        foreach(Segment s in segments)
        {
            hw.Add(s.Window);
        } 

        return hw;
    }
}
