using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;       //Allows us to use Lists. 

public class GameManager : NetworkManager
{

	public static GameManager instance = null;                  //Static instance of GameManager which allows it to be accessed by any other script.

	public bool enableGeneration = true;

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
	}
}