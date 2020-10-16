using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Unity.Editor;
using UnityEngine.EventSystems;
using Firebase;
using Firebase.Auth;


public class HistoryCell : MonoBehaviour, IPointerClickHandler
{
    public Image imgBackground;
    public Text lblRank;
    public Text lblDate;
    public Text lblMoves;
    public Text lblSqures;
    public Text lblDuring;

    private Game data;
    private string strAllRank = "";
    private string strSelfRank = "";
    private string strDate = "";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void hide() {
        imgBackground.enabled = false;
        lblRank.enabled = false;
        lblMoves.enabled = false;
        lblDate.enabled = false;
        lblSqures.enabled = false;
        lblDuring.enabled = false;
    }

    public void show() {
        imgBackground.enabled = true;
        lblRank.enabled = true;
        lblMoves.enabled = true;
        lblDate.enabled = true;
        lblSqures.enabled = true;
        lblDuring.enabled = true;
    }

    public void setData(Game gameData, int allIndex, int selfIndex, bool isSelf) {
        data = gameData;
        int showIndex = -1;
        if (isSelf) {
            showIndex = selfIndex;
        } else {
            showIndex = allIndex;
        }
        if (gameData.matchCount > 0) {
            lblRank.text = "# " + (showIndex + 1);
            strAllRank = "# " + (allIndex + 1);
            strSelfRank = "# " + (selfIndex + 1);
        } else {            
            lblRank.text = "No Rank";
            strAllRank = "No Rank";
            strSelfRank = "No Rank";
        }
        
        lblDate.text = "Date : " + gameData.startDate;
        strDate = gameData.startDate;
        lblMoves.text = "Moves : " + (gameData.getTriHistories().Count - 1);
        lblSqures.text = "Squares : " + gameData.matchCount + " / 18";
        lblDuring.text = "Duration : " + ((gameData.during / 10) / 60).ToString("00")  + " : " + ((gameData.during / 10) % 60).ToString("00");

        FirebaseAuth auth = FirebaseAuth.DefaultInstance;
        FirebaseUser currentUser = auth.CurrentUser;
        string userid = currentUser.UserId;
        if (userid != gameData.userId) {
            GetComponent<Image>().color = new Color32(255,255,255,255);
            lblRank.GetComponent<Text>().color = new Color32(0,0,0,255);
            lblDate.GetComponent<Text>().color = new Color32(0,0,0,255);
            lblMoves.GetComponent<Text>().color = new Color32(0,0,0,255);
            lblSqures.GetComponent<Text>().color = new Color32(0,0,0,255);
            lblDuring.GetComponent<Text>().color = new Color32(0,0,0,255);
            strSelfRank = "No Rank";
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        PlayerPrefs.SetString("historyid", data.id);
        PlayerPrefs.SetString("allrank", strAllRank);
        PlayerPrefs.SetString("selfrank", strSelfRank);
        PlayerPrefs.SetString("date", strDate);
        UnityEngine.SceneManagement.SceneManager.LoadScene("History");
    }

}
