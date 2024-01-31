using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class main : MonoBehaviour
{
    [SerializeField] private GameObject fishEyeLeft;
    [SerializeField] private GameObject fishEyeRight;
    private GameObject newFish;

    // Update is called once per frame
    void Update()
    {

        // instantiate left fish at edge of left pool if 1 is pressed
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Instantiate(fishEyeLeft, new Vector3(-7.5f, -2.5f, 0), transform.rotation);
            
        }

        // instantiate right fish at edge of right pool if 2 is pressed
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            newFish = Instantiate(fishEyeRight, new Vector3(7.5f, -2.5f, 0), transform.rotation);
        }

    }
}
