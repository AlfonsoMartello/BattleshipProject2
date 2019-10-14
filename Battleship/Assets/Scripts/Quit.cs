using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quit : MonoBehaviour
{   public void ExitGame()
    {
        Debug.Log("EXIT THE GAME! BYE!!");
        Application.Quit();
    }

    public GameObject ScoreBoardPanel;
    public GameObject PlayerTurn;
    public void openScoreBoardPanel()
    {
        if (ScoreBoardPanel != null)
        {
            bool isActive = ScoreBoardPanel.activeSelf;
 
            ScoreBoardPanel.SetActive(!isActive);
        }
    }
}
