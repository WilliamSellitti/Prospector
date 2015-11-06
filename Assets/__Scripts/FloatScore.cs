using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum FSState{

	idle,
	pre,
	active,
	post

}

public class FloatScore : MonoBehaviour {

	public FSState state = FSState.idle;
	[SerializeField]
	private int _score = 0;
	public string scoreString;

	public int score{

		get{ return _score; }
		set{
			_score = value;
			scoreString = Utils.AddCommasToNumber (_score);
			GetComponent<GUIText> ().text = scoreString;
		}

	}

	public List<Vector3> bezierPts;
	public List<float> fontSize;
	public float timeStart = -1f;
	public float timeDuration = 1f;
	public string easingCurve = Easing.InOut;

	public GameObject reportFinishTo = null;

	public void Init(List<Vector3> ePts, float eTimeS = 0, float eTimeD = 1){

		bezierPts = new List<Vector3> (ePts);
		if (ePts.Count == 1) {

			transform.position = ePts[0];
			return;

		}

		if (eTimeS == 0)
			eTimeS = Time.time;
		timeStart = eTimeS;
		timeDuration = eTimeD;

		state = FSState.pre;

	}

	public void FSCallback(FloatScore fs){

		score += fs.score;

	}

	// Update is called once per frame
	void Update () {
	
		if (state == FSState.idle)
			return;

		float u = (Time.time - timeStart) / timeDuration;
		float uC = Easing.Ease (u, easingCurve);

		if (u < 0) {

			state = FSState.pre;
			transform.position = bezierPts [0];

		} else {

			if (u > 1){

				uC = 1;
				state = FSState.post;
				if( reportFinishTo != null){

					reportFinishTo.SendMessage("FSCallback", this);
					Destroy (gameObject);

				} else {

					state = FSState.idle;

				}

			} else {

				state = FSState.active;

			}

			Vector3 pos = Utils.Bezier(uC, bezierPts);
			transform.position = pos;
			if(fontSize != null && fontSize.Count > 0){

				int size = Mathf.RoundToInt(Utils.Bezier(uC, fontSize));
				GetComponent<GUIText>().fontSize = size;

			}

		}

	}
}
