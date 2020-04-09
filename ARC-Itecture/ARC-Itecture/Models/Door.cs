using ARC_Itecture;
using System.Collections.Generic;

[System.Serializable]
public class Door
{
    public string Name { get; set; }

    public bool IsFrontDoor { get; set; }
    public List<float> Start { get; set; }
    public List<float> Stop { get; set; }

}
