using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class HealthPack : MonoBehaviour
{

    public int HealhAmount = 30;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            int playerHealth = PlayerController.instance.Health;
            int playerMaxHealth = PlayerController.instance.MaxHealth;
            if (playerHealth >= playerMaxHealth)
            {
                return;
            }
            PlayerController.instance.Health = Mathf.Min(playerHealth + HealhAmount, playerMaxHealth);
            UIHandler.instance.UpdateUIHealth(PlayerController.instance.Health, true);

            Destroy(gameObject);
        }
    }
}
