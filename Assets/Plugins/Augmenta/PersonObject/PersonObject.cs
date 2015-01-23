using UnityEngine;
using System.Collections;

public class PersonObject : MonoBehaviour {

	// Store the pid for convenience
	private int pid;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		// Hide/show the cube
		renderer.enabled = auInterface.debug;
	}

	public int getId(){return pid;}
	public void setId(int id){pid = id;}

	public int getOid(){return auListener.getPeopleArray()[pid].oid;}

	public int getAge(){return auListener.getPeopleArray()[pid].age;}
}
