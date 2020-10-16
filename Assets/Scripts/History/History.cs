using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;


public class History : MonoBehaviour
{
    public Text lblDuring;
    public Image imgDuring;
    public Image imgDirection1;
    public Image imgDirection2;
    public Image imgDirection3;
    public Image imgDirection4;
    public Image imgDirection5;
    public Image imgDirection6;
    public Image imgDirection7;
    public Image imgDirection8;
    public GameObject groupPrefab;
    public HistoryDialog historyDialog;
    public Text lblRank;
    public Text lblDate;

    private string key = "-MGSiIZkQSJaZf3QGEhZ";
    private Vector3[] groupPositions = {
        new Vector3(0.172f, 0.0f, 0.0f),
        new Vector3(0.129f, 0.0f, 0.043f),
        new Vector3(0.129f, 0.0f, -0.043f),
        new Vector3(0.086f, 0.0f, 0.086f),
        new Vector3(0.086f, 0.0f, 0.0f),
        new Vector3(0.086f, 0.0f, -0.086f),
        new Vector3(0.043f, 0.0f, 0.043f),
        new Vector3(0.043f, 0.0f, -0.043f),
        new Vector3(0.0f, 0.0f, 0.086f),
        new Vector3(0.0f, 0.0f, -0.086f),
        new Vector3(-0.043f, 0.0f, 0.043f),
        new Vector3(-0.043f, 0.0f, -0.043f),
        new Vector3(-0.086f, 0.0f, 0.086f),
        new Vector3(-0.086f, 0.0f, 0.0f),
        new Vector3(-0.086f, 0.0f, -0.086f),
        new Vector3(-0.129f, 0.0f, 0.043f),
        new Vector3(-0.129f, 0.0f, -0.043f),
        new Vector3(-0.172f, 0.0f, 0.0f),
    };
    private List<int[]> triShowColors = new List<int[]>();
    private int countSqure = 0;
    private int directionRotate = 1;
    private List<Group> groups;
    private float watchDelay = 0.3f;
    private int watchIndex;
    private Game watchGame;
    private List<int> watchTypes = new List<int>();
    private List<int> watchMoveStart = new List<int>();
    private List<int> watchMoveEnded = new List<int>();
    private List<int> watchHistoryTime = new List<int>();
    private List<int> watchRotateDirection = new List<int>();
    private List<List<int[]>> watchHistoryColors = new List<List<int[]>>();
    private int gameTime;

    public enum Type {
        Ready,
        Watch,
    };
    private Type historyStatus = Type.Ready;
    private int historyType = -1;
    private List<int> i11 = new List<int>();
    private List<int> i12 = new List<int>();
    private List<int> j11 = new List<int>();
    private List<int> j12 = new List<int>();
    private List<int[]> triEndPoint = new List<int[]>();


