using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Camera_Switch : MonoBehaviour
{

    [SerializeField] private Camera main_cam;
    [SerializeField] private Camera tact_cam;
    private GameObject player;
    private bool main_cam_active = true;
    private PlayerInputController controls;
    private Player_Movement_FPS player_script;

    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.FindWithTag("Player");
        player_script = player.GetComponent<Player_Movement_FPS>();
        //tact_cam.gameObject.SetActive(false);
        controls = new PlayerInputController();

        controls.Camera.Switch.performed += ctx => SwitchActiveCam();
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
    private void SwitchActiveCam()
    {
        player_script.ChangeControl();
        if (main_cam_active)
        {
            main_cam.gameObject.SetActive(false);
            tact_cam.gameObject.SetActive(true);
            main_cam_active = false;
        }
        else
        {
            tact_cam.gameObject.SetActive(false);
            main_cam.gameObject.SetActive(true);
            main_cam_active = true;
        }
    }

}
