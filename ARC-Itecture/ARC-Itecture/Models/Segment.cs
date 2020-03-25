using System.Collections.Generic;

[System.Serializable]
public class Segment
{
    public string name { get; set; }
    public List<float> start { get; set; }
    public List<float> stop { get; set; }
    public HouseWindow window { get; set; }
}
