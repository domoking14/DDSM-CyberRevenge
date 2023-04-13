using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBlockMove : MonoBehaviour
{
    Rigidbody2D rb;
    public float moveSpeed = 4.5f;
    public float rotSpeed;
    public Transform movePoint;

    private bool canMove = true, canRotate = true, onGround, blockCollide;

    public LayerMask stopsMovement;

    public Vector3 blockPos;
    public Vector3 blockRot;
    // Start is called before the first frame update
    void Start()
    {
        movePoint.parent = null;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!onGround || !blockCollide)
        {
            transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, movePoint.rotation, rotSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, movePoint.position) <= .02f)
            {
                movePoint.position -= new Vector3(0f, .25f, 0f);
                if (Mathf.Abs(Input.GetAxisRaw("BlockX")) == 1f && canMove)
                {
                    if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(Input.GetAxisRaw("BlockX"), 0f, 0f), .2f, stopsMovement))
                    {
                        movePoint.position += new Vector3(Input.GetAxisRaw("BlockX"), 0f, 0f);
                    }
                }
                if (canRotate)
                {
                    Rotate();
                }
            }
            BlockFall();
        }
        if(onGround || blockCollide)
        {
            movePoint.position = blockPos;
        }
    }

    void BlockFall()
    {
        if(Input.GetKeyDown(KeyCode.Space) && canMove)
        {
            canMove = false;
            canRotate = false;
        }
    }

    
    private void Rotate()
    {
        if(Quaternion.Angle(transform.rotation, movePoint.rotation) <= .02f)
        {
            if (Mathf.Abs(Input.GetAxisRaw("Rotate")) == 1)
            {
                movePoint.Rotate(0f, 0f, 90f);
            }
        }
       
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Ground"))
        {
            blockPos = transform.position;
            onGround = true;
            canMove = false;
            canRotate = false;
            Destroy(rb);
            gameObject.layer = 0;
            gameObject.tag = "PlacedBlock";
        }

        else if (collision.transform.CompareTag("Player"))
        {
            //Destroy(DaBlock);
            //print("This works??");
            gameObject.tag = "PlacedBlock";
        }

        else if (collision.transform.CompareTag("PlacedBlock"))
        {
            blockPos = transform.position;
            blockCollide = true;
            canMove = false;
            canRotate = false;
            Destroy(rb);
            gameObject.layer = 0;
            gameObject.tag = "PlacedBlock";
        }

        else if (collision.transform.CompareTag("Platform"))
        {
            blockPos = transform.position;
            blockCollide = true;
            canMove = false;
            Destroy(rb);
            gameObject.layer = 0;
            gameObject.tag = "PlacedBlock";
        }
    }
}
