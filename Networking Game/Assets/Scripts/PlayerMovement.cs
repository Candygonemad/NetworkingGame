using Unity.Netcode;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    public float speed = .2f;
    private Vector2 movement;
    private Rigidbody2D rb2D;
    public NetworkVariable<Vector2> Position = new NetworkVariable<Vector2>();
    public GameObject bulletPool;
    private GameObject singleBulletPool;

    public AIDirector director;

    // Start is called before the first frame update
    void Start()
    {
        if (!IsOwner)
            return;

        rb2D = GetComponent<Rigidbody2D>();
        //Debug.Log("PM Start");

        singleBulletPool = Instantiate(bulletPool);

        if (!IsHost || !IsServer)
            BulletPoolSpawnServerRpc();
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;
        movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    private void FixedUpdate()
    {
        if (!IsOwner) return;

        rb2D.velocity = movement * speed * Time.fixedDeltaTime;

        transform.position = new Vector3(transform.position.x + rb2D.velocity.x, transform.position.y + rb2D.velocity.y, transform.position.z);

        if (IsServer || IsHost)
        {
            Debug.Log("WOWOWOWOWOW");
            Position.Value = transform.position;
        }
        else
        {
            Debug.Log("Movement Update ServerRPC");
            MoveServerRpc();
        }
        //MoveServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    public void MoveServerRpc()
    {
        Debug.Log("wazzzuppppppppppp");
        Position.Value = transform.position;
    }

    [ServerRpc]
    public void BulletPoolSpawnServerRpc()
    {
        if (!IsOwner)
            return;
        Debug.Log("yooooooooooooooo");
        //singleBulletPool = Instantiate(bulletPool);
        if(!IsHost || !IsServer)
            singleBulletPool.GetComponent<NetworkObject>().SpawnWithOwnership(NetworkManager.Singleton.ServerClientId);
    }
}
