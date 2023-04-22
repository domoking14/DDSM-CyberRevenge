using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NewBlockMove : MonoBehaviour
{
    Rigidbody2D rb;
    public float moveSpeed = 4.5f;
    public float rotSpeed;
    public Transform movePoint;
    public Vector3 rotationPoint;

    //Move Authentic Tetris Falling Movement
    private float previousTime;
    public float fallTime = 0.8f;

    private bool canMove = true, onGround, blockCollide;

    public LayerMask stopsMovement;

    public Vector3 blockPos;
    public Vector3 blockRot;
    
    /*
     CRAP THAT WAS INITALLY GOING TO BE USED FOR A TETRIS LIKE EXPERIENCE [BUT TIME/WORK-LOAD CAUGHT UP WITH ME]

    private bool canRotate = true;
    private float rotTime = .25f;
    private float baseRotTime = .25f;
    private float rotTimeCheck = 0f;
    
    Will be changed/removed later to have the game manager look for each grid and obtain its height and width
    public int height;
    public int width;

    public Transform MovementGrid;
    private Collider2D GridBounds;
    public Vector3 GridMin;
    public Vector3 GridMax;
    public Transform[,] grid;
    */

    // Start is called before the first frame update
    void Start()
    {
        movePoint.parent = null;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //WhereGrid();

        if (!onGround || !blockCollide)
        {
            transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, movePoint.rotation, rotSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, movePoint.position) <= .02f)
            {
                if (Mathf.Abs(Input.GetAxisRaw("BlockX")) == 1 && canMove)
                {
                    if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(Input.GetAxisRaw("BlockX"), 0f, 0f), .2f, stopsMovement))
                    {
                        movePoint.position += new Vector3(Input.GetAxisRaw("BlockX"), 0f, 0f);
                        
                        /*
                        if (!ValidMove())
                            movePoint.position -= new Vector3(-Input.GetAxisRaw("BlockX"), 0f, 0f);
                        */
                    }
                }
                Rotate();
            }
            BlockFall();
        }
        if (onGround || blockCollide)
        {
            movePoint.position = blockPos;
        }
    }

    //This method looks to see if the player is pressing the spacebar while their are able to move the block, then speeds up the fall speed so it gets to the ground faster than initally
    void BlockFall()
    {
        if (Time.time - previousTime > fallTime && canMove)
        {
            movePoint.position += new Vector3(0, -1, 0);

            /*
            if (!ValidMove())
            {
                movePoint.position -= new Vector3(gameObject.transform.parent.position.x, -1, 0);
            }
            */

            previousTime = Time.time;
        }

        if (Input.GetKeyDown(KeyCode.Space) && canMove)
        {
            canMove = false;
            //canRotate = false;
        }
        if (!canMove)
        {
            moveSpeed = 10f;
            movePoint.position += new Vector3(0, -1, 0);
        }
    }

    
    private void Rotate()
    {
        if (Quaternion.Angle(transform.rotation, movePoint.rotation) <= .02f)
        {
            if (Mathf.Abs(Input.GetAxisRaw("Rotate")) == 1)
            {
                movePoint.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), 90);
               /*
                if (!ValidMove())
                    movePoint.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), -90);
               */
                //canRotate = false;
            }
        }

        /*
        if (!canRotate && !onGround && !blockCollide)
        {
            rotTime -= Time.deltaTime;
            if(rotTime <= rotTimeCheck)
            {
                canRotate = true;
                print("Rotate!");
                rotTime = baseRotTime;
            }
        }
        */
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Ground"))
        {
            blockPos = transform.position;
            onGround = true;
            canMove = false;
            //canRotate = false;
            Destroy(rb);
            gameObject.layer = 0;
            gameObject.tag = "PlacedBlock";
            //AddToGrid();
            
            //CheckForLines();
        }

        else if (collision.transform.CompareTag("Player"))
        {
            Destroy(gameObject);
            //print("This works??");
            gameObject.tag = "PlacedBlock";
        }

        else if (collision.transform.CompareTag("PlacedBlock"))
        {
            blockPos = transform.position;
            blockCollide = true;
            canMove = false;
            //canRotate = false;
            Destroy(rb);
            gameObject.layer = 0;
            gameObject.tag = "PlacedBlock";
            //AddToGrid();
            //CheckForLines();
        }

        else if (collision.transform.CompareTag("DeathBlocks"))
        {
            blockPos = transform.position;
            blockCollide = true;
            canMove = false;
            //canRotate = false;
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

    /*
    
    HERE IS CODE I SPENT TIME WORKING ON IN AN ATTEMPT TO MAKE SOMETHING MORE TETRIS-LIKE
    I CANNOT CHANGE THIS BECAUSE THIS "PROJECT" NEEDS TO BE DONE IN LESS THAN A WEEK AS OF TYPING THIS...

    IF I CAN FIGURE OUT HOW TO AVOID ARRAY RANGES BEING OUT OF RANGE, THEN BY ALL MEANS COME BACK HERE AND FIX IT...

    void WhereGrid()
    {
        if (MovementGrid != null)
        {
            width = Convert.ToInt32(MovementGrid.localScale.x);
            height = Convert.ToInt32(MovementGrid.localScale.y);
            grid = new Transform[width, height];
            grid.Initialize();
            //print(grid);

            GridBounds = MovementGrid.GetComponent<Collider2D>();
            GridMin = GridBounds.bounds.min;
            GridMax = GridBounds.bounds.max;
        }

    }
    

    void CheckForLines()
    {
        for (int i = height - 1; i >= 0; i--)
        {
            if (HasLine(i))
            {
                DeleteLine(i);
                RowDown(i);
            }
        }
    }

    bool HasLine(int i)
    {
        for (int j = 0; j < width; j++)
        {
            if (grid[j, i] == null)
                return false;
        }
        return true;
    }

    void DeleteLine(int i)
    {
        for (int j = 0; j < width; j++)
        {
            Destroy(grid[j, i].gameObject);
            grid[j, i] = null;
        }
    }
    void RowDown(int i)
    {
        for (int y = i; y < height; y++)
        {
            for (int j = 0; j < width; j++)
            {
                if (grid[j, y] != null)
                {
                    grid[j, y - 1] = grid[j, y];
                    grid[j, y] = null;
                    grid[j, y - 1].transform.position -= new Vector3(0, 1, 0);
                }
            }
        }
    }

    void AddToGrid()
    {
        foreach (Transform children in transform)
        {
            int roundedX = Mathf.RoundToInt(children.transform.position.x);
            int roundedY = Mathf.RoundToInt(children.transform.position.y);
            
            grid[roundedX, roundedY] = children;
        }
    }

    bool ValidMove()
    {
        foreach (Transform children in transform)
        {
            //children.transform.position = gameObject.transform.position;
            int roundedX = Mathf.RoundToInt(children.transform.position.x);
            int roundedY = Mathf.RoundToInt(children.transform.position.y);

            if (roundedX < GridMin.x || roundedX >= GridMax.x || roundedY < GridMin.y || roundedY >= GridMax.y)
            {
                return false;
            }

            if (grid[roundedX, roundedY] != null)
            { 
                return false;
            }
        }

        return true;
    }
    */
}
