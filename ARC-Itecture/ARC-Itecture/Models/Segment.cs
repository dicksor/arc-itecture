using System.Collections.Generic;

[System.Serializable]
public class Segment
{
    public static int nbSegment = 0;

    public string Name { get; set; }
    public List<float> Start { get; set; }
    public List<float> Stop { get; set; }
    public HouseWindow Window { get; set; }

    public Segment(string name, List<float> start, List<float> stop)
    {
        this.Name = name;
        this.Start = start;
        this.Stop = stop;
    }
}
