using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour {

    public static AnimationManager instance;

    List<AnimationInfo> info = new List<AnimationInfo>();

    void Awake() {
        instance = this;
    }

	void Start () {
		
	}
	
	void Update () {
		foreach(AnimationInfo i in info) {
            i.timer += Time.deltaTime * i.speed;
            if (i.timer >= 1) {
                i.AddChangeRenders();
            }
        }
	}

    public void Register(MachineTile machine) {
        bool hasRegistered = false;
        foreach(AnimationInfo i in info) {
            if (i.machines[0].machine.machineName == machine.machine.machineName) {
                i.machines.Add(machine);
                hasRegistered = true;
                break;
            }
        }

        if (!hasRegistered) {
            AnimationInfo ai = new AnimationInfo() {
                speed = 15
            };
            ai.machines.Add(machine);
            info.Add(ai);
        }
    }

    public void Remove(MachineTile mt) {
        AnimationInfo needsDeleting = null;
        foreach (AnimationInfo i in info) {
            if (i.machines.Contains(mt)) {
                i.machines.Remove(mt);
                if(i.machines.Count <= 0) {
                    needsDeleting = i;
                }
                break;
            }
        }

        if (needsDeleting != null) {
            info.Remove(needsDeleting);
        }
    }
}

[System.Serializable]
public class AnimationInfo {
    public List<MachineTile> machines = new List<MachineTile>();
    //public List<SpriteRenderer> renderers = new List<SpriteRenderer>();
    //public List<Direction> directions = new List<Direction>();
    int index;
    public float timer;
    public float speed;

    public void AddChangeRenders() {
        index++;
        if (index >= machines[0].machine.machineAnimation.Working(Direction.East).Length)
            index = 0;

        for (int i = 0; i < machines.Count; i++) {
            machines[i].spriteRenderer.sprite = machines[i].machine.machineAnimation.Working(machines[i].direction)[index];
        }

        timer = 0;
    }
}

[System.Serializable]
public class MachineAnimation {
    public Sprite idleNorth, idleSouth, idleEast, idleWest;
    public Sprite[] workingNorth, workingSouth, workingEast, workingWest;

    public Sprite Idle(Direction dir) {
        switch (dir) {
            default: case Direction.North: return idleNorth;
            case Direction.South: return idleSouth;
            case Direction.East: return idleEast;
            case Direction.West: return idleWest;
        }
    }

    public Sprite[] Working(Direction dir) {
        switch (dir) {
            default: case Direction.North: return workingNorth;
            case Direction.South: return workingSouth;
            case Direction.East: return workingEast;
            case Direction.West: return workingWest;
        }
    }
}