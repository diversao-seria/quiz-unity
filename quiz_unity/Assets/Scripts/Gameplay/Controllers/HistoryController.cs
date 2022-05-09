using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class HistoryController : MonoBehaviour
{
    public struct HistoryEntry{
        public string name;
        public string time;
        public string score;
    }



    public string history = "CODIGO_60_0.6$CODIGO_50_0.3$CODIGO_20_0.9$";
    
    public const int MAX_HISTORY_ENTRIES = 3;


    
    void Awake()
    {
        history = PlayerPrefs.GetString("history");

        BuildHistory(history);

        //hide Achievments panel (provisorio)
        GameObject.Find("AchievmentsWrapper").gameObject.SetActive(false);
    }

    public string ConvertEntryToString(HistoryEntry entry){
        StringBuilder builder = new StringBuilder();
        builder.Append(entry.name);
        builder.Append("_");
        builder.Append(entry.time.ToString());
        builder.Append("_");
        builder.Append(entry.score.ToString());
        builder.Append('$');

        return builder.ToString();
    }

    public HistoryEntry ConvertStringToEntry(string entry){
        HistoryEntry newEntry = new HistoryEntry();
        string[] entrySplit = entry.Split('_');
        newEntry.name = entrySplit[0];
        newEntry.time = entrySplit[1];
        newEntry.score = entrySplit[2];

        return newEntry;
    }

    // __$__$__$

    void BuildHistory(string history){
        string[] historySplit = history.Split('$');
        int i = 0;
        foreach(string entry in historySplit){
            if(entry != ""){
                i++;

                // Entry = NOME1_TEMPO1_SCORE1
                HistoryEntry newEntry = ConvertStringToEntry(entry);
                Debug.Log(newEntry.name + " " + newEntry.time + " " + newEntry.score);

                // Find gameobject in scene and substitute text with a new entry
                GameObject name = GameObject.Find("Name" + i);
                GameObject time = GameObject.Find("Time" + i);
                GameObject score = GameObject.Find("Score" + i);
                name.GetComponent<Text>().text = newEntry.name;
                time.GetComponent<Text>().text = newEntry.time.ToString();
                score.GetComponent<Text>().text = newEntry.score.ToString();
            }
        }

        // verify if there are less entries than MAX_HISTORY_ENTRIES
        if(i < MAX_HISTORY_ENTRIES){
            for(int j = i+1; j <= MAX_HISTORY_ENTRIES; j++){
                GameObject name = GameObject.Find("Name" + j);
                GameObject time = GameObject.Find("Time" + j);
                GameObject score = GameObject.Find("Score" + j);
                name.GetComponent<Text>().text = "Não há registros";
                time.GetComponent<Text>().text = "Não há registros";	
                score.GetComponent<Text>().text = "Não há registros";       
            }
        }

    }

    public void UpdateHistory(HistoryEntry historyEntry){
        string addedEntry = ConvertEntryToString(historyEntry);
        Debug.Log(addedEntry);
        string history = PlayerPrefs.GetString("history");

        if(history == ""){
            history = addedEntry;
        }else{
            history = addedEntry + history;
        }

        string[] historySplit = history.Split('$');
        if (historySplit.Length > MAX_HISTORY_ENTRIES){
            history = "";
            history = historySplit[0] + "$" + historySplit[1] + "$" + historySplit[2] + "$";
        }

        PlayerPrefs.SetString("history", history);
    }
}
