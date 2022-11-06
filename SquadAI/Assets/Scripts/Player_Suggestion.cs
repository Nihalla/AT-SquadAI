using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Player_Suggestion : MonoBehaviour
{
    private GameObject player;
    private GameObject[] friendly_NPCs;
    private GameObject[] cover_points;
    //private GameObject[] enemy_NPCs;
    private List<GameObject> enemy_NPCs = new();
    private bool suggest_retreat = false;
    private bool suggest_attack = false;
    public GameObject action_button;
    public GameObject retreat_button;
    public GameObject attack_button;
    private bool menu_open = false;
    private int need_to_retreat = 0;
    private int should_attack = 0;
    private AI_Manager manager_script;
    private NavMeshAgent auto_move_agent;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        //player_script = player.GetComponent<Player_Movement_FPS>();
        friendly_NPCs = GameObject.FindGameObjectsWithTag("AI");
        cover_points = GameObject.FindGameObjectsWithTag("Cover");
        //enemy_NPCs = GameObject.FindGameObjectsWithTag("Enemy");
        var temp_array = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject t in temp_array)
        {
            enemy_NPCs.Add(t);
        }

        manager_script = GameObject.FindGameObjectWithTag("AI_Manager").GetComponent<AI_Manager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.GetComponent<Player_Movement_FPS>().InTacticalCam())
        {
            action_button.SetActive(true);
            need_to_retreat = 0;
            should_attack = 0;
            foreach (GameObject NPC in friendly_NPCs)
            {
                if (NPC.GetComponent<AI_State>().GetHealth() <= 5)
                {
                    //suggest_retreat = true;
                    need_to_retreat++;
                }
                if (NPC.GetComponent<AI_State>().GetHealth() >= 8 && NPC.GetComponent<AI_State>().GetState() != AI_State.State.ATTACKING && enemy_NPCs.Count > 0)
                {
                    should_attack++;
                }
                if (NPC.GetComponent<AI_State>().in_cover)
                {
                    need_to_retreat--;
                }

                /*if (!suggest_attack)
                {
                    if (NPC.GetComponent<AI_State>().GetState() == AI_State.State.ATTACKING)
                    {
                        if (Vector3.Distance(NPC.transform.position, NPC.GetComponent<AI_State>().nearest_enemy.transform.position) <= 15)
                        {
                            Debug.Log("in range and attacking and eligible - " + NPC.name);
                        }
                        //NavMeshAgent move_agent = NPC.GetComponent<NavMeshAgent>();
                        //Debug.Log(move_agent.isStopped);
                        //move_agent.
                    }
                }*/
                //move_agent.isStopped
                /*if (move_agent.pathStatus.ToString() == "PathComplete")
                {
                    Debug.Log("finished walking lol");
                }*/
            }

            suggest_retreat = need_to_retreat > 0;
            suggest_attack = should_attack > 0;

        }
        else
        { 
            action_button.SetActive(false);
            retreat_button.SetActive(false);
            attack_button.SetActive(false);
        }

        if (!suggest_retreat)
        {
            retreat_button.GetComponent<Image>().color = Color.gray;
            retreat_button.GetComponent<Button>().interactable = false;
        }
        else
        {
            retreat_button.GetComponent<Image>().color = Color.green;
            retreat_button.GetComponent<Button>().interactable = true;
        }

        if (!suggest_attack)
        {
            attack_button.GetComponent<Image>().color = Color.gray;
            attack_button.GetComponent<Button>().interactable = false;
        }
        else
        {
            attack_button.GetComponent<Image>().color = Color.green;
            attack_button.GetComponent<Button>().interactable = true;
        }

        if (enemy_NPCs.Count <= 0)
        {
            suggest_attack = false;
        }
    }

    public void SuggestRetreat()
    {
        foreach (GameObject NPC in friendly_NPCs)
        {
            if (NPC.GetComponent<AI_State>().GetHealth() <= 5)
            {
                manager_script.FindCover(NPC);
            }
        }

        //send agent to nearest cover
    }
    public void SuggestAttack()
    {
        foreach (GameObject NPC in friendly_NPCs)
        {
            if (NPC.GetComponent<AI_State>().GetHealth() >= 8)
            {
                //manager_script.FindCover(NPC);
                //dist_to_enemy = Vector3.Distance(player.transform.position, transform.position);
                //GameObject nearest_enemy = null;
                auto_move_agent = NPC.GetComponent<NavMeshAgent>();

                float smallest_dist = 999f;
                Vector3 go_to = NPC.transform.position;
                foreach (GameObject enemy in enemy_NPCs)
                {
                    if (enemy != null)
                    {
                        float dist = Vector3.Distance(NPC.transform.position, enemy.transform.position);
                        if (dist < smallest_dist)
                        {
                            smallest_dist = dist;
                            go_to = enemy.transform.position;
                            NPC.GetComponent<AI_State>().nearest_enemy = enemy;
                            //nearest_enemy = enemy;
                        }
                    }
                }
                //Debug.Log("I should be moving to location - " + go_to + " the smallest distance I found was - " + smallest_dist);
                auto_move_agent.destination = go_to;

                NPC.GetComponent<AI_State>().SetToAttack();
                //auto_move_agent.destination = transform.position;
                auto_move_agent.stoppingDistance = 15f;
                if (enemy_NPCs.Count > 0)
                { 
                    NPC.transform.LookAt(NPC.GetComponent<AI_State>().nearest_enemy.transform); 
                }
                //agent.destination = player.transform.position;
                /*Debug.Log("I am - " + NPC.name + " and my closest enemy is in distance = " + Vector3.Distance(NPC.transform.position, nearest_enemy.transform.position));
                if (Vector3.Distance(NPC.transform.position, nearest_enemy.transform.position) <= 20f)
                {
                    Debug.Log("I am in 20f range");
                   
                    
                }*/
            }
        }
        suggest_attack = false;
    }

    public void HandleMenu()
    {
        menu_open = !menu_open;
        retreat_button.SetActive(menu_open);
        attack_button.SetActive(menu_open);
    }

    public void RemoveEnemy(GameObject enemy)
    {
        //enemy_NPCs. 
        if (enemy_NPCs.Contains(enemy))
        {
            enemy_NPCs.Remove(enemy);
        }
    }
}
