
using System.Collections.Generic;
using UnityEngine;

public class BGLOOPING : MonoBehaviour
{
    // Start is called before the first frame update
    public int speed = 4;
    public Vector3 startPosition, loopPosition;
  
    public GameObject bg1;
    public GameObject bg2;
    public GameObject swap;
    void Start()
    {
        startPosition = bg1.transform.position;
        loopPosition = bg2.transform.position;

    }
    void Update()
    {
        transform.Translate(Vector3.left * Time.deltaTime * speed, Space.World);
        if (bg2.transform.position.x < startPosition.x)
        {
            bg1.transform.position = loopPosition;
            swap = bg1;
            bg1 = bg2;
            bg2 = swap;
            //LOOPING SCROLLING CHANGING STUFF

        }
    }
}
