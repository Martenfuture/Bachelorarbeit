using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class HealthPack : MonoBehaviour
{

    public int HealhAmount = 30;

    private bool _isPickedUp = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !_isPickedUp)
        {
            _isPickedUp = true;
            int playerHealth = PlayerController.instance.Health;
            int playerMaxHealth = PlayerController.instance.MaxHealth;
            if (playerHealth >= playerMaxHealth)
            {
                return;
            }
            PlayerController.instance.Health = Mathf.Min(playerHealth + HealhAmount, playerMaxHealth);
            UIHandler.instance.UpdateUIHealth(PlayerController.instance.Health, true);

            gameObject.GetComponent<MeshRenderer>().enabled = false;

            StartCoroutine(ReactivateDelay());
        }
    }

    IEnumerator ReactivateDelay()
    {
        yield return new WaitForSeconds(10);

        _isPickedUp = false;
        gameObject.GetComponent<MeshRenderer>().enabled = true;
    }
}
