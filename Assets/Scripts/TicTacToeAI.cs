using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public enum TicTacToeState { none, cross, circle }

[System.Serializable]
public class WinnerEvent : UnityEvent<int>
{
}

public class TicTacToeAI : MonoBehaviour
{

    int _aiLevel;
    GameManager _gameManager;
    public TMP_Dropdown chooseLevel;

 


    TicTacToeState[,] boardState;

    private bool gameEnded = false;


    [SerializeField]
    private bool _isPlayerTurn;

    [SerializeField] private TMP_Text turnText = null;
 

    [SerializeField]
    private int _gridSize = 3;

    [SerializeField]
    private TicTacToeState playerState = TicTacToeState.cross;
    [SerializeField]
    private TicTacToeState aiState = TicTacToeState.circle;

    [SerializeField]
    private GameObject _xPrefab;

    [SerializeField]
    private GameObject _oPrefab;

    public UnityEvent onGameStarted;

    //Call This event with the player number to denote the winner
    public WinnerEvent onPlayerWin;

    ClickTrigger[,] _triggers;
    GameTimer _timer;
    bool gamestarted = false;

    private void Awake()
    {
        if (onPlayerWin == null)
        {
            onPlayerWin = new WinnerEvent();
        }

        _gameManager = FindObjectOfType<GameManager>();

        _timer = GetComponent<GameTimer>();
        _timer.timesUp.AddListener(onTimeElapse);
    }

    private void onTimeElapse()
    {
        if (_isPlayerTurn)
        {
            EndGame(aiState);
            _timer.StopTimer();
        }
    }

    private void Update()
    {
        if (_isPlayerTurn && gamestarted)
        {
            _timer.StartTimer();
        }else if (!_isPlayerTurn || gameEnded)
        {
            _timer.StopTimer();
        }
    }

    public void StartAI()
    {
        _aiLevel = chooseLevel.value;
        StartGame();

    }

    public void RegisterTransform(int myCoordX, int myCoordY, ClickTrigger clickTrigger)
    {
        _triggers[myCoordX, myCoordY] = clickTrigger;
    }

    private void StartGame()
    {
        gamestarted = true;
        _triggers = new ClickTrigger[3, 3];

        boardState = new TicTacToeState[_gridSize, _gridSize]; // Initialize the board state

        for (int x = 0; x < _gridSize; x++)
        {
            for (int y = 0; y < _gridSize; y++)
            {
                boardState[x, y] = TicTacToeState.none; // Set all states to 'none'
            }
        }
        
        UpdateTurnDisplay();

        onGameStarted.Invoke();
    }

    public void PlayerSelects(int coordX, int coordY)
    {

        if (!gameEnded && _isPlayerTurn && boardState[coordX, coordY] == TicTacToeState.none)
        {
            SetVisual(coordX, coordY, playerState);
            boardState[coordX, coordY] = playerState;
            _isPlayerTurn = false;
            CheckGameState();
            UpdateTurnDisplay();


         //  _gameManager.OnUpdateGridUI("X", coordX, coordY);

            // Delay AI move instead of calling AiMove() directly here
            StartCoroutine(DelayAiMove());
        }

       
    }



    IEnumerator DelayAiMove()
    {
        yield return new WaitForSeconds(2f); // 1 second delay, adjust as needed
        if (!gameEnded && !_isPlayerTurn)
        {
            AiMove();
        }
    }


    public void AiSelects(int coordX, int coordY)
    {

        if (!_isPlayerTurn && boardState[coordX, coordY] == TicTacToeState.none)
        {
            SetVisual(coordX, coordY, aiState);
            boardState[coordX, coordY] = aiState;
            _isPlayerTurn = true;
            CheckGameState();
            UpdateTurnDisplay();


           // _gameManager.OnUpdateGridUI("O", coordX, coordY);
        }
        
    }

