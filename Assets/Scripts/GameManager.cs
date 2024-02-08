using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cinemachine;
using System.Linq;
using System.Linq.Expressions;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    public float turnDuration;
    public CinemachineVirtualCamera virtualCamera;
    private List<SpaceBoy> aliveOnes;
    public float turnTimer;
    public bool timerIsPaused;
    public UnityAction<float, float, GameObject, int> OnTurnChange;
    public UnityEvent OnGameEnd = new UnityEvent();
    private int currentTurn = 0;
    public float rocketDamage;
    public float bulletDamage;
    public float medkitHealing;

    void Start()
    {
        //List all players
        aliveOnes = new List<SpaceBoy>(FindObjectsOfType<SpaceBoy>());

        //Subscribe to each player's UnityEvents
        foreach (SpaceBoy contestant in aliveOnes)
        {
            contestant.OnDeath.AddListener(UpdateContestants);
            contestant.OnTurnEnd.AddListener(NextTurn);
        }

        FirstTurn();
    }

    void Update()
    {
    }

    public void UpdateContestants()
    {
        // Remove dead contestants from list
        for (int i = 0; i < aliveOnes.Count; i++)
        {
            if (aliveOnes[i] == null)
            {
                continue;
            }
            else if (aliveOnes[i].health == 0)
            {
                aliveOnes.RemoveAt(i);
                i++;
            }
        }
    }

    private void FirstTurn()
    {
        virtualCamera.Follow = aliveOnes[0].transform;
        aliveOnes[0].active = true;
    }

    public void NextTurn()
    {
        aliveOnes[currentTurn].active = false;
        currentTurn++;
        if (currentTurn >= aliveOnes.Count)
            currentTurn = 0;

        virtualCamera.Follow = aliveOnes[currentTurn].transform;
        aliveOnes[currentTurn].active = true;
    }

    public void ResumeTimer()
    {
        timerIsPaused = false;
    }

    void CheckPlayers()
    {
        bool onlyTeam = true;
        foreach (SpaceBoy p in aliveOnes)
        {
            if (p.team != aliveOnes[0].team)
            {
                onlyTeam = false;
                continue;
            }
        }
        if (onlyTeam)
        {
            EndGame();
        }
    }

    void EndGame()
    {
        foreach (SpaceBoy p in aliveOnes)
        {
            p.enabled = false;
        }
    }
}
