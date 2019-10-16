using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

/**
* @class description: Controls ScoreBoardPanel view
*/
public class ScoreBoardPanel : MonoBehaviour
{

    public Dropdown numShipsSelector;
    public List<GameObject> ScoreList;
    private ScoreData scoreData;
    private int numShips = 1;

    /**
    * @pre None.
    * @post Adds event listener for dropdown and renders the list with 1 ship selected.
    * @param None.
    * @return None.
    */
    void Start()
    {
        numShipsSelector.onValueChanged.AddListener(onNumShipsSelection);

        loadData();
        updateListView();
    }

    /**
    * @pre loadData has been called.
    * @post Displays top 5 scores based to score board view.
    * @param None.
    * @return None.
    */
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

    /**
    * @pre View is rendered.
    * @post Reloads list view with the newly selected number of ships
    * @param Number of ships choice selection from the view.
    * @return None.
    */
    private void onNumShipsSelection(int choice)
    {
        numShips = choice + 1;
        updateListView();
    }

    /**
    * @pre None.
    * @post Reads saved games data from file and stores it in scoreData.
    * @param None.
    * @return None.
    */
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
