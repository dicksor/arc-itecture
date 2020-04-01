using System.Collections.Generic;
using System.Linq;
using System;
using System.Drawing;


[System.Serializable]
public class Area
{
    public Area(string type, List<List<float>> points)
    {
        this.type = type;
        this.points = points;
        isLightOn = false;
    }

    public string type { get; set; }
    public List<List<float>> points { get; set; }
    public Boolean isLightOn { get; set; }
}
