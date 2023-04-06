using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Tactical_Camera_Controls : MonoBehaviour
{
    PlayerInputController controls;
    public Vector2 move;
    [SerializeField] private Camera this_cam;
    private Vector3 current_pos = Vector3.zero;
    
    //public bool in_tactical;
    //private float rotation;
    //public float turn_smooth_time = 0.1f;
    //private float turn_smooth_velocity = 0.5f;
    //public float speed_multiplier = 1.0f;

    public float turn_speed_max = 1f;
    //private Quaternion cam_rot;
    // Start is called before the first frame update
    void Awake()
    {
       
        
        controls = new PlayerInputController();
        controls.Camera.Movement.performed += ctx => move = ctx.ReadValue<Vector2>();
        controls.Camera.Movement.canceled += ctx => move = Vector2.zero;
        //controls.Camera.Rotation.performed += ctx => rotation = ctx.ReadValue<float>();
        //controls.Camera.Rotation.canceled += ctx => rotation = 0.0f;
        current_pos = this_cam.transform.position;

    }

    private void OnEnable()
    {
        controls.Camera.Enable();
    }
    public void EnableInput()
    {
        controls.Camera.Enable();
    }
    private void OnDisable()
    {
        controls.Camera.Disable();
    }
    public void DisableInput()
    {
        controls.Camera.Disable();
    }
   
    // Update is called once per frame
    void FixedUpdate()
    {
        
        current_pos.Set(current_pos.x + move.x, current_pos.y, current_pos.z + move.y);
        this_cam.transform.position = current_pos;
        var pos = transform.position;
        pos.x = Mathf.Clamp(transform.position.x, -18.0f, 18.0f);
        pos.z = Mathf.Clamp(transform.position.z, -44.0f, -6.0f);
        transform.position = pos;
    }

}
