using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackButton : MonoBehaviour
{

    public int tag;

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onClick() {
        switch (tag) {
            case 2:
                UnityEngine.SceneManagement.SceneManager.LoadScene("Ranks");
            break;
        }
    }

}
