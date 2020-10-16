using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ToggleClick : MonoBehaviour, IPointerClickHandler
    , IBeginDragHandler, IDragHandler, IEndDragHandler
    , IPointerDownHandler
{
    public GameManager gameManager;
    public GameObject gameSelector;
    
    private Vector3 screenPoint;
    private Vector3 offset;

    bool isDrag = false;

    // Start is called before the first frame update
    void Start()
    {
        addPhysicsRaycaster();
    }

    void addPhysicsRaycaster()
    {
        PhysicsRaycaster physicsRaycaster = GameObject.FindObjectOfType<PhysicsRaycaster>();
        if (physicsRaycaster == null)
        {
            Camera.main.gameObject.AddComponent<PhysicsRaycaster>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isDrag) {
            return;
        }
        if (gameManager.getGameStatus() != GameManager.Type.Running) {
            return;
        }
        if (gameManager.getRotateDirection() == 1) {
            gameManager.setRotateDirection(0);
        } else {
            gameManager.setRotateDirection(1);
        }
        Effect.onRunVibrate();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (gameManager.getGameStatus() != GameManager.Type.Running) {
            return;
        }
        Effect.onRunVibrate();
        isDrag = true;
        gameManager.imgHoverToggle.transform.position = new Vector3(0.0f, 0.006f, 0.0f);
        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (gameManager.getGameStatus() != GameManager.Type.Running) {
            return;
        }
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
        // gameSelector.transform.position = curPosition + new Vector3(0.0f, 0.02f, 0.0f);

        gameManager.onDragToggle(curPosition);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (gameManager.getGameStatus() != GameManager.Type.Running) {
            return;
        }
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
        if (eventData.pointerCurrentRaycast.gameObject == null) {
            Debug.Log("Drag End" + curPosition);
        }
        isDrag = false;
        // gameSelector.transform.position = new Vector3(0.0f, 0.00f, 0.0f);

        gameManager.onDragDownToggle(curPosition);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        gameManager.onDragToggleBegin();
    }

}
