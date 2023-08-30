using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    [SerializeField] public string team;
    [SerializeField] private Animator animator;
    public bool isActive;
    public GameObject weapon;
    GameObject projectile;
    [SerializeField] public float maxHealth;
    public float health;
    public int damageMultiplier;
    public int movesPerTurn;
    public int movesLeft;
    Vector2 aimDirection;
    private GameManager gameManager;
    public UnityAction<float, float> OnHealthChange;
    public UnityAction<GameObject> OnWeaponChange;
    public UnityAction<int> OnMovement;
    [SerializeField] private AudioSource deathSound;
    [SerializeField] private AudioSource healingSound;
    [SerializeField] private AudioSource weaponGrabSound;

    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        health = maxHealth;
    }

    void Update()
    {

        if (isActive)
        {
            if (movesLeft > 0)
            {
                if (Input.GetKeyDown(KeyCode.W) && !IsObstructed(Vector2.up))
                {
                    Move(Vector2.up);
                    CheckForLava();
                }
                else if (Input.GetKeyDown(KeyCode.A) && !IsObstructed(Vector2.left))
                {

                    transform.right = Vector2.left;
                    Move(Vector2.right);
                    CheckForLava();
                }
                else if (Input.GetKeyDown(KeyCode.S) && !IsObstructed(Vector2.down))
                {
                    Move(Vector2.down);
                    CheckForLava();
                }
                else if (Input.GetKeyDown(KeyCode.D) && !IsObstructed(Vector2.right))
                {
                    transform.right = Vector2.right;
                    Move(Vector2.right);
                    CheckForLava();
                }
            }

            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
                {
                    aimDirection = new Vector2(0, Input.GetAxisRaw("Vertical"));
                }
                else
                {
                    aimDirection = new Vector2(Input.GetAxisRaw("Horizontal"), 0);
                    transform.right = aimDirection;
                }

                if (!Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 0.1f) + aimDirection / 2, aimDirection, 0.5f, LayerMask.GetMask("Player", "Wall")) && weapon != null)
                {
                    projectile = Instantiate(weapon.GetComponent<WeaponController>().projectile, transform.position, Quaternion.Euler(0, 0, 0));
                    projectile.transform.right = aimDirection;
                    projectile.GetComponent<Rigidbody2D>().velocity = aimDirection;
                    gameManager.PauseTimer();
                    gameManager.virtualCamera.Follow = projectile.transform;
                }


            }
        }
    }

    void Move(Vector2 movement)
    {
        transform.Translate(movement);
        movesLeft--;
        OnMovement.Invoke(movesLeft);
    }

    public void CheckForLava()
    {
        if (Physics2D.Raycast(transform.position, Vector2.right, 0.01f, LayerMask.GetMask("Lava")))
        {
            TakeDamage(50);
        }
    }

    public void TakeDamage(float dmg)
    {
        health -= dmg;
        OnHealthChange.Invoke(health, maxHealth);
        if (health <= 0)
        {
            health = 0;
            Die();
        }
    }
    public void Heal(float h)
    {
        healingSound.Play();
        health += h;
        if (health > maxHealth)
        {
            health = maxHealth;
        }
        OnHealthChange.Invoke(health, maxHealth);
    }

    private void Die()
    {
        deathSound.Play();
        transform.right = Vector2.up;
        this.enabled = false;
        animator.enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
        gameManager.RemovePlayer(this);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            if (collision.gameObject.name.Split(" (")[0] == "MedKit")
            {
                if (health < maxHealth)
                {
                    Heal(collision.GetComponent<MedKitController>().healing);
                    Destroy(collision.gameObject);
                }
            }
            else
            {
                weapon = collision.gameObject;
                animator.ResetTrigger("Rocket Launcher");
                animator.ResetTrigger("Laser Pistol");
                animator.SetBool(collision.name.Split(" (")[0], true);
                collision.gameObject.SetActive(false);
                OnWeaponChange.Invoke(weapon);
                weaponGrabSound.Play();
            }
        }

    }



    bool IsObstructed(Vector2 direction)
    {
        return Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 0.1f) + direction / 2, direction, 0.5f, LayerMask.GetMask("Player", "Obstacle", "Wall"));
    }

}