    // Start is called before the first frame update
    void Start()
    {
        initFirebase();

        triShowColors.Add(new int[]{5, 5, 5, 5});
        triShowColors.Add(new int[]{4, 4, 4, 4});
        triShowColors.Add(new int[]{3, 3, 3, 3});
        triShowColors.Add(new int[]{2, 2, 2, 2});
        triShowColors.Add(new int[]{1, 1, 1, 1});
        triShowColors.Add(new int[]{0, 0, 0, 0});
        triShowColors.Add(new int[]{5, 5, 5, 5});
        triShowColors.Add(new int[]{4, 4, 4, 4});
        triShowColors.Add(new int[]{3, 3, 3, 3});
        triShowColors.Add(new int[]{2, 2, 2, 2});
        triShowColors.Add(new int[]{1, 1, 1, 1});
        triShowColors.Add(new int[]{0, 0, 0, 0});
        triShowColors.Add(new int[]{5, 5, 5, 5});
        triShowColors.Add(new int[]{4, 4, 4, 4});
        triShowColors.Add(new int[]{3, 3, 3, 3});
        triShowColors.Add(new int[]{2, 2, 2, 2});
        triShowColors.Add(new int[]{1, 1, 1, 1});
        triShowColors.Add(new int[]{0, 0, 0, 0});

        triEndPoint = new List<int[]>();
        triEndPoint.Add(new int[]{0, 2, 1, 1});
        triEndPoint.Add(new int[]{0, 3, 2, 0});
        triEndPoint.Add(new int[]{1, 2, 3, 1});
        triEndPoint.Add(new int[]{1, 3, 4, 0});
        triEndPoint.Add(new int[]{2, 2, 4, 1});
        triEndPoint.Add(new int[]{2, 3, 5, 0});
        triEndPoint.Add(new int[]{3, 3, 6, 0});
        triEndPoint.Add(new int[]{4, 2, 6, 1});
        triEndPoint.Add(new int[]{4, 3, 7, 0});
        triEndPoint.Add(new int[]{5, 2, 7, 1});
        triEndPoint.Add(new int[]{6, 2, 8, 1});
        triEndPoint.Add(new int[]{7, 3, 9, 0});
        triEndPoint.Add(new int[]{8, 3, 10, 0});
        triEndPoint.Add(new int[]{9, 2, 11, 1});
        triEndPoint.Add(new int[]{10, 2, 12, 1});
        triEndPoint.Add(new int[]{10, 3, 13, 0});
        triEndPoint.Add(new int[]{11, 2, 13, 1});
        triEndPoint.Add(new int[]{11, 3, 14, 0});
        triEndPoint.Add(new int[]{12, 3, 15, 0});
        triEndPoint.Add(new int[]{13, 2, 15, 1});
        triEndPoint.Add(new int[]{13, 3, 16, 0});
        triEndPoint.Add(new int[]{14, 2, 16, 1});
        triEndPoint.Add(new int[]{15, 3, 17, 0});
        triEndPoint.Add(new int[]{16, 2, 17, 1});

        groups = new List<Group>();
        for (int i = 0; i < 18; ++i) {
            GameObject groupGameobject = Instantiate(groupPrefab, transform, false);
            Group group = groupGameobject.GetComponent<Group>();
            group.transform.localPosition = groupPositions[i];
            groups.Add(group);
        }
        showColor();

        historyDialog.initWatchView();
        lblDuring.color = Color.yellow;
    }

    void initFirebase() {
        key = PlayerPrefs.GetString("historyid");
        string strAllRank = PlayerPrefs.GetString("allrank");
        string strSelfRank = PlayerPrefs.GetString("selfrank");
        string strDate = PlayerPrefs.GetString("date");
        if (strAllRank == "No Rank" && strSelfRank == "No Rank") {
            lblRank.text = "No Rank";
        } else if (strAllRank == "No Rank") {
            lblRank.text = strSelfRank + " of your games";
        } else if (strSelfRank ==  "No Rank") {
            lblRank.text = strAllRank + " of all games";
        } else {
            lblRank.text = strSelfRank + " of your games\n" + strAllRank + " of all games";
        }
        
        lblDate.text = strDate;

        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://trianxiety.firebaseio.com/");
        FirebaseDatabase.DefaultInstance
            .GetReference("game")
            .Child(key)
            .GetValueAsync().ContinueWith(task => {
                if (task.IsFaulted) {
                // Handle the error...
                } else if (task.IsCompleted) {
                    DataSnapshot snapshot = task.Result;
                    watchGame = JsonUtility.FromJson<Game>(snapshot.GetRawJsonValue());
                    watchTypes = watchGame.getHistoryTypes();
                    watchMoveStart = watchGame.getMoveStarts();
                    watchMoveEnded = watchGame.getMoveEndeds();
                    watchHistoryTime = watchGame.getHistoryTimes();
                    watchRotateDirection = watchGame.getRotateDirections();
                    watchHistoryColors = watchGame.getTriHistories();
                    gameTime = watchGame.during;

                    UnityMainThreadDispatcher.Instance().Enqueue(()=>{
                        initStopColors();
                    });                    
                }
        });
    }

