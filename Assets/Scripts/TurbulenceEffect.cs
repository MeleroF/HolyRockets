using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurbulenceEffect : MonoBehaviour
{
    public float turbulenceStrength = 0.5f;
    public float turbulenceSpeed = 1.0f;

    private float seed;
    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
        seed = Random.value * 100f; // Unique per object
    }

    void Update()
    {
        float yOffset = (Mathf.PerlinNoise(Time.time * turbulenceSpeed, seed) - 0.5f) * 2f * turbulenceStrength;
        transform.position = new Vector3(startPos.x, startPos.y + yOffset, startPos.z);
    }
}
