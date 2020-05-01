using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleObjectCreater : MonoBehaviour
{
    [SerializeField]
    private GameObject createObject;

    [SerializeField]
    private int itemCount = 40;

    [SerializeField]
    private float radius = 5f;

    [SerializeField]
    private float repeat = 2f;
    
    // Start is called before the first frame update
    void Start()
    {
        var oneCycle = 2.0f * Mathf.PI;
        for (var i = 0; i < itemCount; i++)
        {
            var point = ((float)i / itemCount) * oneCycle;
            var repeatPoint = point * repeat; //繰り返し位置

            var x = Mathf.Cos(repeatPoint) * radius;
            var y = Mathf.Sin(repeatPoint) * radius;

            var position = new Vector3(x, y, 0);

            Instantiate(
                createObject,
                position,
                Quaternion.identity,
                transform
            );

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
