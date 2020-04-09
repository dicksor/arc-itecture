using ARC_Itecture;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Runtime.Serialization;

[DataContract(Name = "HouseWindow")]
[System.Serializable]
public class HouseWindow
{
    [JsonProperty("start")]
    public List<float> Start { get; set; }

    [JsonProperty("stop")]
    public List<float> Stop { get; set; }

    public HouseWindow(List<float> start, List<float> stop)
    {
        this.Start = start;
        this.Stop = stop;
    }
}
