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
    private NavMeshAgent auto_move_agent;
    private GameObject player;
    private GameObject[] friendly_NPC;
    private GameObject[] cover_points;
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
        friendly_NPC = GameObject.FindGameObjectsWithTag("AI");
        cover_points = GameObject.FindGameObjectsWithTag("Cover");
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
        /*if (selected_char != null)
        {
            Debug.Log("this is character - " + selected_char.name);
            Debug.Log(selected_char.gameObject.GetComponent<AI_State>().GetState());
        }*/
        
    }

    private void FixedUpdate()
    {
        foreach (GameObject NPC in friendly_NPC)
        {
            //Debug.Log("Hi my name is - " + NPC.name);
            AI_State script = NPC.GetComponent<AI_State>();
            

            if (script.GetState() == AI_State.State.COVERING && !script.in_cover)
            {
                FindCover(NPC);
            }
        }
    }

    public void FindCover(GameObject NPC)
    {
        NPC.GetComponent<AI_State>().SetToCover();
        auto_move_agent = NPC.GetComponent<NavMeshAgent>();
        
        float smallest_dist = 999f;
        Vector3 go_to = NPC.transform.position;
        auto_move_agent.stoppingDistance = 0f;
        foreach (GameObject point in cover_points)
        {
            //Debug.Log(point.GetComponent<Cover>().IsTaken());
            if (!point.GetComponent<Cover>().IsTaken())
            {
                //Debug.Log("There are free cover points!");
                float dist = Vector3.Distance(NPC.transform.position, point.transform.position);
                if (dist < smallest_dist)
                {
                    smallest_dist = dist;
                    go_to = point.transform.position;
                }
            }
        }
        //Debug.Log("I should be moving to location - " + go_to + " the smallest distance I found was - " + smallest_dist);
        auto_move_agent.destination = go_to;
        //auto_move_agent.
    }
    private void SelectCharacter()
    {
        if (player_script.InTacticalCam())
        {
            Ray ray = tactical_cam.ScreenPointToRay(mouse_pos);
            RaycastHit hit;

            
            if (Physics.Raycast(ray, out hit, 1000f))
            {
                //Debug.Log(hit.transform.tag);

                if (selected_char == null && hit.transform.tag == "AI")
                {
                    AssignActiveChar(hit);
                }
                else if (selected_char != null && hit.transform.tag == "Floor")
                {
                    
                    selected_char.gameObject.GetComponent<AI_State>().SetToGuard();
                    //Debug.Log(selected_char.gameObject.GetComponent<AI_State>().GetState());
                    agent.destination = hit.point;
                }
                else if (selected_char == hit.transform)
                {
                    Deselect();
                }
                else if (selected_char != hit.transform && hit.transform.tag == "AI")
                {
                    Deselect();
                    AssignActiveChar(hit);
                }
                else if (selected_char != null && hit.transform.tag == "Enemy")
                {
                    selected_char.gameObject.GetComponent<AI_State>().StartChase(hit.transform.gameObject);
                    Deselect();
                }
            }
        }
    }

    private void AssignActiveChar(RaycastHit hit)
    {
        selected_char = hit.transform;
        selected_char.GetChild(1).gameObject.SetActive(true);
        agent = selected_char.gameObject.GetComponent<NavMeshAgent>();
    }
    private void Deselect()
    {
        if (selected_char != null)
        {
            selected_char.GetChild(1).gameObject.SetActive(false);
            selected_char = null;
            agent = null;
        }
    }
    private void Unguard()
    {
        if (selected_char != null)
        {
            selected_char.gameObject.GetComponent<AI_State>().SetToIdle();
        }
    }
}