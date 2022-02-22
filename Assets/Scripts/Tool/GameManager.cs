using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonBehavior<GameManager>
{
    public float tempTimer = 0;

    public bool an = true, fn = true, cn = true, keypad = true;

    public enum GameState
    {
        Menu,
        InGame,
        EndGame,
        Result,
    }
    public GameState gameState;

    public bool isRecentGameWin = false;

    public Test.Boss currentBoss;
}
