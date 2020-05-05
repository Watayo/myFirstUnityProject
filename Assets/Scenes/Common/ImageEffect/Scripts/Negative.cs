using UnityEngine;
using PostEffect;

public class Negative : BasePostEffect
{
    const string SHADER_NAME = "Hidden/Negative";


    private void Awake()
    {
        shaderName = SHADER_NAME;
    }

    private void Update()
    {
    }
}
