using System;
using System.Collections.Generic;

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
    public ScoreData()
    {

    }

    public List<ScoreEntry> GetScoresByNumShips(int numShips)
    {
        return scoreData[numShips];
    }

    public void AddScoreByNumShips(int numShips, ScoreEntry score)
    {
        scoreData[numShips].Add(score);
    }
}

[Serializable]
public class ScoreEntry : IComparable
{
    public int score;
    public string name;

    int IComparable.CompareTo(object obj)
    {
        ScoreEntry otherScore = obj as ScoreEntry;
        if (otherScore.score > score)
        {
            return 1;
        }
        else
        {
            return -1;
        }
    }
}