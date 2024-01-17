using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class GameTimer : MonoBehaviour
{
    [SerializeField] TMP_Text timerDisplay;
    [SerializeField] float timeLapse = 10f;
    [SerializeField] bool startOnAwake = true;

    public UnityEvent timesUp;

    float timer;
    bool isOnTimer;
    // Start is called before the first frame update
    void Start()
    {
        timer = timeLapse;


       // if (startOnAwake) StartTimer();
    }

    public void StartTimer()
    {
        isOnTimer = true;
    }

    public void StopTimer()
    {
        isOnTimer = false;
        timer = timeLapse;
    }

    public void PauseTimer()
    {
        isOnTimer = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isOnTimer)
        {
            timer -= Time.deltaTime;
            timerDisplay.text = timer.ToString("0.00");

            if (timer <= 0)
            {
                timesUp.Invoke();
                timer = timeLapse;
            }
        } else
        {
            timerDisplay.text = null;
        }


    }
}
