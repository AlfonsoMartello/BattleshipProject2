using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
* 
*/
public class ShipController : MonoBehaviour
{
    bool allPartsDestroyed = false;
    bool isSpawned = false;
    bool partCheck = false;
    readonly float EPSILON = 0.01f;
    public bool destoryCheck = false;
    public bool isMoving = false;
    public bool shipReadyToPair = false;
    public Vector3 startPos;
    public List<ShipPartController> parts;
    public TeamController team;
    public int shipLength = 0;
    public int shipTeam = 0;
    public GameObject shipPart;

    /**
    * @pre Start is called before the first frame update
    * @post Set the transform of the TeamController to the parent's TeamController
    * @param None
    * @return None
    */
    private void Start()
    {
        if (transform.parent != null)
        {
            team = transform.parent.GetComponent<TeamController>();
        }
    }

    /**
    * @pre Update is called once per frame.
    * @post Spawn correct number of ships of the correct size if not spawned
    * @param None
    * @return None
    */
    private void Update()
    {
        //spawn
        if (isSpawned == false && shipLength > 0)
        {
            Spawn();
            isSpawned = true;
            startPos = transform.position;
        }

        checkParts();
    }

    /**
    * @pre Called by Update() when not all ships are spawned
    * @post For n length, spawn ship parts 
    * @param None
    * @return None
    */
    private void Spawn()
    {
        //for loop for spawning
        for (int i = 0; shipLength > i; i++)
        {
            parts.Add(Instantiate(shipPart, this.transform).GetComponent<ShipPartController>());
            parts[i].transform.position = new Vector3(this.transform.position.x + i * parts[i].transform.localScale.x, 
                                                      this.transform.position.y, 
                                                      this.transform.position.z);
            parts[i].partTeam = shipTeam;
            Debug.Log(i);
        }
    }

    /**
     * @pre Ship must exist
     * @post Sets ship length.
     * @param int n - length of the ship
     * @return None
     */
    public void SetShipLength(int n)
    {
        shipLength = n;
    }

    /**
     * @pre Ship nmust exist
     * @post Rotates the ship to face right
     * @param None
     * @return None
     */
    public void FaceRight()
    {
        transform.rotation = Quaternion.AngleAxis(0, Vector3.forward);
    }

    /**
     * @pre Ship must exist
     * @post Rotates the ship to face up
     * @param None
     * @return None
     */
    public void FaceUp()
    {
        transform.rotation = Quaternion.AngleAxis(90, Vector3.forward);
    }

    /**
     * @pre Ship must exist
     * @post Rotates the ship to face left
     * @param None 
     * @return None
     */
    public void FaceLeft()
    {
        transform.rotation = Quaternion.AngleAxis(180, Vector3.forward);
    }

    /**
     * @pre Ship must exist
     * @post Rotates the ship to face down
     * @param None
     * @return None
     */
    public void FaceDown()
    {
        transform.rotation = Quaternion.AngleAxis(270, Vector3.forward);
    }

    /**
     * @pre Ship must exist
     * @post Checks each part of the ship to see if moving and renders 
     * green/red depending on whether or not they're in valid spots.
     * @param None
     * @return None
     */
    public void checkParts()
    {
        if (isMoving)
        {
            partCheck = true;
            foreach (ShipPartController part in parts)
            {
                part.rend.sortingLayerName = "Moving";
                if (!part.partReadyToPair)
                {
                    partCheck = false;
                    part.rend.color = Color.red;
                }
                else
                {
                    part.rend.color = Color.green;
                }
            }
            snapToGrid();
        }
    }

    /**
     * @pre Ship must exist
     * @post Snaps ship to grid if all parts of the ship are over top of the
     * board
     * @param None
     * @return None
     */
    private void snapToGrid()
    {
        bool overBoard = true;
        foreach (ShipPartController part in parts)
        {
            overBoard = (part.bondTarget != null) && overBoard;
        }

        if (overBoard && ((System.Math.Abs(Input.GetAxis("Mouse X")) < EPSILON) || (System.Math.Abs(Input.GetAxis("Mouse Y")) < EPSILON)))
        {
            foreach (ShipPartController part in parts)
            {
                part.transform.position = new Vector3(part.bondTarget.transform.position.x,
                                                      part.bondTarget.transform.position.y,
                                                      8);
            }
        }
    }

    /**
     * @pre Ship must exist - assumes thip part is visible
     * @post Disables the ship part rendering to "dissapear the part"
     * @param None 
     * @return None
     */
    public void disappear()
    {
        foreach (ShipPartController part in parts)
        {
            part.rend.enabled = false;
        }
    }

    /**
     * @pre Ship must exist - assumes ship part is not visible
     * @post Renders the ship part.
     * @param None
     * @return None
     */
    public void appear()
    {
        foreach (ShipPartController part in parts)
        {
            part.rend.enabled = true;
        }
    }

    /**
     * @pre Ship part must exist
     * @post Checks each part of the ship for their individual hit flags, checks for a while destroyed ship, check for all ships lost.
     * @param None
     * @return None
     */
    public void hitCheck()
    {
        destoryCheck = true;
        foreach (ShipPartController part in parts)
        {
            if (!part.hit)
            {
                destoryCheck = false;
                
            }
        }

        if (destoryCheck == true)
        {
            team.checkForLoss();
        }
    }

    /**
     * @pre Ship was placed with mouse on board.
     * @post Attempts to bond the ship to the game board in valid location
     * @param None
     * @return None
     */
    public void AttemptBond()
    {
        if (partCheck)
        {   
            shipReadyToPair = true;
            foreach (ShipPartController part in parts)
            {
                part.bound = true;
                part.bondTarget.tag = "Closed";
                part.bondTarget.target = part;
                
                team.checkPlacement();
            }
        }
        else
        {
            transform.position = startPos;
            transform.rotation = Quaternion.identity;
        }

        foreach (ShipPartController part in parts)
        {
            part.rend.sortingLayerName = "Default";
            part.rend.color = Color.white;
        }
    }
}
