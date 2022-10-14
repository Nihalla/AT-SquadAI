using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_Manager : MonoBehaviour
{
    // Start is called before the first frame update
    PlayerInputController controls;
    [SerializeField] private Transform selected_char;
    private Player_Movement_FPS player_script;
    [SerializeField] private Camera tactical_cam;
    private Vector2 mouse_pos;
    private NavMeshAgent agent;
    private GameObject player;
    // Start is called before the first frame update
    private void Awake()
    {
        //controller = GetComponent<CharacterController>();
        controls = new PlayerInputController();
        controls.Player.Select.performed += ctx => SelectCharacter();
        controls.Camera.Look.performed += ctx => mouse_pos = ctx.ReadValue<Vector2>();
        controls.Player.Deselect.performed += ctx => Deselect();
        controls.Player.StopGuard.performed += ctx => Unguard();
        player = GameObject.FindGameObjectWithTag("Player");
        player_script = player.GetComponent<Player_Movement_FPS>();

    }

    private void OnEnable()
    {
        controls.Player.Enable();
        controls.Camera.Enable();
    }
    public void EnableInput()
    {
        controls.Player.Enable();
        controls.Camera.Enable();
    }
    private void OnDisable()
    {
        controls.Player.Disable();
        controls.Camera.Disable();
    }
    public void DisableInput()
    {
        controls.Player.Disable();
        controls.Camera.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        if (!player_script.InTacticalCam())
        {

            Deselect();
        }
    }

    private void SelectCharacter()
    {
        if (player_script.InTacticalCam())
        {
            Ray ray = tactical_cam.ScreenPointToRay(mouse_pos);
            RaycastHit hit;

            
            if (Physics.Raycast(ray, out hit, 1000f))
            {
                Debug.Log(hit.transform.tag);

                if (selected_char == null && hit.transform.tag == "AI")
                {
                    AssignActiveChar(hit);
                }
                else if (selected_char != null && hit.transform.tag == "Floor")
                {
                    selected_char.gameObject.GetComponent<AI_State>().SetToGuard();
                    agent.destination = hit.point;
                }
                else if (selected_char == hit.transform)
                {
                    Deselect();
                }
                else if (selected_char != hit.transform && hit.transform.tag == "AI")
                {
                    AssignActiveChar(hit);
                }
            }
        }
    }

    private void AssignActiveChar(RaycastHit hit)
    {
        selected_char = hit.transform;
        agent = selected_char.gameObject.GetComponent<NavMeshAgent>();
    }
    private void Deselect()
    {
        if (selected_char != null)
        {
            selected_char = null;
            agent = null;
        }
    }
    private void Unguard()
    {
        selected_char.gameObject.GetComponent<AI_State>().SetToIdle();
    }

}
