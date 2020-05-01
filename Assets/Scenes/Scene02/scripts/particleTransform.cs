using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particleTransform : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Vector3 pos = this.gameObject.transform.position;
        //this.gameObject.transform.position = new Vector3(pos.x + Time.deltaTime, pos.y, pos.z);
        //float rotY = Time.deltaTime * 40;
        this.gameObject.transform.Translate(0, Time.deltaTime, 0);
        //this.gameObject.transform.Rotate(0, rotY, 0);
    }
}
