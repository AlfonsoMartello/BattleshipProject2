using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

/**
*  @class description: CanvasScript Contains all game transisitions from main menu, to ship placement menu, to main game interactions.
*  @libraries cited: All UI.Buttons, Dropdowns, and UI Panels set to GameObject variables derive from the UnityEngine.UI Library.
*  @libraries cited: UnityEngine.SceneManagement library was used for the Sceen Manager to reload the game scene.
*  @libraries cited: UnityEngine library uses the default Start and Update methods created in every .cs file.
*/
public class CanvasScript : MonoBehaviour
{
    int numShips;
    public Dropdown shipSelector;
    public UnityEngine.UI.Button confirmButton, startButton, switchButton, returnButton, fireButton, revealShipsButton, pauseButton, continueButton, resumeButton, quitButton, yesButton, noButton, mainMenuButton;
    public GameObject shipSelectorPanel, shipPlacementPanel, gameUIPanel, battleshipGrids, switchPanel, pauseMenu, gameOverMenu, confirmationPanel, gameController;
    public GameObject player1Board, player2Board, player1PlacmentMessage, player2PlacmentMessage, player1ShipsRemainingMessage, player2ShipsRemainingMessage, player2WinMessage, player1WinMessage;
    public TeamController Team1;
    public TeamController Team2;
    public BoardInteraction BattleshipBoard;
    bool showShips = false;
    bool gameOver = false;

    /**
    * @pre Start is called before the first frame update.
    * @post Game will start at the main menu and all Buttons will be given event listeners onClick 
    * @param None.
    * @return None.
    */
    public void Start()
    {
        GameReset();

        // Main Menu Buttons Listener Events.
        shipSelector.onValueChanged.AddListener(SelectShips);
        confirmButton.onClick.AddListener(BeginShipPlacement);

        // Ship Placement Menu Buttons Listener Events.
        returnButton.onClick.AddListener(RestartGame);
        switchButton.onClick.AddListener(SwitchPlayers);
        startButton.onClick.AddListener(StartGame);

        // Game Button Listener Events.
        fireButton.onClick.AddListener(Fire);
        revealShipsButton.onClick.AddListener(DisplayShips);
        pauseButton.onClick.AddListener(PauseGame);

        // Switch Players Panel Listener Events.
        continueButton.onClick.AddListener(PlayersAreSwitched);

        // Pause Menu Button Listener Events.
        resumeButton.onClick.AddListener(ResumeGame);
        quitButton.onClick.AddListener(QuitGame);

        // Confirmation Menu Button Listener Events.
        yesButton.onClick.AddListener(RestartGame);
        noButton.onClick.AddListener(PauseGame);

        // Game Over Menu Button Listener Events.
        mainMenuButton.onClick.AddListener(RestartGame);
    }

    /**
    * @post Update is called once per frame
    * @post Update method will check if all the Team's ships are placed on the board.
    * @post Afterwards it will enable the Start Button and the game will proceed.
    * @post Update method will check for any ships lost or if a player is lost, and notify the players.
    * @post Once A player loses, the Game Over Screen will pop up.
    * @param None.
    * @return None.
    */
    public void Update()
    {
        if (Team1.placeCheck == true && Team2.placeCheck == true)
        {
            startButton.interactable = true;
            Team1.checkForLoss();
            Team2.checkForLoss();
            player1ShipsRemainingMessage.GetComponent<Text>().text = "Ships remaining: " + Team1.shipsLeft;
            player2ShipsRemainingMessage.GetComponent<Text>().text = "Ships remaining: " + Team2.shipsLeft;
        }
        else
        {
            startButton.interactable = false;
        }


        if ((Team1.loseCheck == true || Team2.loseCheck == true) && !gameOver)
        {
            if (Team1.loseCheck == true)
            {
                player1WinMessage.SetActive(false);
                player2WinMessage.SetActive(true);
                Debug.Log("Player 2 Score: " + BattleshipBoard.player2Shots / Team2.numberOfShips);
                WriteScoreToFile(BattleshipBoard.player2Shots);
            }
            else if (Team2.loseCheck == true)
            {
                player1WinMessage.SetActive(true);
                player2WinMessage.SetActive(false);
                Debug.Log("Player 1 Score: " + BattleshipBoard.player1Shots / Team1.numberOfShips);
                WriteScoreToFile(BattleshipBoard.player1Shots);
            }

            shipSelectorPanel.SetActive(false);
            gameUIPanel.SetActive(false);
            battleshipGrids.SetActive(false);
            switchPanel.SetActive(false);
            pauseMenu.SetActive(false);
            gameOverMenu.SetActive(true);
            gameOver = true;
        }
    }

