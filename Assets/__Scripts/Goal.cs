using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
	static public bool goalMet = false;
	
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Projectile")
		{
			Goal.goalMet = true;
			Material mat = GetComponent<Renderer>().material;
			Color c = mat.color;
			c.b = 0;
			mat.color = c;
		}
	}
}
