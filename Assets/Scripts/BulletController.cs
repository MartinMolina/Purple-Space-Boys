using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private GameManager gameManager;
    [SerializeField] private Rigidbody2D body;
    [SerializeField] private GameObject impact;
    [SerializeField] private float damage;
    [SerializeField] private float speed;
    private bool exited;

    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        body.velocity *= speed;
        /*if (body.velocity.y == 0)
        {
            transform.position += new Vector3(0, 1, 0);
            GetComponent<BoxCollider2D>().offset = new Vector2(0, -1);
        }*/
    }

    void OnCollisionExit2D(Collision2D coll)
    {
        exited = true;
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (exited)
        {
            Instantiate(impact, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
