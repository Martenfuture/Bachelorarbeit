using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class WeaponHandler : MonoBehaviour
{
    public static WeaponHandler instance;
    public int StartWeapon = 0;

    public GameObject HitEffectBlood;
    public GameObject HitEffectWall;

    private GameObject _activeWeapon;
    private GameObject[] _weapons;

    private LayerMask _rayCastLayerMask;
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
        _rayCastLayerMask = GameManager.instance.ShootingLayerMask;
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
        Transform camera = GameManager.instance.Camera.transform;


        RaycastHit hit;
        if (Physics.Raycast(camera.position, camera.forward, out hit, 100f, _rayCastLayerMask))
        {
            string hitType = "Wall";
            if (hit.transform.CompareTag("Enemy"))
            {
                hitType = "Enemy";
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
            GameObject hitEffect = null;
            switch (hitType)
            {
                case "Wall":
                    hitEffect = Instantiate(HitEffectWall, hit.point, Quaternion.identity);
                    break;
                case "Enemy":
                    hitEffect = Instantiate(HitEffectBlood, hit.point, Quaternion.identity);
                    break;
            }
            Debug.DrawLine(_activeWeapon.transform.GetChild(0).position, hit.point, Color.red, 1f, true);
            hitEffect.transform.LookAt(camera.forward);
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
