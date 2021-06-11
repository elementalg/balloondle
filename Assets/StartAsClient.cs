using MLAPI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartAsClient : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Starting client...");
        NetworkManager.Singleton.StartClient();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
