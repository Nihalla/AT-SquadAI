using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_State : MonoBehaviour
{
    [SerializeField] private State current_state;
    private GameObject player;
    private float dist = 0f;
    private NavMeshAgent agent;

    enum State
    {
        IDLE = 0,
        MOVING = 1,
        FOLLOWING = 2,
        GUARDING = 3,
        ATTACKING = 4,
        INVALID = -1
    };

    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        this.current_state = State.IDLE;
        agent = this.gameObject.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        dist = Vector3.Distance(player.transform.position, this.gameObject.transform.position);
        if (!player.GetComponent<Player_Movement_FPS>().InTacticalCam())
        {
            if (this.current_state != State.FOLLOWING)
            {
                if (dist > 5f)
                {
                    Debug.Log("Distance is greater than 10 = " + dist + " and I am agent - " + this.gameObject.name);
                    SetToFollow();
                }
            }
        }
        if(this.current_state == State.FOLLOWING)
        {
            Follow();
        }
    }

    public void SetToFollow()
    {
        this.current_state = State.FOLLOWING;
    }

    private void Follow()
    {
        agent.destination = player.transform.position;
        
        if(dist <= 5f)
        {
            this.current_state = State.IDLE;
            agent.destination = this.gameObject.transform.position;
        }
    }
}
