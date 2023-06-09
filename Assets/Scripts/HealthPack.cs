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
            if (playerHealth >= 100)
            {
                return;
            }
            PlayerController.instance.Health = Mathf.Min(playerHealth + HealhAmount, 100);
            UIHandler.instance.UpdateUIHealth(PlayerController.instance.Health, true);

            Destroy(gameObject);
        }
    }
}
