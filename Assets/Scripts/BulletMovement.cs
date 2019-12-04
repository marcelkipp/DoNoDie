using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{

    public float speed;
    public float damage;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(speed != 0)
        {
            transform.position += transform.forward * (speed * Time.deltaTime);
        } else
        {
            Debug.Log("Es wurde kein Speed gesetzt");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        speed = 0;
        Debug.Log("Hit"); 
        Destroy (gameObject);
    }
}
