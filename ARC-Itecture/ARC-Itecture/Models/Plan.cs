/*
 * ARC-Itecture
 * Romain Capocasale, Vincent Moulin and Jonas Freiburghaus
 * He-Arc, INF3dlm-a
 * 2019-2020
 * .NET Course
 */


using ARC_Itecture.DrawCommand;
using ARC_Itecture.DrawCommand.Commands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Shapes;
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

    /// <summary>
    /// Find segment in the plan list by this coordinates.
    /// Find segment from point 1 to point 2 and from point 2 to point 1.
    /// </summary>
    /// <param name="p1">Segment point 1</param>
    /// <param name="p2">Segment point 2</param>
    /// <returns>The finded segment</returns>
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

    public void UpdateWall(Line oldL1, Line oldL2, Point? intersection)
    {
        Point p1L1 = new Point(oldL1.X1, oldL1.Y1);
        Point p2L1 = new Point(oldL1.X2, oldL1.Y2);
        Point p1L2 = new Point(oldL2.X1, oldL2.Y1);
        Point p2L2 = new Point(oldL2.X2, oldL2.Y2);
        Point scaledIntersection = new Point((float)intersection?.X, (float)intersection?.Y);

        ScaleGeometrySave(ref p1L1);
        ScaleGeometrySave(ref p2L1);
        ScaleGeometrySave(ref p1L2);
        ScaleGeometrySave(ref p2L2);
        ScaleGeometrySave(ref scaledIntersection);

        Segment s1 = FindSegmentByCoords(p1L1, p2L1);
        Segment s2 = FindSegmentByCoords(p1L2, p2L2);

        s1.Stop.Clear();
        s1.Stop.Add((float)scaledIntersection.X);
        s1.Stop.Add((float)scaledIntersection.Y);

        s2.Start.Clear();
        s2.Start.Add((float)scaledIntersection.X);
        s2.Start.Add((float)scaledIntersection.Y);
    }

    /// <summary>
    /// Add a window to a plan.  
    /// </summary>
    /// <param name="window1">Window first point</param>
    /// <param name="window2">Window second point</param>
    /// <param name="wall1">Attached wall first point</param>
    /// <param name="wall2">Attached wall second point</param>
    /// <returns>Created HouseWindow object</returns>
    public HouseWindow AddWindow(Point window1, Point window2, Point wall1, Point wall2)
    {
        ScaleGeometrySave(ref window1);
        ScaleGeometrySave(ref window2);
        ScaleGeometrySave(ref wall1);
        ScaleGeometrySave(ref wall2);

        Segment wall = FindSegmentByCoords(wall1, wall2);

        if (wall != null)
        {
            HouseWindow houseWindow = new HouseWindow(new List<float>() { (float)window1.X, (float)window1.Y }, new List<float>() { (float)window2.X, (float)window2.Y });
            wall.Window = houseWindow;
            return houseWindow;
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// Add a area to a plan.
    /// Area is exactly large enough to contain the two specified points.
    /// </summary>
    /// <param name="point1">Area first point</param>
    /// <param name="point2">Area second point</param>
    /// <param name="areaTypeName">Area type</param>
    /// <returns>Created Area object</returns>
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

    /// <summary>
    /// Add a wall to a plan
    /// </summary>
    /// <param name="point1">Wall first point</param>
    /// <param name="point2">Wall second point</param>
    /// <returns>Created wall object</returns>
    public Segment AddWall(Point point1, Point point2)
    {
        ScaleGeometrySave(ref point1);
        ScaleGeometrySave(ref point2);

        Segment.nbSegment++;
        List<float> start = new List<float>() { (float)point1.X, (float)point1.Y };
        List<float> stop = new List<float>() { (float)point2.X, (float)point2.Y };

        Segment segment = new Segment(SEGMENT_STRING + Segment.nbSegment, start, stop, new HouseWindow());
        _segments.Add(segment);
        return segment;
    }

    /// <summary>
    /// Add a camera to a plan
    /// </summary>
    /// <param name="p">Camera location</param>
    public void AddCamera(Point p)
    {
        ScaleGeometrySave(ref p);
     
        _entryPoint.Add((float) p.X);
        _entryPoint.Add((float) p.Y);
    }

    /// <summary>
    /// Add a door to a plan
    /// </summary>
    /// <param name="point1">Door first point</param>
    /// <param name="point2">Door second point</param>
    /// <returns>Created door object</returns>
    public Door AddDoor(Point point1, Point point2)
    {
        ScaleGeometrySave(ref point1);
        ScaleGeometrySave(ref point2);

        Door.NbDoor++;
        List<float> start = new List<float>() { (float)point1.X, (float)point1.Y };
        List<float> stop = new List<float>() { (float)point2.X, (float)point2.Y };

        Door door = new Door(DOOR_STRING + Door.NbDoor, start, stop);

        _doors.Add(door);

        return door;
    }

    /// <summary>
    /// Import plan content. Draw the plan content on the canvas.
    /// Loop throught each plan List (Door, Area, Wall) an entry point to create the plan
    /// </summary>
    /// <param name="receiver">Receiver object</param>
    /// <param name="invoker">Invoker object</param>
    public void ImportDraw(Receiver receiver, Invoker invoker)
    {
        ImportCamera(receiver, invoker);
        Area.ImportAreas(_areas, receiver, invoker, ScaleGeometryLoad);
        Segment.ImportSegments(_segments, receiver, invoker, ScaleGeometryLoad);
        HouseWindow.ImportWindows(ListWindows(), receiver, invoker, ScaleGeometryLoad);
        Door.ImportDoors(_doors, receiver, invoker, ScaleGeometryLoad);
    }

    /// <summary>
    /// Import camera in canvas
    /// </summary>
    /// <param name="receiver">Receiver object</param>
    /// <param name="invoker">Invoker object</param>
    private void ImportCamera(Receiver receiver, Invoker invoker)
    {
        invoker.DrawCommand = new CameraCommand(receiver);
        if (_entryPoint.Count != 0)
        {
            Point cameraPoint = ScaleGeometryLoad(new Point(_entryPoint[0], _entryPoint[1]));
            invoker.InvokeClick(cameraPoint);
        }
    }

    /// <summary>
    /// Remove an object in a plan
    /// </summary>
    /// <param name="obj">Object from all type</param>
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

    /// <summary>
    /// Remove a window from a plan
    /// </summary>
    /// <param name="w">HouseWindow object</param>
    public void RemoveWindow(HouseWindow w)
    {
        List<HouseWindow> hw = ListWindows();
        
        int index = hw.IndexOf(w);
        if(index >= 0)
            _segments[index].Window = new HouseWindow();
    }

    /// <summary>
    /// Create a list of all windows
    /// </summary>
    /// <returns>All windows in the plan</returns>
    public List<HouseWindow> ListWindows()
    {
        List<HouseWindow> hw = new List<HouseWindow>();

        foreach(Segment s in _segments)
        {
            if(s.Window.Start != null)
            {
                hw.Add(s.Window);
            }
        } 

        return hw;
    }

    /// <summary>
    /// Rescale a point from the grid ratio.
    /// The point is passed by ref.
    /// Must be used for save.
    /// </summary>
    /// <param name="point">Point to rescale</param>
    private void ScaleGeometrySave(ref Point point)
    {
        point.X /= _gridRatio;
        point.Y /= _gridRatio;
    }

    /// <summary>
    /// Rescale a point from the grid ratio.
    /// Must be used for load.
    /// </summary>
    /// <param name="point">Point to rescale</param>
    /// <returns>New rescaled point</returns>
    private Point ScaleGeometryLoad(Point point)
    {
        Point canvasScaledPoint = new Point();

        canvasScaledPoint.X = point.X * _gridRatio;
        canvasScaledPoint.Y = point.Y * _gridRatio;

        return canvasScaledPoint;
    }
}
