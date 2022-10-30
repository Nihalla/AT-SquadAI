using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_State : MonoBehaviour
{
    [SerializeField] private State current_state;
    private GameObject player;
    private float dist_to_player = 0f;
    private NavMeshAgent agent;
    public GameObject bullet;
    private float attack_cd = 0.5f;
    private int max_health = 7;
    [SerializeField] private int health;
    public bool in_cover = false;
    private float regen_timer = 10f;
    private bool flip = false;
    private float flip_rotation_timer = 2f;
    private float rotation_speed = 1000f;
    private float desired_rotation;
    private float damping = 10f;
    [SerializeField] private bool has_target = false;
    [SerializeField] private GameObject target;
    //private Collider line_of_sight;

    public enum State
    {
        IDLE = 0,
        MOVING = 1,
        FOLLOWING = 2,
        GUARDING = 3,
        COVERING = 4,
        CHASING = 5,
        INVALID = -1
    };

    // Start is called before the first frame update
    void Awake()
    {
        
        health = max_health;
        player = GameObject.FindGameObjectWithTag("Player");
        current_state = State.IDLE;
        agent = this.gameObject.GetComponent<NavMeshAgent>();
        //line_of_sight = this.gameObject.GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 3)
        {
            current_state = State.COVERING;
            target = null;
            has_target = false;
        }
        else
        {
            dist_to_player = Vector3.Distance(player.transform.position, transform.position);
            if (!player.GetComponent<Player_Movement_FPS>().InTacticalCam())
            {
                if (current_state == State.IDLE)
                {
                    if (dist_to_player > 5f)
                    {
                        //Debug.Log("Distance is greater than 10 = " + dist + " and I am agent - " + this.gameObject.name);
                        SetToFollow();
                    }
                }
            }
            if (current_state == State.FOLLOWING)
            {
                Follow();
            }
            if (current_state != State.FOLLOWING && current_state != State.CHASING && !has_target)
            {
                desired_rotation = transform.eulerAngles.y;
                if (flip_rotation_timer > 0)
                {
                    flip_rotation_timer -= Time.deltaTime;
                }
                else
                {
                    flip_rotation_timer = 2f;
                    flip = !flip;
                }
                LookAround(flip);
            }
        }

        if (current_state == State.CHASING)
        {
            var distance_to_target = Vector3.Distance(target.transform.position, transform.position);
            if (distance_to_target >= 20)
            {
                agent.destination = target.transform.position;
            }
            else
            {
                agent.destination = transform.position;
            }
        }

        if (has_target && current_state!= State.COVERING)
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

    private void FixedUpdate()
    {
        RegenHealth();
    }

    public State GetState()
    {
        return current_state;
    }
    public void SetToFollow()
    {
        current_state = State.FOLLOWING;
    }

    private void Follow()
    {
        agent.destination = player.transform.position;
        
        if(dist_to_player <= 5f)
        {
            current_state = State.IDLE;
            agent.destination = transform.position;
        }
    }

    public void SetToGuard()
    {
        current_state = State.GUARDING;
    }

    public void SetToIdle()
    {
        current_state = State.IDLE;
    }

    private void LookAround(bool flip)
    {
        if (flip)
        {
            desired_rotation += rotation_speed * Time.deltaTime;
        }
        else
        {
            desired_rotation -= rotation_speed * Time.deltaTime;
        }
        
        var quat_rotation = Quaternion.Euler(transform.eulerAngles.x, desired_rotation, transform.eulerAngles.z);
        transform.rotation = Quaternion.Lerp(transform.rotation, quat_rotation, Time.deltaTime * damping);
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
    public void OnTriggerStay(Collider other)
    {
        if (has_target == false && (other.gameObject.tag == "Enemy"))
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

    private void RegenHealth()
    {
        if (health < 10)
        {
            if (regen_timer <= 0)
            {
                health++;
                regen_timer = 10f;
            }
            else
            {
                regen_timer -= Time.deltaTime;
            }
        }    
        if (health > 3 && current_state == State.COVERING)
        {
            current_state = State.IDLE;
        }
    }

    public void TakeDamage()
    {
        health--;
    }

    public void StartChase(GameObject selected_target)
    {
        has_target = true;
        target = selected_target;
        current_state = State.CHASING;
    }
}
