using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneSellector : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("a"))
        {
            SceneManager.LoadScene("Scene1");
        }
        if (Input.GetKey("b"))
        {
            SceneManager.LoadScene("Scene2");
        }
    }

    
}