    private void SetVisual(int coordX, int coordY, TicTacToeState targetState)
    {
        if (_triggers[coordX, coordY] != null)
        {
            Instantiate(
           targetState == TicTacToeState.circle ? _oPrefab : _xPrefab,
           _triggers[coordX, coordY].transform.position,
           Quaternion.identity);

        }
        else
        {
            Debug.Log("_triggers is null..");
        }

    }

    private void AiMove()
    {
        if (gameEnded) return;

        if (_aiLevel == 0)
        {
            // Debug.Log("easy level!!");
            EasyLevelAiMove();
        }
        else if (_aiLevel == 1)
        {
            // Debug.Log("Hard level!!");
            HardLevelAiMove();
        }

    }

    private void EasyLevelAiMove()
    {
        float rate = 0.2f;
        float randomChance = UnityEngine.Random.Range(0, 0.9f);


        if (randomChance < rate)
        {
            TryToMakeAWinningMove();

            Debug.Log("creating a winning move.." + randomChance);
        }

        // First, check if the AI can block the player's winning move
        if (TryToBlockThePlayerFromWinning()) return;



        // Then, choose a random spot
        SelectRandomSpot();
    }

    private void HardLevelAiMove()
    {
        List<Func<bool>> strategies = new List<Func<bool>>
    {
        TryToMakeAWinningMove,
        TryToBlockThePlayerFromWinning,
        TakeCenterOrCorner,

        // TrySettingUpFutureWin
    };

        // Shuffle the strategies to randomize the order
        strategies = Shuffle(strategies);

        // Execute the strategies in randomized order
        foreach (var strategy in strategies)
        {
            if (strategy())
                return;
        }

        // If none of the strategies were applicable, select a random spot
        SelectRandomSpot();
    }

    // Method to shuffle the list of functions
    private List<Func<bool>> Shuffle(List<Func<bool>> list)
    {
        System.Random rng = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            Func<bool> value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
        return list;
    }

    private bool TryToMakeAWinningMove()
    {
        // Debug.Log("try making a winning move");

        foreach (var spot in GetAvailableSpots())
        {
            if (IsWinningMove(spot.x, spot.y, aiState))
            {
                // Debug.Log($"AI finds winning move at [{spot.x}, {spot.y}]");
                AiSelects(spot.x, spot.y);
                return true;
            }
        }
        return false;
    }

    /* private bool TrySettingUpFutureWin()
     {
         Debug.Log("try settup the future win");
         foreach (var spot in GetAvailableSpots())
         {
             boardState[spot.x, spot.y] = aiState; // Temporarily make a move
             if (IsFutureWinPossible(spot.x, spot.y))
             {
                 AiSelects(spot.x, spot.y);
                 boardState[spot.x, spot.y] = TicTacToeState.none; // Revert the move
                 return true;
             }
             boardState[spot.x, spot.y] = TicTacToeState.none; // Revert the move
         }
         return false;
     }

     private bool IsFutureWinPossible(int x, int y)
     {
         // Check if this move leads to a potential winning move in the next turn
         // Simplified example: Check if there's another spot that could be a winning move
         foreach (var futureSpot in GetAvailableSpots())
         {
             if (IsWinningMove(futureSpot.x, futureSpot.y, aiState))
             {
                 return true;
             }
         }
         return false;
     }*/

    private bool TryToBlockThePlayerFromWinning()
    {
        //Debug.Log("try block the player from winning");

        foreach (var spot in GetAvailableSpots())
        {
            if (IsWinningMove(spot.x, spot.y, playerState))
            {
                AiSelects(spot.x, spot.y);
                return true;
            }
        }
        return false;
    }


