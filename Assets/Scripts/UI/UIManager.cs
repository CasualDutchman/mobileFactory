using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Screens { Hud, Shop, Select }

public class UIManager : MonoBehaviour {

    public Factory factory;

    public Screens currentScreen = Screens.Hud;
    Transform currentShow;

    public RectTransform clipBoard;
    public Transform upperLayer, downLayer;
    public Transform uiSelect, uiShop;

    Animation anim;

    public RectTransform shopScroll;
    public GameObject shopItem;

    public Image selectionIcon;
    public Text selectionName;

	void Start () {
        anim = clipBoard.GetComponent<Animation>();
    }
	
	void Update () {
		
	}

    public void SetupShop(Machine[] list) {
        shopScroll.sizeDelta = new Vector2((250 * list.Length) + (20 * (list.Length - 1)), 300);
        for (int i = 0; i < list.Length; i++) {
            int k = i;
            GameObject go = Instantiate(shopItem, shopScroll);
            go.GetComponent<Button>().onClick.AddListener(() => factory.AddTile(k));
            go.transform.GetChild(0).GetComponent<Image>().sprite = list[i].icon;
            go.transform.GetChild(1).GetComponent<Text>().text = list[i].machineName;
            go.transform.GetChild(2).GetComponent<Text>().text = "$" + list[i].price + ".000";
        }
    }

    public void SelectMachine(Machine machine) {
        selectionIcon.sprite = machine.icon;
        selectionName.text = machine.machineName;
    }

    public void ChangeScreen(Screens screen) {
        if (currentScreen == screen)
            return;

        if (currentShow != null && screen != Screens.Hud) {
            currentShow.SetParent(upperLayer);
            currentShow.localPosition = Vector3.zero;
            currentShow.localEulerAngles = Vector3.zero;
        }

        if (screen == Screens.Select) {
            SelectUI(uiSelect);
        }
        else if (screen == Screens.Shop) {
            SelectUI(uiShop);
        }

        if (currentScreen == Screens.Hud) {
            anim.Play("up");
        }
        else if (screen == Screens.Hud) {
            anim.Play("down");

            StartCoroutine(OnEndAnimation(1));

            //Hide(uiShop);
            //Hide(uiSelect);

            currentShow = null;
        } 
        else {
            anim.Play("rip");
            StartCoroutine(OnEndAnimation(0));
        }

        currentScreen = screen;
    }

    IEnumerator OnEndAnimation(int i) {
        bool playing = true;
        yield return new WaitForEndOfFrame();
        while (playing) {
            if (!anim.isPlaying) {
                if (i == 0 && upperLayer.GetChild(0) != null) {
                    Hide(upperLayer.GetChild(0));
                    playing = false;
                }
                if (i == 1 && downLayer.GetChild(0) != null) {
                    Hide(downLayer.GetChild(0));
                    playing = false;
                }
            }
            yield return new WaitForEndOfFrame();
        }
    }

    void SelectUI(Transform t) {
        t.SetParent(downLayer);
        t.localPosition = Vector3.zero;
        t.localEulerAngles = Vector3.zero;
        currentShow = t;
        t.gameObject.SetActive(true);
    }

    void Hide(Transform t) {
        t.SetParent(clipBoard);
        t.gameObject.SetActive(false);
    }
}
