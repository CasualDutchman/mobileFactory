using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Machine", menuName = "Machine/Machine", order = 1)]
public class Machine : ScriptableObject {

    public Sprite icon;
    public string machineName;
    public int price;

    public MachineAnimation machineAnimation;
    public int pixelHeight;
}
