using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Scoreboard : MonoBehaviour {

	public static Scoreboard S;
	public GameObject prefabFloatingScore;

	public bool ________________;
	
	[SerializeField]
	private int _score = 0;
	private string _scoreString;

	public int score {

		get{ return _score; }
		set {

			_score = value;
			_scoreString = Utils.AddCommasToNumber(_score);

		}

	}

	public string ScoreString{

		get { return _scoreString; }
		set{

			_scoreString = value;
			GetComponent<GUIText> ().text = _scoreString;

		}

	}

	void Awake(){

		S = this;

	}

	public void FSCallback(FloatScore fs){

		score = fs.score;

	}

	public FloatScore CreateFloatingScore(int amt, List<Vector3> pts){

		GameObject go = Instantiate(prefabFloatingScore) as GameObject;
		FloatScore fs = go.GetComponent<FloatScore>();
		fs.score = amt;
		fs.reportFinishTo = this.gameObject;
		fs.Init(pts);
		return(fs);

	}

}
