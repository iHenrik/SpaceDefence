using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private GameObject ammo;

    private float fireRate = 0.2f;
    private float nextFire = 0f;

    private void Update()
    {
        FollowMouse();

        if (Input.GetMouseButtonUp(0) && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;

            var ammoObject = Instantiate(ammo, transform.position, transform.rotation);
            ammoObject.GetComponent<Ammo>().AmmoUser = Ammo.AmmoUserType.Player;
        }
    }

    private void FollowMouse()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(
            new Vector3(Input.mousePosition.x, Input.mousePosition.y, Mathf.Abs(Camera.main.transform.position.z - transform.position.z)));

        transform.position = mousePosition;
    }
}
