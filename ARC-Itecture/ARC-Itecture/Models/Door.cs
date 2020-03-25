using System.Collections.Generic;

[System.Serializable]
public class Door
{
    public string name { get; set; }

    public bool isFrontDoor { get; set; }
    public List<float> start { get; set; }
    public List<float> stop { get; set; }
}
