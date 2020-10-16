using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game
{
   public string id;
   public string userId;
   public int matchCount = 0; 
   public int during = 0;
   public string startDate;
   public string rank;
   public string triString;
   public string historyTime;
   public string moveStart;
   public string moveEnded;
   public string rotationDirection;
   public string historyType;


   public Game() {

   }

    public Game(string id, 
        string userId, 
        int matchCount, 
        int during, 
        string startDate, 
        List<List<int[]>> triHistories,
        List<int> historyTimes,
        List<int> moveStarts,
        List<int> moveEndeds,
        List<int> rotationDirections,
        List<int> hitoryTypes
        )
    {
        this.id = id;
        this.rank = matchCount.ToString("00") + during.ToString("00000");
        this.userId = userId;
        this.matchCount = matchCount;
        this.during = during;
        this.startDate = startDate;
        this.triString = "";
        foreach (List<int[]> colorList in triHistories) {
            string colorListString = "";    
            foreach (int[] colors in colorList) {                
                string color = "";
                color = colors[0] + "," + colors[1] + "," + colors[2] + "," + colors[3];
                colorListString = colorListString + color + ":";
            }
            this.triString = this.triString + colorListString + ";";
        }
        this.historyTime = "";
        foreach (int historyTime in historyTimes) {
            this.historyTime = this.historyTime + historyTime + ",";
        }
        this.moveStart = "";
        foreach (int moveStart in moveStarts) {
            this.moveStart = this.moveStart + moveStart + ",";
        }
        this.moveEnded = "";
        foreach (int moveEnded in moveEndeds) {
            this.moveEnded = this.moveEnded + moveEnded + ",";
        }
        this.rotationDirection = "";
        foreach (int rotationDirection in rotationDirections) {
            this.rotationDirection = this.rotationDirection + rotationDirection + ",";
        }
        this.historyType = "";
        foreach (int historyType in hitoryTypes) {
            this.historyType = this.historyType + historyType + ",";
        }
    }

    public List<List<int[]>> getTriHistories() {
        List<List<int[]>> triHistories = new List<List<int[]>>();
        string[] colorListString = triString.Split(char.Parse(";"));
        for (int i = 0; i < colorListString.Length - 1; i++) {
            string[] colorsString = colorListString[i].Split(char.Parse(":"));
            List<int[]> colorList = new List<int[]>();
            for (int j = 0; j < colorsString.Length - 1; j++) {
                string[] strings = colorsString[j].Split(char.Parse(","));
                colorList.Add(new int[] { IntParseFast(strings[0]), IntParseFast(strings[1]), IntParseFast(strings[2]), IntParseFast(strings[3])});
            }
            triHistories.Add(colorList);
        }
        return triHistories;
    }

    public List<int> getHistoryTimes() {
        List<int> historyTimes = new List<int>();
        string[] historyTimeStrs = historyTime.Split(char.Parse(","));
        for (int i = 0; i < historyTimeStrs.Length - 1; i++) {
            string historyTimeStr = historyTimeStrs[i];
            historyTimes.Add(IntParseFast(historyTimeStr));
        }
        Debug.Log("historyTimeStr : " + historyTimes.Count);
        return historyTimes;
    }

    public List<int> getMoveStarts() {
        List<int> moveStarts = new List<int>();
        string[] moveStartStrs = moveStart.Split(char.Parse(","));
        for (int i = 0; i < moveStartStrs.Length - 1; i++) {
            string moveStartStr = moveStartStrs[i];
            moveStarts.Add(IntParseFast(moveStartStr));
        }
        return moveStarts;
    }

    public List<int> getMoveEndeds() {
        List<int> moveEndeds = new List<int>();
        string[] moveEndedStrs = moveEnded.Split(char.Parse(","));
        for (int i = 0; i < moveEndedStrs.Length - 1; i++) {
            string moveEndedStr = moveEndedStrs[i];
            moveEndeds.Add(IntParseFast(moveEndedStr));
        }
        return moveEndeds;
    }

    public List<int> getHistoryTypes() {
        List<int> historyTypes = new List<int>();
        string[] historyTypeStrs = historyType.Split(char.Parse(","));
        for (int i = 0; i < historyTypeStrs.Length - 1; i++) {
            string historyTypeStr = historyTypeStrs[i];
            historyTypes.Add(IntParseFast(historyTypeStr));
        }
        return historyTypes;
    }

    public List<int> getRotateDirections() {
        List<int> rotaionDirections = new List<int>();
        string[] rotationDirectionStrs = rotationDirection.Split(char.Parse(","));
        for (int i = 0; i < rotationDirectionStrs.Length - 1; i++) {
            string rotationDirectionStr = rotationDirectionStrs[i];
            rotaionDirections.Add(IntParseFast(rotationDirectionStr));
        }
        return rotaionDirections;
    }

    public static int IntParseFast(string value)
    {
        int result = 0;
            for (int i = 0; i < value.Length; i++)
            {
                char letter = value[i];
                result = 10 * result + (letter - 48);
            }
        return result;
    }

}
