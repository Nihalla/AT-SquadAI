using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn_Enemies : MonoBehaviour
{
    [SerializeField] private GameObject spawnpoints;
    private int enemies;
    private float spawn_enemy_timer;
    [SerializeField] private GameObject enemy_prefab;

    // Start is called before the first frame update
    void Start()
    {
        enemies = FindObjectsOfType<Enemy_Behaviour>().Length;
    }

    // Update is called once per frame
    void Update()
    {
        enemies = FindObjectsOfType<Enemy_Behaviour>().Length;
        if(enemies < 3)
        {
            if(spawn_enemy_timer <=0)
            {
                Spawn();
                spawn_enemy_timer = 5f;
            }
            else
            {
                spawn_enemy_timer -= Time.deltaTime;
            }
        }
    }

    private void Spawn()
    {
        Instantiate(enemy_prefab, spawnpoints.transform.GetChild(Random.Range(0, 2)).position, Quaternion.identity);
    }
}
