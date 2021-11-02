using Unity.Netcode;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    public float speed = .2f;
    private Vector2 movement;
    private Rigidbody2D rb2D;

    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
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

        Debug.Log("In fixed update");
        MoveServerRpc();
    }

    [ServerRpc]
    public void MoveServerRpc(ServerRpcParams rpcParams = default)
    {
        Debug.Log("In ServerRPC");
        rb2D.velocity = movement * speed * Time.fixedDeltaTime;

        this.gameObject.GetComponent<NetworkedPlayer>().Position.Value += rb2D.velocity;
        MoveClientRpc();
    }

    [ClientRpc]
    private void MoveClientRpc()
    {
        transform.position = this.gameObject.GetComponent<NetworkedPlayer>().Position.Value;
    }
}
