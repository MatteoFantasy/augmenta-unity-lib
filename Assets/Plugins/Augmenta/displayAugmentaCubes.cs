using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Augmenta;

public class displayAugmentaCubes : MonoBehaviour {

	// This class is an example of how to use the messages sent by the library : PersonEntered / PersonUpdated / PersonWillLeave
	// The best practice is to use them whenever you need to instanciate, maintain, and destroy objects representing the people in the scene
	//
	// If you only need to loop through the people's array to get information, the easiest way is to access the auListener's array like this :
	//
	// foreach(KeyValuePair<int, Person> pPair in auListener.getPeopleArray()) {
	//   	Debug.Log("The point with id ["+pPair.Key+"] is at x="+pPair.Value.centroid.x+" and y="+pPair.Value.centroid.y);
	// }

	private Dictionary<int,GameObject> arrayPersonCubes = new Dictionary<int,GameObject>();

	public Material	[] materials;
	public GameObject boundingPlane; // Put the people on this plane
	public GameObject personMarker; // Used to represent people moving about in our example
	
	void Start () {
		// Launched at scene startup
	}

	void Update () {
		// Called once per frame

		// This loop is for demonstration purposes only !
		// This is how you loop through all the Augmenta points from anywhere in the code
		/*
		foreach(KeyValuePair<int, Person> pPair in auListener.getPeopleArray()) {
			Debug.Log("The point with id ["+pPair.Key+"] is at x="+pPair.Value.centroid.x+" and y="+pPair.Value.centroid.y);
		}
		*/
	}
	
	public void PersonEntered(Person person){
		//Debug.Log("Person entered pid : " + person.pid);
		if(!arrayPersonCubes.ContainsKey(person.pid)){
			GameObject personObject = (GameObject)Instantiate(personMarker, Vector3.zero, Quaternion.identity);
			personObject.transform.parent = boundingPlane.transform.parent.transform;
			movePerson(person, personObject);

			personObject.renderer.material = materials[person.pid % materials.Length];
			arrayPersonCubes.Add(person.pid,personObject);
		}
	}

	public void PersonUpdated(Person person) {
		//Debug.Log("Person updated pid : " + person.pid);
		if(arrayPersonCubes.ContainsKey(person.pid)){
			GameObject cubeToMove = arrayPersonCubes[person.pid];
			movePerson(person, cubeToMove);
		}
	}

	public void PersonWillLeave(Person person){
		//Debug.Log("Person leaving with ID " + person.pid);
		if(arrayPersonCubes.ContainsKey(person.pid)){
			//Debug.Log("Destroying cube");
			GameObject cubeToRemove = arrayPersonCubes[person.pid];
			arrayPersonCubes.Remove(person.pid);
			//delete it from the scene	
			Destroy(cubeToRemove);
		}
	}

	public void SetDrawCubes(bool bEnable)
	{
		//bDrawCube = bEnable;
		
		ArrayList ids = new ArrayList();
		
		foreach(KeyValuePair<int, GameObject> cube in arrayPersonCubes) {
			ids.Add(cube.Key);
		}
		foreach(int id in ids) {
			if(arrayPersonCubes.ContainsKey(id)){
				GameObject pC = arrayPersonCubes[id];
				pC.renderer.enabled = bEnable;
			}
		}
		
	}
	
	public void clearAllPersons(){
		Debug.Log("Clear all cubes");
		foreach(var pKey in arrayPersonCubes.Keys){
			Destroy(arrayPersonCubes[pKey]);
		}
		arrayPersonCubes.Clear();
	}
	
	//maps the Augmenta coordinate system into one that matches the size of the boundingPlane
	private void movePerson(Person person, GameObject personObject){

		Transform pt = personObject.transform;
		Transform bt = boundingPlane.transform;
		Bounds meshBounds = boundingPlane.GetComponent<MeshFilter>().sharedMesh.bounds;

		// Reset at zero
		pt.position = Vector3.zero;
		pt.rotation = Quaternion.identity;

		// Rotate
		pt.Rotate(bt.rotation.eulerAngles);

		// Move inside the area depending on the centroid's value and depending on the scale
		pt.Translate( (float)(.5 - person.centroid.x) * meshBounds.size.x * -1 * bt.localScale.x, 0, (float)(person.centroid.y - .5) * meshBounds.size.z * -1 * bt.localScale.z);

		// Put the bottom of the cubes on the plane (not the center)
		pt.Translate( 0, pt.localScale.y/2, 0 );

		// Move to reach the position of the area
		pt.Translate(bt.position, Space.World);

	}	
}
