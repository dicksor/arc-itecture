/*
 * ARC-Itecture
 * Romain Capocasale, Vincent Moulin and Jonas Freiburghaus
 * He-Arc, INF3dlm-a
 * 2019-2020
 * .NET Course
 */

using ARC_Itecture;
using ARC_Itecture.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
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
}
