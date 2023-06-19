using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    public PlayerInputActions PlayerControls;

    public int Health = 100;
    public int MaxHealth = 100;
    private int _startMaxHealth = 100;

    private InputAction _fire;
    private InputAction _fireHold;
    private void Awake()
    {
        instance = this;
        PlayerControls = new PlayerInputActions();
    }

    private void Start()
    {
        GameManager.instance.OnDifficultyChange += ChangeDifficulty;
    }

    private void OnEnable()
    {
        _fire = PlayerControls.Player.Fire;
        _fire.Enable();
        _fire.performed += FireSingle;


        _fireHold = PlayerControls.Player.FireHold;
        _fireHold.Enable();
        _fireHold.performed += FireHoldStart;
        _fireHold.canceled += FireHoldCanceled;
    }

    private void OnDisable()
    {
        _fire.Disable();
        _fireHold.Disable();
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;
        UIHandler.instance.UpdateUIHealth(Health, false);
        Debug.Log("Player Health: " + Health);
        if (Health <= 0)
        {
            Debug.Log("Player Died");
        }
    }

    private void ChangeDifficulty(DifficultySetting difficultySettings)
    {
        MaxHealth = Mathf.RoundToInt(difficultySettings.PlayerHealthMultiplier * _startMaxHealth);
        UIHandler.instance.UpdateUIVariable("PlayerHealth", MaxHealth.ToString());
    }

    private void FireSingle(InputAction.CallbackContext context)
    {
        WeaponHandler.instance.FireWeapon();
    }

    private void FireHoldStart(InputAction.CallbackContext context)
    {
        WeaponHandler.instance.FireWeaponHold();
    }
    private void FireHoldCanceled(InputAction.CallbackContext context)
    {
        WeaponHandler.instance.FireWeaponRelease();
    }
}
