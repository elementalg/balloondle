using MLAPI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawn : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Vector3 randomPosition = new Vector3(Random.Range(-8f, 8f), Random.Range(-4f, 4f), 0f);
        transform.position = randomPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (NetworkManager.Singleton.IsClient)
        {
            Vector3 randomPosition = new Vector3(Random.Range(-8f, 8f), Random.Range(-4f, 4f), 0f);
            transform.position = randomPosition;
        }
    }
}
