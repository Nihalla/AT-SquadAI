using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Behaviour : MonoBehaviour
{
    private float force = 200f;
    private Rigidbody body;

    private void Awake()
    {
        body = gameObject.GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void Update()
    {
        body.AddRelativeForce(new Vector3(0, 0, force));
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("I shot - " + collision.gameObject.tag);
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Player_Movement_FPS>().TakeDamage();
        }
        if (collision.gameObject.tag == "AI")
        {
            collision.gameObject.GetComponent<AI_State>().TakeDamage();
        }
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<Enemy_Behaviour>().TakeDamage();
        }
        Destroy(this.gameObject);
    }
}
