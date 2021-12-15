using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public abstract class Enemy : NetworkBehaviour
{
    public int health = 3;
    public float speed = 4f;
    public Vector3 direction;
    public int timeAlive = 7;
    public int rotationSpeed = 720;

    public NetworkVariable<Vector2> Position = new NetworkVariable<Vector2>();
    public NetworkVariable<Quaternion> Rotation = new NetworkVariable<Quaternion>();

    // Start is called before the first frame update
    public virtual void Start()
    {
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        transform.position += speed * direction * Time.deltaTime;
        Quaternion toRotation = Quaternion.LookRotation(Vector3.forward, direction);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);

        if(IsHost || IsServer)
        {
            Position.Value = transform.position;
            Rotation.Value = transform.rotation;
        }
        else
        {
            EnemyMoveServerRpc();
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
            Destroy();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //Debug.Log("Enemy HIT Player");
            collision.gameObject.GetComponent<Health>().TakeDamage(1);
        }
    }

    private void OnEnable()
    {
        Invoke("Destroy", timeAlive);
    }

    private void Destroy()
    {
        if(IsHost || IsServer)
        {
            Destroy(this.gameObject);
        }
            
        else
            DestroyServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    public void EnemyMoveServerRpc()
    {
        Position.Value = transform.position;
        Rotation.Value = transform.rotation;
    }

    [ServerRpc(RequireOwnership = false)]
    public void DestroyServerRpc()
    {
        gameObject.GetComponent<NetworkObject>().Despawn(true);
        NetworkManager.Destroy(gameObject);
        Destroy(this.gameObject);
    }
}
