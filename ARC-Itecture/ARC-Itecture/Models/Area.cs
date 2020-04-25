/*
 * ARC-Itecture
 * Romain Capocasale, Vincent Moulin and Jonas Freiburghaus
 * He-Arc, INF3dlm-a
 * 2019-2020
 * .NET Course
 */

using System.Collections.Generic;
using System.Linq;
using System;
using System.Drawing;
using ARC_Itecture.DrawCommand;
using ARC_Itecture.DrawCommand.Commands;

[System.Serializable]
public class Area
{
    public string Type { get; set; }
    public List<List<float>> Points { get; set; }
    public Boolean IsLightOn { get; set; }
    
    public Area(string type, List<List<float>> points)
    {
        this.Type = type;
        this.Points = points;
        IsLightOn = false;
    }

    /// <summary>
    /// Return the X and Z minimum and maximum point of an area
    /// </summary>
    /// <returns>Tuple of PointF that contain the minimum and the maximum point</returns>
    public Tuple<PointF, PointF> GetMinMaxPoints()
    {
        List<float> minPoints = this.Points.SelectMany(x => x.Select((v, i) => new { v, i }))
                 .GroupBy(x => x.i, x => x.v)
                 .OrderBy(g => g.Key)
                 .Select(g => g.Min())
                 .ToList();
        float minX = minPoints[0];
        float minY = minPoints[1];

        List<float> maxPoints = this.Points.SelectMany(x => x.Select((v, i) => new { v, i }))
             .GroupBy(x => x.i, x => x.v)
             .OrderBy(g => g.Key)
             .Select(g => g.Max())
             .ToList();

        float maxX = maxPoints[0];
        float maxY = maxPoints[1];

        return new Tuple<PointF, PointF>(new PointF(minX, minY), new PointF(maxX, maxY));
    }

    /// <summary>
    /// Import areas in the canvas
    /// </summary>
    /// <param name="doors">Areas list</param>
    /// <param name="receiver">Receiver object</param>
    /// <param name="invoker">Invoker object</param>
    /// <param name="scaleGeometryLoad">Scale geometry load function</param>
    public static void ImportAreas(List<Area> areas, Receiver receiver, Invoker invoker, Func<System.Windows.Point, System.Windows.Point> scaleGeometryLoad)
    {
        foreach (Area area in areas)
        {
            invoker.DrawCommand = new AreaCommand(receiver, area.Type);
            invoker.PreviewCommand = new PreviewAreaCommand(receiver);

            Tuple<PointF, PointF> minMaxPoints = area.GetMinMaxPoints();

            System.Windows.Point areaTopLeftPoint = scaleGeometryLoad(new System.Windows.Point(minMaxPoints.Item1.X, minMaxPoints.Item1.Y));
            invoker.InvokeClick(areaTopLeftPoint);
            invoker.InvokeMouseMove(areaTopLeftPoint);

            System.Windows.Point areaBottomRightPoint = scaleGeometryLoad(new System.Windows.Point(minMaxPoints.Item2.X, minMaxPoints.Item2.Y));
            invoker.InvokeMouseMove(areaBottomRightPoint);
            invoker.InvokeClick(areaBottomRightPoint);
        }
    }
}
