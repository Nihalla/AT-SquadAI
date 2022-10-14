using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Movement_FPS : MonoBehaviour
{

    PlayerInputController controls;
    public Vector2 move;
    public Vector2 look;
    public float deadzone = 0.1F;
    public float speed = 10.0f;
    public float turn_smooth_time = 0.1f;
    private float turn_smooth_velocity = 0.5f;
    public float speed_multiplier = 1.0f;
    public float turn_speed_max = 1f;

    CharacterController controller;
    public Transform cam_transform;
    private bool move_forward;
    private bool in_tactical = false;
    // Start is called before the first frame update
    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        controls = new PlayerInputController();
        controls.Player.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => move = Vector2.zero;
        controls.Player.Look.performed += ctx => look = ctx.ReadValue<Vector2>();
        controls.Player.Look.canceled += ctx => look = Vector2.zero;

    }

    private void OnEnable()
    {
        controls.Player.Enable();
    }
    public void EnableInput()
    {
        controls.Player.Enable();
    }
    private void OnDisable()
    {
        controls.Player.Disable();
    }
    public void DisableInput()
    {
        controls.Player.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        if (!in_tactical)
        {
            if (move.y > 0)
            {
                move_forward = true;
            }
            else
            {
                move_forward = false;
            }
            if ((move_forward) || (look.x != 0))
            {
                Vector3 input_direction = new Vector3(move.x, 0.0f, move.y);
                Vector3 mouse_direction = new Vector3(look.x, 0, 0);
                Vector3 rotate = RotateCalc(input_direction, cam_transform.eulerAngles.y, mouse_direction);
                Vector3 movement = XZMoveCalc(rotate, input_direction);

                //movement.y = jump_velocity;

                controller.Move(movement * Time.deltaTime);
                //Debug.Log(look);
            }
            if (move.y < 0)
            {
                Vector3 input_direction = new Vector3(0.0f, 0.0f, move.y);
                //Vector3 mouse_direction = new Vector3(look.x, 0, 0);
                Vector3 rotate = RotateCalc(new Vector3(0, 0, 0), cam_transform.eulerAngles.y, new Vector3(0, 0, 0));
                Vector3 forward = Quaternion.Euler(rotate).normalized * Vector3.forward;
                Vector3 movement = -forward * speed * speed_multiplier;
                controller.Move(movement * Time.deltaTime);
            }
        }
    }

    private Vector3 RotateCalc(Vector3 input_direction, float anchor_rotation, Vector3 mouse_direction)
    {

        input_direction.Normalize();
        mouse_direction.Normalize();
        float rotateAngle = Mathf.Atan2(mouse_direction.x, input_direction.z) * Mathf.Rad2Deg  + anchor_rotation;
        float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, rotateAngle, ref turn_smooth_velocity, turn_smooth_time, turn_speed_max);
        transform.rotation = Quaternion.Euler(0.0f, smoothAngle, 0.0f);

        return new Vector3(0.0f, rotateAngle, 0.0f);
    }

    private Vector3 XZMoveCalc(Vector3 direction, Vector3 input)
    {

        speed_multiplier = 0.5f + 2 * input.magnitude;
        
        Vector3 forward = Quaternion.Euler(direction).normalized * Vector3.forward;
        Vector3 movement = forward * speed * speed_multiplier;
        if ((!Compare2Deadzone(move.x) && !Compare2Deadzone(move.y)))
        {
            movement = Vector3.zero;
        }
        return movement;
    }

    private bool Compare2Deadzone(float value)
    {
        if (value < deadzone)
        {
            if (value > -deadzone)
            {
                return false;
            }
        }
        return true;
    }

    public void ChangeControl()
    {
        in_tactical = !in_tactical;
    }

    public bool InTacticalCam()
    {
        return in_tactical;
    }
}
