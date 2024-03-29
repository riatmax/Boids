using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewAttractor : MonoBehaviour
{
    static public Vector3 POS = Vector3.zero;

    [Header("Set in Inspector")]
    public float radius = 10.0f;
    public float xPhase = 0.5f;
    public float yPhase = 0.4f;
    public float zPhase = 0.1f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // FiexedUpdate is called once per physics update (i.e. 50x/second)
    void FixedUpdate()
    {
        Vector3 tPos = Vector3.zero;
        Vector3 scale = transform.localScale;
        tPos.x = Mathf.Sin(xPhase + Time.time) * radius * scale.x;
        tPos.y = Mathf.Sin(yPhase + Time.time) * radius * scale.y + 50f;
        tPos.z = Mathf.Sin(zPhase + Time.time) * radius * scale.z;
        transform.position = tPos;
        POS = tPos;

    }
}
