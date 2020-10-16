using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Unity.Editor;
using Firebase;
using Firebase.Auth;
using Firebase.Database;

public class RankController : MonoBehaviour
{
    public HistoryCell cellFab;
    public HistoryCell originFab;
    public int numberToCreate;
    public bool isUser;

    private string userid;
    private List<Game> gameDatas = new List<Game>();
    private List<Game> gameAllDatas = new List<Game>();
    private List<HistoryCell> historyCells = new List<HistoryCell>();

    // Start is called before the first frame update
    void Start()
    {
        // Populate();
        historyCells.Add(originFab);
        if (!isUser) {            
            initAllFirebase();
        } else {            
            initUserFirebase();
        }        
        originFab.hide();        
    }

    void initUserFirebase() {
        FirebaseAuth auth = FirebaseAuth.DefaultInstance;
        FirebaseUser currentUser = auth.CurrentUser;
        userid = currentUser.UserId;

        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://trianxiety.firebaseio.com/");
        FirebaseDatabase.DefaultInstance
            .GetReference("game")
            .OrderByChild("rank")
            .GetValueAsync().ContinueWith(task => {
                if (task.IsFaulted) {
                // Handle the error...
                } else if (task.IsCompleted) {
                    DataSnapshot gameSnapshot = task.Result;
                    foreach (var rankSnapshot in gameSnapshot.Children)
                    {
                        Game game = JsonUtility.FromJson<Game>(rankSnapshot.GetRawJsonValue());
                        if (isUser) {
                            if (game.userId == userid) {
                                gameDatas.Add(game);
                            }
                        }                       
                    }
                    if (gameDatas.Count > 0) {
                        UnityMainThreadDispatcher.Instance().Enqueue(()=>{
                            initAllFirebase();
                        });
                    }
                }
        });
    }

    void initAllFirebase() {
        FirebaseAuth auth = FirebaseAuth.DefaultInstance;
        FirebaseUser currentUser = auth.CurrentUser;
        userid = currentUser.UserId;

        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://trianxiety.firebaseio.com/");
        FirebaseDatabase.DefaultInstance
            .GetReference("game")
            .OrderByChild("rank")
            .LimitToLast(100)
            .GetValueAsync().ContinueWith(task => {
                if (task.IsFaulted) {
                // Handle the error...
                } else if (task.IsCompleted) {
                    DataSnapshot gameSnapshot = task.Result;
                    if (isUser) {
                        foreach (var rankSnapshot in gameSnapshot.Children)
                        {
                            Game game = JsonUtility.FromJson<Game>(rankSnapshot.GetRawJsonValue());
                            gameAllDatas.Add(game);       
                        }
                        UnityMainThreadDispatcher.Instance().Enqueue(()=>{
                            showUpdateView();
                        });
                    } else {
                        foreach (var rankSnapshot in gameSnapshot.Children)
                        {
                            Game game = JsonUtility.FromJson<Game>(rankSnapshot.GetRawJsonValue());
                            gameDatas.Add(game);                   
                        }
                        if (gameDatas.Count > 0) {                            
                            UnityMainThreadDispatcher.Instance().Enqueue(()=>{
                                showInitView();
                            });
                        }
                    }                    
                }
        });
    }

    private void showInitView() {
        Debug.Log("gameAllDatas : " + gameAllDatas.Count);
        originFab.show();
        HistoryCell newObj;
        for (int i = 0; i < gameDatas.Count - 1; i++)
        {
            newObj = (HistoryCell)Instantiate(cellFab, transform);
            historyCells.Add(newObj);
        }

        originFab.setData(gameDatas[historyCells.Count - 1], 0, 0, false);
        for (int i = 1; i < historyCells.Count; i++)
        {
            HistoryCell historyCell = historyCells[i];
            Game game = gameDatas[historyCells.Count - i - 1];
            historyCell.setData(game, i, i, false);
        }
    }

    private void showUpdateView() {
        Debug.Log("gameDatas : " + gameDatas.Count);
        originFab.show();
        HistoryCell newObj;
        for (int i = 0; i < gameDatas.Count - 1; i++)
        {
            newObj = (HistoryCell)Instantiate(cellFab, transform);
            historyCells.Add(newObj);
        }
        
        for (int i = 0; i < historyCells.Count; i++)
        {
            HistoryCell historyCell = historyCells[i];
            Game game = gameDatas[historyCells.Count - i - 1];
            for (int j = 0; j < gameAllDatas.Count; j++)
            {
                Game gameAll = gameAllDatas[gameAllDatas.Count - j - 1];
                if (gameAll.rank == game.rank) {
                    if (i == 0) {
                        originFab.setData(game, j, i, true);
                    } else {
                        historyCell.setData(game, j, i, true);
                    }                    
                }        
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
