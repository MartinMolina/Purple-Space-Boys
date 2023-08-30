using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketController : MonoBehaviour
{
    private GameManager gameManager;
    [SerializeField] private Rigidbody2D body;
    [SerializeField] private GameObject explosion;
    [SerializeField] private float damage;
    [SerializeField] private float speed;
    private bool flag;

    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        body.velocity *= speed;
        if (body.velocity.y == 0)
        {
            transform.position += new Vector3(0, 1, 0);
            GetComponent<BoxCollider2D>().offset = new Vector2(0, -1);
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (flag)
        {
            Instantiate(explosion, transform.position, new Quaternion());
            coll.gameObject.GetComponent<PlayerController>()?.TakeDamage(damage);
            gameManager.NextTurn();
            Destroy(gameObject);
        }
    }

    void OnCollisionExit2D(Collision2D coll)
    {
        flag = true;
    }
}
