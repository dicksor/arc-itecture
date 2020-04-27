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
using System.Runtime.Serialization;
using System.Windows;

[DataContract(Name = "window")]
[System.Serializable]
public class HouseWindow
{
    [JsonProperty("start")]
    public List<float> Start { get; set; }

    [JsonProperty("stop")]
    public List<float> Stop { get; set; }

    public HouseWindow()
    {

    }

    public HouseWindow(List<float> start, List<float> stop)
    {
        this.Start = start;
        this.Stop = stop;
    }

    /// <summary>
    /// Import windows in the canvas
    /// </summary>
    /// <param name="doors">Windows list</param>
    /// <param name="receiver">Receiver object</param>
    /// <param name="invoker">Invoker object</param>
    /// <param name="scaleGeometryLoad">Scale geometry load function</param>
    public static void ImportWindows(List<HouseWindow> windows, Receiver receiver, Invoker invoker, Func<Point, Point> scaleGeometryLoad)
    {
        foreach (HouseWindow hw in windows.ToArray())
        {
            invoker.PreviewCommand = new PreviewWindowCommand(receiver);
            invoker.DrawCommand = new WindowCommand(receiver);

            Point windowStartPoint = scaleGeometryLoad(new Point(hw.Start[0], hw.Start[1]));
            invoker.InvokeClick(windowStartPoint);
            invoker.InvokeMouseMove(windowStartPoint);

            Point windowEndPoint = scaleGeometryLoad(new Point(hw.Stop[0], hw.Stop[1]));
            invoker.InvokeMouseMove(windowEndPoint);
            invoker.InvokeClick(windowEndPoint);
        }
    }
}
