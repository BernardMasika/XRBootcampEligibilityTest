using System.Collections;
using UnityEngine;

public class GamePiece : MonoBehaviour{

	[SerializeField]
	private float _animTime = 2f;
	[SerializeField]
	AnimationCurve _growthCurve;

	private void OnEnable()
	{
		StartCoroutine(SpawnRoutine());
	}

	IEnumerator SpawnRoutine(){
		yield return null;
		for(float t = 0 ; t <= _animTime; t += Time.deltaTime){
			yield return new WaitForFixedUpdate();
			transform.localScale = Vector3.one * _growthCurve.Evaluate( t/_animTime);
		}
	}


}
