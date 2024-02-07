using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : UIController
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Image healthBar;
    [SerializeField] private TMP_Text health;
    [SerializeField] private Image weapon;
    [SerializeField] private Image timeBar;
    [SerializeField] private TMP_Text movesLeft;
    [SerializeField] private TMP_Text victoryCaption;

    void Start()
    {
        gameManager.OnTurnChange += UpdateHUD;
        foreach (PlayerController player in FindObjectsOfType<PlayerController>())
        {
            /*player.OnHealthChange += UpdateHealth;
            player.OnWeaponChange += UpdateWeapon;
            player.OnMovement += UpdateMoves;*/
        }
    }

    void Update()
    {
        timeBar.fillAmount = gameManager.turnTimer / gameManager.turnDuration;
    }

    public void UpdateHUD(float health, float maxHealth, GameObject weapon, int movesLeft)
    {
        UpdateHealth(health, maxHealth);
        UpdateWeapon(weapon);
        UpdateMoves(movesLeft);
    }

    void UpdateHealth(float h, float m)
    {
        health.text = h.ToString();
        healthBar.fillAmount = h / m;
    }

    public void UpdateWeapon(GameObject w)
    {
        try
        {
            weapon.enabled = true;
            weapon.sprite = w.GetComponent<SpriteRenderer>().sprite;
        }
        catch
        {
            weapon.enabled = false;
        }

    }

    public void UpdateMoves(int m)
    {
        movesLeft.text = m.ToString();
    }

    public void AnnounceWinner(string winner)
    {
        victoryCaption.text = "Team " + winner + " wins!";
    }
}