    /**
    * @pre Program will start at the Ship Selection Menu or Ship Selection Menu is active.
    * @param value parameter will recieve the Dropdown option value.
    * @post SelectShips method will set the confirmation button to be interactable for any selection other than the empty option.
    * @return None.
    */
    private void SelectShips(int value)
    {
        value = shipSelector.value;
        Debug.Log("You have selected " + value + " ships");

        if (value > 0)
        {
            confirmButton.interactable = true;
            numShips = value;
        }
        else
        {
            confirmButton.interactable = false;
        }
    }


    /**
    * @pre Teams will be given a number of ships based on the shipSelector option value and be instaintiated within incremental sizes.
    * @pre BeginShipPlacement will listen for the Confirm Button onClick event.
    * @pre Confirm Button is active.
    * @post BeginShipPlacement method will disable the Ship Selection Menu and enable the Ship Placement Menu.
    * @post Player Grids will be set active for players to began placing their fleets. Grid images will be disabled until ship placement is complete.
    * @post Player Fleets will be instantiated for players to place ships into the grids.
    * @param None.
    * @return None.
    */
    private void BeginShipPlacement()
    {
        Debug.Log("You have clicked the Confirm button!");

        shipSelectorPanel.SetActive(false);
        shipPlacementPanel.SetActive(true);
        battleshipGrids.SetActive(true);
        gameOverMenu.SetActive(false);
        Team1.SetNumberOfShips(numShips);
        Team2.SetNumberOfShips(numShips);
    }

    /**
    * @pre Ship Placement Menu must be set active.
    * @pre SwitchPlayers Method will listen for the Switch Button onClick event.
    * @post SwitchPlayers Method will switch players during the Ship Placement Menu.
    * @param None.
    * @return None. 
    */
    private void SwitchPlayers()
    {
        if (player1Board.GetComponent<Image>().enabled == true)
        {
            player1Board.GetComponent<Image>().enabled = false;
            player1PlacmentMessage.SetActive(true);
            player2Board.GetComponent<Image>().enabled = true;
            player2PlacmentMessage.SetActive(false);
            Team1.appearShips();
            Team2.disappearShips();
        }
        else
        {
            player1Board.GetComponent<Image>().enabled = true;
            player1PlacmentMessage.SetActive(false);
            player2Board.GetComponent<Image>().enabled = false;
            player2PlacmentMessage.SetActive(true);
            Team2.appearShips();
            Team1.disappearShips();
        }
    }

    /**
    * @pre SwitchPanel must be active. An Attack must be confirmed.
    * @pre PlayersAreSwitched Method will listen for the Continue Button onClick event.
    * @post Game will resume after Continue Button onClick event.
    * @param None.
    * @return None.
    */
    private void PlayersAreSwitched()
    {
        switchPanel.SetActive(false);
        battleshipGrids.SetActive(true);
        gameUIPanel.SetActive(true);
    }

    /**
    * @pre Ship Placement menu must be active.
    * @pre DisplayShips Method will listen for the Reveal Ships Button onClick event.
    * @post DisplayShips Method will allow the players to toggle the visibility of the players ships.
    * @post Player Board images should be enabled for the opposing player.
    * @param None.
    * @return None.
    */
    private void DisplayShips()
    {
        if (BattleshipBoard.player1Turn)
        {
            if (showShips == false)
            {
                Team2.appearShips();
                showShips = true;
            }
            else
            {
                Team2.disappearShips();
                showShips = false;
            }
        }
        else if (BattleshipBoard.player2Turn)
        {
            if (showShips == false)
            {
                Team1.appearShips();
                showShips = true;
            }
            else
            {
                Team1.disappearShips();
                showShips = false;
            }
        }
    }

    /**
    * @pre StartGame method will listen for the Start Button onClick event.
    * @post StartGame method will start the Battleship game once all ships have been placed.
    * @post StartGame method will enable player board images.
    * @post Ship Placement menu will be closed and the game will proceed to the Battleship Game.
    * @param None.
    * @return None.
    */
    private void StartGame()
    {
        Debug.Log("You have clicked the StartGame button!");

        shipPlacementPanel.SetActive(false);
        player1Board.GetComponent<Image>().enabled = true;
        player2Board.GetComponent<Image>().enabled = false;
        gameUIPanel.SetActive(true);
        Team1.disappearShips();
        Team2.disappearShips();
    }

