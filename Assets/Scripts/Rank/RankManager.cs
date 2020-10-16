using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using UnityEngine.UI;


public class RankManager : MonoBehaviour
{

    public Image imgDialogBack;
    public Text lblDialogTitle;
    public Text lblDialogDesc;
    public GameObject btnDialogGot;
    public Image imgDialogTouch;

    // Start is called before the first frame update
    void Start()
    {
        hideDialogView();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void hideDialogView() {
        imgDialogTouch.enabled = false;
        imgDialogBack.enabled = false;
        btnDialogGot.SetActive(false);
        lblDialogTitle.enabled = false;
        lblDialogDesc.enabled = false;
    }

    void showDialogView() {
        imgDialogTouch.enabled = true;
        imgDialogBack.enabled = true;
        btnDialogGot.SetActive(true);
        lblDialogTitle.enabled = true;
        lblDialogDesc.enabled = true;
    }

    public void onShowDialogView() {
        showDialogView();
    }

    public void onHideDialogView() {
        hideDialogView();
    }

}
