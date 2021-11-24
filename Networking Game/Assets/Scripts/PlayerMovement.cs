using Unity.Netcode;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    public float speed = .2f;
    private Vector2 movement;
    private Rigidbody2D rb2D;
    public NetworkVariable<Vector2> Position = new NetworkVariable<Vector2>();

    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsClient) return;
        movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    private void FixedUpdate()
    {
        if (!IsLocalPlayer) return;

        rb2D.velocity = movement * speed * Time.fixedDeltaTime;

        if (NetworkManager.Singleton.IsServer)
        {

            Position.Value = transform.position;
        }
        else
        {
            MoveServerRpc();
        }
        transform.position = new Vector3(transform.position.x + rb2D.velocity.x, transform.position.y + rb2D.velocity.y, transform.position.z);
    }

    [ServerRpc(RequireOwnership = false)]
    public void MoveServerRpc()
    {
        Position.Value = transform.position;
    }
}
