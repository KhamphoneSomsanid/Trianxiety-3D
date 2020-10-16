using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Unity.Editor;
using GoogleMobileAds.Api;

public class GameManager : MonoBehaviour
{
    // Game UI Parameters
    public EndedDialog endedDialog;
    public MenuDialog menuDialog;
    public ToolMenuDialog toolMenuDialog;
    public Text lblTimeTracker;
    public Image imgDirection1;
    public Image imgDirection2;
    public Image imgDirection3;
    public Image imgDirection4;
    public Image imgDirection5;
    public Image imgDirection6;
    public Image imgDirection7;
    public Image imgDirection8;
    public Image imgHandle;
    public Image imgTimerBack;
    public CameraAction cameraObject;
    public Effect effect;
    public Button btnNewGame;
    public Text lblNewGame;
    public Button btnMainMenu;
    public Text lblMainMenu;
    public List<Group> groups;
    public GameObject groupPrefab;
    public GameObject imgHoverTri;
    public GameObject imgHoverRect1;
    public GameObject imgHoverRect2;
    public GameObject imgHoverToggle;
    public Text lblRank;
    public Text lblDate;

    // Game Control Parameters
    private bool is3DCamera = false;
    private int directionRotate = 1;
    private int countTime = 0;
    private int gameTime = 0;
    private bool isTimer = false;
    private int countSqure = 0;
    private int amountGroup = 18;
    private List<int> i11 = new List<int>();
    private List<int> i12 = new List<int>();
    private List<int> j11 = new List<int>();
    private List<int> j12 = new List<int>();
    private List<int[]> triEndPoint = new List<int[]>();
    private List<int[]> triInitColors;
    private int triHistoryIndex = 0;    
    private string userId;
    private float timeWatchDelay;
    
    private DatabaseReference reference;
    private InterstitialAd interstitial;
    private int moveStart = -1;
    private int moveEnded = -1;
    // (0, 1, 2, 3, 4) => ("Change Direction", "Move Square", "Rotation Square", "Double Rotation", "Change Triangle")
    private int historyType = -1;

    // Save History Parameters
    private List<List<int[]>> triHistoryShowColors;
    private List<int> triHistoryTime;
    private List<int> triHistoryMoveStart;
    private List<int> triHistoryMoveEnded;
    private List<int> triHistoryRotationDirection;
    private List<int> triHistoryType;

    // Shared Parameters
    public List<int[]> triShowColors;

    public static bool isDirectStart = false;

    public enum Type {
        Prepare,
        Start,
        Running,
        Failed,
        Success,
        Ready,
        Watch,
    };
    private Type gameStatus = Type.Prepare;

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

    void Awake()
    {
        Input.multiTouchEnabled = false;
    }

    void Start() {
        gameStatus = Type.Prepare;

        triHistoryShowColors = new List<List<int[]>>();
        triHistoryTime = new List<int>();
        triHistoryMoveStart = new List<int>();
        triHistoryMoveEnded = new List<int>();
        triHistoryRotationDirection = new List<int>();
        triHistoryType = new List<int>();

        triInitColors = new List<int[]>();
        triInitColors.Add(new int[]{5, 5, 5, 5});
        triInitColors.Add(new int[]{4, 4, 4, 4});
        triInitColors.Add(new int[]{3, 3, 3, 3});
        triInitColors.Add(new int[]{2, 2, 2, 2});
        triInitColors.Add(new int[]{1, 1, 1, 1});
        triInitColors.Add(new int[]{0, 0, 0, 0});
        triInitColors.Add(new int[]{5, 5, 5, 5});
        triInitColors.Add(new int[]{4, 4, 4, 4});
        triInitColors.Add(new int[]{3, 3, 3, 3});
        triInitColors.Add(new int[]{2, 2, 2, 2});
        triInitColors.Add(new int[]{1, 1, 1, 1});
        triInitColors.Add(new int[]{0, 0, 0, 0});
        triInitColors.Add(new int[]{5, 5, 5, 5});
        triInitColors.Add(new int[]{4, 4, 4, 4});
        triInitColors.Add(new int[]{3, 3, 3, 3});
        triInitColors.Add(new int[]{2, 2, 2, 2});
        triInitColors.Add(new int[]{1, 1, 1, 1});
        triInitColors.Add(new int[]{0, 0, 0, 0});

        groups = new List<Group>();
        for (int i = 0; i < amountGroup; ++i) {
            GameObject groupGameobject = Instantiate(groupPrefab, transform, false);
            Group group = groupGameobject.GetComponent<Group>();
            group.transform.localPosition = groupPositions[i];
            group.setGameManager(this);
            group.setGroupIndex(i);
            groups.Add(group);
        }

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

        triShowColors = new List<int[]>();
        foreach(int[] colors in triInitColors) {
            int[] setColors = new int[] { colors[0], colors[1], colors[2], colors[3]};
            triShowColors.Add(setColors);    
        }

        showColor();
        showDirection();
        hideRankView();

        lblTimeTracker.enabled = false;
        imgTimerBack.enabled = false;

        endedDialog.hide();
        toolMenuDialog.hide();

        btnNewGame.interactable = false;
        btnMainMenu.interactable = false;
        lblNewGame.color = Color.gray;
        lblMainMenu.color = Color.gray;  

        onFirebaseAuth();
        // MobileAds.Initialize(initStatus => { });
        // RequestInterstitial();
    }

