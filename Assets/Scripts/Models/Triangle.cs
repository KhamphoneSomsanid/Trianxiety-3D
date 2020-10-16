using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class Triangle : MonoBehaviour
    , IPointerClickHandler
    , IBeginDragHandler
    , IDragHandler
    , IEndDragHandler
    , IPointerDownHandler
{
    public Material[] materials = new Material[6];

    private GameManager gameManager;
    private Group group;
    private bool isDrag = false;
    private int triIndex = 0;
    private Vector3 screenPoint;
    private Vector3 offset;
    private bool isHover = false;

    // Start is called before the first frame update
    void Start() {
        addPhysicsRaycaster();
    }

    private void addPhysicsRaycaster() {
        PhysicsRaycaster physicsRaycaster = GameObject.FindObjectOfType<PhysicsRaycaster>();
        if (physicsRaycaster == null)
        {
            Camera.main.gameObject.AddComponent<PhysicsRaycaster>();
        }
    }

    public void setMaterial(int color) {
        GetComponent<Renderer>().material = materials[color];
    }

    public void setParent(GameManager manager, Group parent) {
        gameManager = manager;
        group = parent;
    }

    public void setTriIndex(int index) {
        triIndex = index;
    }

    public void onFailAction(bool isflag) {
        if (isHover) return;
        if (isflag) Effect.onRunVibrate();
        isHover = true;
        transform.position += new Vector3(0.0f, 0.02f, 0.0f);
    }

    public void onRestartAction() {
        if (!isHover) return;
        isHover = false;
        transform.position += new Vector3(0.0f, -0.02f, 0.0f);
    }

    // Update is called once per frame
    void Update() {
        
    }
   
    // void OnMouseDown () {
    //     gameManager.restart();
    // }

    public void OnBeginDrag(PointerEventData eventData) {
        if (gameManager.getGameStatus() != GameManager.Type.Running) {
            return;
        }
        isDrag = true;
        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        offset = group.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
    }

    public void OnDrag(PointerEventData eventData) {
        if (gameManager.getGameStatus() != GameManager.Type.Running) {
            return;
        }
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
        // group.transform.position = curPosition + new Vector3(0.00f, 0.04f, 0.0f);
        gameManager.onDragGroup(group, group.groupIndex, curPosition);
    }

    public void OnEndDrag(PointerEventData eventData) {
        if (gameManager.getGameStatus() != GameManager.Type.Running) {
            return;
        }
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
        // group.transform.position = curPosition;
        isDrag = false;
        gameManager.onDragDownGroup(group, group.groupIndex, curPosition);
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (isDrag) {
            Debug.Log("Drag");
            return;
        }
        if (gameManager.getGameStatus() != GameManager.Type.Running) {
            Debug.Log("Running");
            return;
        }
        Debug.Log("Rotate");
        if (gameManager.getRotateDirection() == 1) {
            group.onRatateLeft();
        } else {
            group.onRatateRight();
        }
        // Effect.onRunVibrate();
        gameManager.onPlaySoundClick();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        gameManager.onDragGroupBegin(group.groupIndex);
    }
    
}
