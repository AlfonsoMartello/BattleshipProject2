using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
* @class description: BoardInteraction handles grid button presses and the Fire/Yes/Confirm/Start
* UI buttons
* @libraries: Libary used for UI is UnityEngine.UI
*/
public class BoardInteraction : MonoBehaviour
{
    public int player1Shots, player2Shots = 0; // !< Number of shots taken by each player
    public Sprite[] onClickIcons; //!< Array of sprites for 'Hit', 'Miss', and 'Mark'
    public Button[] spacesAvailableBoard1; //!< Array of buttons for Player1's board.
    public Button[] spacesAvailableBoard2; //!< Array of buttons for Player2's board.
    public bool player1Turn = true, player2Turn = false; //!< Player1 forced to go first. Switch after players use up their turn.
    public UnityEngine.UI.Button yesButton, fireButton, confirmButton, startButton; //!< Button objects for boardInteraction event listeners
    public GameObject gameUIPanel, battleshipGrids, switchPanel, player1Board, player2Board, playerTurn; //!< GameObjects for UI panels


    /**
    * @pre: Start is called before the first frame update.
    * @post Start will be ready to handle UI button pressed events.
    * @param: None.
    * @return: None.
    */
    private void Start()
    {

        yesButton.onClick.AddListener(YesButtonReset);
        fireButton.onClick.AddListener(FireButtonLockIn);
        confirmButton.onClick.AddListener(ConfirmButtonInteractableOff);
        startButton.onClick.AddListener(StartButtonCommencePlay);


    }

    /**
    * @pre: On-click function for the array of buttons that make up the battleship grid for Player1.
    * @post: On-click places a checkmark to let the user know which coordinate they currently have
    * picked on the Player1 battleship grid.
    * @param: Num is the index of the button pressed on-click in spacesAvailableBoard1
    * to set the sprite to a checkmark.
    * @return: None.
    */
    public void BattleshipSpacesBoard1(int num)
    {
        if (player1Turn)
        {
            for(int i = 0; i < spacesAvailableBoard1.Length; i++)
            {
                if(spacesAvailableBoard1[i].image.sprite == null)
                {
                    spacesAvailableBoard1[i].interactable = true;
                }

                if (spacesAvailableBoard1[i].image.sprite == onClickIcons[2])
                {
                    spacesAvailableBoard1[i].image.sprite = null;
                    spacesAvailableBoard1[i].interactable = true;
                }
            }
            spacesAvailableBoard1[num].image.sprite = onClickIcons[2];
            spacesAvailableBoard1[num].interactable = false;
        }
    }

    /**
    * @pre: On-click function for the array of buttons that make up the battleship grid for Player2.
    * @post: On-click places a checkmark to let the user know which coordinate they currently have
    * picked on the Player2 battleship grid.
    * @param: Num is the index of the button pressed on-click in spacesAvailableBoard2 
    * to set the sprite to a checkmark.
    * @return: None.
    */
    public void BattleshipSpacesBoard2(int num)
    {
        if (player2Turn)
        {

            for (int i = 0; i < spacesAvailableBoard2.Length; i++)
            {
                if (spacesAvailableBoard2[i].image.sprite == null)
                {
                    spacesAvailableBoard2[i].interactable = true;
                }

                else if (spacesAvailableBoard2[i].image.sprite == onClickIcons[2])
                {
                    spacesAvailableBoard2[i].image.sprite = null;
                    spacesAvailableBoard2[i].interactable = true;
                }
            }


            spacesAvailableBoard2[num].image.sprite = onClickIcons[2];
            spacesAvailableBoard2[num].interactable = false;
        }
    }

    /**
    * @pre: A player presses the 'Yes' button to exit to the main menu.
    * @post: The buttons' sprites get reset to nothing, sets the buttons to be interactable
    * for the next game.
    * @param: None.
    * @return: None.
    */
    public void YesButtonReset()
    {
        for(int i = 0; i < spacesAvailableBoard1.Length; i++)
        {
            spacesAvailableBoard1[i].interactable = true;
            spacesAvailableBoard1[i].GetComponent<Image>().sprite = null;
            spacesAvailableBoard2[i].interactable = true;
            spacesAvailableBoard2[i].GetComponent<Image>().sprite = null;
            player1Turn = true;
            player2Turn = true;
        }
    }

