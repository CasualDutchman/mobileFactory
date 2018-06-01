using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helper  {

    public static Direction GetNextDirection(Direction dir) {
        switch (dir) {
            default: case Direction.North: return Direction.East;
            case Direction.South: return Direction.West;
            case Direction.East: return Direction.South;
            case Direction.West: return Direction.North;
        }
    }

    public static Direction GetPrevDirection(Direction dir) {
        switch (dir) {
            default: case Direction.North: return Direction.West;
            case Direction.South: return Direction.East;
            case Direction.East: return Direction.North;
            case Direction.West: return Direction.South;
        }
    }
}
