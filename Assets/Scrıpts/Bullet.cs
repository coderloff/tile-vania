using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] float bulletSpeed = 25f;
    float xSpeed;
    PlayerMovement playerMovement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerMovement = FindObjectOfType<PlayerMovement>();
        xSpeed = playerMovement.transform.localScale.x * bulletSpeed;
    }

    void Update()
    {
        rb.velocity = new Vector2(xSpeed, 0f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Enemy")
        {
            Destroy(other.gameObject);
        }
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        Destroy(gameObject);
    }
}