    private void RequestInterstitial()
    {
        #if UNITY_ANDROID
            string adUnitId = "ca-app-pub-1819197481638467/4697440757";
        #elif UNITY_IPHONE
            string adUnitId = "ca-app-pub-3940256099942544/4411468910";
        #else
            string adUnitId = "unexpected_platform";
        #endif

        // Initialize an InterstitialAd.
        this.interstitial = new InterstitialAd(adUnitId);
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the interstitial with the request.
        this.interstitial.LoadAd(request);

        this.interstitial.OnAdClosed += HandleOnAdClosed;
    }

    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        this.interstitial.LoadAd(new AdRequest.Builder().Build());
    }

    private void showGoogleADS()
    {
        // if (this.interstitial.IsLoaded()) {
        //     Invoke ( "delayShowADS", 1.0f );        
        // }
    }

    private void hideRankView() {
        lblRank.enabled = false;
        lblDate.enabled = false;
    }

    private void showRankView() {
        lblRank.enabled = true;
        lblDate.enabled = true;
    }

    private void delayShowADS() {
        this.interstitial.Show();
    }

    private void onFirebaseAuth() {
        FirebaseAuth auth = FirebaseAuth.DefaultInstance;
        auth.SignInAnonymouslyAsync().ContinueWith(task =>
        {
            if (task.IsCanceled){
                return;
            }
            if (task.IsFaulted)
            {
                return;
            }

            FirebaseUser fUser = task.Result;
            userId = fUser.UserId;

            Debug.Log("UserID : " + userId);
        });

        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://trianxiety.firebaseio.com/");
        reference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    private void showColor() {
        for (int i = 0; i < amountGroup; ++i) {
            groups[i].setColors(triShowColors[i]);    
        }        
    }

    private int chooseColorIndex = 0;
    private int[] colorCount = new int[6];
    private List<int[]> beginShowColors = new List<int[]>();
    private void changeColor() {
        chooseColorIndex++;
        initShowColors();
        for (int i = 0; i < 18; i++) {
            for (int j = 0; j < 4; j++) {
                int attachColor = getColorMaxIndex(i, j);
                if (attachColor == -1) {
                    changeColor();
                    return;
                }
                beginShowColors[i][j] = attachColor;
                colorCount[attachColor]--;
            }
        }
        for (int i = 0; i < 18; i++) {
            for (int j = 0; j < 4; j++) {
                triShowColors[i][j] = beginShowColors[i][j];
            }
        }
        Debug.Log("chooseColorIndex : " + chooseColorIndex);
        showColor();
        chooseColorIndex = 0;
    }

    private void initShowColors() {
        colorCount = new int[]{12, 12, 12, 12, 12, 12};

        beginShowColors.Clear();
        beginShowColors.Add(new int[]{-1, -1, -1, -1});
        beginShowColors.Add(new int[]{-1, -1, -1, -1});
        beginShowColors.Add(new int[]{-1, -1, -1, -1});
        beginShowColors.Add(new int[]{-1, -1, -1, -1});
        beginShowColors.Add(new int[]{-1, -1, -1, -1});
        beginShowColors.Add(new int[]{-1, -1, -1, -1});
        beginShowColors.Add(new int[]{-1, -1, -1, -1});
        beginShowColors.Add(new int[]{-1, -1, -1, -1});
        beginShowColors.Add(new int[]{-1, -1, -1, -1});
        beginShowColors.Add(new int[]{-1, -1, -1, -1});
        beginShowColors.Add(new int[]{-1, -1, -1, -1});
        beginShowColors.Add(new int[]{-1, -1, -1, -1});
        beginShowColors.Add(new int[]{-1, -1, -1, -1});
        beginShowColors.Add(new int[]{-1, -1, -1, -1});
        beginShowColors.Add(new int[]{-1, -1, -1, -1});
        beginShowColors.Add(new int[]{-1, -1, -1, -1});
        beginShowColors.Add(new int[]{-1, -1, -1, -1});
        beginShowColors.Add(new int[]{-1, -1, -1, -1});
    }

    private int getColorMaxIndex(int iIndex, int jIndex) {
        int colorIndex = -1;
        int[] randomColorArray = new int[]{
            UnityEngine.Random.Range(0, 100) * colorCount[0],
            UnityEngine.Random.Range(0, 100) * colorCount[1],
            UnityEngine.Random.Range(0, 100) * colorCount[2],
            UnityEngine.Random.Range(0, 100) * colorCount[3],
            UnityEngine.Random.Range(0, 100) * colorCount[4],
            UnityEngine.Random.Range(0, 100) * colorCount[5],
            };
        int all2Matchs = 0;
        foreach (int[] matchs in beginShowColors) {
            int[] colorValues1 = new int[6] {0, 0, 0, 0, 0, 0};
            for (int i = 0; i < 4; i++) {
                if (matchs[i] < 0) continue;
                colorValues1[matchs[i]]++;
            }
            foreach (int colorValue in colorValues1) {
                if (colorValue == 2) all2Matchs++;
            }
        }

        int[] colorValues = new int[6] {0, 0, 0, 0, 0, 0};
        for (int i = 0; i < 4; i++) {
            if (beginShowColors[iIndex][i] < 0) continue;
            colorValues[beginShowColors[iIndex][i]]++;
        }
        for (int i = 0; i < 6; i++) {
            int colorValue = colorValues[i];
            if (colorValue > 1) randomColorArray[i] = 0;
            if (colorValue == 1) {
                if (all2Matchs > 8) randomColorArray[i] = 0;
            }
        }

        foreach(int[] triPoint in triEndPoint) {
            if (iIndex == triPoint[2] && jIndex == triPoint[3]) {
                randomColorArray[beginShowColors[triPoint[0]][triPoint[1]]] = 0;
                break;
            }
        }

        int maxColor = 0;
        for (int i = 0; i < 6; i++) {
            int randomColor = randomColorArray[i];
            if (randomColor > maxColor) {
                maxColor = randomColor;
                colorIndex = i;
            }
        }
        return colorIndex;
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

    // Update is called once per frame
    void Update() {
        switch (gameStatus) {
            case Type.Prepare:
                onRunPrepare();
            break;
            case Type.Start:
                onStartGame();
            break;
            case Type.Failed:
                onRunFailed();
            break;
            case Type.Success:
                onRunSuccess();
            break;
        }        
    }

    private int changeColorIndex = 0;
    private int changeColorDivider = 16;

    private void onRunPrepare() {
        if (changeColorIndex % changeColorDivider == 0) {
            changeColor();
            changeColorIndex = 0;
            if (isDirectStart) {
                gameStatus = Type.Start;
                isDirectStart = false;
                return;
            }
        }        
        changeColorIndex++;
    }

    private void onStartGame() {
        btnNewGame.interactable = true;
        btnMainMenu.interactable = true;
        lblNewGame.color = Color.white;
        lblMainMenu.color = Color.white;

        triHistoryShowColors = new List<List<int[]>>();
        triHistoryTime = new List<int>();
        triHistoryMoveStart = new List<int>();
        triHistoryMoveEnded = new List<int>();
        triHistoryRotationDirection = new List<int>();
        triHistoryType = new List<int>();

        is3DCamera = false;
        onSwitchCamera();
        menuDialog.hide();
        lblTimeTracker.enabled = true;
        lblTimeTracker.color = Color.white;
        imgTimerBack.enabled = true;

        i11 = new List<int>();
        j11 = new List<int>();
        i12 = new List<int>();
        j12 = new List<int>();
        
        countTime = 0;
        gameTime = 0;
        isTimer = true;
        beginTimer();

        historyType = 0;
        moveStart = -1;
        moveEnded = -1;
        onAddHistoies();
       
        gameStatus = Type.Running;
    }

    private void onRunFailed() {
        gameStatus = Type.Ready;

        isTimer = false;
        isMenuClick = true;
        for (int i = 0; i < i11.Count; i++) {
            int groupCnt1, triCnt1, groupCnt2, triCnt2;
            groupCnt1 = i11[i];
            triCnt1 = j11[i];
            groupCnt2 = i12[i];
            triCnt2 = j12[i];
            groups[groupCnt1].onFailAction(triCnt1, false);
            groups[groupCnt2].onFailAction(triCnt2, false);
        }
        
        triHistoryIndex = triHistoryShowColors.Count - 1;
        countSqure = getMatchSquares();
        endedDialog.show();
        endedDialog.setMoveText("Moves\n" + triHistoryShowColors.Count + " / " + triHistoryShowColors.Count);
        endedDialog.setSquareText("Squres\n" + countSqure + " / 18");

        is3DCamera = true;
        onSwitchCamera();
        effect.onRunSoundFailed();

        writeNewGame(countSqure);

        showGoogleADS();
    }

    private void onRunSuccess() {        
        isTimer = false;
        triHistoryIndex = triHistoryShowColors.Count - 1;

        for (int i = 0; i < 18; i++) {
            Group group = groups[i];
            group.onWhiteAction(triShowColors[i]);
        }

        endedDialog.show();
        endedDialog.setMoveText("Moves\n" + triHistoryShowColors.Count + " / " + triHistoryShowColors.Count);
        endedDialog.setMoveText("Squres\n18 / 18");

        countSqure = getMatchSquares();
        writeNewGame(countSqure);
        isMenuClick = true;

        onPlaySoundSquareAll();
        lblTimeTracker.color = Color.yellow;

        is3DCamera = true;
        onSwitchCamera();

        gameStatus = Type.Ready;

        showGoogleADS();
    }

    private void onReset() {
        gameStatus = Type.Ready;

        for (int i = 0; i < i11.Count; i++) {
            int groupCnt1, triCnt1, groupCnt2, triCnt2;
            groupCnt1 = i11[i];
            triCnt1 = j11[i];
            groupCnt2 = i12[i];
            triCnt2 = j12[i];
            groups[groupCnt1].onRestartAction(triCnt1);
            groups[groupCnt2].onRestartAction(triCnt2);
        }

        triHistoryShowColors = new List<List<int[]>>();

        triHistoryIndex = 0;
        countTime = 0;
        directionRotate = 1;
        showDirection();
        countSqure = 0;
        isTimer = false;
        lblTimeTracker.text = ((countTime / 10) / 60).ToString("00")  + " : " + ((countTime / 10) % 60).ToString("00");
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

    private bool isGameSuccess() {
        bool flag = true;
        foreach (int[] matchs in triShowColors) {            
            int value = matchs[0];
            for (int i = 1; i < 4; i++) {
                if (value !=  matchs[i]) {
                    flag = false;
                }
            }
        }
        return flag;
    }

    private void beginTimer() {
        countTime = 0;
        CancelInvoke();
        Invoke ( "timeTraker", 0.1f );
    }
 
    private void timeTraker() {
        if (!isTimer) return;
        Invoke ( "timeTraker", 0.1f );
        countTime++;
        lblTimeTracker.text = ((countTime / 10) / 60).ToString("00")  + " : " + ((countTime / 10) % 60).ToString("00");
    }

    private void onAddHistoies() {
        triHistoryType.Add(historyType);
        triHistoryRotationDirection.Add(directionRotate);
        triHistoryMoveStart.Add(moveStart);
        triHistoryMoveEnded.Add(moveEnded);
        triHistoryTime.Add(countTime);
        List<int[]> addColors = new List<int[]>();
        foreach(int[] colors in triShowColors) {
            int[] setColors = new int[] { colors[0], colors[1], colors[2], colors[3]};
            addColors.Add(setColors);    
        }
        triHistoryShowColors.Add(addColors);
    }

    private void onSwitchCamera() {
        if (is3DCamera) {            
            cameraObject.onChangeCamera3D();
        } else {
            cameraObject.onChangeCamera2D();            
        }
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

    private float getDistance(Vector3 v1, Vector3 v2) {
        return (float)Math.Sqrt((v1.x - v2.x) * (v1.x - v2.x) + (v1.y - v2.y) * (v1.y - v2.y) + (v1.z - v2.z) * (v1.z - v2.z));        
    }

    private int getMinIndex(Vector3 position, int defaultIndex) {
        float min = 0.051f;
        int minIndex = -1;
        for (int i = 0; i < 18; i++) {
            if (i == defaultIndex) continue;
            Vector3 targetPos = groupPositions[i];
            float distance = getDistance(targetPos, position);
            if (distance < min) {
                minIndex = i;
                min = distance;
            }
        }
        return minIndex;
    }

    private bool isCheckSameSquare(int squareIndex) {
        bool isSameSquare = true;
        int defaultValue = triShowColors[squareIndex][0];
        for (int i = 1; i < 4; ++i) {
            int value = triShowColors[squareIndex][i];
            if (defaultValue != value) {
                isSameSquare = false;
            }
        }
        return isSameSquare;
    }

    public void setRotateDirection(int direction) {
        this.directionRotate = direction;
        showDirection();

        historyType = 0;
        moveStart = -1;
        moveEnded = -1;
        onAddHistoies();
    }

    public int getRotateDirection() {
        return directionRotate;
    }

    public void setGameStatus(Type status) {
        this.gameStatus = status;
    }

    public Type getGameStatus() {
        return gameStatus;
    }

    public void onUpdateColor(int type, int moveStart, int moveEnded) {
        historyType = type;
        this.moveStart = moveStart;
        this.moveEnded = moveEnded;
        onAddHistoies();

        showColor();
        if (isGameEnded()) {
            gameStatus = Type.Failed;
            return;
        }
        if (isGameSuccess()) {
            gameStatus = Type.Success;
            return;   
        }
        gameStatus = Type.Running;
    }

    private int beforeTriMin1 = -1;
    private int beforeTriMin2 = -1;

    public void onDragToggleBegin() {
        imgHoverToggle.transform.position = new Vector3(0.0f, 0.006f, 0.0f);
        Effect.onRunVibrate();
    }

    public void onDragToggle(Vector3 position) {
        int minIndex1 = getMinIndex(position, -1);
        if (minIndex1 == -1) {
            imgHoverTri.transform.position = Vector3.zero;
            return;
        }
        int minIndex2 = getMinIndex(position, minIndex1);
        if (minIndex2 == -1) {
            imgHoverTri.transform.position = Vector3.zero;
            return;
        }

        foreach(int[] endPoints in triEndPoint) {
            if ((endPoints[0] == minIndex1 && endPoints[2] == minIndex2) || (endPoints[2] == minIndex1 && endPoints[0] == minIndex2)) {
                // groups[endPoints[0]].onHoverTriangle(endPoints[1], true);
                // groups[endPoints[2]].onHoverTriangle(endPoints[3], false);
                if (!((beforeTriMin1 == minIndex1) && (beforeTriMin2 == minIndex2))) {
                    beforeTriMin1 = minIndex1;
                    beforeTriMin2 = minIndex2;
                    isMainVibrate = false;
                }
                Vector3 triPosition = (groups[minIndex1].transform.position + groups[minIndex2].transform.position) / 2;
                imgHoverTri.transform.position = triPosition + new Vector3(0.0f, 0.006f, 0.0f);
                if (!isMainVibrate) {
                    Effect.onRunVibrate();
                    isMainVibrate = true;
                }
                break;
            }
        }
    }

    public void onDragDownToggle(Vector3 position) {
        // foreach (Group selGroup in groups) {
        //     selGroup.onUnHoverTriangle();
        // }

        beforeTriMin1 = -1;
        beforeTriMin2 = -1;
        isMainVibrate = false;
        imgHoverTri.transform.position = Vector3.zero;
        imgHoverToggle.transform.position = Vector3.zero;

        int minIndex1  = getMinIndex(position, -1);
        if (minIndex1 == -1) {
            Debug.Log("minIndex1 : " + minIndex1 + " --- default : -1");
            return;
        }
        int minIndex2 = getMinIndex(position, minIndex1);
        if (minIndex2 == -1) {
            Debug.Log("minIndex2 : " + minIndex2 + " --- default : " + minIndex1);
            return;
        }

        foreach(int[] endPoints in triEndPoint) {
            if ((endPoints[0] == minIndex1 && endPoints[2] == minIndex2) || (endPoints[2] == minIndex1 && endPoints[0] == minIndex2)) {
                int value1 = triShowColors[endPoints[0]][endPoints[1]];
                int value2 = triShowColors[endPoints[2]][endPoints[3]];
                triShowColors[endPoints[0]][endPoints[1]] = value2;
                triShowColors[endPoints[2]][endPoints[3]] = value1;
                Debug.Log("value1 : " + value1 + " --- value2 : " + value2);
                onUpdateColor(4, endPoints[0], endPoints[2]);
                break;
            }
        }
        
        if (!isGameSuccess()) {
            bool isSameSquare = isCheckSameSquare(minIndex1) || isCheckSameSquare(minIndex2);
            if (isSameSquare) {
                onPlaySoundSquareOne();
                if (isCheckSameSquare(minIndex1)) {
                    Group group = groups[minIndex1];
                    group.onWhiteAction(triShowColors[minIndex1]);
                } 
                if (isCheckSameSquare(minIndex2)) {
                    Group group = groups[minIndex2];
                    group.onWhiteAction(triShowColors[minIndex2]);
                }
                gameTime = countTime;
            }
            // Effect.onRunVibrate();
        }        
    }

    private bool isMainVibrate = false;

    public void onDragGroupBegin(int groupIndex) {
        imgHoverRect1.transform.position = groupPositions[groupIndex]+ new Vector3(0.00f, 0.006f, 0.0f);
        Effect.onRunVibrate();
    }    

    public void onDragGroup(Group group, int groupIndex, Vector3 position) {
        int minIndex = getMinIndex(position, groupIndex);
        
        for (int i = 0; i < groups.Count; ++i) {
            if ((i == minIndex) || (i == groupIndex)) continue;
            Group selGroup = groups[i];
            selGroup.onUnHoverGroup();
            imgHoverRect2.transform.position = Vector3.zero;
        }

        if (minIndex > -1) {
            Group targetGroup = groups[minIndex];
            // group.transform.position = groupPositions[minIndex]+ new Vector3(0.00f, 0.04f, 0.0f);
            imgHoverRect2.transform.position = groupPositions[minIndex]+ new Vector3(0.00f, 0.006f, 0.0f);
            // targetGroup.onHoverGroup();
            isMainVibrate = false;
        } else {
            float distance = getDistance(Vector3.zero, position);
            if (distance < 0.043f) {
                if (!isMainVibrate) {
                    Effect.onRunVibrate();
                    isMainVibrate = true;
                    imgHoverRect2.transform.position = new Vector3(0.0f, 0.006f, 0.0f);
                }
            }
        }
    }

    public void onDragDownGroup(Group group, int groupIndex, Vector3 position) {
        foreach (Group selGroup in groups) {
            selGroup.onUnHoverGroup();
        }
        isMainVibrate = false;

        imgHoverRect1.transform.position = Vector3.zero;
        imgHoverRect2.transform.position = Vector3.zero;

        int minIndex = getMinIndex(position, groupIndex);
        if (minIndex == -1) {
            float distance = getDistance(Vector3.zero, position);
            if (distance < 0.043f) {
                int[] groupValue = triShowColors[groupIndex];
                int triValue1 = groupValue[0];
                int triValue2 = groupValue[1];

                groupValue[0] = groupValue[3];
                groupValue[1] = groupValue[2];
                groupValue[3] = triValue1;
                groupValue[2] = triValue2;

                // Effect.onRunVibrate();

                onUpdateColor(3, groupIndex, -1);
            }
            // group.transform.position = groupPositions[groupIndex];
        } else {
            Group targetGroup = groups[minIndex];
            // group.transform.position = groupPositions[minIndex];
            targetGroup.onChangePost(group, groupPositions[groupIndex], groupPositions[minIndex], groupIndex, minIndex);

            // Effect.onRunVibrate();  
        }
    }

    private bool isMenuClick = true;

    // Toolbar Buttom Event
    public void onClickStartBtn() {
        isMenuClick = false;
        if (gameStatus == Type.Ready) {
            onReset();

            endedDialog.hide();            
            changeColor();
            gameStatus = Type.Start;
            hideRankView();
        } else {
            gameStatus = Type.Ready;
            // isTimer = false;
            btnNewGame.interactable = false;
            btnMainMenu.interactable = false;
            lblNewGame.color = Color.gray;
            lblMainMenu.color = Color.gray;
            toolMenuDialog.show();
            toolMenuDialog.lblDetail.text = "Are you sure that you want to quit the ongoing game and start a new game?";
        }
    }

    public void onClickMenuBtn() {
        isMenuClick = true;
        if (gameStatus == Type.Ready) {
            endedDialog.hide();

            onReset();
            menuDialog.show();
            gameStatus = Type.Prepare;

            hideRankView();
        } else {
            gameStatus = Type.Ready;
            // isTimer = false;
            btnNewGame.interactable = false;
            btnMainMenu.interactable = false;
            lblNewGame.color = Color.gray;
            lblMainMenu.color = Color.gray;
            toolMenuDialog.show();
            toolMenuDialog.lblDetail.text = "Are you sure that you want to quit the ongoing game and go to the main menu?";
        }
    }

    // Toolbar Dialog Button Event
    public void onClickDialogYes() {
        Debug.Log("onClickDialogYes");
        toolMenuDialog.hide();
        hideRankView();
        onReset();
        
        if (isMenuClick) {
            menuDialog.show();
            gameStatus = Type.Prepare;
            lblTimeTracker.enabled = false;
            imgTimerBack.enabled = false;
        } else {
            btnNewGame.interactable = true;
            btnMainMenu.interactable = true;
            lblNewGame.color = Color.white;
            lblMainMenu.color = Color.white;

            changeColor();
            gameStatus = Type.Start;
        }        
    }

    public void onClickDialogNo() {
        Debug.Log("onClickDialogNo");
        btnNewGame.interactable = true;
        btnMainMenu.interactable = true;
        lblNewGame.color = Color.white;
        lblMainMenu.color = Color.white;
        toolMenuDialog.hide();

        gameStatus = Type.Running;
        // isTimer = true;
        // Invoke ( "timeTraker", 1f );        
    }

    private void writeNewGame(int matchCount) {
        string key = reference.Child("game").Push().Key;

        string dateAndTimeVar = System.DateTime.Now.ToString("yyyy-MM-dd");

        watchGame = new Game(key, userId, matchCount, gameTime, dateAndTimeVar, triHistoryShowColors, triHistoryTime, triHistoryMoveStart, triHistoryMoveEnded, triHistoryRotationDirection, triHistoryType);
        watchTypes = watchGame.getHistoryTypes();
        watchMoveStart = watchGame.getMoveStarts();
        watchMoveEnded = watchGame.getMoveEndeds();
        watchHistoryTime = watchGame.getHistoryTimes();
        watchRotateDirection = watchGame.getRotateDirections();
        watchHistoryColors = watchGame.getTriHistories();
        initStopColors();

        string json = JsonUtility.ToJson(watchGame);
        reference.Child("game").Child(key).SetRawJsonValueAsync(json).ContinueWith (t => {
            if(t.IsFaulted) {
                Debug.Log("Faulted..");
            }

            if(t.IsCanceled) {
                Debug.Log("Cancelled..");
            }

            if (t.IsCompleted) {
                if (watchGame.matchCount == 0) {
                    UnityMainThreadDispatcher.Instance().Enqueue(()=>{
                        showNoRankView();
                    }); 
                } else {
                    FirebaseDatabase.DefaultInstance
                        .GetReference("game")
                        .OrderByChild("rank")
                        .GetValueAsync().ContinueWith(task => {
                            if (task.IsFaulted) {
                            // Handle the error...
                            } else if (task.IsCompleted) {
                                DataSnapshot gameSnapshot = task.Result;
                                
                                List<Game> gameDatas = new List<Game>();
                                foreach (var rankSnapshot in gameSnapshot.Children)
                                {                                    
                                    Game game = JsonUtility.FromJson<Game>(rankSnapshot.GetRawJsonValue());
                                    gameDatas.Add(game);
                                }

                                int selfIndex = 0;
                                int allIndex = 0;
                                for (int i = 0; i < gameDatas.Count; i++)
                                {
                                    allIndex++;
                                    Game game = gameDatas[gameDatas.Count - i - 1];
                                    if (game.userId == userId) {
                                        selfIndex++;
                                    }

                                    if (game.id == watchGame.id) {
                                        break;
                                    }
                                }

                                UnityMainThreadDispatcher.Instance().Enqueue(()=>{
                                    showVauleRankView(allIndex, selfIndex);
                                });              
                            }
                    });
                }                
            }
        });
    }

    private void showVauleRankView(int allIndex, int selfIndex) {
        if (allIndex < 100) {
            lblRank.text = "# " + selfIndex + " of your games\n# " + allIndex + " of all games";
        } else {
            lblRank.text = "# " + selfIndex + " of your games";
        }
        lblDate.text = watchGame.startDate;
        showRankView();
    }

    private void showNoRankView() {
        lblDate.text = watchGame.startDate;
        lblRank.text = "No Rank";
        showRankView();
    }
    
    private float watchDelay = 0.3f;
    private int watchIndex;
    private Game watchGame;
    private List<int> watchTypes = new List<int>();
    private List<int> watchMoveStart = new List<int>();
    private List<int> watchMoveEnded = new List<int>();
    private List<int> watchHistoryTime = new List<int>();
    private List<int> watchRotateDirection = new List<int>();
    private List<List<int[]>> watchHistoryColors = new List<List<int[]>>();

    public void onWatchClick() {
        onReset();
        btnNewGame.interactable = false;
        btnMainMenu.interactable = false;
        lblNewGame.color = Color.gray;
        lblMainMenu.color = Color.gray;

        gameStatus = Type.Watch;
        watchIndex = 0;
        onWatchAction();
    }

    public void onStopClick() {
        gameStatus = Type.Ready;

        StopCoroutine("onWatchOneAction");
        StopCoroutine("onWatchTwoAction");

        initStopColors();
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
        int watchTime = watchHistoryTime[watchIndex];
        lblTimeTracker.text = ((watchTime / 10) / 60).ToString("00")  + " : " + ((watchTime / 10) % 60).ToString("00");
        if (gameTime == watchTime) {
            lblTimeTracker.color  = Color.yellow;            
        } else {
            lblTimeTracker.color  = Color.white;
        }
        if (gameTime == 0) {
            lblTimeTracker.color  = Color.white;
        }
        endedDialog.setMoveText("Moves\n" + (watchIndex) + " / " + (watchHistoryColors.Count - 1));
        countSqure = getMatchSquares();
        endedDialog.setSquareText("Squares\n" + countSqure + " / " + watchGame.matchCount);
    }

    private int watchClickCount = 1;

    private void onWatchAction() {
        historyType = watchTypes[watchIndex];
        watchClickCount++;
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
        imgHandle.transform.position = position;
        imgHandle.GetComponent<Image>().sprite = Resources.Load <Sprite>("Images/thumb_1");
        yield return new WaitForSeconds(watchDelay);
        imgHandle.GetComponent<Image>().sprite = Resources.Load <Sprite>("Images/thumb_2");
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
        imgHandle.transform.position = position1;
        imgHandle.GetComponent<Image>().sprite = Resources.Load <Sprite>("Images/thumb_1");
        yield return new WaitForSeconds(watchDelay);
        imgHandle.GetComponent<Image>().sprite = Resources.Load <Sprite>("Images/thumb_2");
        yield return new WaitForSeconds(watchDelay);
        imgHandle.transform.position = position2;
        imgHandle.GetComponent<Image>().sprite = Resources.Load <Sprite>("Images/thumb_1");
        yield return new WaitForSeconds(watchDelay);
        imgHandle.GetComponent<Image>().sprite = Resources.Load <Sprite>("Images/thumb_2");
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

    private void initStopColors() {
        btnNewGame.interactable = true;
        btnMainMenu.interactable = true;
        lblNewGame.color = Color.white;
        lblMainMenu.color = Color.white;

        watchIndex = watchHistoryColors.Count - 1;
        showWatch();
    }

    public void onShowPrev() {
        for (int i = 0; i < i11.Count; i++) {
            int groupCnt1, triCnt1, groupCnt2, triCnt2;
            groupCnt1 = i11[i];
            triCnt1 = j11[i];
            groupCnt2 = i12[i];
            triCnt2 = j12[i];
            groups[groupCnt1].onRestartAction(triCnt1);
            groups[groupCnt2].onRestartAction(triCnt2);
        }
        endedDialog.btnBack.SetActive(true);
        watchIndex--;
        if (watchIndex == 0) {
            endedDialog.btnPrev.SetActive(false);
        }
        showWatch();
    }

    public void onShowNext() {
        endedDialog.btnPrev.SetActive(true);
        watchIndex++;
        if (watchIndex == watchHistoryColors.Count - 1) {
            endedDialog.btnBack.SetActive(false);
        }
        showWatch();
    }

    public void onPlaySoundClick() {
        effect.onRunSoundClick();
    }

    public void onPlaySoundSquareOne() {
        effect.onRunSoundCompleteOne();
    }

    public void onPlaySoundSquareAll() {
        effect.onRunSoundCompleteAll();
    }

    public void onChangeCallback() {
        showColor();
        gameStatus = Type.Running;
    }

}
