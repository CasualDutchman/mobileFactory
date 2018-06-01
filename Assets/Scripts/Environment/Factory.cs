using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;

public class Factory : MonoBehaviour {

    public UIManager uiManager;

    MachineTile[,] machineList;

    public EventSystem eventSystem;

    public Vector2Int gridSize;
    public Tilemap layer1, layer2, layerGame;
    public TileBase floorTile, decalTile;

    public GameObject spriter;
    public Machine[] machines;

    bool selected = false;
    Vector2Int selecting = new Vector2Int(-1, -1);
    public GameObject selectionBox;

    void Start() {
        machineList = new MachineTile[gridSize.x, gridSize.y];

        uiManager.SetupShop(machines);

        Camera.main.transform.position = new Vector3(gridSize.x / 2, gridSize.y / 2, -10);
        GenerateLayer1();
        GenerateLayer2();
    }

    void GenerateLayer1() {
        for (int y = 0; y < gridSize.y; y++) {
            for (int x = 0; x < gridSize.x; x++) {
                layer1.SetTile(new Vector3Int(x, y, 0), floorTile);
            }
        }
    }

    void GenerateLayer2() {
        for (int y = 0; y < gridSize.y; y++) {
            for (int x = 0; x < gridSize.x; x++) {
                layer2.SetTile(new Vector3Int(x, y, 0), decalTile);
            }
        }
    }

    void Update() {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0) && !eventSystem.IsPointerOverGameObject()) {
            Vector3 mouseToWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Select(Mathf.FloorToInt(mouseToWorld.x), Mathf.FloorToInt(mouseToWorld.y));
        }
#endif
#if UNITY_ANDROID
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) {
            if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)) {
                Vector3 mouseToWorld = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                Select(Mathf.FloorToInt(mouseToWorld.x), Mathf.FloorToInt(mouseToWorld.y));
            }
        }
#endif
    }

    void Select(int x, int y) {
        selected = true;
        selecting = new Vector2Int(x, y);
        selectionBox.transform.position = new Vector3(x, y);
        selectionBox.SetActive(true);

        if(machineList[x, y] == null) {
            uiManager.ChangeScreen(Screens.Shop);
        } else {
            uiManager.SelectMachine(machineList[x, y].machine);
            uiManager.ChangeScreen(Screens.Select);
        }
    }

    public void Deselect() {
        selected = false;
        selecting = new Vector2Int(-1, -1);
        selectionBox.SetActive(false);
        uiManager.ChangeScreen(Screens.Hud);
    }

    public void AddTile(int i) {
        GameObject go = Instantiate(spriter);
        go.transform.position = new Vector3(selecting.x + 0.5f, selecting.y + 0.5f + ((machines[i].pixelHeight - 16) * ((1 / 16f) * 0.5f)), 0);

        SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
        sr.sortingOrder = gridSize.y - selecting.y;

        machineList[selecting.x, selecting.y] = new MachineTile() {
            machine = machines[i],
            direction = Direction.East,
            spriteRenderer = sr
        };

        AnimationManager.instance.Register(sr, machineList[selecting.x, selecting.y]);

        uiManager.ChangeScreen(Screens.Select);
        Select(selecting.x, selecting.y);
    }

    public void RotateClockwise() {
        machineList[selecting.x, selecting.y].direction = Helper.GetNextDirection(machineList[selecting.x, selecting.y].direction);
    }

    public void RotateCounterClockwise() {
        machineList[selecting.x, selecting.y].direction = Helper.GetPrevDirection(machineList[selecting.x, selecting.y].direction);
    }

    public void RemoveTile() {
        MachineTile mt = machineList[selecting.x, selecting.y];
        AnimationManager.instance.Remove(mt.spriteRenderer);

        Destroy(mt.spriteRenderer.gameObject);
        machineList[selecting.x, selecting.y] = null;

        Deselect();
    }

}

public enum Direction { North, South, East, West}

public class MachineTile {
    public Machine machine;
    public Direction direction;
    public SpriteRenderer spriteRenderer;
}
