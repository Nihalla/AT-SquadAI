using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

public class Camera_Switch : MonoBehaviour
{

    [SerializeField] private Camera main_cam;
    [SerializeField] private Camera tact_cam;
    private GameObject player;
    private bool main_cam_active = true;
    private PlayerInputController controls;
    private Player_Movement_FPS player_script;
    private Tactical_Camera_Controls cam_script;
    private int layer = 3;
    public Material m_on;
    public Material m_off;
    private GameObject[] objects;

    // Start is called before the first frame update
    void Awake()
    {
        objects = GetSceneObjects();
        player = GameObject.FindWithTag("Player");
        player_script = player.GetComponent<Player_Movement_FPS>();
        cam_script = tact_cam.GetComponent<Tactical_Camera_Controls>();
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
            SwitchOpacity();
        }
        else
        {
            tact_cam.gameObject.SetActive(false);
            main_cam.gameObject.SetActive(true);
            main_cam_active = true;
            SwitchOpacity();
        }
    }

    private void GetObjectsInLayer(GameObject[] root, int layer)
    {
        // List<GameObject> Selected = new List<GameObject>();
        foreach (GameObject t in root)
        {
            if (t.layer == layer && t.tag == "Wall" && player_script.InTacticalCam())
            {
                //Selected.Add(t);
                t.GetComponent<Renderer>().material = m_off;
            }
            else if (t.layer == layer && !player_script.InTacticalCam())
            {
                t.GetComponent<Renderer>().material = m_on;
            }
        }

    }

    private static GameObject[] GetSceneObjects()
    {
        return Resources.FindObjectsOfTypeAll<GameObject>()
                .Where(go => go.hideFlags == HideFlags.None).ToArray();
    }

    public void SwitchOpacity()
    {
        GetObjectsInLayer(objects, layer);
    }

}
