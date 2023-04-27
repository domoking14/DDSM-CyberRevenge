using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using Cinemachine;

public class BlockSpawn : MonoBehaviour
{
    public static BlockSpawn BS;
    public CinemachineVirtualCamera vCam;

    //Add more Spawners if you need... Copy/Paste with a numbered name + Spawn
    public GameObject firstSpawn;
    public GameObject secondSpawn;
    public GameObject thridSpawn;
    public GameObject fourthSpawn;
    public GameObject fifthSpawn;
    
    public Transform Spawner;

    /*
    public Transform Grid;
    private Collider2D GridBounds;
    public Vector3 gridMinVec;
    public int gridWidth, gridHeight;
    public Transform[,] TESTgrid;
    */

    //Simiar to the gameobjects, Add a coresponiding bool simialr to the ones on the bottom [You can move to the WhatSpawner Method Now] 
    public bool spawnOne, spawnTwo, spawnThree, spawnFour, spawnFive;
    

    public List<Object> testBlocks = new List<Object>();
    public List<Object> playerBlocks = new List<Object>();
    public List<Object> PlayedBlocks = new List<Object>();
    private int playerBlockSize = 5;
    public GameObject block;

    //Normal Blocks
    public GameObject SquareBlock; //0
    public GameObject ZblockL; //1
    public GameObject ZblockR; //2
    public GameObject LblockR; //3
    public GameObject LblockL; //4
    public GameObject LineBlock; //5
    public GameObject Tblock; //6

    //BlockSpawning limit (So the player can't spawn 2 blocks at a time)
    public int blockSpawnLim = 0;
    public int blockSpawnOG = 0;
    public int blockSpawnMax = 1;
    public float blockTime = 2;
    public float OGblockTime = 2;
    public float blockTimeCheck = 0;
    //private int blocksPlaced = 0;
    //private int blocksPlacedMX = 8;

    //private float time = 3;
    //private float timeOG = 15;
    public bool startSpawn = false;
    private bool spawnDAblocks = false;
    private bool playerMove = true;

    public GameObject _PlayerMove;
    private PlayerMove PM;
    private void Awake()
    {
        RandomLoad();
        PM = _PlayerMove.GetComponent<PlayerMove>();
        var vCam = GetComponent<CinemachineVirtualCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!startSpawn && playerMove)
        {
            WhatSpawner();
            NoSpawn();
        }
        

        if (Input.GetKeyDown(KeyCode.Q) && blockSpawnLim == blockSpawnOG && spawnDAblocks && spawnOne)
        {
            int rand = Random.Range(0, playerBlocks.Count);
            Object[] blockLand = playerBlocks.ToArray();
            var PlayableBlock = Instantiate(blockLand[rand], firstSpawn.transform.position, Quaternion.identity) as GameObject;
            blockSpawnLim++;
            block = PlayableBlock;
            vCam.Follow = PlayableBlock.transform;
            PlayedBlocks.Add(PlayableBlock);
        }
        if (spawnTwo)
        {
            if (Input.GetKeyDown(KeyCode.Q) && blockSpawnLim == blockSpawnOG && spawnDAblocks)
            {
                int rand = Random.Range(0, playerBlocks.Count);
                Object[] test = playerBlocks.ToArray();
                var PlayableBlock = Instantiate(test[rand], secondSpawn.transform.position, Quaternion.identity) as GameObject;
                blockSpawnLim++;
                block = PlayableBlock;
                vCam.Follow = PlayableBlock.transform;
                PlayedBlocks.Add(PlayableBlock);
            }
        }
        if (spawnThree)
        {
            if (Input.GetKeyDown(KeyCode.Q) && blockSpawnLim == blockSpawnOG && spawnDAblocks)
            {
                int rand = Random.Range(0, playerBlocks.Count);
                Object[] test = playerBlocks.ToArray();
                var PlayableBlock = Instantiate(test[rand], thridSpawn.transform.position, Quaternion.identity) as GameObject;
                blockSpawnLim++;
                block = PlayableBlock;
                vCam.Follow = PlayableBlock.transform;
                PlayedBlocks.Add(PlayableBlock);
            }
        }
        if (spawnFour)
        {
            if (Input.GetKeyDown(KeyCode.Q) && blockSpawnLim == blockSpawnOG && spawnDAblocks)
            {
                int rand = Random.Range(0, playerBlocks.Count);
                Object[] test = playerBlocks.ToArray();
                var PlayableBlock = Instantiate(test[rand], fourthSpawn.transform.position, Quaternion.identity) as GameObject;
                blockSpawnLim++;
                block = PlayableBlock;
                vCam.Follow = PlayableBlock.transform;
                PlayedBlocks.Add(PlayableBlock);
            }
        }
        if (spawnFive)
        {
            if (Input.GetKeyDown(KeyCode.Q) && blockSpawnLim == blockSpawnOG && spawnDAblocks)
            {
                int rand = Random.Range(0, playerBlocks.Count);
                Object[] test = playerBlocks.ToArray();
                var PlayableBlock = Instantiate(test[rand], fifthSpawn.transform.position, Quaternion.identity) as GameObject;
                blockSpawnLim++;
                block = PlayableBlock;
                vCam.Follow = PlayableBlock.transform;
                PlayedBlocks.Add(PlayableBlock);
            }
        }

