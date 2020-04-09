using ARC_Itecture;
using Newtonsoft.Json;
using System.Collections.Generic;

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

}
