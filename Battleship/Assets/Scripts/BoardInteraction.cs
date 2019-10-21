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
    public TeamController Team1;
    public TeamController Team2;

    public int previousHitIndex = -1;
    public int currentDirection = 0; //0 continues Right, 1 continues Down, 2 continues Left, 3 continues Up || defaults to Right
    public int[] targetArray = new int[4];
    public List<int> currentShip = new List<int>();
    public int shotsIndex = 0;

    public List<int> totalShots = new List<int>();
    public int randomIndex;
    public int listsize = -1;

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
        if (player1Turn) //Actually player 2's turn
        {
            for (int i = 0; i < spacesAvailableBoard1.Length; i++)
            {
                if (spacesAvailableBoard1[i].image.sprite == null)
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
        for (int i = 0; i < spacesAvailableBoard1.Length; i++)
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
        if (Team2.aiDifficulty == 0)
        {
            playerGame();
        }
        else if (Team2.aiDifficulty == 1)
        {
            AIeasyGame();
        }
        else if (Team2.aiDifficulty == 2)
        {
            AInormalGame();
        }
        else if (Team2.aiDifficulty == 3)
        {
            AIhardGame();
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
        for (int i = 0; i < spacesAvailableBoard1.Length; i++)
        {
            spacesAvailableBoard1[i].interactable = false;
            spacesAvailableBoard2[i].interactable = false;
        }
    }

    /**
    * @pre: A player presses the 'Start' button after setting their ships.
    * @post: Sets Player1's board to interactive and Player2's board to NOT be interactive,
    * forcing whoever is playing as Player2 to go first. This also sets the buttons' sprites to nothing.
    * @param: None.
    * @return: None.
    */
    public void StartButtonCommencePlay()
    {
        for (int i = 0; i < spacesAvailableBoard1.Length; i++)
        {
            spacesAvailableBoard1[i].interactable = true;
            player1Board.GetComponent<Image>().enabled = true;
            spacesAvailableBoard2[i].interactable = false;
            player2Board.GetComponent<Image>().enabled = false;
            spacesAvailableBoard1[i].image.sprite = null;
            spacesAvailableBoard2[i].image.sprite = null;
        }
    }

    public void playerGame()
    {
        bool hasPlayed1 = false, hasPlayed2 = false;

        if (player1Turn) //actually player 2 turn
        {
            for (int i = 0; i < spacesAvailableBoard1.Length; i++)
            {

                if (spacesAvailableBoard1[i].image.sprite == onClickIcons[2]) //checks for hit
                {
                    if (spacesAvailableBoard1[i].GetComponent<buttonController>().target == null) //if the selected button does not contain a ship part. target is an instance of ShipPartController.
                    {
                        spacesAvailableBoard1[i].image.sprite = onClickIcons[0];
                        hasPlayed1 = true;
                    }

                    else
                    {
                        spacesAvailableBoard1[i].GetComponent<buttonController>().target.Hit(); //sets the ShipPartController as hit and checks if the player has lost
                        spacesAvailableBoard1[i].image.sprite = onClickIcons[1];
                        hasPlayed1 = true;
                    }
                    break;
                }
            }

            for (int i = 0; i < spacesAvailableBoard1.Length; i++) //sets up next player's turn
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

            if (hasPlayed1) //changes player turn panel text and interaction. Prevents other panels from appearing. Shows switch panel string. The "Continue" button (not in this script) changes viewed panel.
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

        else if (player2Turn) //actually player 1 turn
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

    public void AIeasyGame()
    {
        bool hasPlayed1 = false, hasPlayed2 = false;



        if (player1Turn) //actually player 2 turn
        {
            //THIS LINE IS NEW
            List<int> listNumbers = new List<int>();
            //int randomIndex;
            do
            {
                randomIndex = Random.Range(0, spacesAvailableBoard1.Length - 1);
            } while (listNumbers.Contains(randomIndex));
            listNumbers.Add(randomIndex);

            if (spacesAvailableBoard1[randomIndex].GetComponent<buttonController>().target == null) //if the selected button does not contain a ship part. target is an instance of ShipPartController.
            {
                spacesAvailableBoard1[randomIndex].image.sprite = onClickIcons[0];
                hasPlayed1 = true;
            }

            else
            {
                spacesAvailableBoard1[randomIndex].GetComponent<buttonController>().target.Hit(); //sets the ShipPartController as hit and checks if the player has lost
                spacesAvailableBoard1[randomIndex].image.sprite = onClickIcons[1];
                hasPlayed1 = true;
            }

            // until this line
            for (int i = 0; i < spacesAvailableBoard1.Length; i++) //sets up next player's turn
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
            if (hasPlayed1) //changes player turn panel text and interaction. Prevents other panels from appearing. Shows switch panel string. The "Continue" button (not in this script) changes viewed panel.
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
        else if (player2Turn) //actually player 1 turn
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
    public int[] CheckEdges(int index)
    {
        int[] temp = new int[4];

        if ((index + 1) % 8 == 0)
        {
            temp[0] = -1;
        }
        else
        {
            temp[0] = index + 1;
        }

        if ((index + 8) > 63)
        {
            temp[1] = -1;
        }
        else
        {
            temp[1] = index + 8;
        }

        if ((index - 1) % 8 == 7)
        {
            temp[2] = -1;
        }
        else
        {
            temp[2] = index - 1;
        }

        if ((index - 8) < 0)
        {
            temp[3] = -1;
        }
        else
        {
            temp[3] = index - 8;
        }
        return temp;
    }

    public int CheckShot(int index)
    {
        int nextShot = 0;
        if (currentDirection == 0)
        {
            if (((index + 1) % 8 == 0) && ((index + 1) > 63))
            {
                nextShot = -1;
            }
            else
            {
                nextShot = index + 1;
            }

        }

        else if (currentDirection == 1)
        {
            if ((index + 8) > 63)
            {
                nextShot = -1;
            }
            else
            {
                nextShot = index + 8;
            }
        }

        else if (currentDirection == 2)
        {
            if (((index - 1) % 8 == 7) && ((index - 1) < 0))
            {
                nextShot = -1;
            }
            else
            {
                nextShot = index - 1;
            }
        }

        else if (currentDirection == 3)
        {
            if ((index - 8) < 0)
            {
                nextShot = -1;
            }
            else
            {
                nextShot = index - 8;
            }
        }
        Debug.Log(nextShot);
        return nextShot;
    }

    public void AInormalGame()
    {
        bool hasPlayed1 = false, hasPlayed2 = false;

        if (player1Turn) //actually player 2 turn
        {
            Debug.Log("We made it!");
            //if(listsize > -1)
            //{
            //    Debug.Log(totalShots[listsize]);
            //}
            

            List<int> listNumbers = new List<int>();

            if (previousHitIndex == -1)
            {
                Debug.Log("a - no previous ship hit - random shots");

                do
                {
                    Debug.Log("a1 - choose random index");
                    randomIndex = Random.Range(0, spacesAvailableBoard1.Length - 1);
                } while (listNumbers.Contains(randomIndex));
                listNumbers.Add(randomIndex);

                if (totalShots.Contains(randomIndex) == false)
                {
                    Debug.Log("b - shot has not been fired before");
                    if (spacesAvailableBoard1[randomIndex].GetComponent<buttonController>().target == null) //if the selected button does not contain a ship part. target is an instance of ShipPartController.
                    {
                        Debug.Log("b1 - miss");
                        spacesAvailableBoard1[randomIndex].image.sprite = onClickIcons[0];
                        hasPlayed1 = true;

                        listsize ++ ;
                        


                    }

                    else
                    {
                        Debug.Log("c - hit");
                        spacesAvailableBoard1[randomIndex].GetComponent<buttonController>().target.Hit(); //sets the ShipPartController as hit and checks if the player has lost
                        spacesAvailableBoard1[randomIndex].image.sprite = onClickIcons[1];
                        hasPlayed1 = true;

                        listsize++;

                        previousHitIndex = randomIndex;
                        targetArray = CheckEdges(previousHitIndex);
                        currentShip.Add(previousHitIndex);
                        shotsIndex++;
                    }

                    totalShots.Add(randomIndex);
                }
                else
                {
                    Debug.Log("e - shot was already fired choose again");
                    while (totalShots.Contains(randomIndex))
                    {
                        Debug.Log("e1 - choose random index");
                        randomIndex = Random.Range(0, spacesAvailableBoard1.Length - 1);
                    }

                    if (spacesAvailableBoard1[randomIndex].GetComponent<buttonController>().target == null) //if the selected button does not contain a ship part. target is an instance of ShipPartController.
                    {
                        Debug.Log("f - miss");
                        spacesAvailableBoard1[randomIndex].image.sprite = onClickIcons[0];
                        hasPlayed1 = true;
                        listsize++;

                    }

                    else
                    {
                        Debug.Log("g - hit");
                        spacesAvailableBoard1[randomIndex].GetComponent<buttonController>().target.Hit(); //sets the ShipPartController as hit and checks if the player has lost
                        spacesAvailableBoard1[randomIndex].image.sprite = onClickIcons[1];
                        hasPlayed1 = true;
                        listsize++;

                        previousHitIndex = randomIndex;
                        targetArray = CheckEdges(previousHitIndex);
                        currentShip.Add(previousHitIndex);
                        shotsIndex++;
                    }

                    totalShots.Add(randomIndex);

                }


            }

            else
            {
                Debug.Log("h - a ship has been hit - continue path");
                if (currentDirection > 3)
                {
                    Debug.Log("i - all directions shot at - random shooting");
                    currentDirection = 0;
                    previousHitIndex = -1;
                    currentShip.Clear();
                    shotsIndex = 0;


                    if (totalShots.Contains(randomIndex) == false)
                    {
                        Debug.Log("j - shot has not been fired before");
                        if (spacesAvailableBoard1[randomIndex].GetComponent<buttonController>().target == null) //if the selected button does not contain a ship part. target is an instance of ShipPartController.
                        {
                            Debug.Log("j1 - miss");
                            spacesAvailableBoard1[randomIndex].image.sprite = onClickIcons[0];
                            hasPlayed1 = true;
                            listsize++;


                        }

                        else
                        {
                            Debug.Log("k - hit");
                            spacesAvailableBoard1[randomIndex].GetComponent<buttonController>().target.Hit(); //sets the ShipPartController as hit and checks if the player has lost
                            spacesAvailableBoard1[randomIndex].image.sprite = onClickIcons[1];
                            hasPlayed1 = true;
                            listsize++;

                            previousHitIndex = randomIndex;
                            targetArray = CheckEdges(previousHitIndex);
                            currentShip.Add(previousHitIndex);
                            shotsIndex++;
                        }

                        totalShots.Add(randomIndex);
                    }
                    else
                    {
                        Debug.Log("l - shot has been fired before");
                        while (totalShots.Contains(randomIndex))
                        {
                            Debug.Log("l2 - choose random again");
                            randomIndex = Random.Range(0, spacesAvailableBoard1.Length - 1);
                        }

                        currentShip.Clear();
                        shotsIndex = 0;

                        if (spacesAvailableBoard1[randomIndex].GetComponent<buttonController>().target == null) //if the selected button does not contain a ship part. target is an instance of ShipPartController.
                        {
                            Debug.Log("l3 - miss");
                            spacesAvailableBoard1[randomIndex].image.sprite = onClickIcons[0];
                            hasPlayed1 = true;
                            listsize++;



                        }

                        else
                        {
                            Debug.Log("m - hit");
                            spacesAvailableBoard1[randomIndex].GetComponent<buttonController>().target.Hit(); //sets the ShipPartController as hit and checks if the player has lost
                            spacesAvailableBoard1[randomIndex].image.sprite = onClickIcons[1];
                            hasPlayed1 = true;
                            listsize++;


                            previousHitIndex = randomIndex;
                            targetArray = CheckEdges(previousHitIndex);
                            currentShip.Add(previousHitIndex);
                            shotsIndex++;
                        }

                        totalShots.Add(randomIndex);

                    }

                }
                else
                {
                    Debug.Log("n - shot is in target array");
                    while (targetArray[currentDirection] == -1)
                    {
                        Debug.Log("n1 - check target array for next valid shot");
                        currentDirection++;
                    }

                    if (currentShip.Contains(targetArray[currentDirection]))
                    {
                        Debug.Log("o - previous shot was in target array - shot next index over");
                        Debug.Log(currentDirection);
                        if (CheckShot(previousHitIndex) == -1)
                        {
                            Debug.Log("p - next index over is not valid");
                            currentDirection = currentDirection + 2;
                            Debug.Log((CheckShot(previousHitIndex)));
                            Debug.Log(spacesAvailableBoard1[targetArray[currentDirection]]);

                            if (totalShots.Contains(targetArray[currentDirection]) == false)
                            {
                                Debug.Log("q - shot opposite target array direction");
                                Debug.Log(currentDirection);
                                if (spacesAvailableBoard1[targetArray[currentDirection]].GetComponent<buttonController>().target == null) //if the selected button does not contain a ship part. target is an instance of ShipPartController.
                                {
                                    Debug.Log("r - miss");
                                    spacesAvailableBoard1[targetArray[currentDirection]].image.sprite = onClickIcons[0];
                                    hasPlayed1 = true;
                                    listsize++;

                                    //currentDirection = 0;
                                    //previousHitIndex = -1;
                                    //currentShip.Clear();
                                    //shotsIndex = 0;


                                }

                                else
                                {
                                    Debug.Log("s - hit");
                                    

                                    spacesAvailableBoard1[targetArray[currentDirection]].GetComponent<buttonController>().target.Hit(); //sets the ShipPartController as hit and checks if the player has lost
                                    spacesAvailableBoard1[targetArray[currentDirection]].image.sprite = onClickIcons[1];
                                    hasPlayed1 = true;
                                    listsize++;

                                    shotsIndex++;
                                    currentShip.Add(targetArray[currentDirection]);
                                    previousHitIndex = targetArray[currentDirection];

                                }
                                totalShots.Add(targetArray[currentDirection]);

                            }
                            else
                            {
                                Debug.Log("t - all array shots fired & ship is full - shoot random");
                                while (totalShots.Contains(randomIndex))
                                {
                                    Debug.Log("t2 - chose random number");
                                    randomIndex = Random.Range(0, spacesAvailableBoard1.Length - 1);
                                }

                                currentShip.Clear();
                                shotsIndex = 0;

                                if (spacesAvailableBoard1[randomIndex].GetComponent<buttonController>().target == null) //if the selected button does not contain a ship part. target is an instance of ShipPartController.
                                {
                                    Debug.Log("t3 - miss");
                                    spacesAvailableBoard1[randomIndex].image.sprite = onClickIcons[0];
                                    hasPlayed1 = true;
                                    listsize++;


                                }

                                else
                                {
                                    Debug.Log("u - hit");
                                    spacesAvailableBoard1[randomIndex].GetComponent<buttonController>().target.Hit(); //sets the ShipPartController as hit and checks if the player has lost
                                    spacesAvailableBoard1[randomIndex].image.sprite = onClickIcons[1];
                                    hasPlayed1 = true;
                                    listsize++;

                                    previousHitIndex = randomIndex;
                                    targetArray = CheckEdges(previousHitIndex);
                                    currentShip.Add(previousHitIndex);
                                    shotsIndex++;
                                }

                                totalShots.Add(randomIndex);
                            }


                        }
                        else
                        {
                            Debug.Log("v - target array was shot at and next shot is valid");
                            //Debug.Log(currentShip[shotsIndex]);
                            Debug.Log(targetArray[currentDirection]);
                            Debug.Log(currentDirection);

                            if (totalShots.Contains((CheckShot(previousHitIndex))) == false)
                            {
                                Debug.Log("v1 - shot has NOT been shot before");
                                if (spacesAvailableBoard1[CheckShot(previousHitIndex)].GetComponent<buttonController>().target == null) //if the selected button does not contain a ship part. target is an instance of ShipPartController.
                                {
                                    Debug.Log("w - miss");
                                    spacesAvailableBoard1[CheckShot(previousHitIndex)].image.sprite = onClickIcons[0];
                                    hasPlayed1 = true;
                                    listsize++;
                                    totalShots.Add((CheckShot(previousHitIndex)));

                                    currentDirection = currentDirection + 2;
                                    Debug.Log(currentDirection);
                                }

                                else
                                {
                                    Debug.Log("x - hit");

                                    spacesAvailableBoard1[CheckShot(previousHitIndex)].GetComponent<buttonController>().target.Hit(); //sets the ShipPartController as hit and checks if the player has lost
                                    spacesAvailableBoard1[CheckShot(previousHitIndex)].image.sprite = onClickIcons[1];

                                    
                                    shotsIndex++;
                                    currentShip.Add(CheckShot(previousHitIndex));
                                    totalShots.Add((CheckShot(previousHitIndex)));
                                    previousHitIndex = CheckShot(previousHitIndex);

                                    hasPlayed1 = true;
                                    listsize++;
                                }
                                
                            }
                            else
                            {
                                Debug.Log("y - shot has been shot before - pick again opposite direction");
                                Debug.Log(currentDirection);
                                //Debug.Log(currentShip[shotsIndex]);

                                currentDirection = currentDirection + 2;

                                while (targetArray[currentDirection] == -1)
                                {
                                    Debug.Log("y1 - check target array for next valid shot");
                                    currentDirection++;
                                }
                                if (spacesAvailableBoard1[targetArray[currentDirection]].GetComponent<buttonController>().target == null) //if the selected button does not contain a ship part. target is an instance of ShipPartController.
                                {
                                    Debug.Log("y2 - miss");
                                    spacesAvailableBoard1[targetArray[currentDirection]].image.sprite = onClickIcons[0];
                                    hasPlayed1 = true;
                                    listsize++;

                                    currentDirection++;


                                    while (targetArray[currentDirection] == -1)
                                    {
                                        Debug.Log("y3 - check target array for next valid shot");
                                        currentDirection++;
                                    }


                                }

                                else
                                {
                                    Debug.Log("z - hit");
                                   
                                    spacesAvailableBoard1[targetArray[currentDirection]].GetComponent<buttonController>().target.Hit(); //sets the ShipPartController as hit and checks if the player has lost
                                    spacesAvailableBoard1[targetArray[currentDirection]].image.sprite = onClickIcons[1];

                                    shotsIndex++;
                                    currentShip.Add(targetArray[currentDirection]);
                                    previousHitIndex = targetArray[currentDirection];

                                    hasPlayed1 = true;
                                    listsize++;

                                }
                                totalShots.Add(targetArray[currentDirection]);

                            }

                        }

                    }
                    else
                    {
                        Debug.Log("aa - shoot at target shot");
                        Debug.Log(currentDirection);
                        //Debug.Log(currentShip[shotsIndex]);
                        Debug.Log(targetArray[currentDirection]);

                        if (totalShots.Contains(targetArray[currentDirection]) == false)
                        {
                            Debug.Log("bb - shot has NOT been shot at before");
                            if (spacesAvailableBoard1[targetArray[currentDirection]].GetComponent<buttonController>().target == null) //if the selected button does not contain a ship part. target is an instance of ShipPartController.
                            {
                                Debug.Log("cc - miss");
                                spacesAvailableBoard1[targetArray[currentDirection]].image.sprite = onClickIcons[0];
                                hasPlayed1 = true;
                                listsize++;
                                totalShots.Add(targetArray[currentDirection]);

                                currentDirection++;

                                while (targetArray[currentDirection] == -1)
                                {
                                    Debug.Log("cc1 - check target array for next valid shot");
                                    currentDirection++;
                                }


                            }

                            else
                            {
                                Debug.Log("dd - hit");
                               

                                spacesAvailableBoard1[targetArray[currentDirection]].GetComponent<buttonController>().target.Hit(); //sets the ShipPartController as hit and checks if the player has lost
                                spacesAvailableBoard1[targetArray[currentDirection]].image.sprite = onClickIcons[1];

                                shotsIndex++;
                                currentShip.Add(targetArray[currentDirection]);
                                
                                totalShots.Add(targetArray[currentDirection]);
                                previousHitIndex = targetArray[currentDirection];

                                hasPlayed1 = true;
                                listsize++;

                            }
                            

                        }
                        else
                        {
                            Debug.Log("ee - shot has been shot at before - got to next target valid target");
                            Debug.Log(currentDirection);
                            while (targetArray[currentDirection] == -1)
                            {
                                Debug.Log("ee1 - check target array for next valid shot");
                                currentDirection++;
                            }
                            if (spacesAvailableBoard1[targetArray[currentDirection]].GetComponent<buttonController>().target == null) //if the selected button does not contain a ship part. target is an instance of ShipPartController.
                            {
                                Debug.Log("ff - miss");
                                spacesAvailableBoard1[targetArray[currentDirection]].image.sprite = onClickIcons[0];
                                hasPlayed1 = true;
                                listsize++;

                                currentDirection++;

                                while (targetArray[currentDirection] == -1)
                                {
                                    Debug.Log("ff1 - check target array for next valid shot");
                                    currentDirection++;
                                }


                            }

                            else
                            {
                                Debug.Log("gg - hit");
                                

                                spacesAvailableBoard1[targetArray[currentDirection]].GetComponent<buttonController>().target.Hit(); //sets the ShipPartController as hit and checks if the player has lost
                                spacesAvailableBoard1[targetArray[currentDirection]].image.sprite = onClickIcons[1];
                                hasPlayed1 = true;
                                listsize++;

                                shotsIndex++;
                                currentShip.Add(targetArray[currentDirection]);
                                previousHitIndex = targetArray[currentDirection];

                            }
                            totalShots.Add(targetArray[currentDirection]);

                        }


                    }
                }



            }
            // until this line
            for (int i = 0; i < spacesAvailableBoard1.Length; i++) //sets up next player's turn
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
            if (hasPlayed1) //changes player turn panel text and interaction. Prevents other panels from appearing. Shows switch panel string. The "Continue" button (not in this script) changes viewed panel.
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
        else if (player2Turn) //actually player 1 turn
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

    public void AIhardGame()
    {
        bool hasPlayed1 = false, hasPlayed2 = false;

        if (player1Turn) //actually player 2 turn
        {
            for (int i = 0; i < spacesAvailableBoard1.Length; i++)
            {
                Debug.Log("ai hard game");
                //if (spacesAvailableBoard1[i].GetComponent<buttonController>().target == null) //if the selected button contains a ship part, fire
                //{
                //    spacesAvailableBoard1[i].image.sprite = onClickIcons[0];
                //    hasPlayed1 = true;
                //}

                //else

                if (spacesAvailableBoard1[i].GetComponent<buttonController>().target != null && spacesAvailableBoard1[i].image.sprite != onClickIcons[1])
                {
                    Debug.Log("found a not null one");
                    spacesAvailableBoard1[i].GetComponent<buttonController>().target.Hit(); //sets the ShipPartController as hit and checks if the player has lost
                    spacesAvailableBoard1[i].image.sprite = onClickIcons[1];
                    hasPlayed1 = true;
                    break;
                }

            }

            for (int i = 0; i < spacesAvailableBoard1.Length; i++) //sets up next player's turn
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

            if (hasPlayed1) //changes player turn panel text and interaction. Prevents other panels from appearing. Shows switch panel string. The "Continue" button (not in this script) changes viewed panel.
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
                playerTurn.GetComponent<Text>().text = "It's computer's Turn";
            }
        }
        else
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
                playerTurn.GetComponent<Text>().text = "It's the computer's Turn";
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
}
