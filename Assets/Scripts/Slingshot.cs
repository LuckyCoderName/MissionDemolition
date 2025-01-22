using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
	[Header("set in inspector")]
	public GameObject prefabProjectile;
	public float velMult=8f;
	[Header("set dynamically")]
	public GameObject launchPoint;
	public Vector3 launchPos;
	public GameObject projectile;
	public bool aimingMode;
	private Rigidbody _projectileRigidbody;
	private float _maxMagnitude;
	void Awake()
	{
		Transform launchPointTrans = transform.Find("LaunchPoint");
		launchPoint = launchPointTrans.gameObject;
		launchPoint.SetActive(false);
		launchPos = launchPointTrans.position;
		_maxMagnitude = gameObject.GetComponent<SphereCollider>().radius;
	}
	void OnMouseEnter()
	{
		print("slingshot:MouseEnter");
		launchPoint.SetActive(true);
	}
	void OnMouseExit()
	{
		print("slingshot:onMouseExit");
		launchPoint.SetActive(false);
	}
	void OnMouseDown()
	{
		aimingMode=true;
		projectile=Instantiate(prefabProjectile) as GameObject;
		projectile.transform.position = launchPos;
		_projectileRigidbody=projectile.GetComponent<Rigidbody>();
		_projectileRigidbody.isKinematic=true;
	}
	void Update()
	{
		if (!aimingMode) return;
		Vector3 mousePos2D = Input.mousePosition;
		mousePos2D.z = -Camera.main.transform.position.z;
		Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);
		
		Vector3 mouseDelta = mousePos3D-launchPos;
		if (mouseDelta.magnitude>_maxMagnitude)
		{
			mouseDelta.Normalize();
			mouseDelta*=_maxMagnitude;
		}
		Vector3 projPos = launchPos + mouseDelta;
		projectile.transform.position = projPos;
		
		if (Input.GetMouseButtonUp(0))
		{
			aimingMode=false;
			_projectileRigidbody.isKinematic=false;
			_projectileRigidbody.velocity = -mouseDelta * velMult;
			FollowCam.POI = projectile;
			projectile = null;
		}
	}
		
}