    /**
    * @pre Fire method will listen for the Fire Button onClick event.
    * @post Fire method will report in game console that the Fire Button has been clicked.
    * @param None.
    * @return None.
    */
    private void Fire()
    {
        Debug.Log("Fire!");
        Team1.disappearShips();
        Team2.disappearShips();
        showShips = false;
    }

    /**
    * @pre PauseGame method will listen for an onClick event from either the Pause Button or the No Button.
    * @post PauseGame method will pull up the pause menu once the Pause button is clicked.
    * @param None.
    * @return None.
    */
    private void PauseGame()
    {
        Debug.Log("You have clicked the Pause button!");
        gameUIPanel.SetActive(false);
        battleshipGrids.SetActive(false);
        pauseMenu.SetActive(true);
        Team1.disappearShips();
        Team2.disappearShips();
        showShips = false;

        confirmationPanel.SetActive(false);
        quitButton.interactable = true;
        resumeButton.interactable = true;
    }

    /**
    * @pre ResumeGame method will listen for an onClick event from the Resume Button while the Pause Menu is active.
    * @post ResumeGame method will disable the Pause menu and return back to the game.
    * @param None.
    * @return None.
    */
    private void ResumeGame()
    {
        Debug.Log("You have clicked the Resume button!");
        gameUIPanel.SetActive(true);
        battleshipGrids.SetActive(true);
        pauseMenu.SetActive(false);
    }

    /**
    * @pre QuitGame method wil listen for an onClick event from either the Quit Button or the Main Menu Button.
    * @post QuitGame method will set the Confirm Selection Panel, from here the user will provide confirmation to call GameReset.
    * @post Clicking the No button will return the user back to the pause menu.
    * @param None.
    * @return None.
    */
    public void QuitGame()
    {
        Debug.Log("You have clicked the Quit button!");
        confirmationPanel.SetActive(true);
        quitButton.interactable = false;
        resumeButton.interactable = false;
    }

    /**
    * @pre GameReset method will first be called by start to begin Battleship Game.
    * @post GameReset method will start the game at the Ship Selection menu at the start of the program.
    * @param None.
    * @return None.
    */
    public void GameReset()
    {
        shipSelector.value = 0;

        confirmButton.interactable = false;
        quitButton.interactable = true;
        resumeButton.interactable = true;

        player1Board.GetComponent<Image>().enabled = false;
        player1PlacmentMessage.SetActive(true);
        player2Board.GetComponent<Image>().enabled = true;
        player2PlacmentMessage.SetActive(false);

        shipSelectorPanel.SetActive(true);
        shipPlacementPanel.SetActive(false);
        gameUIPanel.SetActive(false);
        battleshipGrids.SetActive(false);
        switchPanel.SetActive(false);
        pauseMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        confirmationPanel.SetActive(false);
    }

    /**
    * @pre RestartGame method will listen for an onClick event from either the Yes Button or the Return Button.
    * @post RestartGame method will reload the scene, back to the Ship Selection Main Menu.
    * @param None.
    * @return None.
    */
    public void RestartGame()
    {
        /**
        * The line of code for the RestartGame() function can be found in the Unity Answer Forum: https://answers.unity.com/questions/46918/reload-scene-when-dead.html
        */
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ExitGame()
    {
        Debug.Log("EXIT THE GAME! BYE!!");
        Application.Quit();
    }

    private void WriteScoreToFile(int score)
    {
        ScoreData scoreData = new ScoreData();
        BinaryFormatter bf = new BinaryFormatter();
        if (File.Exists(Application.persistentDataPath + "/savedGames.gd"))
        {
            FileStream readFile = File.Open(Application.persistentDataPath + "/savedGames.gd", FileMode.Open);
            scoreData = (ScoreData)bf.Deserialize(readFile);
            readFile.Close();
        }

        scoreData.AddScoreByNumShips(Team1.numberOfShips, score);
        FileStream writeFile = File.Create(Application.persistentDataPath + "/savedGames.gd");
        bf.Serialize(writeFile, scoreData);
        writeFile.Close();

        foreach (int a in scoreData.GetScoresByNumShips(1))
        {
            Debug.Log("1 Ship: " + a);
        }
    }
}
