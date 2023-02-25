using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class portalSpawer : MonoBehaviour
{
   [SerializeField]List<GameObject> enemies = new List<GameObject>();
    [SerializeField] Animator myanim;

    // Start is called before the first frame update
    void OnEnable()
    {

        StartCoroutine(SpawnEnemies());

    }
    
      
         IEnumerator SpawnEnemies()
        {
            int _spawnNumberOfEnemies = Random.Range(1,3);
            int enemyspawnCounter = 0;

            while (enemyspawnCounter < _spawnNumberOfEnemies)
            {
                enemies[0].transform.position = transform.position;
                enemies[0].SetActive(true);
                enemies.Remove(enemies[0]);

                enemyspawnCounter++;

                yield return new WaitForSeconds(.3f);

            }
            gameObject.SetActive(false);
        }
    
}
