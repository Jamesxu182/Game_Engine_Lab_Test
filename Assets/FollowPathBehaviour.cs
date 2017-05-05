using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPathBehaviour : SteeringBehaviour
{
    public List<Vector3> shell;

    private int index = 0;

    public Vector3 centre = Vector3.zero;

    // Use this for initialization
    void Start()
    {
        FindTheCloestPoint();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override Vector3 Calculate()
    {
        Vector3 next = shell[index];

        if (Vector3.Distance(transform.position, next + centre) < 1.0f)
        {
            if(index == (shell.Count -1))
            {
                index = 0;
            }

            else
            {
                index++;
            }

        }

        return boid.SeekForce(next + centre);
    }

    private void FindTheCloestPoint()
    {
        float minDistance = 0;
        if(shell.Count > 0)
        {
            minDistance = Vector3.Distance(shell[0] + centre, transform.position);
            index = 0;
        }

        for(int i = 1; i < shell.Count; i++)
        {
            float distance = Vector3.Distance(shell[i] + centre, transform.position);
            if(distance < minDistance)
            {
                minDistance = distance;
                index = i;
            }
        }
    }
}
