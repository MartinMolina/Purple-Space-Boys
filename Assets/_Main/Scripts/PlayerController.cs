using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private int speed;
    [SerializeField] public int health;
    [SerializeField] private GameObject shootPrefab;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Animator animator;
    [SerializeField] private float invincibilityTimer;

    private float invincibilityCooldown;

    private void Start()
    {
        if (gameObject.name == "Player")
            Respawn();  
        
    }

    private void Update()
    {
        invincibilityCooldown += Time.deltaTime;

        if (gameObject.name == "Player")
        {
            Aim();

            float horizontalAxis = Input.GetAxis("Horizontal");
            float verticalAxis = Input.GetAxis("Vertical");


            transform.position += Mathf.Abs(horizontalAxis) * transform.right * speed * Time.deltaTime;
            if (horizontalAxis < 0)
                transform.rotation = Quaternion.Euler(0, 180, 0);
            else if (horizontalAxis > 0)
                transform.rotation = Quaternion.Euler(0, 0, 0);

            transform.position += verticalAxis * transform.up * speed * Time.deltaTime;

            if (horizontalAxis != 0 || verticalAxis != 0)
                animator.SetBool("Moving", true);
            else
                animator.SetBool("Moving", false);

            if (Input.GetKey(KeyCode.K))
            {
                health = 0;
            }

            if (Input.GetKeyDown(KeyCode.Mouse0) && animator.GetBool("Launcher"))
            {
                Shoot();
                animator.SetBool("Launcher", false);
            }
        }

        if (health <= 0)
            Kill();

    }


    private void Aim()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector2 direction = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);
        shootPoint.transform.right = direction;

    }

    private void Shoot()
    {
        Instantiate(shootPrefab, shootPoint.position, shootPoint.rotation);
    }

    private void Respawn()
    {
        transform.position = spawnPoint.position;
    }

    private void Kill()
    {
        GetComponent<AudioSource>().Play();
        transform.rotation = Quaternion.Euler(0, transform.rotation.y, 90);
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<Animator>().enabled = false;
        GetComponent<PlayerController>().enabled = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Lava" && invincibilityCooldown >= invincibilityTimer)
        {
            health -= 50;
            invincibilityCooldown = 0;
        }
    }
}