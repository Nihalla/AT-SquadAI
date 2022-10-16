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
    public GameObject bullet;
    private float attack_cd = 0.5f;
    [SerializeField] private bool has_target = false;
    [SerializeField] private GameObject target;
    //private Collider line_of_sight;

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
        //line_of_sight = this.gameObject.GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        dist = Vector3.Distance(player.transform.position, this.gameObject.transform.position);
        if (!player.GetComponent<Player_Movement_FPS>().InTacticalCam())
        {
            if (this.current_state == State.IDLE)
            {
                if (dist > 5f)
                {
                    //Debug.Log("Distance is greater than 10 = " + dist + " and I am agent - " + this.gameObject.name);
                    SetToFollow();
                }
            }
        }
        if(this.current_state == State.FOLLOWING)
        {
            Follow();
        }

        if (has_target)
        {
            if (attack_cd <= 0)
            {
                Attack(target.gameObject);
                attack_cd = 0.5f;
            }
            else
            {
                attack_cd -= Time.deltaTime;
            }
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

    public void SetToGuard()
    {
        this.current_state = State.GUARDING;
    }

    public void SetToIdle()
    {
        this.current_state = State.IDLE;
    }

    public void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other);
        if (has_target == false && other.gameObject.tag == "Enemy")
        {
            target = other.gameObject;
            has_target = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject == target)
        {
            has_target = false;
        }
    }

    private void Attack(GameObject current_target)
    {
        gameObject.transform.LookAt(current_target.transform);
        Vector3 bullet_spawn = this.gameObject.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).position;
        Instantiate(bullet, bullet_spawn, gameObject.transform.rotation);    
    }
}
