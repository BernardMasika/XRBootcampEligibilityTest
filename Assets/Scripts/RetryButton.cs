using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Button))]
public class RetryButton : MonoBehaviour
{
	private void Awake()
	{
		GetComponent<Button>().onClick.AddListener(Retry);
	}

	public void Retry(){
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}
