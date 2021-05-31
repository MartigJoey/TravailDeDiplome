using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public Transform tr;
    public Renderer renderer;
    int yMovement;
    // Start is called before the first frame update
    void Start()
    {
        yMovement = 1;
    }

    // Update is called once per frame
    void Update()
    {
        //if (tr.position.y >= 5)
        //    yMovement = -1;
        //
        //if (tr.position.y <= 0)
        //    yMovement = 1;
        //
        //tr.Translate(new Vector3(0, yMovement, 0) * Time.deltaTime);
    }
}
