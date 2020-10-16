using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HistoryDialog : MonoBehaviour
{
    public Image imgHandle;
    public GameObject btnPrev;
    public GameObject btnBack;
    public GameObject btnEndRank;
    public GameObject btnEndWatch;
    public GameObject btnEndStop;
    public Text lblSqure;
    public Text lblMoves;
    public Text lblMoveBy;
    public History historyManager;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setSquareText(string str) {
        lblSqure.text = str;
    }

    public void setMoveText(string str) {
        lblMoves.text = str;
    }

    public void initWatchView() {
        imgHandle.enabled = false;
        btnBack.SetActive(false);
        btnEndStop.SetActive(false);
        btnEndWatch.SetActive(false);
        btnEndRank.SetActive(false);
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
                historyManager.onWatchClick();
            break;
            case "btn_stop":
                imgHandle.enabled = false;
                btnEndWatch.SetActive(true);
                btnEndStop.SetActive(false);
                btnEndRank.SetActive(true);
                btnPrev.SetActive(true);
                btnBack.SetActive(false);
                lblMoveBy.enabled = true;
                historyManager.onStopClick();
            break;
            case "btn_prev":
                historyManager.onShowPrev();
            break;
            case "btn_next":
                historyManager.onShowNext();
            break;
        }
    }

}
