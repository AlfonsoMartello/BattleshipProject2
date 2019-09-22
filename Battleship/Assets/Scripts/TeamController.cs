﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
* 
*/
public class TeamController : MonoBehaviour
{
    public List<ShipController> Ships;
    public int numberOfShips;
    bool isNumShipsSelected = false;
    public GameObject ship;
    public bool loseCheck = false;
    public bool placeCheck = false;
    public int team = 0;
    public int shipsLeft = 0;

    /**
    * Start is called before the first frame update.
    */
    void Start()
    {

    }

    /**
    * @pre Update is called once per frame.
    * @post Spawn ships if needed, keep number of selected ships true, update number of ships remaining.
    * @param None
    * @return None
    */
    void Update()
    {
        if (isNumShipsSelected == false && numberOfShips > 0)
        {
            spawnShips(numberOfShips);
            isNumShipsSelected = true;
            shipsLeft = numberOfShips;
        }
    }

    /**
     * @pre Number of ships has been selected by the user
     * @post Set numberofShips
     * @param int shipAmount
     * @return None
     */
    public void SetNumberOfShips(int shipAmmount)
    {
        numberOfShips = shipAmmount;
    }

    /**
     * @pre Initial Number of ships is known
     * @post Spawn shps of correct size for the team
     * @param int Length
     * @return None
     */
    void spawnShips(int length)
    {
        for (int i = 0; numberOfShips > i; i++)
        {
            Ships.Add(Instantiate(ship, this.transform.position,Quaternion.identity,this.transform).GetComponent<ShipController>());
            Ships[i].transform.position = new Vector3(this.transform.position.x, 
                                                      this.transform.position.y + i * 30, 
                                                      this.transform.position.z);
            Ships[i].SetShipLength(i+1);
            Ships[i].shipTeam = team;
            Debug.Log(i);
        }
    }

    /**
     * @pre Called when a ship part has been hit, then ship calls this function in Team
     * @post If any ships remain, set lossCheck to false
     * @param None
     * @return None
     */
    public void checkForLoss()
    {
        loseCheck = true;
        shipsLeft = 0;
        foreach (ShipController ship in Ships)
        {
            if (!ship.destoryCheck)
            {
                loseCheck = false;
                shipsLeft++;
            }
        }

        
    }

    /**
    * @pre Team must have ships
    * @post Disable rendering for all ships in the Team 
    * @param None
    * @return None
    */
    public void disappearShips()
    {
        foreach (ShipController ship in Ships)
        {
            ship.disappear();
        }
    }

    /**
     * @pre Team must have ships
     * @post Renders all ships within the Team
     * @param None
     * @return None
     */
    public void appearShips()
    {
        foreach (ShipController ship in Ships)
        {
            ship.appear();
        }
    }

    /**
     * @pre Called when ShipController AttemptBond() is called
     * @post Checks each ship in the team for correct placement
     * @param None
     * @return None
     */
    public void checkPlacement()
    {
        placeCheck = true;
        foreach (ShipController ship in Ships)
        {
            if (!ship.shipReadyToPair)
            {
                placeCheck = false;

            }
        }
    }
}
