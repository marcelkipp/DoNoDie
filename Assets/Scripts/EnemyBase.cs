using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBase : MonoBehaviour
{
    public float life = 100;
    public float attack = 10;

    public Image healthBar;
    public GameObject healthbarCanvas;

    private void Start()
    {
        healthbarCanvas.SetActive(false);
    }

    private void Update()
    {
        healthbarCanvas.transform.LookAt(healthbarCanvas.transform.position + Camera.main.transform.rotation * Vector3.back, Camera.main.transform.rotation * Vector3.up);
    }

    // return true wenn Leben <= 0 ist
    public bool reduceLife(float reduce)
    {

        life = life - reduce;

        healthbarCanvas.SetActive(true);
        healthBar.fillAmount = life / 100f;

        if (life <= 0)
        {
            Destroy(healthbarCanvas); 
            return true;
        }  else
        {
            return false;
        }
    }
}
