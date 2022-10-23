using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cover : MonoBehaviour
{
    [SerializeField] private bool is_taken = false;
    [SerializeField] private GameObject occupied_by;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (!is_taken)
        {
            if (collision.gameObject.tag == "AI")
            {
                is_taken = true;
                occupied_by = collision.gameObject;
                occupied_by.GetComponent<AI_State>().in_cover = true;
            }
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (is_taken)
        {
            if(collision.gameObject== occupied_by)
            {
                is_taken = false;
                occupied_by.GetComponent<AI_State>().in_cover = false;
                occupied_by = null;
            }
        }
    }

    public bool IsTaken()
    {
        return is_taken;
    }
}
