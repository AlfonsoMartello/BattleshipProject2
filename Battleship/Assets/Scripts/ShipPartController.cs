using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
* 
*/
public class ShipPartController : MonoBehaviour
{
    Collider2D collide;
    public ShipController parent;
    public bool hit = false;
    public bool bound = false;
    public SpriteRenderer rend;
    public buttonController bondTarget;
    public int partTeam;
    
    public bool partReadyToPair = false;
    


    /**
    * Start is called before the first frame update.
    */
    private void Start()
    {
        parent = transform.parent.GetComponent<ShipController>();
        rend = GetComponent<SpriteRenderer>();
    }

    /**
    * @pre Update is called once per frame.
    * @post If ship is moving, check for WASD input and rotate ship accordingly.
    * @param none 
    * @return none
    */
    private void Update()
    {
        if (parent.isMoving)
        {
            transform.parent.transform.position = Camera.main.ScreenToWorldPoint( new Vector3 (Input.mousePosition.x,
                                                                                  Input.mousePosition.y,
                                                                                  -Camera.main.transform.position.z));
            if (Input.GetKeyDown("a"))
            {
                parent.FaceLeft();
            }
            else if (Input.GetKeyDown("d"))
            {
                parent.FaceRight();
            }
            else if (Input.GetKeyDown("s"))
            {
                parent.FaceDown();
            }
            else if (Input.GetKeyDown("w"))
            {
                parent.FaceUp();
            }
        }
        else
        {

        }
    }

    /**
    * @pre Ship part has been hit
    * @post Change ship part flag to true, call parent ship's check function
    * @param None
    * @return None
    */
    public void Hit()
    {
        hit = true;
        parent.hitCheck();
    }

    /**
     * @pre Mouse has been clicked
     * @post Change parent ships isMoving status to true
     * @param None
     * @retrun None
     */
    private void OnMouseDown()
    {
        if (!parent.shipReadyToPair)
        {
            Debug.Log("TouchedShip");
            parent.isMoving = true;
        }

    }

    /**
     * @pre Mouse has been released
     * @post Attempt to bond parent ship to board
     * @param None
     * @return None
     */
    private void OnMouseUp()
    {
        
        if (!bound)
        parent.AttemptBond();
        parent.isMoving = false;
    }


    /**
     * @pre Colliders made contact
     * @post Attempt to pair/bond colliders of ship part and board square
     * @param Collider2D collision
     * @return None
     */
    private void OnTriggerEnter2D (Collider2D collision)  //colliders of buttons and of ship are connected. Collider is the button collider, so checks if their is already a ship places w/ help from OnTriggerStay2D in buttonController.
    {
        if (collision.tag == "Open")
        {
            if (collision.GetComponent<buttonController>().buttonTeam == partTeam)
            {
                partReadyToPair = true;
                bondTarget = collision.GetComponent<buttonController>();
            }
        }

    }

    /**
     * @pre Colliders are currently in contact
     * @post Maintain bond between ship part and board square
     * @param Collider2D collision
     * @return None
     */
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Open")
        {
            if (collision.GetComponent<buttonController>().buttonTeam == partTeam)
            {
                partReadyToPair = true;
                bondTarget = collision.GetComponent<buttonController>();
            }
        }
    }

    /**
     * @pre Colliders have left contact
     * @post Remove pair and revert bonding to unbonded state
     * @param Collider2D collision
     * @return None
     */
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Open")
        {
            if (collision.GetComponent<buttonController>().buttonTeam == partTeam)
            {
                partReadyToPair = false;
                bondTarget = null;
            }
        }
    }
}
