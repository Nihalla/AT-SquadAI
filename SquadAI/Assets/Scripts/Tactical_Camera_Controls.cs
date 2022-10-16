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
        //Vector3 rot_input_direction = new Vector3(rotation.x, 0.0f, rotation.y);
        //Vector3 forward = Quaternion.Euler(current_pos).normalized * Vector3.forward;
        //Rotate();
        //Vector3 forward = Quaternion.Euler(input_direction).normalized * Vector3.forward;
        //Vector3 movement = forward * 10f;
        //this_cam.gameObject.GetComponent<CharacterController>().Move(movement * Time.deltaTime);
        //Vector3 dir = new Vector3(move.x, 0, move.y);
        //Vector3 rotated_dir = this_cam.transform.rotation * dir;
        current_pos.Set(current_pos.x + move.x, current_pos.y, current_pos.z + move.y);
        this_cam.transform.position = current_pos;
        //this_cam.transform.rotation = Quaternion.Euler(new Vector3(45, this_cam.transform.rotation.eulerAngles.y + rotation, 0));
        //this_cam.transform.forward.Set(0,this_cam.transform.forward.y + rotation ,0);
        //this_cam.transform.rotation.eulerAngles.Set(this_cam.transform.rotation.eulerAngles.x, this_cam.transform.rotation.eulerAngles.y + rotation, this_cam.transform.rotation.eulerAngles.z);
        //Debug.Log(this_cam.transform.forward);

        //Debug.Log(rotation);
        //Vector3 input_direction = new Vector3(move.x, 0.0f, move.y);
        //Vector3 mouse_direction = new Vector3(look.x, 0, 0);
        //Vector3 rotate = RotateCalc(input_direction, this.transform.rotation.eulerAngles.y, rot_input_direction);
        //Vector3 movement = XZMoveCalc(rotate, input_direction);

        //movement.y = jump_velocity;

        //this_cam.gameObject.GetComponent<CharacterController>().Move(movement * Time.deltaTime);
    }

}
