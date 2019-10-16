using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBoardPanel : MonoBehaviour
{

    public Dropdown numShipsSelector;
    public List<GameObject> ScoreList;
    private ScoreData scoreData;
    private int numShips = 1;
    // Start is called before the first frame update
    void Start()
    {
        numShipsSelector.onValueChanged.AddListener(onNumShipsSelection);

        loadData();
        updateListView();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void updateListView()
    {
        List<ScoreEntry> scoreEntries = scoreData.GetScoresByNumShips(numShips);
        scoreEntries.Sort();

        for (int i = 0; i < ScoreList.Capacity; i++)
        {
            if (i < scoreEntries.Capacity)
            {
                try // Pay no attention to catching this out of bounds exception
                {
                    ScoreList[i].GetComponentsInChildren<Text>()[0].text = scoreEntries[i].name;
                    ScoreList[i].GetComponentsInChildren<Text>()[1].text = scoreEntries[i].score.ToString();
                }
                catch {}
            }
            else
            {
                ScoreList[i].GetComponentsInChildren<Text>()[0].text = "";
                ScoreList[i].GetComponentsInChildren<Text>()[1].text = "";
            }

        } 
    }

    private void onNumShipsSelection(int choice)
    {
        numShips = choice + 1;
        updateListView();
    }

    private void loadData()
    {
        BinaryFormatter bf = new BinaryFormatter();
        scoreData = new ScoreData();
        if (File.Exists(Application.persistentDataPath + "/savedGames.gd"))
        {
            FileStream readFile = File.Open(Application.persistentDataPath + "/savedGames.gd", FileMode.Open);
            scoreData = (ScoreData)bf.Deserialize(readFile);
            readFile.Close();
        }
    }
}
