using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockChange : MonoBehaviour
{
    BoxCollider2D bc;

    // Start is called before the first frame update
    void Start()
    {
        bc = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.tag != "PlacedBlock" && gameObject.tag == "Block")
        {
            gameObject.tag = "PlacedBlock";
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Ground"))
        {
            gameObject.layer = 0;
            gameObject.tag = "PlacedBlock";
            bc.size.Set(.16f, 16);
            bc.offset.Set(0, 7.91f);
            if (gameObject.tag != "PlacedBlock")
            {
                gameObject.layer = 0;
                gameObject.tag = "PlacedBlock";
            }
        }

        else if (collision.transform.CompareTag("Block"))
        {
            gameObject.tag = "PlacedBlock";
            gameObject.layer = 0;

            if (gameObject.tag != "PlacedBlock")
            {
                gameObject.layer = 0;
                gameObject.tag = "PlacedBlock";
            }
        }

        else if (collision.transform.CompareTag("PlacedBlock"))
        {
            gameObject.layer = 0;
            gameObject.tag = "PlacedBlock";
            //bc.size.Set(.16f, 16);
            //bc.offset.Set(0, 7.91f);
            if (gameObject.tag != "PlacedBlock")
            {
                gameObject.layer = 0;
                gameObject.tag = "PlacedBlock";
            }
        }
    }
}
