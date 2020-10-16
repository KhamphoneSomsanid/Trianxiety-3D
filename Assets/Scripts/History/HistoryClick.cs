using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HistoryClick : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onClick(string tag) {
        switch (tag) {
            case "btn_start":
                GameManager.isDirectStart = true;
                UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene");
            break;
            case "btn_menu":
                GameManager.isDirectStart = false;
                UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene");
            break;
        }
    }

}
