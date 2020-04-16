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

    [JsonProperty("entryPoint")]
    public List<float> _entryPoint;

    [JsonProperty("segments")]
    public List<Segment> _segments;

    [JsonProperty("areas")]
    public List<Area> _areas;

    [JsonProperty("doors")]
    public List<Door> _doors;

    public double GridRatio
    {
        set
        {
            _gridRatio = value;
        }
    }

    private double _gridRatio;

    private const string SEGMENT_STRING = "seg";
    private const string DOOR_STRING = "door";

    public Plan(Rect bounds)
    {
        this._gridRatio = bounds.Width;
        _entryPoint = new List<float>(); // camera
        _segments = new List<Segment>(); // wall
        _areas = new List<Area>(); // areas
        _doors = new List<Door>(); // doors
    }

    private Segment FindSegmentByCoords(Point p1, Point p2)
    {
        Segment segment = null;
        foreach(Segment s in _segments)
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
        ScaleGeometrySave(ref window1);
        ScaleGeometrySave(ref window2);
        ScaleGeometrySave(ref wall1);
        ScaleGeometrySave(ref wall2);

        Segment wall = FindSegmentByCoords(wall1, wall2);
        HouseWindow houseWindow = new HouseWindow(new List<float>() { (float)window1.X, (float)window1.Y}, new List<float>() { (float)window2.X, (float)window2.Y});
        wall.Window = houseWindow;
        return houseWindow;
    }

    public Area AddArea(Point point1, Point point2, string areaTypeName)
    {
        ScaleGeometrySave(ref point1);
        ScaleGeometrySave(ref point2);

        List<List<float>> corners = new List<List<float>>
            {
                new List<float>() { (float)point1.X, (float)point1.Y },
                new List<float>() { (float)point2.X, (float)point1.Y },
                new List<float>() { (float)point2.X, (float)point2.Y },
                new List<float>() { (float)point1.X, (float)point2.Y }
            };
        Area area = new Area(areaTypeName, corners);
        _areas.Add(area);
        return area;
    }

    public Segment AddWall(Point point1, Point point2)
    {
        ScaleGeometrySave(ref point1);
        ScaleGeometrySave(ref point2);

        Segment.nbSegment++;
        List<float> start = new List<float>() { (float)point1.X, (float)point1.Y };
        List<float> stop = new List<float>() { (float)point2.X, (float)point2.Y };

        Segment segment = new Segment(SEGMENT_STRING + Segment.nbSegment, start, stop);
        _segments.Add(segment);
        return segment;
    }

    public void AddCamera(Point p)
    {
        ScaleGeometrySave(ref p);
     
        _entryPoint.Add((float) p.X);
        _entryPoint.Add((float) p.Y);
    }

    public Door AddDoor(Point point1, Point point2)
    {
        Door.NbDoor++;
        List<float> start = new List<float>() { (float)point1.X, (float)point1.Y };
        List<float> stop = new List<float>() { (float)point2.X, (float)point2.Y };

        Door door = new Door(DOOR_STRING + Door.NbDoor, start, stop);

        _doors.Add(door);

        return door;
    }

    public void ImportDraw(Receiver receiver, Invoker invoker)
    {
        // Entry points
        invoker.DrawCommand = new CameraCommand(receiver);
        if(_entryPoint.Count != 0)
        {
            Point cameraPoint = ScaleGeometryLoad(new Point(_entryPoint[0], _entryPoint[1]));
            invoker.InvokeClick(cameraPoint);
        }

        // Areas
        foreach (Area area in _areas)
        {
            invoker.DrawCommand = new AreaCommand(receiver, area.Type);
            invoker.PreviewCommand = new PreviewAreaCommand(receiver);

            Tuple<PointF, PointF> minMaxPoints = area.GetMinMaxPoints();
            Point areaTopLeftPoint = ScaleGeometryLoad(new Point(minMaxPoints.Item1.X, minMaxPoints.Item1.Y));
            invoker.InvokeClick(areaTopLeftPoint);
            invoker.InvokeMouseMove(areaTopLeftPoint);

            Point areaBottomRightPoint = ScaleGeometryLoad(new Point(minMaxPoints.Item1.X, minMaxPoints.Item1.Y));
            invoker.InvokeMouseMove(areaBottomRightPoint);
            invoker.InvokeClick(areaBottomRightPoint);
        }

        // Segments
        foreach(Segment segment in _segments)
        {
            invoker.DrawCommand = new WallCommand(receiver);
            Point wallStartPoint = ScaleGeometryLoad(new Point(segment.Start[0], segment.Start[1]));
            invoker.InvokeClick(wallStartPoint);
            Point wallEndPoint = ScaleGeometryLoad(new Point(segment.Stop[0], segment.Stop[1]));
            invoker.InvokeClick(wallEndPoint);

            if (segment.Window != null)
            {
                invoker.PreviewCommand = new PreviewWindowCommand(receiver);
                invoker.DrawCommand = new WindowCommand(receiver);

                Point windowStartPoint = ScaleGeometryLoad(new Point(segment.Window.Start[0], segment.Window.Start[1]));
                invoker.InvokeClick(windowStartPoint);
                invoker.InvokeMouseMove(windowStartPoint);

                Point windowEndPoint = ScaleGeometryLoad(new Point(segment.Window.Stop[0], segment.Window.Stop[1]));
                invoker.InvokeMouseMove(windowEndPoint);
                invoker.InvokeClick(windowEndPoint);
            }
        }

        foreach(Door door in _doors)
        {
            invoker.DrawCommand = new DoorCommand(receiver);
            invoker.PreviewCommand = new PreviewDoorCommand(receiver);

            Point doorStart = ScaleGeometryLoad(new Point(door.Start[0], door.Start[1]));
            invoker.InvokeClick(doorStart);
            invoker.InvokeMouseMove(new Point(door.Start[0], door.Start[1]));

            Point doorEnd = ScaleGeometryLoad(new Point(door.Stop[0], door.Stop[1]));
            invoker.InvokeMouseMove(doorEnd);
            invoker.InvokeClick(doorEnd);
        }
    }

    public void RemoveObject(Object obj)
    {
        switch(obj)
        {
            case Segment s:
                _segments.Remove(s);
                break;
            case Area a:
                _areas.Remove(a);
                break;
            case HouseWindow w:
                this.RemoveWindow(w);
                break;
            case Door d:
                _doors.Remove(d);
                break;
            case System.Windows.Controls.Image i:
                CameraCommand.ResetIsAlreadyUsed();
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
        _segments[index].Window = null;

        Debug.WriteLine(index);
    }

    public List<HouseWindow> ListWindows()
    {
        List<HouseWindow> hw = new List<HouseWindow>();

        foreach(Segment s in _segments)
        {
            hw.Add(s.Window);
        } 

        return hw;
    }

    private void ScaleGeometrySave(ref Point point)
    {
        point.X /= _gridRatio;
        point.Y /= _gridRatio;
    }

    private Point ScaleGeometryLoad(Point point)
    {
        Point canvasScaledPoint = new Point();

        canvasScaledPoint.X = point.X * _gridRatio;
        canvasScaledPoint.Y = point.Y * _gridRatio;

        return canvasScaledPoint;
    }
}
