using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBoids : MonoBehaviour
{
    public Rigidbody rb;
    private NewNeighborhood nh;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        nh = GetComponent<NewNeighborhood>();

        pos = Random.insideUnitSphere * Spawner.S.spawnRadius;

        Vector3 vel = Random.onUnitSphere * Spawner.S.velocity;

        rb.velocity = vel;

        LookAhead();

        Renderer[] rends = gameObject.GetComponentsInChildren<Renderer>();

        foreach (Renderer r in rends)
        {
            r.material.color = Color.black;
        }
        TrailRenderer tRend = GetComponent<TrailRenderer>();
        tRend.material.SetColor("_TintColor", Color.white);
    }
    
    private void LookAhead()
    {
        transform.LookAt(pos + rb.velocity);
    }
   
    public Vector3 pos
    {
        get
        {
            return transform.position;
        }
        set
        {
            transform.position = value; 
        }
    }

    private void FixedUpdate()
    {
        Vector3 vel = rb.velocity;
        Spawner spn = Spawner.S;

        //Collision Avoidance - avoid neigbors who are too close
        Vector3 velAvoid = Vector3.zero;
        Vector3 tooClosePos = nh.avgClosePos;
        // If the response is Vector3.zero, then no need to react
        if (tooClosePos != Vector3.zero)
        {
            velAvoid = pos - tooClosePos;
            velAvoid.Normalize();
            velAvoid *= spn.velocity;
        }

        Vector3 velAvoidObs = Vector3.zero;
        Vector3 tooClosePosObs = nh.avgClosePosObs;

        if (tooClosePosObs != Vector3.zero)
        {
            velAvoidObs = pos - tooClosePosObs;
            velAvoidObs.Normalize();
            velAvoid *= spn.velocity;
        }

        //Velocity matching - Try to match velocity with neigbors
        Vector3 velAlign = nh.avgVel;
        // Only do more if the velAlign is not Vector3.zero
        if (velAlign != Vector3.zero)
        {
            // we're really interested in direction, so normalize the velocity
            velAlign.Normalize();
            // and then set it to the speeed we chose
            velAlign *= spn.velocity;
        }

        //Flock centering - move towards the center of local neighbors
        Vector3 velCenter = nh.avgPos;
        if (velCenter != Vector3.zero)
        {
            velCenter -= transform.position;
            velCenter.Normalize();
            velCenter *= spn.velocity;
        }

        //ATTRACTION - Move towards the Atttractor
        Vector3 delta = NewAttractor.POS - pos;
        //Check whether we're attracted or avoiding the Attractor
        bool attracted = (delta.magnitude > spn.attractPushDist);
        Vector3 velAttract = delta.normalized * spn.velocity;

        //Apply all the velocities
        float fdt = Time.fixedDeltaTime;
        if (velAvoid != Vector3.zero || velAvoidObs != Vector3.zero) 
        {
            if (velAvoid != Vector3.zero)
            {
                vel = Vector3.Lerp(vel, velAvoid, spn.collAvoid);
            }
            else
            {
                vel = Vector3.Lerp(vel, velAvoidObs, spn.collAvoid);
            }
        }
        else
        {
            if (velAlign != Vector3.zero)
            {
                vel = Vector3.Lerp(vel, velAlign, spn.velMatching * fdt);
            }
            if (velCenter != Vector3.zero)
            {
                vel = Vector3.Lerp(vel, velAlign, spn.flockCentering * fdt);
            }
            if (velAttract != Vector3.zero)
            {
                if (attracted)
                {
                    vel = Vector3.Lerp(vel, velAttract, spn.attractPull * fdt);
                }
                else
                {
                    vel = Vector3.Lerp(vel, -velAttract, spn.attractPush * fdt);
                }
            }
        }

        //set vel to the velocity set on the spawner singleton
        vel = vel.normalized * spn.velocity;
        // Finally assign this to the Rigidbody
        rb.velocity = vel;
        //Lock in the direction of the new velocity
        LookAhead();
    }
}
