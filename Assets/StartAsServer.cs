using MLAPI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartAsServer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Starting as server.");
        NetworkManager.Singleton.StartServer();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
