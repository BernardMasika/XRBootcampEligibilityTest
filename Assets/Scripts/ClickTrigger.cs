using UnityEngine;

public class ClickTrigger : MonoBehaviour
{
    TicTacToeAI _ai;

    [SerializeField]
    private int _myCoordX = 0;
    [SerializeField]
    private int _myCoordY = 0;

    [SerializeField]
    private bool canClick;

    AudioSource _audioSource;

    private void Awake()
    {

        _ai = FindObjectOfType<TicTacToeAI>();

    }


    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();


        _ai.onGameStarted.AddListener(AddReference);
        _ai.onGameStarted.AddListener(() => SetInputEndabled(true));
        _ai.onPlayerWin.AddListener((win) => SetInputEndabled(false));
    }

    private void SetInputEndabled(bool val)
    {
        canClick = val;
    }

    private void AddReference()
    {
        _ai.RegisterTransform(_myCoordX, _myCoordY, this);
        canClick = true;
    }



#if UNITY_ANDROID
        // Code specific to Android build, you know it right we use VR controller yeah? 

          private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Grabbable>() != null && canClick)
        {
            _ai.PlayerSelects(_myCoordX, _myCoordY);
            Debug.Log("sent my coordXY");
        }
    }
#elif UNITY_STANDALONE
    // Code specific to PC build since it' all about mouse and keyboard right :)

    private void OnMouseDown()
    {
        if (canClick)
        {
            _ai.PlayerSelects(_myCoordX, _myCoordY);


        }
    }

    private void OnTriggerEnter(Collider other)
    {
        _audioSource.Play();
    }

#endif


}
