using UnityEngine;
using UnityEngine.AI;

public class RoverMovment : MonoBehaviour
{

    public Vector3 StartDestination;
    public GameObject artifact,spawner;

    public NavMeshAgent agent;

    public bool PickedUp, StartMovment;

    private bool flag=true;
    private float dist;
    private bool TrueClue;



    // Use this for initialization
    void Start()
    {
        StartDestination = this.transform.position;

      

    }

    // Update is called once per frame
    void Update()
    {
        if ((StartMovment == true))
        {

            if ((PickedUp == false) && (flag == true))
            {
                flag = false;
                agent.SetDestination(artifact.transform.position);
            }
            else if ((PickedUp == true) && (flag == true))
            {
                flag = false;
                if (TrueClue == false)
                    agent.SetDestination(StartDestination);
                else agent.SetDestination(spawner.transform.position);
            }
        }
        dist = Vector3.Distance(this.transform.position, artifact.transform.position);
        if ((dist<1) && (PickedUp==false))
        {
            flag = true;
            PickedUp = true;
            TrueClue = artifact.GetComponent<ClueObject>().TrueClue;
        }


    }
}
