using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum GameMode
	{
		idle,
		playing,
		levelEnd
	}
public class MissionDemolition : MonoBehaviour
{
	static private MissionDemolition S; // скрытый объект одиночка
	
	[Header("Set In Inspector")]
	public Text uitLevel;
	public Text uitShots;
	public Text uitButton;
	public Vector3 castlePos;
	public GameObject[] castles;
	
	[Header("Set Dynamicaly")]
	public int level; // текущий уровень
	public int levelMax;// кол-во уровеней
	public GameObject castle; //текущий замок
	public int shotsTaken;
	public GameMode mode = GameMode.idle; 
	public string showing = "Show Slingshot";
	
	void Start()
	{
		S = this;
		level = 0;
		levelMax = castles.Length;
		StartLevel();
	}
	void StartLevel()
	{
		if (castle!=null) // уничтожение прошлого замка если он есть
		{
			Destroy(castle);
		}
		
		// уничтожение прошлых снарядов если они есть
		GameObject[] gos = GameObject.FindGameObjectsWithTag("Projectile");
		foreach(GameObject g in gos)
		{
			Destroy(g);
		}
		
		// создать новый замок
		castle = Instantiate<GameObject>(castles[level]);
		castle.transform.position = castlePos;
		shotsTaken = 0;
		
		SwitchView("Show Both");
		ProjectileLine.S.Clear();
		
		Goal.goalMet = false;
		
		UpdateGUI();
		
		mode = GameMode.playing;
	}
	
	void UpdateGUI()
	{
		//Показать данные в элементах ПИ
		uitLevel.text = "Level: " +(level+1) + " of " + levelMax;
		uitShots.text = "Shots Taken: " + shotsTaken;
	}
	
	void Update()
	{
		UpdateGUI();
		if ((mode == GameMode.playing)&&Goal.goalMet)
		{
			mode = GameMode.levelEnd;
			SwitchView("Show Both");
			Invoke("NextLevel",2f);
		}
	}
	void NextLevel()
	{
		level++;
		if (level == levelMax)
		{
			level = 0;
		}
		StartLevel();
	}
	public void SwitchView(string eView = "")
	{
		if (eView == "")
		{
			eView = uitButton.text;
		}
		showing = eView;
		switch (showing)
		{
			case "Show Slingshot":
				FollowCam.POI = null;
				uitButton.text = "Show Castle";
				break;
			case "Show Castle":
				FollowCam.POI = S.castle;
				uitButton.text = "Show Both";
				break;
			case "Show Both":
				FollowCam.POI = GameObject.Find("ViewBoth");
				uitButton.text = "Show Slingshot";
				break;
		}
	}
	public static void ShotFired()
	{
		S.shotsTaken++;
	}
}
