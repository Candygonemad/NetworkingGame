using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Bullet : NetworkBehaviour
{
    public float timeBetweenShots = .5f;
    public float moveSpeed = 5f;
    public Vector2 moveDirection;

    [HideInInspector]
    public int damage;

    public NetworkVariable<Vector2> Position = new NetworkVariable<Vector2>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
    }

    private void OnEnable()
    {
        Invoke("Destroy", 15f);
    }

    public void SetMoveDirection(Vector2 dir)
    {
        moveDirection = dir;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Enemy")
        {
            collision.gameObject.GetComponent<Enemy>().TakeDamage(damage);
            Destroy();
        }
    }

    private void Destroy()
    {
        gameObject.SetActive(false);
    }

    [ServerRpc(RequireOwnership = false)]
    public void BulletMoveServerRpc()
    {
        if (!IsOwner)
            return;
        Debug.Log("Bullet moving");
        Position.Value = transform.position;
    }
}
