using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{

    [field: SerializeField] protected float ParallaxEffectSpeed { get; set; } = 1f;
    private float Length { get; set; }
    private float StartPosition { get; set; }
    public GameObject MainCamera { get; set; }

    private void Awake()
    {
        if ((MainCamera = GameObject.Find("Interface/MainCamera")) is null)
        {
            Debug.LogError(
                "ERROR: <ParallaxEffect> - Interface/MainCamera game object was not found in game object " +
                "hierarchy."
            );
            Application.Quit(1);
        }
        
        if ((this.gameObject.GetComponent<SpriteRenderer>()) is null)
        {
            Debug.LogError(
                "ERROR: <ParallaxEffect> - " + this.gameObject.name + " game object is missing " +
                "SpriteRenderer component."
            );
            Application.Quit(1);
        }
    }

    private void Start()
    {
        StartPosition = this.gameObject.transform.position.x;
        Length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    private void Update()
    {
        float temp = (MainCamera.transform.position.x * (1 - ParallaxEffectSpeed));
        float distance = (MainCamera.transform.position.x * ParallaxEffectSpeed) ;
        
        this.gameObject.transform.position = new Vector3(
            StartPosition + distance, this.gameObject.transform.position.y, this.gameObject.transform.position.z
            );

        if (temp > StartPosition + Length)
        {
            StartPosition += Length;
        }
        else if (temp < StartPosition - Length)
        {
            StartPosition -= Length;
        }
    }
}
