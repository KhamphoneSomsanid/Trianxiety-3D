using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolMenuDialog : MonoBehaviour
{
    public Image imgDialogMenu;
    public GameObject btnYes;
    public GameObject btnNo;
    public Text lblTitle;
    public Text lblDetail;
    public GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void hide() {        
        imgDialogMenu.enabled = false;
        btnYes.SetActive(false);
        btnNo.SetActive(false);
        lblTitle.enabled = false;
        lblDetail.enabled = false;
    }

    public void show() {
        imgDialogMenu.enabled = true;
        btnYes.SetActive(true);
        btnNo.SetActive(true);
        lblTitle.enabled = true;
        lblDetail.enabled = true;
    }

    public void onClick(string tag) {
        switch (tag) {
            case "btn_yes":
                gameManager.onClickDialogYes();
            break;
            case "btn_no":
                gameManager.onClickDialogNo();
            break;
        }
    }

}
