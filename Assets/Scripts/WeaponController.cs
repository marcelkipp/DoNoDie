using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponController : MonoBehaviour
{

    public GameObject bullet;
    public GameObject firePoint;
    public Camera cam;

    private float rayLength;

    PlayerControlls controls;

    private void Awake()
    {

        controls = new PlayerControlls();
        controls.Gameplay.Shoot1.performed += _ => Shoot();
    }

    void Start()
    {

    }

    void Update()
    {
        
    }

    void Shoot()
    {
        if (firePoint != null)
        {
            //drawShootLine();
            GameObject vfx = Instantiate(bullet, firePoint.transform.position, Quaternion.Euler(firePoint.transform.rotation.eulerAngles.x, firePoint.transform.rotation.eulerAngles.y, firePoint.transform.rotation.eulerAngles.z));
            //vfx.transform.LookAt(GetHitPoint(firePoint.transform.position));
        } else
        {
            Debug.Log("Kein firePoint übergeben");
        }
    
    }

    void OnEnable()
    {
        controls.Gameplay.Enable();
    }

    void OnDisable()
    {
        controls.Gameplay.Disable();
    }

    Vector3 GetHitPoint(Vector3 muzzlePosition)
    {
        // Unless you've mucked with the projection matrix, firing through
        // the center of the screen is the same as firing from the camera itself.
        Ray crosshair = new Ray(cam.transform.position, cam.transform.forward);
        

        // Cast a ray forward from the camera to see what's 
        // under the crosshair from the player's point of view.
        Vector3 aimPoint;
        RaycastHit hit;
        if (Physics.Raycast(crosshair, out hit, rayLength))
        {
            aimPoint = hit.point;
            Debug.Log("jh");
        }
        else
        {
            aimPoint = crosshair.origin + crosshair.direction * rayLength;
        }

        // Now we know what to aim at, form a second ray from the tool.
        Ray beam = new Ray(muzzlePosition, aimPoint - muzzlePosition);

        // If we don't hit anything, just go straight to the aim point.
        if (!Physics.Raycast(beam, out hit, rayLength))
            return aimPoint;

        // Otherwise, stop at whatever we hit on the way.
        return hit.point;
    }

    void drawShootLine()
    {
        Color color = new Color(0, 0, 1.0f);
        Debug.DrawLine(firePoint.transform.position, GetHitPoint(firePoint.transform.position), color, 3f);
    }
}
