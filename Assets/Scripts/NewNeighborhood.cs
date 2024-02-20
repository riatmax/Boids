using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewNeighborhood : MonoBehaviour
{
    // Modified Neighborhood class to account for NewBoids changes

    [Header("Set Dynamically")]
    public List<NewBoids> neighbors;

    public List<GameObject> obstacles;

    private SphereCollider coll;

    void Start()
    {
        neighbors = new List<NewBoids>();
        coll = GetComponent<SphereCollider>();
        coll.radius = Spawner.S.neighborDist / 2;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (coll.radius != Spawner.S.neighborDist / 2)
        {
            coll.radius = Spawner.S.neighborDist / 2;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Obstacle")
        {
            NewBoids b = other.GetComponent<NewBoids>();
            if (b != null)
            {
                if (neighbors.IndexOf(b) == -1)
                {
                    neighbors.Add(b);
                }
            }
        }
        else
        {
            GameObject obs = other.gameObject;
            if (obstacles.IndexOf(obs) == -1)
            {
                obstacles.Add(obs);
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.tag != "Obstacle")
        {
            NewBoids b = other.GetComponent<NewBoids>();
            if (b != null)
            {
                if (neighbors.IndexOf(b) != -1)
                {
                    neighbors.Remove(b);
                }
            }
        }
        else
        {
            GameObject obs = other.gameObject;
            if (obstacles.IndexOf(obs) != -1)
            {
                obstacles.Remove(obs);
            }
        }
    }


    public Vector3 avgPos
    {
        get
        {
            Vector3 avg = Vector3.zero;
            if (neighbors.Count == 0) return avg;

            for (int i = 0; i < neighbors.Count; i++)
            {
                avg += neighbors[i].pos;
            }
            avg /= neighbors.Count;

            return avg;

        }
    }

    public Vector3 avgVel
    {
        get
        {
            Vector3 avg = Vector3.zero;
            if (neighbors.Count == 0) return avg;

            for (int i = 0; i < neighbors.Count; i++)
            {
                avg += neighbors[i].rb.velocity;
            }
            avg /= neighbors.Count;

            return avg;
        }
    }

    public Vector3 avgClosePos
    {
        get
        {
            Vector3 avg = Vector3.zero;
            Vector3 delta;
            int nearCount = 0;
            for (int i = 0; i < neighbors.Count; i++)
            {
                delta = neighbors[i].pos - transform.position;
                if (delta.magnitude <= Spawner.S.collDist)
                {
                    avg += neighbors[i].pos;
                    nearCount++;
                }
            }

            if (nearCount == 0) return avg;

            avg /= nearCount;
            return avg;
        }
    }
    public Vector3 avgClosePosObs
    {
        get
        {
            Vector3 avg = Vector3.zero;
            Vector3 delta;
            int nearCount = 0;
            for (int i = 0; i < obstacles.Count; i++)
            {
                delta = obstacles[i].transform.position - transform.position;
                if (delta.magnitude <= Spawner.S.collDist)
                {
                    avg += obstacles[i].transform.position;
                    nearCount++;
                }
            }

            if (nearCount == 0)
            {
                return avg;
            }
            avg /= nearCount;
            return avg;
        }
    }
}
