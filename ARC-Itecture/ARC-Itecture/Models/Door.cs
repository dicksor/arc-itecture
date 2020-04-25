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

    public static int NbDoor = 0; // Number of door in the plan 

    public Door(string name, List<float> start, List<float> stop)
    {
        this.Name = name;
        this.Start = start;
        this.Stop = stop;
        this.IsFrontDoor = false;
    }

    /// <summary>
    /// Import doors in the canvas
    /// </summary>
    /// <param name="doors">Doors list</param>
    /// <param name="receiver">Receiver object</param>
    /// <param name="invoker">Invoker object</param>
    /// <param name="scaleGeometryLoad">Scale geometry load function</param>
    public static void ImportDoors(List<Door> doors, Receiver receiver, Invoker invoker, Func<Point, Point> scaleGeometryLoad)
    {
        foreach (Door door in doors)
        {
            invoker.DrawCommand = new DoorCommand(receiver);
            invoker.PreviewCommand = new PreviewDoorCommand(receiver);

            Point doorStart = scaleGeometryLoad(new Point(door.Start[0], door.Start[1]));
            invoker.InvokeClick(doorStart);
            invoker.InvokeMouseMove(doorStart);

            Point doorEnd = scaleGeometryLoad(new Point(door.Stop[0], door.Stop[1]));
            invoker.InvokeMouseMove(doorEnd);
            invoker.InvokeClick(doorEnd);
        }
    }
}
