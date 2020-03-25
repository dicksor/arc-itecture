using System.Collections.Generic;
using System.Runtime.Serialization;

[DataContract(Name = "Window")]
[System.Serializable]
public class HouseWindow
{
    public List<float> start { get; set; }
    public List<float> stop { get; set; }
}
