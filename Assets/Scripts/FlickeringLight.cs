using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickeringLight : MonoBehaviour
{

    Light light;
    public GameObject materialObject;
    public float minWaitTime = 0.1f;
    public float maxWaitTime = 0.5f;

    void Start()
    {
        light = GetComponent<Light>();
        StartCoroutine(Flashing()); 
    }

    IEnumerator Flashing ()
    {
        while(true)
        {
            yield return new WaitForSeconds(Random.Range(minWaitTime, maxWaitTime));
            light.enabled = !light.enabled;
        }
    }
}
