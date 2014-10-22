using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitManager : MonoBehaviour {

	public List<GenericFactory> factories;

	public List<Character> units;

	public Unit unit;

	
	// Use this for initialization
	void Start () {
		factories = new List<GenericFactory>();
		factories.Add(new GenericFactory(unit.gameObject));
		units = new List<Character>(GameObject.FindObjectsOfType<Unit>());
	}
}
