using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformGenerator : MonoBehaviour {

    [Header("Game Objects/Components:")]
    [SerializeField] private Transform generationPoint;
    [SerializeField] private GameObject platform;
    [SerializeField] private GameObject goldCoin;
    [SerializeField] private GameObject mage;

    [Header("Variables:")]
    [SerializeField] private float distanceBetweenPlatforms = 5f;
    [SerializeField] private bool createPlatform = true;

    private int platformsGenerated = 0; // Counter for the number of platforms generated.
    private const int platformsToGenerate = 30; // The number of platforms to generate.

    private void Start() {
    }

    private void Update() {
        if (createPlatform) {
            float randomHeight = randomPlatformHeight();
            Vector3 randomPlatformPosition = new Vector3(transform.position.x, randomHeight, transform.position.z);
            Vector3 goldCoinPosition = new Vector3(transform.position.x, randomHeight + .4f, transform.position.z);

            if (transform.position.x < generationPoint.position.x) {
                transform.position = new Vector2(transform.position.x + distanceBetweenPlatforms, transform.position.y);
                Instantiate(platform, randomPlatformPosition, transform.rotation);
                Instantiate(goldCoin, goldCoinPosition, transform.rotation);
                platformsGenerated++;
            }
        }

        if (platformsGenerated == platformsToGenerate) {
            createPlatform = false;
        }
    }

    private float randomPlatformHeight() {
        return Random.Range(-0.5f, 1f);
    }
}
