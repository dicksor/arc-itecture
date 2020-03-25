using System.Collections.Generic;

[System.Serializable]
public class Plan
{
    public float wallHeight;
    public float wallWidth;
    public float windowH1;
    public float windowH2;
    public float doorH2;
    public List<float> entryPoint;
    public List<Segment> segments;
    public List<Area> areas;
    public List<Door> doors;
}
