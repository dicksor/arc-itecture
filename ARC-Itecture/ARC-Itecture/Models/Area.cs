using System.Collections.Generic;
using System.Linq;
using System;
using System.Drawing;


[System.Serializable]
public class Area
{
    public Area(string type, List<List<float>> points)
    {
        this.type = type;
        this.points = points;
        isLightOn = false;
    }

    public string type { get; set; }
    public List<List<float>> points { get; set; }
    public Boolean isLightOn { get; set; }

    /// <summary>
    /// Return the X and Z minimum and maximum point of an area
    /// </summary>
    /// <returns>Tuple of PointF that contain the minimum and the maximum point</returns>
    public Tuple<PointF, PointF> GetMinMaxPoints()
    {
        List<float> minPoints = this.points.SelectMany(x => x.Select((v, i) => new { v, i }))
                 .GroupBy(x => x.i, x => x.v)
                 .OrderBy(g => g.Key)
                 .Select(g => g.Min())
                 .ToList();
        float minX = minPoints[0];
        float minY = minPoints[1];

        List<float> maxPoints = this.points.SelectMany(x => x.Select((v, i) => new { v, i }))
             .GroupBy(x => x.i, x => x.v)
             .OrderBy(g => g.Key)
             .Select(g => g.Max())
             .ToList();

        float maxX = maxPoints[0];
        float maxY = maxPoints[1];

        return new Tuple<PointF, PointF>(new PointF(minX, minY), new PointF(maxX, maxY));
    }
}
