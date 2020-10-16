using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public delegate void OnClickPreViewButton();

public class EndedDialog : MonoBehaviour
{

    public Image imgEndMenu;
    public Image imgHandle;
    public GameObject btnPrev;
    public GameObject btnBack;
    public GameObject btnEndRank;
    public GameObject btnEndWatch;
    public GameObject btnEndStop;
    public Text lblSqure;
    public Text lblMoves;
    public Text lblMoveBy;
    public GameManager gameManager;

    private float timeWatchDelay = 0.0f;
    private int watchIndex = 0;
    private int moveCnt = 0;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void hide() {
        imgEndMenu.enabled = false;
        imgHandle.enabled = false;
        btnPrev.SetActive(false);
        btnBack.SetActive(false);
        btnEndStop.SetActive(false);
        btnEndRank.SetActive(false);
        btnEndWatch.SetActive(false);
        lblSqure.enabled = false;
        lblMoves.enabled = false;
        lblMoveBy.enabled = false;
    }

    public void show() {
        imgEndMenu.enabled = true;
        btnPrev.SetActive(true);
        btnEndRank.SetActive(true);
        // btnEndWatch.SetActive(true);
        lblSqure.enabled = true;
        lblMoves.enabled = true;
        lblMoveBy.enabled = true;
    }

    public void setMoveText(string str) {
        lblMoves.text = str;
    }

    public void setSquareText(string str) {
        lblSqure.text = str;
    }

    public void onClick(string tag) {
        switch (tag) {
            case "btn_rank":
                UnityEngine.SceneManagement.SceneManager.LoadScene("Ranks");
            break;
            case "btn_watch":
                imgHandle.enabled = true;
                btnEndWatch.SetActive(false);
                btnEndStop.SetActive(true);
                btnEndRank.SetActive(false);
                btnPrev.SetActive(false);
                btnBack.SetActive(false);
                lblMoveBy.enabled = false;
                gameManager.onWatchClick();
            break;
            case "btn_stop":
                imgHandle.enabled = false;
                btnEndWatch.SetActive(true);
                btnEndStop.SetActive(false);
                btnEndRank.SetActive(true);
                btnPrev.SetActive(true);
                btnBack.SetActive(false);
                lblMoveBy.enabled = true;
                gameManager.onStopClick();
            break;
            case "btn_prev":
                gameManager.onShowPrev();
            break;
            case "btn_next":
                gameManager.onShowNext();
            break;
        }
    }

}
