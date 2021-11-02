using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 3;
    public float speed = 4f;
    public Vector3 direction;
    public int timeAlive = 7;
    public int rotationSpeed = 720;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += speed * direction * Time.deltaTime;
        Quaternion toRotation = Quaternion.LookRotation(Vector3.forward, direction);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
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
            Debug.Log("Enemy HIT Player");
            collision.gameObject.GetComponent<Health>().TakeDamage(1);
        }
    }

    private void OnEnable()
    {
        Invoke("Destroy", timeAlive);
    }

    private void Destroy()
    {
        Destroy(this.gameObject);
    }
}
