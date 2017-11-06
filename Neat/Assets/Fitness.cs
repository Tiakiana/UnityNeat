using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fitness : MonoBehaviour {
	public Transform Begin, Goal, Car;
	// Use this for initialization
	public float fitness;
    public static Fitness ftns;

    private void Awake()
    {
        ftns = this;
    }

    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		fitness = Vector2.Distance (Begin.position, Goal.position) - Vector2.Distance (Car.position,Goal.position);
	}

    public float GetFitness(GameObject car) {
        return Vector2.Distance(Begin.position, Goal.position) - Vector2.Distance(car.transform.position, Goal.position);
    }
}
