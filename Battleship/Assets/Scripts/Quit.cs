using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
* @class description: Quit handles exiting the game when the ExitGame button is pressed and Toggle ScoreBoard Button is pressed
* UI buttons
* @libraries: Libary used for UI is UnityEngine.UI
*/

public class Quit : MonoBehaviour
{
    public GameObject ScoreBoardPanel;

    /**
    * @pre: Start is called before the first frame update.
    * @post Start will be ready to handle Quit button pressed events.
    * @param: None.
    * @return: None.
    */
    private void Start()
    {
        ScoreBoardPanel.SetActive(false);
    }

    /**
    * @pre: A player press the ExitGame Button
    * @post Close the game window and exit the game
    * @param: None.
    * @return: None.
    */
    public void ExitGame()
    {
        Debug.Log("EXIT THE GAME! BYE!!");
        Application.Quit();
    }

    /**
    * @pre: A player press the Toggle ScoreBoard Button
    * @post: Open the ScoreBoardPanel when the ScoreBoardPanel is not active and vice versa.
    * @param: None.
    * @return: None.
    */


    public void openScoreBoardPanel()
    {
        if (ScoreBoardPanel != null)
        {
            bool isActive = ScoreBoardPanel.activeSelf;

            ScoreBoardPanel.SetActive(!isActive);
        }
    }
}
