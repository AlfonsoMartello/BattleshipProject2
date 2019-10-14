using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonController : MonoBehaviour
{
    public ShipPartController target;
    public Collider2D collide;
    public int buttonTeam;

    /**
    * @pre Start is called before the first frame update.
    * @post Checks the object the script is attached to. If one exists, attach to the collide variable.
    * @param None.
    * @return None.
    */
    private void Start()
    {
        collide = this.GetComponent<Collider2D>();
    }

    // Update is called once per frame. Not used, so it's not documented.
    void Update()
    {
        
    }

    /**
    * @pre Target must be able to run Hit.
    * @post Attemts to run the Hit function on the attached target.
    * @param None.
    * @return None.
    */
    public void Onclickhit()
    {
        if (target != null)
            target.Hit();
        else
        {

        }
    }

    /**
    * @pre Object must have an attached trigger collider.
    * @post If it comes in contact with the collider of a viable object, it assigns that to target.
    * @param takes parameter collision, the collider of what it hits.
    * @return None.
    */
    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("Something touching the board");
        if (collision.gameObject.GetComponent<ShipPartController>().partTeam == buttonTeam //must be in own board
            && collision.gameObject.GetComponent<ShipPartController>().parent.shipReadyToPair) //
        {
            Debug.Log(collision);
            target = collision.gameObject.GetComponent<ShipPartController>();
            this.tag = "Closed";
        }
        else
        {

        }
        
    }
}
