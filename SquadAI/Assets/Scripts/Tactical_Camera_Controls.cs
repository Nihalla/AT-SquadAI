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

    // Start is called before the first frame update
    void Awake()
    {
        controls = new PlayerInputController();
        controls.Camera.Movement.performed += ctx => move = ctx.ReadValue<Vector2>();
        controls.Camera.Movement.canceled += ctx => move = Vector2.zero;
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
        current_pos.Set(current_pos.x + move.x, current_pos.y , current_pos.z + move.y);
        this_cam.transform.position = current_pos;
    }
}
