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
using System.Windows;

[System.Serializable]
public class Segment
{
    public static int nbSegment = 0;

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("start")]
    public List<float> Start { get; set; }

    [JsonProperty("stop")]
    public List<float> Stop { get; set; }

    [JsonProperty("window")]
    public HouseWindow Window { get; set; }

    public Segment(string name, List<float> start, List<float> stop, HouseWindow hw)
    {
        this.Name = name;
        this.Start = start;
        this.Stop = stop;
        this.Window = hw;
    }

    /// <summary>
    /// Allow to find a segment from its coordinates
    /// </summary>
    /// <param name="p1">Segment first point</param>
    /// <param name="p2">Segment second point</param>
    /// <returns>The finded segment, null otherwise</returns>
    public Segment FindSegmentByCoord(Point p1, Point p2)
    {
        Segment s = null;

            if(Math.Floor(p1.X) == Math.Floor(Start[0]) && 
               Math.Floor(p1.Y) == Math.Floor(Start[1]) && 
               Math.Floor(p2.X) == Math.Floor(Stop[0]) && 
               Math.Floor(p2.Y) == Math.Floor(Stop[1]))
            {
                s = this;
            }

        return s;
    }

    /// <summary>
    /// Import segments in the canvas
    /// </summary>
    /// <param name="doors">Segments list</param>
    /// <param name="receiver">Receiver object</param>
    /// <param name="invoker">Invoker object</param>
    /// <param name="scaleGeometryLoad">Scale geometry load function</param>
    public static void ImportSegments(List<Segment> segments, Receiver receiver, Invoker invoker, Func<Point, Point> scaleGeometryLoad)
    {
        invoker.DrawCommand = new WallCommand(receiver);
        foreach (Segment segment in segments.ToArray())
        {
            Point wallStartPoint = scaleGeometryLoad(new Point(segment.Start[0], segment.Start[1]));
            Point wallEndPoint = scaleGeometryLoad(new Point(segment.Stop[0], segment.Stop[1]));

            invoker.InvokeClick(wallStartPoint);
            invoker.InvokeClick(wallEndPoint);
            receiver.StartNewWall();
        }
    }
}
