using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamondScript : MonoBehaviour
{
    World_Builder wb;
    public GameObject particle;

    // Start is called before the first frame update
    void Start()
    {
        wb = GameObject.FindGameObjectsWithTag("GameManager")[0].GetComponent<World_Builder>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            wb.lowerCurrentDiamonds();
            Instantiate(particle, transform.position, Quaternion.identity); 
            Destroy(gameObject);
        }
    }
}
