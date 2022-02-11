using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonBehavior<GameManager>
{
    public float tempTimer = 0;

    public GameState gameState;

    public enum GameState
    {
        Menu,
        InGame,
        EndGame,
        Result,
    }
}
