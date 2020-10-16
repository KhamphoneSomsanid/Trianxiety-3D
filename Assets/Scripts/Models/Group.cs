using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Group : MonoBehaviour
{

    public enum Type {
        RotateRight,
        RotateLeft,
        MoveDest,
        ChangeCard,
        None
    };

    Type status = Type.None;

    public GameObject triPreb;
    List<Triangle> triangles;
    GameManager gameManager;

    public int groupIndex;
    private bool isHover = false;

    Vector3[] triPositions = {
        new Vector3(0.0204f, 0.0f, 0.0204f),
        new Vector3(0.0204f, 0.0f, -0.0204f),
        new Vector3(-0.0204f, 0.0f, 0.0204f),
        new Vector3(-0.0204f, 0.0f, -0.0204f),
    };
    Vector3 targetPostion, originPostion;
    int targetIndex, originIndex;
    Group targetG;

    int animIndex = 0;

    void Awake() {
        triangles = new List<Triangle>();
        for (int i = 0; i < 4; ++i) {
            GameObject triGameobject = Instantiate(triPreb, transform, false);
            Triangle triangle = triGameobject.GetComponent<Triangle>();
            triangle.transform.position = triPositions[i];
            switch (i) 
            {
                case 0:
                    triangle.transform.rotation = Quaternion.Euler(0, 0, 0);
                break;
                case 1:
                    triangle.transform.rotation = Quaternion.Euler(0, 90, 0);
                break;
                case 2:
                    triangle.transform.rotation = Quaternion.Euler(0, 270, 0);
                break;
                case 3:
                    triangle.transform.rotation = Quaternion.Euler(0, 180, 0);
                break;
            }    
            triangle.setTriIndex(i);
            triangles.Add(triangle);
        }
        status = Type.None;
    }

    public void setGameManager(GameManager manager) {
        gameManager = manager;
        foreach (Triangle triangle in triangles) {
            triangle.setParent(manager, this);    
        }        
    }

    public void setGroupIndex(int groupIn) {
        groupIndex = groupIn;
    }

    public void setColors(int[] colors) {
        for (int i = 0; i < colors.Length; ++i) {
            int color = colors[i];
            triangles[i].setMaterial(color);
        }
    }

    public void onWhiteAction(int[] colors) {
        StartCoroutine(whiteAction(colors));
    }

    IEnumerator whiteAction(int[] colors) {
        setColors(new int[]{6, 6, 6, 6});
        yield return new WaitForSeconds(0.3f);
        setColors(colors);
    }

    public void onFailAction(int cnt, bool isflag) {
        triangles[cnt].onFailAction(isflag);
    }

    public void onRestartAction(int cnt) {
        triangles[cnt].onRestartAction();
    }

    public void onRatateLeft() {
        // animIndex = 0;
        // gameManager.setGameStatus(GameManager.Type.Ready);
        // status = Type.RotateLeft;
        Debug.Log("Rotate Start");
        StartCoroutine(onActionRotateLeft());
    }

    IEnumerator onActionRotateLeft() {
        gameManager.imgHoverRect1.transform.position = transform.position + new Vector3(0.0f, 0.006f, 0.0f);
        yield return new WaitForSeconds(0.1f);
        Debug.Log("Rotate Down");
        gameManager.imgHoverRect1.transform.position = Vector3.zero;

        int[] rows = gameManager.triShowColors[groupIndex];
        int value1 = rows[0];
        int value2 = rows[1];
        int value3 = rows[2];
        int value4 = rows[3];
        rows[0] = value3;
        rows[1] = value1;
        rows[2] = value4;
        rows[3] = value2;
        gameManager.onUpdateColor(2, groupIndex, groupIndex);
    }

    public void onRatateRight() {
        // animIndex = 0;
        // gameManager.setGameStatus(GameManager.Type.Ready);
        // status = Type.RotateRight;
        Debug.Log("Rotate Start");
        StartCoroutine(onActionRotateRight());
    }

    IEnumerator onActionRotateRight() {
        gameManager.imgHoverRect1.transform.position = transform.position + new Vector3(0.0f, 0.006f, 0.0f);
        yield return new WaitForSeconds(0.1f);
        Debug.Log("Rotate Down");
        gameManager.imgHoverRect1.transform.position = Vector3.zero;

        int[] rows = gameManager.triShowColors[groupIndex];
        int value1 = rows[0];
        int value2 = rows[1];
        int value3 = rows[2];
        int value4 = rows[3];
        rows[0] = value2;
        rows[1] = value4;
        rows[2] = value1;
        rows[3] = value3;
        gameManager.onUpdateColor(2, groupIndex, groupIndex);
    }

    public void onChangePost(Group targetGroup, Vector3 target, Vector3 origin, int tIndex, int oIndex) {
        animIndex = 0;
        // gameManager.setGameStatus(GameManager.Type.Ready);
        // status = Type.MoveDest;   

        targetPostion = target;
        originPostion = origin;
        targetIndex = tIndex;
        originIndex = oIndex;
        targetG = targetGroup;

        StartCoroutine(onActionDest());
    }

    IEnumerator onActionDest() {
        gameManager.imgHoverRect2.transform.position = transform.position + new Vector3(0.0f, 0.006f, 0.0f);
        yield return new WaitForSeconds(0.1f);
        gameManager.imgHoverRect2.transform.position = Vector3.zero;

        int[] row1 = gameManager.triShowColors[targetIndex];
        int value11 = row1[0];
        int value12 = row1[1];
        int value13 = row1[2];
        int value14 = row1[3];

        int[] row2 = gameManager.triShowColors[originIndex];
        int value21 = row2[0];
        int value22 = row2[1];
        int value23 = row2[2];
        int value24 = row2[3];
        
        row1[0] = value21;
        row1[1] = value22;
        row1[2] = value23;
        row1[3] = value24;

        row2[0] = value11;
        row2[1] = value12;
        row2[2] = value13;
        row2[3] = value14;
        gameManager.onUpdateColor(1, originIndex, targetIndex);
    }

    public void onHoverGroup() {
        if (isHover) return;
        // Effect.onRunVibrate();
        isHover = true;
        transform.position += new Vector3(0.0f, 0.01f, 0.0f);
    }

    public void onUnHoverGroup() {
        if (!isHover) return;
        isHover = false;
        transform.position += new Vector3(0.0f, -0.01f, 0.0f);
    }

    public void onHoverTriangle(int index, bool isflag) {
        for (int i = 0; i < 4; ++i) {
            if (i == index) continue;
            triangles[i].onRestartAction();
        }
        triangles[index].onFailAction(isflag);
    }

    public void onUnHoverTriangle() {
        foreach (Triangle triangle in triangles) {
            triangle.onRestartAction();    
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (status) {
            case Type.RotateLeft:
                if (animIndex < 5) {
                    transform.position += new Vector3(0.0f, -0.003f, 0.0f);
                } else if (animIndex < 10) {
                    transform.rotation = Quaternion.Euler(0, 18.0f * (animIndex - 4), 0);
                } else if (animIndex < 15) {
                    transform.position += new Vector3(0.0f, 0.003f, 0.0f);
                } else {
                    animIndex = 0;
                    transform.rotation = Quaternion.Euler(0, 0.0f, 0);

                    status = Type.None;
                    int[] rows = gameManager.triShowColors[groupIndex];
                    int value1 = rows[0];
                    int value2 = rows[1];
                    int value3 = rows[2];
                    int value4 = rows[3];
                    rows[0] = value3;
                    rows[1] = value1;
                    rows[2] = value4;
                    rows[3] = value2;
                    gameManager.onUpdateColor(2, groupIndex, groupIndex);
                }
                animIndex++;
            break;
            case Type.RotateRight:
                if (animIndex < 5) {
                    transform.position += new Vector3(0.0f, -0.003f, 0.0f);
                } else if (animIndex < 10) {
                    transform.rotation = Quaternion.Euler(0, -18.0f * (animIndex - 4), 0);
                } else if (animIndex < 15) {
                    transform.position += new Vector3(0.0f, 0.003f, 0.0f);
                } else {
                    animIndex = 0;
                    transform.rotation = Quaternion.Euler(0, 0.0f, 0);

                    status = Type.None;
                    int[] rows = gameManager.triShowColors[groupIndex];
                    int value1 = rows[0];
                    int value2 = rows[1];
                    int value3 = rows[2];
                    int value4 = rows[3];
                    rows[0] = value2;
                    rows[1] = value4;
                    rows[2] = value1;
                    rows[3] = value3;
                    gameManager.onUpdateColor(2, groupIndex, groupIndex);
                }
                animIndex++;
            break;
            case Type.MoveDest:
                if (animIndex < 10) {
                    transform.position += new Vector3((targetPostion.x - originPostion.x) / 10, 0.0f, (targetPostion.z - originPostion.z) / 10);
                } else if (animIndex < 15) {
                    transform.position += new Vector3(0.0f, -0.004f, 0.0f);
                } else {
                    animIndex = 0;
                    transform.position = originPostion;
                    targetG.transform.position = targetPostion;
                    status = Type.None;

                    int[] row1 = gameManager.triShowColors[targetIndex];
                    int value11 = row1[0];
                    int value12 = row1[1];
                    int value13 = row1[2];
                    int value14 = row1[3];

                    int[] row2 = gameManager.triShowColors[originIndex];
                    int value21 = row2[0];
                    int value22 = row2[1];
                    int value23 = row2[2];
                    int value24 = row2[3];
                    
                    row1[0] = value21;
                    row1[1] = value22;
                    row1[2] = value23;
                    row1[3] = value24;

                    row2[0] = value11;
                    row2[1] = value12;
                    row2[2] = value13;
                    row2[3] = value14;
                    gameManager.onUpdateColor(1, originIndex, targetIndex);
                }
                animIndex++;
            break;
        }
    }
    
}
