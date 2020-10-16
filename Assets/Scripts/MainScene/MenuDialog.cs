using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuDialog : MonoBehaviour
{
    public Image imgStartMenu;
    public GameObject btnStart;
    public GameObject btnRank;
    public GameObject btnRules;
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
        imgStartMenu.enabled = false;
        btnStart.SetActive(false);
        btnRank.SetActive(false);
        btnRules.SetActive(false);
    }

    public void show() {
        imgStartMenu.enabled = true;
        btnStart.SetActive(true);
        btnRank.SetActive(true);
        btnRules.SetActive(true);
    }

    public void onClick(string tag) {
        switch (tag) {
            case "btn_start":
                gameManager.setGameStatus(GameManager.Type.Start);
            break;
            case "btn_rank":
                UnityEngine.SceneManagement.SceneManager.LoadScene("Ranks");
            break;
            case "btn_rule":
                UnityEngine.SceneManagement.SceneManager.LoadScene("Rules");
            break;
        }
    }

}
