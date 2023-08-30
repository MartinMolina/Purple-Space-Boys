using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    public float turnDuration;
    public CinemachineVirtualCamera virtualCamera;
    private List<PlayerController> player;
    private int i;
    public float turnTimer;
    public bool timerIsPaused;
    public UnityAction<float, float, GameObject, int> OnTurnChange;
    public UnityEvent OnGameEnd = new UnityEvent();


    // Start is called before the first frame update
    void Start()
    {
        player = new List<PlayerController>(FindObjectsOfType<PlayerController>());
        i = -1;
    }

    // Update is called once per frame
    void Update()
    {
        if (!timerIsPaused)
        {
            if (turnTimer <= 0)
            {
                NextTurn();
            }
            else
            {
                turnTimer -= Time.deltaTime;
            }
        }
    }

    public void RemovePlayer(PlayerController playerToRemove)
    {
        player.Remove(playerToRemove);
        CheckPlayers();
    }

    public void NextTurn()
    {
        ResumeTimer();
        if (i != -1 && i < player.Count)
            player[i].isActive = false;
        i++;
        if (i >= player.Count)
        {
            i = 0;
        }
        virtualCamera.Follow = player[i].transform;
        player[i].isActive = true;
        player[i].movesLeft = player[i].movesPerTurn;
        turnTimer = turnDuration;
        OnTurnChange.Invoke(player[i].health, player[i].maxHealth, player[i].weapon, player[i].movesLeft);
    }

    public void PauseTimer()
    {
        timerIsPaused = true;
        try
        {
            player[i].isActive = false;
        }
        catch { }
    }

    public void ResumeTimer()
    {
        timerIsPaused = false;
    }

    void CheckPlayers()
    {
        bool onlyTeam = true;
        foreach (PlayerController p in player)
        {
            if (p.team != player[0].team)
            {
                onlyTeam = false;
            }
        }
        if (onlyTeam)
        {
            EndGame();
        }
    }

    void EndGame()
    {
        PauseTimer();
        foreach (PlayerController p in player)
        {
            p.enabled = false;
        }
        FindObjectOfType<HUDController>()?.AnnounceWinner(player[0].team);
        OnGameEnd.Invoke();
    }
}