    /**
    * @pre: A player presses the 'Fire' button
    * @post: A player, depending on whose turn it is, locks in their choice to fire,
    * turning the temporary checkmark into a 'Miss' or 'Hit' sprite on the button they chose to attack.
    * @param: None.
    * @return: None.
    */
    public void FireButtonLockIn()
    {
        bool hasPlayed1 = false, hasPlayed2 = false;
        
        if (player1Turn)
        {
            for (int i = 0; i < spacesAvailableBoard1.Length; i++)
            {

                if (spacesAvailableBoard1[i].image.sprite == onClickIcons[2])
                {
                    if(spacesAvailableBoard1[i].GetComponent<buttonController>().target == null)
                    {
                         spacesAvailableBoard1[i].image.sprite = onClickIcons[0]
;                        hasPlayed1 = true;
                    }

                    else
                    {
                        spacesAvailableBoard1[i].GetComponent<buttonController>().target.Hit();
                        spacesAvailableBoard1[i].image.sprite = onClickIcons[1];
                        hasPlayed1 = true;
                    }
                    break;
                }
            }

            for (int i = 0; i < spacesAvailableBoard1.Length; i++)
            {
                if (hasPlayed1)
                {
                    spacesAvailableBoard1[i].interactable = false;
                    if (spacesAvailableBoard2[i].image.sprite == null)
                    {
                        spacesAvailableBoard2[i].interactable = true;
                    }
                }
                else
                {
                    if (spacesAvailableBoard1[i].image.sprite == null)
                    {
                        spacesAvailableBoard1[i].interactable = true;
                    }
                }
            }

            if(hasPlayed1)
            {
                player1Turn = false;
                player1Board.GetComponent<Image>().enabled = false;
                player2Turn = true;
                player2Board.GetComponent<Image>().enabled = true;
                gameUIPanel.SetActive(false);
                battleshipGrids.SetActive(false);
                switchPanel.SetActive(true);
                playerTurn.GetComponent<Text>().text = "It's Player 1's Turn";
                player2Shots++;
            }
            else
            {
                player1Turn = true;
                player1Board.GetComponent<Image>().enabled = true;
                player2Turn = false;
                player2Board.GetComponent<Image>().enabled = false;
                gameUIPanel.SetActive(false);
                battleshipGrids.SetActive(false);
                switchPanel.SetActive(true);
                playerTurn.GetComponent<Text>().text = "It's Player 2's Turn";
            }
        }

        else if (player2Turn)
        {
            for (int i = 0; i < spacesAvailableBoard2.Length; i++)
            {

                if (spacesAvailableBoard2[i].image.sprite == onClickIcons[2])
                {
                    if (spacesAvailableBoard2[i].GetComponent<buttonController>().target == null)
                    {
                        spacesAvailableBoard2[i].image.sprite = onClickIcons[0];
                        hasPlayed2 = true;
                    }

                    else
                    {
                        spacesAvailableBoard2[i].GetComponent<buttonController>().target.Hit();
                        spacesAvailableBoard2[i].image.sprite = onClickIcons[1];
                        hasPlayed2 = true;
                    }
                    break;
                }
            }

            for (int i = 0; i < spacesAvailableBoard2.Length; i++)
            {
                if (hasPlayed2)
                {
                    spacesAvailableBoard2[i].interactable = false;
                    if (spacesAvailableBoard1[i].image.sprite == null)
                    {
                        spacesAvailableBoard1[i].interactable = true;
                    }
                }
                else
                {
                    if (spacesAvailableBoard2[i].image.sprite == null)
                    {
                        spacesAvailableBoard2[i].interactable = true;
                    }
                }
            }

            if (hasPlayed2)
            {
                player1Turn = true;
                player1Board.GetComponent<Image>().enabled = true;
                player2Turn = false;
                player2Board.GetComponent<Image>().enabled = false;
                battleshipGrids.SetActive(false);
                gameUIPanel.SetActive(false);
                switchPanel.SetActive(true);
                playerTurn.GetComponent<Text>().text = "It's Player 2's Turn";
                player1Shots++;
            }
            else
            {
                player1Turn = false;
                player1Board.GetComponent<Image>().enabled = false;
                player2Turn = true;
                player2Board.GetComponent<Image>().enabled = true;
                battleshipGrids.SetActive(false);
                gameUIPanel.SetActive(false);
                switchPanel.SetActive(true);
                playerTurn.GetComponent<Text>().text = "It's Player 1's Turn";
            }
        }
    }

    /**
    * @pre: A player presses the 'Confirm' button on the ship selection panel.
    * @post: Sets both boards to not be interactive, making sure players don't press buttons
    * while on the ship selection panel.
    * @param: None.
    * @return: None.
    */
    public void ConfirmButtonInteractableOff()
    {
        for(int i = 0; i < spacesAvailableBoard1.Length; i++)
        {
            spacesAvailableBoard1[i].interactable = false;
            spacesAvailableBoard2[i].interactable = false;
        }
    }

    /**
    * @pre: A player presses the 'Start' button after setting their ships.
    * @post: Sets Player1's board to interactive and Player2's board to NOT be interactive,
    * forcing whoever is playing as Player1 to go first. This also sets the buttons' sprites to nothing.
    * @param: None.
    * @return: None.
    */
    public void StartButtonCommencePlay()
    {
        for(int i = 0; i < spacesAvailableBoard1.Length; i++)
        {
            spacesAvailableBoard1[i].interactable = true;
            player1Board.GetComponent<Image>().enabled = true;
            spacesAvailableBoard2[i].interactable = false;
            player2Board.GetComponent<Image>().enabled = false;
            spacesAvailableBoard1[i].image.sprite = null;
            spacesAvailableBoard2[i].image.sprite = null;
        }
    }
}
