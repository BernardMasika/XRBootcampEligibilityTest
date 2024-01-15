using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    string[,] grid = new string[3, 3];
    [SerializeField] TMP_Text gridValuesUI;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }



    public void OnUpdateUI(string pieceType, int coordX, int coordY)
    {
        grid[coordX, coordY] = pieceType;

        gridValuesUI.text = $"[ {grid[0, 0]} ]" + "," + $"[ {grid[0, 1]} ]" + "," + $"[ {grid[0, 2]} ]\r\n" +
            $"[ {grid[1, 0]} ]" + "," + $"[ {grid[1, 1]} ]" + "," + $"[ {grid[1, 2]} ]\r\n" +
            $"[ {grid[2, 0]} ]" + "," + $"[ {grid[2, 1]} ]" + "," + $"[ {grid[2, 2]} ]\r\n" +
            $"";

    }


}
