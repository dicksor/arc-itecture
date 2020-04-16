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

    public static int NbDoor = 0;

    public Door(string name, List<float> start, List<float> stop)
    {
        this.Name = name;
        this.Start = start;
        this.Stop = stop;
        this.IsFrontDoor = false;
    }
}
