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
public class ScoreEntry
{
    public int score;
    public string name;
}