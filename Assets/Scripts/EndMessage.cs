using TMPro;
using UnityEngine;

public class EndMessage : MonoBehaviour
{

    [SerializeField]
    private TMP_Text _playerMessage = null;

    public void OnGameEnded(int winner)
    {
        // _playerMessage.text = winner == -1 ? "It's Draw" : winner == 1 ? "You Lost" : "You Won!!";

       // Debug.Log("Winner value received: " + winner);

        switch (winner)
        {
            case -1:
                _playerMessage.text = "It's a Draw";
                break;
            case 0:
                _playerMessage.text = "You Won!!";
                break;
            case 1:
                _playerMessage.text = "You Lost";
                break;
            default:
                _playerMessage.text = "Error: Invalid game outcome";
                Debug.LogError("Invalid winner value: " + winner);
                break;
        }
    }
}
