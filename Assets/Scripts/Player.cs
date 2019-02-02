using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private GameObject _ammo;

    [SerializeField]
    private GameObject _barrelTip;

    [SerializeField]
    private float _cooldownTime = 2f;

    [SerializeField]
    private AudioClip _machineGunClip;

    private float _fireRate = 0.1f;
    private float _nextFire = 0f;
    private float _speed = 5f;
    private float _cooldownEndTime = 0f;
    private float _nextMachineGunAudioPlay = 0;

    [HideInInspector]
    public bool HasCooldown = false;

    private void Start()
    {
        HasCooldown = true;
        _cooldownEndTime = Time.time + _cooldownTime;
    }

    private void Update()
    {
        FollowMouse();
        FireBurst();

        if(HasCooldown && Time.time > _cooldownEndTime)
        {
            HasCooldown = false;
        }
    }

    private void FireBurst()
    {
        if(Input.GetMouseButton(0) && Time.time > _nextFire)
        {
            _nextFire = Time.time + _fireRate;

            var ammoObject = Instantiate(_ammo, _barrelTip.transform.position, transform.rotation);
            ammoObject.GetComponent<Ammo>().AmmoUser = Ammo.AmmoUserType.Player;

            if(Time.time > _nextMachineGunAudioPlay)
            {
                _nextMachineGunAudioPlay = Time.time + _machineGunClip.length;
                AudioSource.PlayClipAtPoint(_machineGunClip, transform.position);
            }
        }
    }

    private void FollowMouse()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Mathf.Abs(Camera.main.transform.position.z - transform.position.z)));
        transform.position = mousePosition;
    }
}
