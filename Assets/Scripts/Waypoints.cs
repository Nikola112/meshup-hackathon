using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    public static Waypoints Instance;

    private List<Waypoint> waypoints;

    private void Awake()
    {
        Instance = this;

        waypoints = new List<Waypoint>();

        var children = transform.GetComponentsInChildren<Waypoint>();

        for (int i = 0; i < children.Length; i++)
        {
            waypoints.Add(children[i]);
        }
    }

    public Waypoint GetNearestWaypoint(Objective target, Vector3 position)
    {
        List<Waypoint> objectiveWaypoints = waypoints.Where(w => w.target == target).ToList();

        Waypoint theChoosenOne = null;
        Vector3 pos;
        float dist = float.MaxValue;

        for (int i = 0; i < objectiveWaypoints.Count; i++)
        {
            float currentDist = Vector3.Distance(objectiveWaypoints[i].transform.position, position);
            if (currentDist < dist)
            {
                theChoosenOne = objectiveWaypoints[i];
                pos = theChoosenOne.transform.position;
                dist = currentDist;
            }
        }

        return theChoosenOne;
    }
}
