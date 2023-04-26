using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public static PlayerMove PM;

    //For Player 
    Rigidbody2D pb;
    SpriteRenderer sr;
    Animator anim;
    public GameObject player;
    public Vector3 vel;
    public float gravDown, maxDVel;
    public float playSP = 15f;
    public float jumpVel = 20f;
    private float jumpVal = 0;
    private float maxJump = 2;
    private bool playGround, playPlat, doubleJump, isMove;
    public bool canMove; 
    public bool canSpawn = false;
    public Transform spawnPoint;
    public int playerLives = 3;
    //For Dash Jump
    private bool canDash;
    public float dashTime, dashSpeed, dashJumpIncrease, timeBTWDashes;

    public GameObject _BlockSpawn;
    private BlockSpawn BLS;
    

    //public Transform _MoveGrid;
    
    void Awake()
    {
        pb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        canMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            if (!playGround || playPlat && vel.y > -maxDVel)
            {
                vel.y -= gravDown * Time.deltaTime;
            }
            if (vel.y < -maxDVel)
            {
                playGround = true;
                vel.y = 0;
            }

            if (playGround || playPlat)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    DashAblility();
                }
                if (vel.y > 0 && playGround)
                {
                    playGround = false;
                }
                else if (vel.y > 0 && playPlat)
                {
                    playPlat = false;
                }
                else if (vel.y < 0 && !playGround && !playPlat)
                {
                    vel.y -= gravDown * Time.deltaTime;
                }
                if (vel.y <= 0 && playGround)
                {
                    playGround = true;
                    playPlat = false;
                }
                else if (vel.y <= 0 && playPlat)
                {
                    playPlat = true;
                    playGround = false;
                }
            }
            if (Input.GetButtonDown("Jump") && jumpVal < maxJump && doubleJump)
            {
                Jump();
            }

            vel.x = Input.GetAxisRaw("PlayerX") * playSP;
            if (vel.x > 0 || vel.x < 0)
            {
                isMove = true;
                if (vel.x > 0)
                {
                    sr.flipX = false;
                    anim.SetBool("Move", isMove);
                }
                else if (vel.x < 0)
                {
                    sr.flipX = true;
                    anim.SetBool("Move", isMove);
                }
            }
            else if (vel.x == 0)
            {
                isMove = false;
                anim.SetBool("Move", isMove);
            }
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (UIManager.UIM.gameState == 0)
            {
                UIManager.UIM.gameState = 1;
            }
            else
            {
                print("Can't Pause Right Now");
            }
        }

        CheckBlockSpawn();
        WhatPlayerDo();
        anim.SetFloat("XMove", vel.x);
        pb.MovePosition(transform.position + vel * Time.deltaTime);

    }
  
    private void Jump()
    {
        vel.y = jumpVel;
        jumpVal++;
        playGround = false;
        playPlat = false;
        anim.SetBool("Grounded", false);
        anim.SetFloat("Jumps", jumpVal);
    }

    void DashAblility()
    {
        if (canDash)
        {
            StartCoroutine(Dash());
        }
    }
    
    IEnumerator Dash()
    {
        canDash = false;
        playSP = dashSpeed;
        jumpVel = dashJumpIncrease;
        yield return new WaitForSeconds(dashTime);
        playSP = 7.5f;
        jumpVel = 12f;
        yield return new WaitForSeconds(timeBTWDashes);
        canDash = true;
    }

    void Respawn()
    {
        if(playerLives >= 0)
        {
            UIManager.UIM.gameState = 3;
        }
        else if(playerLives < 0)
        {
            UIManager.UIM.UpdateLives(playerLives);
            transform.position = spawnPoint.position;
        }
        
    }

    void WhatPlayerDo()
    {
        if(playGround || playPlat && canSpawn)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                canMove = false;
                //pb.constraints = RigidbodyConstraints2D.FreezeAll;
            }
        }
        if (canSpawn && !canMove)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                canMove = true;
                //canSpawn = false;
                //pb.constraints = RigidbodyConstraints2D.None;
                //pb.constraints = RigidbodyConstraints2D.FreezeRotation;
            }
        }
    }

    void CheckBlockSpawn()
    {
        _BlockSpawn = GameObject.FindGameObjectWithTag("BlockSpawn");
        BLS = _BlockSpawn.GetComponent<BlockSpawn>();
    }
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.transform.CompareTag("Ground"))
        {
            doubleJump = true;
            canDash = true;
            maxJump = 2;
            jumpVal = 0;
            vel.y = 0;
            anim.SetBool("Grounded", true); 
        }

        if (col.transform.CompareTag("Platform"))
        {
            playPlat = true;
            doubleJump = true;
            canDash = true;
            jumpVal = 0;
            vel.y = 0;
            anim.SetBool("Grounded", true);
        }

        if (col.transform.CompareTag("PlacedBlock") || col.transform.CompareTag("Block"))
        {
            playPlat = true;
            doubleJump = true;
            canDash = true;
            jumpVal = 0;
            vel.y = 0;
            anim.SetBool("Grounded", true);
        }

        if (col.transform.CompareTag("DeathFloor"))
        {
            playerLives--;
            Respawn();
        }
        if (col.transform.CompareTag("DeathBlocks"))
        {
            playerLives--;
            Respawn();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("SpawnOne"))
        {
            canSpawn = true;
            BLS.spawnOne = true;
            BLS.spawnTwo = false;
            BLS.spawnThree = false;
        }
        if (collision.transform.CompareTag("SpawnTWO"))
        {
            canSpawn = true;
            BLS.spawnOne = false;
            BLS.spawnTwo = true;
            BLS.spawnThree = false;
        }
        if (collision.transform.CompareTag("SpawnTHREE"))
        {
            canSpawn = true;
            BLS.spawnOne = false;
            BLS.spawnTwo = false;
            BLS.spawnThree = false;
        }

        if (collision.transform.CompareTag("PlatformZone"))
        {
            canSpawn = false;
            BLS.spawnOne = false;
            BLS.spawnTwo = false;
            BLS.spawnThree = false;
        }

        if (collision.transform.CompareTag("SpawnPoint"))
        {
            spawnPoint = collision.transform;
            GameManager.GM.SetSpawnPoint(collision.transform);
        }

        if (collision.transform.CompareTag("Finish"))
        {
            UIManager.UIM.gameState = 2;
        }
        /*
        if (collision.transform.CompareTag("SpawnGrids"))
        {
            canSpawn = true;
            //_MoveGrid = collision.transform;
            //GameManager.GM.SetGrid(collision.transform);
        }
        */
    }
}
