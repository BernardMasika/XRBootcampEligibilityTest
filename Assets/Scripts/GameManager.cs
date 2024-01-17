using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    string[,] grid = new string[3, 3];
    [SerializeField] TMP_Text gridValuesUI;
    [SerializeField] TMP_Text recordsUI;
    [SerializeField] Button quitButton;

    int wins = 0;
    int lose = 0;
    int draw = 0;

    private static GameManager instance = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }
    }

    private void Update()
    {
        quitButton.onClick.AddListener(OnQuitGame);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
      //  gridValuesUI = GameObject.Find("Grid values").GetComponent<TMP_Text>();
        recordsUI = GameObject.Find("records").GetComponent<TMP_Text>();
        quitButton = GameObject.Find("Quit Button").GetComponent<Button>();

        OnDisplayRecords();
    }

    public void OnUpdateGridUI(string pieceType, int coordX, int coordY)
    {
        grid[coordX, coordY] = pieceType;

        gridValuesUI.text = $"[ {grid[0, 0]} ]" + "," + $"[ {grid[0, 1]} ]" + "," + $"[ {grid[0, 2]} ]\r\n" +
            $"[ {grid[1, 0]} ]" + "," + $"[ {grid[1, 1]} ]" + "," + $"[ {grid[1, 2]} ]\r\n" +
            $"[ {grid[2, 0]} ]" + "," + $"[ {grid[2, 1]} ]" + "," + $"[ {grid[2, 2]} ]\r\n" +
            $"";

    }

    public void OnUpdateRecords(int winnerValue)
    {
        switch (winnerValue)
        {
            case -1:
                draw++;
                OnDisplayRecords();
                break;
            case 0:
                wins++;
                OnDisplayRecords();
                break;
            case 1:
                lose++;
                OnDisplayRecords();
                break;
            default:
               
                Debug.LogError("Invalid winner value: " + winnerValue);
                break;
        }
    }

    void OnDisplayRecords()
    {
        recordsUI.text = $"Wins: {wins}\r\n" +
            $"Loses: {lose}\r\n" +
            $"Draws: {draw}";

        Debug.Log("wins: " +" " + wins + "lose: " + " " + lose + "draw: " + " " + draw);
    }

    public void OnQuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
