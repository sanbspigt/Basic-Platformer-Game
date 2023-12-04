using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    [SerializeField]
    private Transform[] BGS;
    private float[] parallaxScales;

    public float smoothing;

    private Transform cam;

    private Vector3 prevCamPos;

    private void Awake()
    {
        cam = Camera.main.transform;
    }

    private void Start()
    {
        prevCamPos = cam.position;

        parallaxScales = new float[BGS.Length];
        for (int i = 0; i < BGS.Length; i++)
        {
            parallaxScales[i] = BGS[i].position.z * -1f;
        }
    }

    private void Update()
    {
        ParallaxEffector();
    }

    void ParallaxEffector()
    {
        for (int i = 0; i < BGS.Length; i++)
        {
            float parallax = (prevCamPos.x - cam.position.x)
                * parallaxScales[i];

            float targetPosX = BGS[i].position.x + parallax;

            Vector3 bgTargetPos = new Vector3(targetPosX,
                BGS[i].position.y, BGS[i].position.z);

            BGS[i].position = Vector3.Lerp(BGS[i].position,bgTargetPos,smoothing*Time.deltaTime);

        }
        prevCamPos = cam.position;
    }

}
