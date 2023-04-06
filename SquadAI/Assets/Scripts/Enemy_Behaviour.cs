using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_Behaviour : MonoBehaviour
{
    private GameObject target;
    private bool has_target;
    private float attack_cd = 1f;
    private int health = 5;
    [SerializeField] private GameObject bullet;
    private Player_Suggestion manager_script;
    private Camera_Switch camera_script;
    private NavMeshAgent agent;
    private float timer = 3f;
    private float max_timer = 3f;
    private Vector2 rand_range = new Vector2(-3, 3);
    // private float respawn_timer = 5f;
    // Start is called before the first frame update
    void Start()
    {
        manager_script = GameObject.FindGameObjectWithTag("Game_Manager").GetComponent<Player_Suggestion>();
        camera_script = GameObject.FindGameObjectWithTag("Camera_Manager").GetComponent<Camera_Switch>();
        agent = gameObject.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (has_target)
        {
            gameObject.transform.LookAt(target.gameObject.transform);
            if (attack_cd <= 0)
            {
                Attack(target.gameObject);
                attack_cd = 1f;
            }
            else
            {
                attack_cd -= Time.deltaTime;
            }
        }
        if (health <= 0)
        {
            camera_script.RemoveEnemy(this.gameObject);
            manager_script.RemoveEnemy(this.gameObject);
            Destroy(this.gameObject);
        }
    }

    private void FixedUpdate()
    {
        if (has_target)
        {
            agent.destination = target.transform.position;
        }
        else
        {
            if (timer <= 0)
            {
                Vector3 destination = transform.position + new Vector3(Random.Range(rand_range.x, rand_range.y), 0, Random.Range(rand_range.x, rand_range.y));
                agent.SetDestination(destination);
                timer = max_timer;
            }
            else
            {
                timer -= Time.deltaTime;
            }
        }
    }

    public void TakeDamage()
    {
        health--;
    }

    public void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other);
        if (has_target == false && (other.gameObject.tag == "AI" || other.gameObject.tag == "Player"))
        {
            target = other.gameObject;
            has_target = true;
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (has_target == false && (other.gameObject.tag == "AI" || other.gameObject.tag == "Player"))
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
        
        Vector3 bullet_spawn = this.gameObject.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).position;
        Instantiate(bullet, bullet_spawn, gameObject.transform.rotation);
    }
}
