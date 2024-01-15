using TMPro;
using UnityEngine;

public class GridSystem : MonoBehaviour
{
    int[,] grid = new int[3, 3];
    public TMP_Text displayUI;
    // Start is called before the first frame update
    void Start()
    {
        grid[0, 0] = 0;
        grid[1, 0] = 45;
        grid[2, 0] = 30;

        grid[0, 1] = 99;
        grid[1, 1] = 5;
        grid[2, 1] = 20;

        grid[0, 2] = 67;
        grid[1, 2] = 70;
        grid[2, 2] = 22;

    }

    // Update is called once per frame
    void Update()
    {
        displayUI.text = $"[{grid[0, 0]}]" + "," + $"[{grid[0, 1]}]" + "," + $"[ {grid[0, 2]} ]\r\n" +
            $"[{grid[1, 0]}]" + "," + $"[{grid[1, 1]}]" + "," + $"[ {grid[1, 2]} ]\r\n";
    }
}
