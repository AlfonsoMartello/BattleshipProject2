using System;
using System.Collections.Generic;

/**
* @class description: Seralizable ScoreData to be written to file to store
* previous game files
*/
[Serializable]
public class ScoreData
{
    Dictionary<int, List<ScoreEntry>> scoreData = new Dictionary<int, List<ScoreEntry>>()
    {
        {1, new List<ScoreEntry>() },
        {2, new List<ScoreEntry>() },
        {3, new List<ScoreEntry>() },
        {4, new List<ScoreEntry>() },
        {5, new List<ScoreEntry>() }
    };

    /**
    * @pre Object is intialized.
    * @post None.
    * @param Index for number of ships.
    * @return ScoreData corresponding to the specified number of ships.
    */
    public List<ScoreEntry> GetScoresByNumShips(int numShips)
    {
        return scoreData[numShips];
    }

    /**
    * @pre Object is initalized.
    * @post Adds new score entry.
    * @param Nummber of ships and score.
    * @return None.
    */
    public void AddScoreByNumShips(int numShips, ScoreEntry score)
    {
        scoreData[numShips].Add(score);
    }
}

/**
* @class description: Seralizable ScoreEntry to be written to file to store
* a specific game entry
*/
[Serializable]
public class ScoreEntry : IComparable
{
    public int score;
    public string name;

    /**
    * @pre Both objects are intialized.
    * @post None.
    * @param Other object that can be interpreted as a ScoreEntry
    * @return The smaller object based on their score value.
    */
    int IComparable.CompareTo(object obj)
    {
        ScoreEntry otherScore = obj as ScoreEntry;
        if (otherScore.score > score)
        {
            return -1;
        }
        else
        {
            return 1;
        }
    }
}