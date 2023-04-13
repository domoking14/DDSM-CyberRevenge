using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockMove : MonoBehaviour
{
    Rigidbody2D rb;
    public Vector3 velocity, rotVel;
    public float speedH, gravDown, maxDownVel, rotateH;
    public bool OnGround;
    public GameObject DaBlock;
    private bool canMove = true;
    public bool blockCollide, blockDestroy;
    public float blockTime = 3;
    public float OGblockTime = 3;
    private float blockTimeCheck = 0;
    private bool Rotated = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();    
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && !canMove)
        {
            canMove = true;
        }

        DaBlock = GameObject.FindGameObjectWithTag("Block");

        if (canMove)
        {
            velocity.x = Input.GetAxisRaw("BlockX") * speedH;
            rotVel.z = Input.GetAxisRaw("Rotate") * rotateH;

            if (!OnGround && !blockCollide && velocity.y > -maxDownVel)
            {
                velocity.y = Input.GetAxisRaw("BlockY") * gravDown;
            }
            else if (OnGround || blockCollide)
            {
                velocity.y = 0;
            }
            if(!OnGround && !blockCollide)
            {
                rb.MovePosition(transform.position + velocity * Time.deltaTime);
                rb.MoveRotation(rotVel.z);
                SpawnTimer();
            }
        }

        NewRotate();
        BlockFall();
    }

    public void BlockFall()
    {
        if(Input.GetKeyDown(KeyCode.Space) && canMove && rb != null)
        {
            rb.constraints = RigidbodyConstraints2D.FreezePositionX;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            rb.gravityScale = 15;
            velocity.y -= gravDown * Time.deltaTime;
            canMove = false;
        }
        else if(Input.GetKeyDown(KeyCode.Space) && !canMove)
        {
            //print("Why are you pressing space??");
        }
    }

    public void NewRotate()
    {
        if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.X))
        {
            if (canMove && rb != null)
            {
                print("Rotated");
                rb.constraints = RigidbodyConstraints2D.None;
                Rotated = true;
            }
            else if(rb != null)
            {
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            }
        }
       
        if (Rotated)
        {
            rotateH = -90;
            Rotated = false;
        }


    }

    public void SpawnTimer()
    {
        //I need to update this so that the player can't spawn a block until the pervoius block is "Placed"
        blockTime -= Time.deltaTime;
        if(blockTime <= blockTimeCheck)
        {
            canMove = false;
            rb.constraints = RigidbodyConstraints2D.FreezePositionX;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            rb.gravityScale = 15;
            velocity.y -= gravDown * Time.deltaTime;
            blockTime = OGblockTime;
            gameObject.layer = 0; //<-- May change this to another layer...
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Ground"))
        {
            OnGround = true;
            canMove = false;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            Destroy(rb);
            gameObject.layer = 0;
            gameObject.tag = "PlacedBlock";
        }

        else if (collision.transform.CompareTag("Player"))
        {
            Destroy(DaBlock);
            //print("This works??");
            gameObject.tag = "PlacedBlock";
        }

        else if (collision.transform.CompareTag("PlacedBlock"))
        {
            blockCollide = true;
            canMove = false;
            Destroy(rb);
            gameObject.layer = 0;
            gameObject.tag = "PlacedBlock";
        }

        else if (collision.transform.CompareTag("Platform"))
        {
            blockCollide = true;
            canMove = false;
            Destroy(rb);
            gameObject.layer = 0;
            gameObject.tag = "PlacedBlock";
        }
    }
}
