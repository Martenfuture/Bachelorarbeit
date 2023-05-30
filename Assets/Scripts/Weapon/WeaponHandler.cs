using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class WeaponHandler : MonoBehaviour
{
    public static WeaponHandler instance;
    public int StartWeapon = 0;

    public LayerMask RayCastLayerMask;
    public GameObject HitEffect;

    private GameObject _activeWeapon;
    private GameObject[] _weapons;

    private float _currentFireRate;
    private float _currentDamage;

    private bool _fireHold = false;
    private bool _fired = false;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        _weapons = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            _weapons[i] = transform.GetChild(i).gameObject;
            transform.GetChild(i).gameObject.SetActive(false);
        }

        transform.GetChild(StartWeapon).gameObject.SetActive(true);
        _activeWeapon = transform.GetChild(StartWeapon).gameObject;
        _currentFireRate = _activeWeapon.GetComponent<WeapponStats>().FireRate;
        _currentDamage = _activeWeapon.GetComponent<WeapponStats>().Damage;
    }

    public void FireWeapon()
    {
        if (_fired && !_fireHold)
        {
            return;
        }
        StartCoroutine(FireDelay());
        Transform child = _activeWeapon.transform.GetChild(0).gameObject.transform;
        Debug.DrawLine(_activeWeapon.transform.GetChild(0).position, _activeWeapon.transform.GetChild(0).position + _activeWeapon.transform.GetChild(0).forward * 100f, Color.red, 1f, true);


        RaycastHit hit;
        if (Physics.Raycast(child.position, child.forward, out hit, 100f, RayCastLayerMask))
        {
            if (hit.transform.CompareTag("Enemy"))
            {
                GameObject parentObject = hit.transform.parent.gameObject;
                while (parentObject.GetComponent<EnemyController>() == null)
                {
                    parentObject = parentObject.transform.parent.gameObject;
                    if (parentObject == null)
                    {
                        Debug.LogError("Enemy not found");
                        break;
                    }
                }
                if (hit.transform.name == "HitBoxHead")
                {
                    parentObject.GetComponent<EnemyController>().TakeDamage(_currentDamage * 2);
                } else
                {
                    parentObject.GetComponent<EnemyController>().TakeDamage(_currentDamage);
                }
            }
            GameObject hitEffect = Instantiate(HitEffect, hit.point, Quaternion.identity);
            hitEffect.transform.LookAt(child.forward);
            hitEffect.transform.localScale = hitEffect.transform.localScale * 0.25f;
            StartCoroutine(DeleteHitEffect(hitEffect));
        }
    }

    public void FireWeaponHold()
    {
        _fireHold = true;
        StartCoroutine(FireHold());
    }

    public void FireWeaponRelease()
    {
        _fireHold = false;
    }

    public void ChangeWeapon(GameObject weapon)
    {
        foreach (GameObject w in _weapons)
        {
            if (w.name == weapon.name)
            {
                _activeWeapon.SetActive(false);
                w.SetActive(true);
                _activeWeapon = w;
                break;
            }
        }
    }

    IEnumerator FireHold()
    {
        while (_fireHold)
        {
            FireWeapon();
            yield return new WaitForSeconds(_currentFireRate);
        }
    }

    IEnumerator FireDelay()
    {
        _fired = true;
        yield return new WaitForSeconds(_currentFireRate);
        _fired = false;
    }


    IEnumerator DeleteHitEffect(GameObject hitEffect)
    {
        yield return new WaitForSeconds(10f);
        Destroy(hitEffect);
    }
}