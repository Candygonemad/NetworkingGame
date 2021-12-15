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

    [HideInInspector]
    public AIDirector director;

    public NetworkVariable<Vector2> Position = new NetworkVariable<Vector2>();
    public NetworkVariable<Quaternion> Rotation = new NetworkVariable<Quaternion>();
    // Start is called before the first frame update
    void Start()
    {
        director = GameObject.Find("AI Director").GetComponent<AIDirector>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
        if(!IsHost)
            BulletMoveServerRpc();
    }

    private void OnEnable()
    {
        Invoke("Destroy", 7f);
    }

    public void SetMoveDirection(Vector2 dir)
    {
        moveDirection = dir;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Enemy")
        {
            director.enemiesHit++;
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
        //Debug.Log("Bullet Shmoving");
        Position.Value = transform.position;
        Rotation.Value = transform.rotation;
    }
}
