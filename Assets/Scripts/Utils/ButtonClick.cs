using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonClick : MonoBehaviour
{
    public GameManager gameManager;


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
                gameManager.onClickStartBtn();
            break;
            case "btn_menu":
                gameManager.onClickMenuBtn();
            break;
        }
    }
    
}
