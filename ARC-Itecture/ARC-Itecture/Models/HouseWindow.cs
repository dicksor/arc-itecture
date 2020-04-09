using ARC_Itecture;
using System.Collections.Generic;
using System.Runtime.Serialization;

[DataContract(Name = "Window")]
[System.Serializable]
public class HouseWindow
{
    public List<float> Start { get; set; }
    public List<float> Stop { get; set; }

    public HouseWindow(List<float> start, List<float> stop)
    {
        Start = start;
        Stop = stop;
    }
}
