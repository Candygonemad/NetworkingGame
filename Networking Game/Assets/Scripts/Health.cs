using Unity.Netcode;
using UnityEngine;

public class Health : NetworkBehaviour
{
    public NetworkVariable<int> maxHealth = new NetworkVariable<int>(3);
    [HideInInspector]
    public NetworkVariable<int> currentHealth;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        // Health should be modified server-side only
        if (!IsServer) return;
        currentHealth.Value -= amount;

        // TODO: You can play a VFX/SFX here if needed

        if (currentHealth.Value <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
