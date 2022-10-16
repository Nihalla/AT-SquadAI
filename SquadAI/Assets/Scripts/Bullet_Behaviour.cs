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
        Destroy(this.gameObject);
    }
}
