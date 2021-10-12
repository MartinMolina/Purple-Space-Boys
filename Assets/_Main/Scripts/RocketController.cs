using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketController : MonoBehaviour
{
    [SerializeField] private int speed;
    [SerializeField] private int damage;

    private void Update()
    {
        transform.position += transform.right * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Item" && collision.gameObject.name != "Player")
            {
                Destroy(gameObject);
                collision.GetComponent<PlayerController>().health -= damage;
            }
    }
}