    private void showDirection() {
        if (directionRotate == 1) {
            imgDirection1.enabled = true;
            imgDirection2.enabled = false;
            imgDirection3.enabled = true;
            imgDirection4.enabled = false;
            imgDirection5.enabled = true;
            imgDirection6.enabled = false;
            imgDirection7.enabled = true;
            imgDirection8.enabled = false;
        } else {
            imgDirection1.enabled = false;
            imgDirection2.enabled = true;
            imgDirection3.enabled = false;
            imgDirection4.enabled = true;
            imgDirection5.enabled = false;
            imgDirection6.enabled = true;
            imgDirection7.enabled = false;
            imgDirection8.enabled = true;
        }
    }

    private void showColor() {
        for (int i = 0; i < 18; ++i) {
            groups[i].setColors(triShowColors[i]);    
        }        
    }

    private int getMatchSquares() {
        int square = 0;
        foreach (int[] matchs in triShowColors) {
            bool flag = true;
            int value = matchs[0];
            for (int i = 1; i < 4; i++) {
                if (value !=  matchs[i]) {
                    flag = false;
                }
            }
            if (flag) {
                square++;
            }
        }
        return square;
    }

    public void onStopClick() {
        historyStatus = Type.Ready;

        StopCoroutine("onWatchOneAction");
        StopCoroutine("onWatchTwoAction");

        initStopColors();
    }

    private void initStopColors() {
        watchIndex = watchHistoryColors.Count - 1;
        showWatch();
    }

    private void showWatch() {
        triShowColors = watchHistoryColors[watchIndex];
        for (int i = 0; i < i11.Count; i++) {
            int groupCnt1, triCnt1, groupCnt2, triCnt2;
            groupCnt1 = i11[i];
            triCnt1 = j11[i];
            groupCnt2 = i12[i];
            triCnt2 = j12[i];
            groups[groupCnt1].onRestartAction(triCnt1);
            groups[groupCnt2].onRestartAction(triCnt2);
        }
        isGameEnded();
        for (int i = 0; i < i11.Count; i++) {
            int groupCnt1, triCnt1, groupCnt2, triCnt2;
            groupCnt1 = i11[i];
            triCnt1 = j11[i];
            groupCnt2 = i12[i];
            triCnt2 = j12[i];
            groups[groupCnt1].onFailAction(triCnt1, false);
            groups[groupCnt2].onFailAction(triCnt2, false);
        }
        showColor();
        directionRotate = watchRotateDirection[watchIndex];
        showDirection();
        Debug.Log("watchIndex : " + watchIndex);
        int watchTime = watchHistoryTime[watchIndex];
        lblDuring.text = ((watchTime / 10) / 60).ToString("00")  + " : " + ((watchTime / 10) % 60).ToString("00");
        if (gameTime == watchTime) {
            lblDuring.color  = Color.yellow;
        } else {
            lblDuring.color  = Color.white;            
        }
        if (gameTime == 0) {
            lblDuring.color  = Color.white;
        }
        Debug.Log("gameTime : " + gameTime + "   watchTime : " + watchTime);
        historyDialog.setMoveText("Moves\n" + (watchIndex) + " / " + (watchHistoryColors.Count - 1));
        countSqure = getMatchSquares();
        historyDialog.setSquareText("Squares\n" + countSqure + " / " + watchGame.matchCount);
    }

    private bool isGameEnded() {
        bool isFlag = false;
        i11 = new List<int>();
        j11 = new List<int>();
        i12 = new List<int>();
        j12 = new List<int>();
        foreach(int[] triPoint in triEndPoint) {
            if (triShowColors[triPoint[0]][triPoint[1]] == triShowColors[triPoint[2]][triPoint[3]]) {
                i11.Add(triPoint[0]); j11.Add(triPoint[1]); i12.Add(triPoint[2]); j12.Add(triPoint[3]);
                isFlag = true;
            }
        }
        return isFlag;
    }

    private void onShowCrashingView() {
        isGameEnded();
        for (int i = 0; i < i11.Count; i++) {
            int groupCnt1, triCnt1, groupCnt2, triCnt2;
            groupCnt1 = i11[i];
            triCnt1 = j11[i];
            groupCnt2 = i12[i];
            triCnt2 = j12[i];
            groups[groupCnt1].onFailAction(triCnt1, false);
            groups[groupCnt2].onFailAction(triCnt2, false);
        }
    }

