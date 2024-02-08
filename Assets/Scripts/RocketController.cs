using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D body;
    [SerializeField] private GameObject explosion;
    [SerializeField] private float damage;
    [SerializeField] private float speed;
    private bool exited;

    void Start()
    {
        /*if (body.velocity.y == 0)
            {
                transform.position += new Vector3(0, 1, 0);
                GetComponent<BoxCollider2D>().offset = new Vector2(0, -1);
            }*/
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (exited)
        {
            Instantiate(explosion, transform.position, new Quaternion());
            Destroy(gameObject);
        }
        else
            exited = true;
    }
}
