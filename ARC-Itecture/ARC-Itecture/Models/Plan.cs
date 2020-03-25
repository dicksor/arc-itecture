using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;

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

}
