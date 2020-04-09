using ARC_Itecture;
using System.Collections.Generic;
using System.Runtime.Serialization;

[DataContract(Name = "Window")]
[System.Serializable]
public class HouseWindow:IDrawComponent
{
    public List<float> Start { get; set; }
    public List<float> Stop { get; set; }

    public string GetName()
    {
        throw new System.NotImplementedException();
    }

    public void RemoveComponent()
    {
        throw new System.NotImplementedException();
    }
}
