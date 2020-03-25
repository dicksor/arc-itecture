using System.Collections.Generic;
using System.Linq;
using System;
using System.Drawing;


[System.Serializable]
public class Area
{
    public string type { get; set; }
    public List<List<float>> points { get; set; }
    public Boolean isLightOn { get; set; }
}
