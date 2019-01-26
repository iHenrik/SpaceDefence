using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private GameObject laser;

    private float fireRate = 0.25f;
    private float nextFire = 0f;

    void Update()
    {
        FollowMouse();

        if (Input.GetMouseButtonUp(0) && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;

            Instantiate(laser, transform.position, transform.rotation);
        }
    }

    private void FollowMouse()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(
            new Vector3(Input.mousePosition.x, Input.mousePosition.y, Mathf.Abs(Camera.main.transform.position.z - transform.position.z)));

        transform.position = mousePosition;
    }
}
