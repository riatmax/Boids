using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtAttractor : MonoBehaviour
{
    private int cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = 0;
    }

    // Update is called once per frame
    void Update()
    {
        switch (cam)
        {
            case 0:
                transform.LookAt(Attractor.POS);
                break;
            case 1:
                transform.LookAt(NewAttractor.POS);
                break;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (cam == 1)
            {
                cam = 0;
            }
            else
            {
                cam = 1;
            }
        }
           
    }
}
