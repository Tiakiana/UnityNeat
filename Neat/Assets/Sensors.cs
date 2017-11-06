using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensors : MonoBehaviour {
	public float SensorLength = 1;
    CarControl ctrl;
	// Use this for initialization
	void Start () {
        //StartCoroutine("AIThing");
        ctrl = GetComponent<CarControl>();
        Physics2D.IgnoreLayerCollision(8,8,true);
	}

	public float SensorLeft(){
		float res = 0;
		RaycastHit2D ray = Physics2D.Raycast (transform.position, transform.up, SensorLength);
		if (ray.collider != null && ray.collider.gameObject.tag == "Obstacle") {
			res = 1 - ray.distance;
			Debug.DrawRay(transform.position,transform.up*ray.distance,Color.red);

		} else {
			
			Debug.DrawRay(transform.position,transform.up*SensorLength);
		}

		return res;
	}

	public float SensorFront(){
		float res = 0;
		RaycastHit2D ray = Physics2D.Raycast (transform.position, transform.right, SensorLength);
		if (ray.collider != null && ray.collider.gameObject.tag == "Obstacle") {
			
			res = 1 - ray.distance;
			Debug.DrawRay (transform.position, transform.right * ray.distance, Color.red);
		} else {
			
			Debug.DrawRay(transform.position,transform.right*SensorLength);
		}

		return res;
	}

	public float SensorRight(){
			float res = 0;
		RaycastHit2D ray = Physics2D.Raycast (transform.position, -transform.up, SensorLength);
			if (ray.collider != null && ray.collider.gameObject.tag == "Obstacle") {
			res = 1 - ray.distance;
			Debug.DrawRay(transform.position,-transform.up*ray.distance,Color.red);
				
			} else {
			
			Debug.DrawRay(transform.position,-transform.up*SensorLength);
		}

		return res;
	}

	public float SensorCrookedLeft(){
		float res = 0;
		RaycastHit2D ray = Physics2D.Raycast (transform.position,transform.TransformDirection(new Vector2(1,1)), SensorLength);
		if (ray.collider != null && ray.collider.gameObject.tag == "Obstacle") {
			res = 1 - ray.distance;
			Debug.DrawRay(transform.position,transform.TransformDirection(new Vector2(1,1)*ray.distance),Color.red);

		} else {
			
			Debug.DrawRay(transform.position,transform.TransformDirection(new Vector2(1,1)*SensorLength));
		}
		return res;
	}

	public float SensorCrookedRight(){
		float res = 0;
		RaycastHit2D ray = Physics2D.Raycast (transform.position,transform.TransformDirection(new Vector2(1,-1)), SensorLength);
		if (ray.collider != null && ray.collider.gameObject.tag == "Obstacle") {
			res = 1 - ray.distance;
			Debug.DrawRay(transform.position,transform.TransformDirection(new Vector2(1,-1)*ray.distance),Color.red);

		} else {
			
			Debug.DrawRay(transform.position,transform.TransformDirection(new Vector2(1,-1)*SensorLength));
		}

		return res;

	}

	public float[] Sensorium = new float[5];
    public float courage = 0.6f;
    IEnumerator AIThing()
    {
        while (true)
        {
            float leftbias, rightbias;
            leftbias = Sensorium[0] + Sensorium[1] + Sensorium[2];
            rightbias = Sensorium[2] + Sensorium[4] + Sensorium[3];
            if (leftbias>courage && leftbias>rightbias)
            {
                ctrl.TurnRight();
            }
            if (rightbias>courage && rightbias>leftbias)
            {
                ctrl.TurnLeft();
            }

            yield return null;
        }


    }



    // Update is called once per frame
    void Update () {
		Sensorium [0] = SensorLeft();
		Sensorium [1] = SensorCrookedLeft();
		Sensorium [2] = SensorFront();
		Sensorium [3] = SensorCrookedRight ();
		Sensorium [4] = SensorRight();
	}
}
