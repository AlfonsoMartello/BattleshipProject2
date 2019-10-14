using System;
using System.Collections.Generic;

[Serializable]
public class ScoreData
{
    Dictionary<int, List<int>> scoreData = new Dictionary<int, List<int>>()
    {
        {1, new List<int>() },
        {2, new List<int>() },
        {3, new List<int>() },
        {4, new List<int>() },
        {5, new List<int>() }
    };
    public ScoreData()
    {

    }

    public List<int> GetScoresByNumShips(int numShips)
    {
        return scoreData[numShips];
    }

    public void AddScoreByNumShips(int numShips, int score)
    {
        scoreData[numShips].Add(score);
    }
}