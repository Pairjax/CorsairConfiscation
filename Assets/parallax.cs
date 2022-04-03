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

    void Update()
    {
        PreCamPo = cam.position;

        for (int i = 0; i < Backgrounds.Length; i++)
        {
            float parallax_x = (PreCamPo.x - cam.position.x) * ParallaxScales[i];
            float parallax_y = (PreCamPo.y - cam.position.y) * ParallaxScales[i];

            float BackgroundTargetPosX = Backgrounds[i].position.x + parallax_x;
            float BackgroundTargetPosY = Backgrounds[i].position.y + parallax_y;

            Vector3 BackgroundTargetPos = new Vector3(BackgroundTargetPosX, BackgroundTargetPosY, Backgrounds[i].position.z);

            Backgrounds[i].position = Vector3.Lerp(Backgrounds[i].position, BackgroundTargetPos, Smoothing * Time.deltaTime);


        }
    }
}
