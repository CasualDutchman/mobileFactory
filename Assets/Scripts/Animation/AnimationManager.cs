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

    public void Register(SpriteRenderer render, MachineTile machine) {
        bool hasRegistered = false;
        foreach(AnimationInfo i in info) {
            if (i.machine == machine) {
                i.renderers.Add(render);
                hasRegistered = true;
                break;
            }
        }

        if (!hasRegistered) {
            AnimationInfo ai = new AnimationInfo() {
                machine = machine,
                speed = 15
            };
            ai.renderers.Add(render);
            info.Add(ai);
        }
    }

    public void Remove(SpriteRenderer sr) {
        AnimationInfo needsDeleting = null;
        foreach (AnimationInfo i in info) {
            if (i.renderers.Contains(sr)) {
                i.renderers.Remove(sr);
                if(i.renderers.Count <= 0) {
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
    public MachineTile machine;
    public List<SpriteRenderer> renderers = new List<SpriteRenderer>();
    int index;
    public float timer;
    public float speed;

    public void AddChangeRenders() {
        index++;
        if (index >= machine.machine.machineAnimation.Working(machine.direction).Length)
            index = 0;

        foreach(SpriteRenderer sr in renderers) {
            sr.sprite = machine.machine.machineAnimation.Working(machine.direction)[index];
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