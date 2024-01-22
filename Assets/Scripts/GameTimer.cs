using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class GameTimer : MonoBehaviour
{
    [SerializeField] TMP_Text timerDisplay;
    [SerializeField] float timeLapse = 10f;
    [SerializeField] AudioSource _timerSound;
    

    public UnityEvent timesUp;

    float timer;
    bool isOnTimer;
    // Start is called before the first frame update
    void Start()
    {
        timer = timeLapse;

        _timerSound = GetComponent<AudioSource>();
     
    }

    public void StartTimer()
    {
        _timerSound.Play();
        isOnTimer = true;
    }

    public void StopTimer()
    {
        _timerSound?.Stop();
        isOnTimer = false;
        timer = timeLapse;
    }

    public void PauseTimer()
    {
        _timerSound.Stop();
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
