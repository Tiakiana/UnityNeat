using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CarControl : MonoBehaviour {
		public string frem;
		public string tilbage;
		public string venstre;
		public string hojre;
		public string skyd;
		public float hastighed;
		public float acceleration;
		public float drejeHastighed;
		public float draen;
		public float maxHastighed;
		public float genladningstid = 1;
		public int Health;
		public Slider Healthbar;
		float reload  = 0;
		public bool Player1,Dead;
		AudioSource ass;
		public GameObject skud;

		public Transform barrel;


		Rigidbody2D r2d;

    IEnumerator AIThing() {
        while (true)
        {
            
            
            yield return null;
        }


    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Dead = true;
        hastighed = 0;
        acceleration = 0;
    }

    // Use this for initialization
    void Start () {
			r2d = GetComponent<Rigidbody2D>();
			r2d.gravityScale = 0;
			StartCoroutine("Controls");
			
		}

		//der mangler  point og menu og genstart.
	public void TurnRight(){
        if (!Dead)
        {

        transform.RotateAround(transform.position, Vector3.forward, -drejeHastighed * Time.deltaTime);
        }

	}
	public void TurnLeft(){
        if (!Dead)
        {

        transform.RotateAround(transform.position, Vector3.forward, drejeHastighed * Time.deltaTime);
        }
	}
		

		public IEnumerator Controls() {
			
			while (true)
			{
            
				if (Input.GetKey(venstre))
				{
					//    Debug.Log("fremad!");
					transform.RotateAround(transform.position, Vector3.forward, drejeHastighed * Time.deltaTime);

				}

				if (Input.GetKey(hojre))
				{
					transform.RotateAround(transform.position, Vector3.forward, -drejeHastighed * Time.deltaTime);
				}

				if (Input.GetKey(frem)&& hastighed< maxHastighed)
				{
					hastighed += acceleration/100;
				}
			hastighed += acceleration/100;



				if (Input.GetKey(tilbage) && hastighed > -maxHastighed)
				{
					hastighed -= acceleration / 100;
				}


				transform.position += transform.right * hastighed * Time.deltaTime;

				if (hastighed>=0.01f)
				{
					hastighed -= draen / 100;
				}
				if (hastighed<=0.01f)
				{
					hastighed += draen / 100;
				}

				


				yield return new WaitForSeconds(0.0001f);
			}

		}


		
		// Update is called once per frame
		void Update () {
			
			
		}
	}

