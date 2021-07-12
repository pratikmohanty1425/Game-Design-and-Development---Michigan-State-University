using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullHealPickup : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Health PlayerHealth = collision.gameObject.GetComponent<Health>();
            PlayerHealth.ReceiveHealing(PlayerHealth.maximumHealth);
            Destroy(this.gameObject);
        }
    }
}
