using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PostEffect;
using System.Linq;

public enum SwitchModes
{
    HumanMode,
    AutoMode,
    MomentaryHumanMode

};

public enum PosteffecTypes
{
    TROUGH,
    NEGATIVE,
    GLITCH,
    DISTORTION,
}

[RequireComponent(typeof(Through))]
[RequireComponent(typeof(Negative))]
[RequireComponent(typeof(Glitch))]
[RequireComponent(typeof(Distortion))]


public class cameraEffect : MonoBehaviour
{
    #region serialized data 
    [SerializeField] private float effectSpan = 120;
    private Material curMat;
    #endregion

    #region private data
    private List<Material> materials;
    private List<bool> isEffectOn;

    Material throughMaterial;
    Material negativeMaterial;
    Material glitchMaterial;
    Material distortionMaterial;

    BasePostEffect postEffect, temp;
    #endregion



    #region effect parameters used only MomentaryHumanMode
    [SerializeField] [Range(0, 240)] int NegativeTime = 50;
    [SerializeField] [Range(0, 240)] int glitchTime = 50;
    [SerializeField] [Range(0, 240)] int distortionTime = 20;
    #endregion

    #region enum
    [SerializeField] private SwitchModes switchMode = SwitchModes.HumanMode;
    [SerializeField] PosteffecTypes posteffectType = PosteffecTypes.NEGATIVE;

    public SwitchModes SwitchMode
    {
        get { return switchMode; }
        set { switchMode = value; }
    }

    public PosteffecTypes PosteffectType
    {
        get { return posteffectType; }
        set { posteffectType = value; }
    }
    #endregion

    private void Awake()
    {

    }

    void Start()
    {
        init();
    }

    void Update()
    {
        //Automatically
        if (SwitchMode == SwitchModes.AutoMode)
        {
            if (Time.frameCount % effectSpan == 0)
            {
                curMat = materials[Random.Range(0, materials.Count)];
            }
        }
        //human
        else if (SwitchMode == SwitchModes.HumanMode)
        {
            inputs();
            switch (PosteffectType)
            {
                case PosteffecTypes.TROUGH:
                    curMat = throughMaterial;
                    break;

                case PosteffecTypes.NEGATIVE:
                    curMat = negativeMaterial;
                    break;
               
                case PosteffecTypes.GLITCH:
                    curMat = glitchMaterial;
                    break;
                
                case PosteffecTypes.DISTORTION:
                    curMat = distortionMaterial;
                    break;
               
            }
        }
        else if (SwitchMode == SwitchModes.MomentaryHumanMode)
        {
            inputs();
            switch (PosteffectType)
            {
                case PosteffecTypes.NEGATIVE:
                    if (!isEffectOn[1])
                    {
                        StartCoroutine("invertColorCoroutine");
                    }
                    break;

             
                case PosteffecTypes.GLITCH:
                    if (!isEffectOn[4])
                    {
                        StartCoroutine("glitchCoroutine");
                    }
                    break;

               
                case PosteffecTypes.DISTORTION:
                    if (!isEffectOn[12])
                    {
                        StartCoroutine("distortionCoroutine");
                    }
                    break;
               
            }
        }

    }

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        Graphics.Blit(src, dest, curMat);
    }

    void init()
    {
        throughMaterial = GetComponent<Through>().material;
        negativeMaterial = GetComponent<Negative>().material;
        glitchMaterial = GetComponent<Glitch>().material;
        distortionMaterial = GetComponent<Distortion>().material;
       



        materials = new List<Material>();
        materials.Add(throughMaterial);
        materials.Add(negativeMaterial);      
        materials.Add(glitchMaterial);
        materials.Add(distortionMaterial);
       
        isEffectOn = Enumerable.Repeat(false, materials.Count).ToList();

        curMat = throughMaterial;
        PosteffectType = PosteffecTypes.TROUGH;
    }


    void inputs()
    {

        if (Input.GetKeyDown("1"))
        {
            posteffectType = PosteffecTypes.TROUGH;
            postEffect = this.GetComponent<Through>();
            postEffect.IsActive = true;
        }
        else if (Input.GetKeyDown("2"))
        {
            posteffectType = PosteffecTypes.NEGATIVE;
            postEffect = this.GetComponent<Negative>();
            postEffect.IsActive = true;

        }
        else if (Input.GetKeyDown("3"))
        {
            posteffectType = PosteffecTypes.GLITCH;
            postEffect.GetComponent<Glitch>();
            postEffect.IsActive = true;
        }
        else if (Input.GetKeyDown("4"))
        {
            posteffectType = PosteffecTypes.DISTORTION;
            postEffect = this.GetComponent<Distortion>();
            postEffect.IsActive = true;

        }
    }


      

    
}