    public void onWatchClick() {
        for (int i = 0; i < i11.Count; i++) {
            int groupCnt1, triCnt1, groupCnt2, triCnt2;
            groupCnt1 = i11[i];
            triCnt1 = j11[i];
            groupCnt2 = i12[i];
            triCnt2 = j12[i];
            groups[groupCnt1].onRestartAction(triCnt1);
            groups[groupCnt2].onRestartAction(triCnt2);
        }

        watchIndex = 0;
        historyStatus = Type.Watch;
        onWatchAction();
    }

    private void onWatchAction() {
        historyType = watchTypes[watchIndex];
        switch (historyType) {
            case 0:
            case 2:
                StartCoroutine(onWatchOneAction(historyType));
            break;
            case 1:
            case 3:
            case 4:
                StartCoroutine(onWatchTwoAction(historyType));
            break;
        }
    }

    IEnumerator onWatchOneAction(int index) {        
        Vector3 position;
        if (index == 0) {
            position = Camera.main.WorldToScreenPoint(Vector3.zero);
        } else {
            position = Camera.main.WorldToScreenPoint(groupPositions[watchMoveStart[watchIndex]]);
        }
        historyDialog.imgHandle.transform.position = position;
        historyDialog.imgHandle.GetComponent<Image>().sprite = Resources.Load <Sprite>("Images/thumb_1");
        yield return new WaitForSeconds(watchDelay);
        historyDialog.imgHandle.GetComponent<Image>().sprite = Resources.Load <Sprite>("Images/thumb_2");
        showWatch();
        yield return new WaitForSeconds(watchDelay);
        watchIndex++;
        if (watchIndex < watchTypes.Count) {
            onWatchAction();
        } else {
            yield return new WaitForSeconds(3.0f);
            onWatchClick();
        }
    }

    IEnumerator onWatchTwoAction(int index) {
        Vector3 position1, position2;
        if (index == 1) {
            position1 = Camera.main.WorldToScreenPoint(groupPositions[watchMoveStart[watchIndex]]);
            position2 = Camera.main.WorldToScreenPoint(groupPositions[watchMoveEnded[watchIndex]]);
        } else if (index == 3) {
            position1 = Camera.main.WorldToScreenPoint(groupPositions[watchMoveStart[watchIndex]]);
            position2 = Camera.main.WorldToScreenPoint(Vector3.zero);
        } else {
            position1 = Camera.main.WorldToScreenPoint(Vector3.zero);
            position2 = Camera.main.WorldToScreenPoint((groupPositions[watchMoveStart[watchIndex]] + groupPositions[watchMoveEnded[watchIndex]]) / 2);
        }
        historyDialog.imgHandle.transform.position = position1;
        historyDialog.imgHandle.GetComponent<Image>().sprite = Resources.Load <Sprite>("Images/thumb_1");
        yield return new WaitForSeconds(watchDelay);
        historyDialog.imgHandle.GetComponent<Image>().sprite = Resources.Load <Sprite>("Images/thumb_2");
        yield return new WaitForSeconds(watchDelay);
        historyDialog.imgHandle.transform.position = position2;
        historyDialog.imgHandle.GetComponent<Image>().sprite = Resources.Load <Sprite>("Images/thumb_1");
        yield return new WaitForSeconds(watchDelay);
        historyDialog.imgHandle.GetComponent<Image>().sprite = Resources.Load <Sprite>("Images/thumb_2");
        showWatch();
        yield return new WaitForSeconds(watchDelay);
        watchIndex++;
        if (watchIndex < watchTypes.Count) {
            onWatchAction();
        } else {
            yield return new WaitForSeconds(3.0f);
            onWatchClick();
        }
    }

    public void onShowPrev() {
        historyDialog.btnBack.SetActive(true);
        watchIndex--;
        if (watchIndex == 0) {
            historyDialog.btnPrev.SetActive(false);
        }
        showWatch();
    }

    public void onShowNext() {
        historyDialog.btnPrev.SetActive(true);
        watchIndex++;
        if (watchIndex == watchHistoryColors.Count - 1) {
            historyDialog.btnBack.SetActive(false);
        }
        showWatch();
    }

    public void setHistoryStatus(Type type) {
        historyStatus = type;
    }

}
