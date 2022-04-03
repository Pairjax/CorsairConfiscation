using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class parallax : MonoBehaviour
{
    public Transform[] Backgrounds;                //Array of background and forground
    private float[] ParallaxScales;
    public float Smoothing = 1f;                        //Minimal 1

    private Transform cam;
    private Vector3 PreCamPo;

    void Awake()
    {
        cam = Camera.main.transform;
    }

    // Use this for initialization
    void Start()
    {
        PreCamPo = cam.position;

        ParallaxScales = new float[Backgrounds.Length];

        for (int i = 0; i < Backgrounds.Length; i++)
        {
            ParallaxScales[i] = Backgrounds[i].position.z * -1;
        }
    }
}