        if (blockSpawnLim == blockSpawnMax)
        {
            BlockCooldown();
        }
        if (startSpawn && !playerMove)
        {
            CanSpawn();
        }
        if(startSpawn && playerMove)
        {
            NoSpawn();
            vCam.Follow = _PlayerMove.transform;
        }
    }

    
    //This will be edited so that the player can't spawn a block until the prevoius block is "placed" 
    public void BlockCooldown()
    {
        blockTime -= Time.deltaTime;
        if (blockTime >= blockTimeCheck && Input.GetButtonDown("Jump"))
        {
            blockTime = OGblockTime;
            blockSpawnLim = blockSpawnOG;
            spawnDAblocks = true;
        }

        else if (blockTime <= blockTimeCheck)
        {
            print("Spawn Block");
            blockTime = OGblockTime;
            blockSpawnLim = blockSpawnOG;
            spawnDAblocks = true;
        }
    }

    private void CanSpawn()
    {
        if (startSpawn)
        {
            spawnDAblocks = true;
        }
        else if (startSpawn && spawnDAblocks && Input.GetKeyDown(KeyCode.Q))
        {
            spawnDAblocks = false;
            BlockCooldown();
        }

        if (startSpawn && Input.GetKeyDown(KeyCode.E))
        {
            startSpawn = false;
            spawnDAblocks = false;
            playerMove = true;
        }
    }

    void NoSpawn()
    {
        if (PM.canSpawn)
        {
            startSpawn = true;
            if (Input.GetKeyDown(KeyCode.Q))
            {
                playerMove = false;
            }
        }
        else if (!PM.canSpawn)
        {
            startSpawn = false;
            playerMove = true;
            WhatSpawner();
            vCam.Follow = _PlayerMove.transform;
        }
    }

    /*
    public void SetBlockGrid()
    {
        if (Grid != null)
        {
            gridWidth = System.Convert.ToInt32(Grid.localScale.x);
            gridHeight = System.Convert.ToInt32(Grid.localScale.y);

            GridBounds = Grid.GetComponent<Collider2D>();
            gridMinVec = GridBounds.bounds.min;
            TESTgrid = new Transform[gridWidth, gridHeight];
        }
    }
    */

    public void WhatSpawner()
    {
        if (spawnOne && PM.canSpawn)
        {
            firstSpawn.SetActive(true);
            secondSpawn.SetActive(false);
            thridSpawn.SetActive(false);
            fourthSpawn.SetActive(false);
            fifthSpawn.SetActive(false);
            Spawner = firstSpawn.transform;
            //Grid = PM._MoveGrid;
            //SetBlockGrid();
        }
        if (spawnTwo)
        {
            firstSpawn.SetActive(false);
            secondSpawn.SetActive(true);
            thridSpawn.SetActive(false);
            fourthSpawn.SetActive(false);
            fifthSpawn.SetActive(false);
            Spawner = secondSpawn.transform;
        }
        if (spawnThree)
        {
            firstSpawn.SetActive(false);
            secondSpawn.SetActive(false);
            thridSpawn.SetActive(true);
            fourthSpawn.SetActive(false);
            fifthSpawn.SetActive(false);
            Spawner = thridSpawn.transform;
        }
        if (spawnFour)
        {
            firstSpawn.SetActive(false);
            secondSpawn.SetActive(false);
            thridSpawn.SetActive(false);
            fourthSpawn.SetActive(true);
            Spawner = fourthSpawn.transform;
        }
        if (spawnFive)
        {
            firstSpawn.SetActive(false);
            secondSpawn.SetActive(false);
            thridSpawn.SetActive(false);
            fourthSpawn.SetActive(false);
            fifthSpawn.SetActive(true);
            Spawner = fifthSpawn.transform;
        }

    }

    public void RandomLoad()
    {
        //This is really how coding goes sometimes... you start with a complicated way and then you figure out an easier way of solving an issue
        //I may want to change this so that i can blocks already in the list for a specific level
        for (int x = 0; x < playerBlockSize; x++)
        {
            int rand = Random.Range(0, testBlocks.Count);
            Object[] yowie = testBlocks.ToArray();
            playerBlocks.Add(yowie[rand]);
            testBlocks.Remove(yowie[rand]);
        }
    }
}
