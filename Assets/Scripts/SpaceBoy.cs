using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class SpaceBoy : MonoBehaviour
{
    private GameManager gameManager;
    public UnityEvent OnDeath = new UnityEvent();
    public UnityEvent OnTurnEnd = new UnityEvent();
    public UnityAction<float> OnHealthChange;
    public float maxHealth;
    public float health;
    public string team;
    public bool active;
    public int movesPerTurn;
    public int movesLeft;
    public GameObject weapon;
    private bool firing;

    // Start is called before the first frame update
    public void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        health = maxHealth;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        switch (collider.tag)
        {
            case "Rocket Launcher":
            case "Pistol":
            case "Hook Launcher":
                if (weapon == null || weapon.tag != collider.tag)
                    weapon = collider.gameObject;
                break;

            case "Medkit":
                Heal(gameManager.medkitHealing);
                break;

            case "Rocket":
                if (firing)
                    firing = false;
                else
                    Hurt(gameManager.rocketDamage);
                break;
            case "Bullet":
                if (firing)
                    firing = false;
                else
                    Hurt(gameManager.bulletDamage);
                break;

        }
    }


    void BeginTurn()
    {
        active = true;
    }

    void EndTurn()
    {
        active = false;
    }

    public void Move(Vector2 movement)
    {
        transform.Translate(movement);
        movesLeft--;
    }

    public void Fire(Vector2 direction)
    {
        firing = true;
        GameObject projectile = Instantiate(weapon.GetComponent<WeaponController>().projectile, transform.position, Quaternion.Euler(0, 0, 0));
        projectile.GetComponent<Rigidbody2D>().velocity = direction;
        projectile.transform.right = direction;
    }

    void Hurt(float damage)
    {
        health -= damage;
        if (health < 0)
            health = 0;
    }

    void Heal(float healing)
    {
        health += healing;
        if (health > maxHealth)
            health = maxHealth;
    }

}
