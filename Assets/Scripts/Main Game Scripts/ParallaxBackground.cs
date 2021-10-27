using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour {

    private float length;
    private float startPosition;
    public new GameObject camera;
    [SerializeField] private float parallaxEffect;

    private void Start() {
        startPosition = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    private void FixedUpdate() {
        float temp = camera.transform.position.x * (1 - parallaxEffect);

        // How far it has moved from the start point (World Space)
        float distance = camera.transform.position.x * parallaxEffect;

        transform.position = new Vector3(startPosition + distance, transform.position.y, transform.position.z);

        if (temp > startPosition + length)
            startPosition += length;
        else if (temp < startPosition - length)
            startPosition -= length;
    }
}
