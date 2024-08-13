using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class GateController : MonoBehaviour
{
    public GameObject monsterPrefab;
    public Transform player;
    public ParticleSystem par;
    private MenuController menuController;
    private float timer = 0f;
    private float spawnInterval = 10f;
    public int monsterCount = 3;
    void Start()
    {
        menuController = FindObjectOfType<MenuController>();
        timer = spawnInterval;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer = 0f;
            float distance = Vector3.Distance(player.position, transform.position);
            if (distance < 50f && monsterCount <= 3)
            {
                par.Play();
                GameObject monster = Instantiate(monsterPrefab, transform.position, transform.rotation);
                NavMeshAgent navAgent = monster.GetComponent<NavMeshAgent>();
                /*menuController.MonsterOn(monsterPrefab.name, transform.position);*/
                monsterCount--;
                menuController.GateOnMonster(gameObject, monsterCount);
            }
            else
            {
                par.Stop();
            }
        }
        if (monsterCount == 0)
        {
            Destroy(gameObject);
            menuController.OnObjectDestroyed(gameObject);
        }
        
    }

}