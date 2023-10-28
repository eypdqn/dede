using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Bullet : MonoBehaviour
{
    [SerializeField] float bulletSpeed = 20f;
    Rigidbody2D bulletRigidBody2d;
    PlayerMovement Player;
    float xSpeed;
    void Start()
    {
        bulletRigidBody2d = GetComponent<Rigidbody2D>();
        Player = FindAnyObjectByType<PlayerMovement>();
        xSpeed = Player.transform.localScale.x * bulletSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        bulletRigidBody2d.velocity = new Vector2(xSpeed, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject);
        }
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