    private bool TakeCenterOrCorner()
    {
        // Debug.Log("take center coner");

        if (boardState[1, 1] == TicTacToeState.none)
        {
            AiSelects(1, 1);
            return true;
        }


        List<Vector2Int> corners = new List<Vector2Int>
    {
        new Vector2Int(0, 0),
        new Vector2Int(0, 2),
        new Vector2Int(2, 0),
        new Vector2Int(2, 2)
    };

        foreach (var corner in corners)
        {
            if (boardState[corner.x, corner.y] == TicTacToeState.none)
            {
                AiSelects(corner.x, corner.y);
                return true;
            }
        }
        return false;
    }

    private void SelectRandomSpot()
    {
        Vector2Int aiChoice = ChooseRandomSpot();
        AiSelects(aiChoice.x, aiChoice.y);
    }

    private List<Vector2Int> GetAvailableSpots()
    {
        List<Vector2Int> availableSpots = new List<Vector2Int>();
        for (int x = 0; x < _gridSize; x++)
        {
            for (int y = 0; y < _gridSize; y++)
            {
                if (boardState[x, y] == TicTacToeState.none)
                {
                    availableSpots.Add(new Vector2Int(x, y));
                }
            }
        }
        return availableSpots;
    }

    private Vector2Int ChooseRandomSpot()
    {
        var availableSpots = GetAvailableSpots();
        int index = UnityEngine.Random.Range(0, availableSpots.Count);
        return availableSpots[index];
    }


    private void CheckGameState()
    {


        // Check for a win for both player and AI


        if (CheckForWin(playerState))
        {

            EndGame(playerState);
            return;
        }
        else if (CheckForWin(aiState))
        {

            EndGame(aiState);
            return;
        }

        if (IsBoardFull())
        {
            EndGame(TicTacToeState.none);
            return;
        }

      //  Debug.Log("No winner or tie yet. Game continues.");
    }

    private bool CheckForWin(TicTacToeState state)
    {
        // Check rows, columns, and diagonals for a win
        for (int i = 0; i < _gridSize; i++)
        {
            if (IsLineMatch(0, i, 1, 0, state) || IsLineMatch(i, 0, 0, 1, state))
            {
                return true;
            }
        }
        if (IsLineMatch(0, 0, 1, 1, state) || IsLineMatch(0, _gridSize - 1, 1, -1, state))
        {
            return true;
        }
        return false;
    }

    private bool IsLineMatch(int startX, int startY, int stepX, int stepY, TicTacToeState state)
    {
        for (int i = 0; i < _gridSize; i++)
        {
            if (boardState[startX + stepX * i, startY + stepY * i] != state)
            {

                return false;
            }
        }

        return true;
    }


    private bool IsBoardFull()
    {
        for (int x = 0; x < _gridSize; x++)
        {
            for (int y = 0; y < _gridSize; y++)
            {
                if (boardState[x, y] == TicTacToeState.none)
                    return false;
            }
        }
        return true;
    }

    private void EndGame(TicTacToeState winnerState)
    {
        gameEnded = true;
        gamestarted = false;
        int winner;
        if (winnerState == TicTacToeState.none)
        {
            winner = -1; // Tie
        }
        else if (winnerState == playerState)
        {
            winner = 0; // Player wins
        }
        else
        {
            winner = 1; // AI wins
        }
      //  Debug.Log($"Game Ended. Winner: {winner}");
        onPlayerWin.Invoke(winner);
        _gameManager.OnUpdateRecords(winner);
    }

    public void UpdateTurnDisplay()
    {
        if (gameEnded)
        {
            turnText.text = "";
        }
        else 
        {

            turnText.text = _isPlayerTurn ? "Your Turn" : "AI Turn";
        }

    }

    private bool IsWinningMove(int x, int y, TicTacToeState player)
    {
        // Temporarily setting the move
        boardState[x, y] = player;


        bool won = IsLineMatch(0, y, 1, 0, player) || IsLineMatch(x, 0, 0, 1, player) ||
                   IsLineMatch(0, 0, 1, 1, player) || IsLineMatch(0, _gridSize - 1, 1, -1, player);

        // Revert the move
        boardState[x, y] = TicTacToeState.none;
        return won;
    }



}
