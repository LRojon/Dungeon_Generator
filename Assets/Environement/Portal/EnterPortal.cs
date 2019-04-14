using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterPortal : MonoBehaviour
{

    public GameObject targetPortal;
    public int exitX;
    public int exitY;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "Player")
        {
            Vector3 Position = targetPortal.transform.position;
            col.gameObject.transform.position = new Vector3(Position.x + exitX, Position.y + exitY);
        }
    }
}
