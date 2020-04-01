using System.Collections.Generic;

[System.Serializable]
public class Segment
{
    public Segment(string name, List<float> start, List<float> stop)
    {
        this.name = name;
        this.start = start;
        this.stop = stop;
    }

    public static int nbSegment = 0;

    public string name { get; set; }
    public List<float> start { get; set; }
    public List<float> stop { get; set; }
    public HouseWindow window { get; set; }
}
