using UnityEngine;
using System.Collections;

using System.Collections.Generic;       //Allows us to use Lists. 

public class GameManager : MonoBehaviour
{

	public static GameManager instance = null;                  //Static instance of GameManager which allows it to be accessed by any other script.

	public bool enableGeneration = true;

	public enum State { Boot, Menu, Load, Game, Upgrade, Error }
	public State state = State.Boot;
	public State startState = State.Game;

	public GameObject generationPlane;

	public string seed = "";

	private int level = -1;                                     //Current level number, expressed in game as "Day 1".

	//Awake is always called before any Start functions
	void Awake ()
	{
		//Check if instance already exists
		if (instance == null)

			//if not, set instance to this
			instance = this;

		//If instance already exists and it's not this:
		else if (instance != this)

			//Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
			Destroy(gameObject);

		//Sets this to not be destroyed when reloading scene
		DontDestroyOnLoad(gameObject);

		//Call the InitGame function to initialize the first level 
		InitGame();
	}

	// Start the game
	void InitGame ()
	{
		Debug.Log("Initializing Game...");

		if (enableGeneration)
		{
			PrepareRandom();
		}

		SwitchState(startState);
	}

	void PrepareRandom()
	{
		if (seed == null || seed == "")
		{
			seed = System.DateTime.Now.Millisecond.ToString();
		}
		Debug.Log("Seed: " + seed);
		Random.InitState(seed.GetHashCode());
	}

	public void SwitchState (State newState)
	{
		Debug.Log("Switching from " + state + " to " + newState + "...");
		switch (newState)
		{
		case State.Boot:
			Debug.Log("Booting...");
			break;
		case State.Menu:
			Debug.Log("Showing Main Menu...");
			break;
		case State.Load:
			Debug.Log("Loading...");
			break;
		case State.Game:
			Debug.Log("Starting Game...");
			level = 0;
			break;
		case State.Upgrade:
			Debug.Log("Showing Upgrade Screen...");
			break;
		default:
			Debug.LogError("Unknown state given: " + newState.ToString());
			break;
		}
	}

	//Update is called every frame.
	void Update ()
	{

	}

}