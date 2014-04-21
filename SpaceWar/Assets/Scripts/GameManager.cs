using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class GameManager : MonoBehaviour {

	List<GameObject> allyPatrolShip = new List<GameObject>();
	public static List<GameObject> enemyForces = new List<GameObject>();
	public static List<GameObject> allyForces = new List<GameObject>();
	public GameObject enemyForceGO, allyForceGO;
	public int enemyForceCount = 10;
	public int allyForceCount = 10;

	public static float assaultDelay = 5f; //for the enemy forces assault after they warp in
	public static bool warpedDiversion = true; //change to false, turn true after short delay, this starts off proceedings!
	public static bool warpedForces;
	public static bool warpedAllies;
	public static bool jammerSearch; 

	GameObject motherShip;
	GameObject teaser;
	GameObject jammer;
	Vector3 warpPos;
	int whichAllyPatrol; //the ally patrol ship that goes to check out the diversion
	int roll;
	
	void Start () 
	{
		teaser = GameObject.Find("EnemyTeaser");
		motherShip = GameObject.Find ("Mothership");
		jammer = GameObject.Find ("Jammer");

		int cnt = 1;
		for(int i = 0; i < 2; i++)
		{
			allyPatrolShip.Add(GameObject.Find ("AllyPatrolShip"+cnt));
			allyPatrolShip[i].renderer.material.color = Color.blue;
			allyPatrolShip[i].GetComponent<StateMachine>().SwitchState(new PatrolState(allyPatrolShip[i], teaser));
			cnt++;
		}
		teaser.renderer.material.color = Color.red;

		WarpInDiversion(); //call on timer, initial diversion
		WarpForces(); //call on timer
		WarpAllies();
	}

	void WarpInDiversion()
	{
		teaser.GetComponent<StateMachine>().SwitchState(new WarpState(teaser, allyPatrolShip[roll]));

		roll = (int)Random.Range (0, 2);
		if(roll == 0)
		{
			allyPatrolShip[0].GetComponent<StateMachine>().SwitchState(new AlertState(allyPatrolShip[0], teaser)); //enemy = ally in this case
			whichAllyPatrol = 1;
		}
		else
		{
			allyPatrolShip[1].GetComponent<StateMachine>().SwitchState(new AlertState(allyPatrolShip[1], teaser)); //enemy = ally in this case
			whichAllyPatrol = 0;
		}
		warpedDiversion = true;
		StartCoroutine("RadioDelay");
	}

	void WarpForces() //forces warp into existance!
	{
		float xPos = -20, yPos = 0, zPos = -40; //change the spawn in position of the enemy forces here!
		for(int i = 0; i < enemyForceCount; i++)
		{
			Vector3 randPos = new Vector3(xPos, yPos, zPos);
			GameObject enemyForce = Instantiate(enemyForceGO, randPos, Quaternion.identity) as GameObject;
			xPos += Random.Range (5, 10);
			yPos += Random.Range (-3, 3);
			zPos += Random.Range (-3, 3);
			
			enemyForce.name = "EnemyForce";
			enemyForce.renderer.material.color = Color.red;
			enemyForce.GetComponent<StateMachine>().SwitchState(new WarpState(enemyForce, motherShip));
			enemyForces.Add(enemyForce);
		}
		warpedForces = true;
	}

	void WarpAllies() //forces warp into existance!
	{
		float xPos = -20, yPos = 0, zPos = -80; //change the spawn in position of the enemy forces here!
		for(int i = 0; i < allyForceCount; i++)
		{
			Vector3 randPos = new Vector3(xPos, yPos, zPos);
			GameObject allyForce = Instantiate(allyForceGO, randPos, Quaternion.identity) as GameObject;
			xPos += Random.Range (5, 10);
			yPos += Random.Range (-3, 3);
			zPos += Random.Range (-3, 3);
			
			allyForce.name = "AllyForce";
			allyForce.renderer.material.color = Color.blue;
			allyForce.GetComponent<StateMachine>().SwitchState(new AlertState(allyForce, allyForce)); //CIRCLE FIRE STATE
			allyForces.Add(allyForce);
		}
		warpedAllies = true;
	}

	IEnumerator RadioDelay()
	{
		yield return new WaitForSeconds(assaultDelay);
		allyPatrolShip[whichAllyPatrol].GetComponent<StateMachine>().SwitchState(new RadioState(allyPatrolShip[whichAllyPatrol], jammer));
		jammerSearch = true;
	}
}
