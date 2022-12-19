using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using T22_AD_Sim.Assets.Scripts;
//custom editor for AgrerateBody
[CustomEditor(typeof(AgreggateBody))]
public class AgreggateBodyEditor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		AgreggateBody myScript = (AgreggateBody)target;
		if(GUILayout.Button("Recalculate Center Of Mass"))
		{
			myScript.calculateCOOM();
		}
	}
}

public class AgreggateBody : MonoBehaviour{
    // the purpose of this class is to better the cars center of mass
	// although this value can be precalculated, having to ask other departments "how even it is" is annoying
	
	//make variable readonly in the editor
	[ReadOnly]
	public Vector3 compositeCenterOfMass;
	
 
	void Start(){
		
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	//calculates C.O.M (Center Of Mass)
	public void calculateCOOM(){
		double massSum=0;
		Vector3 centerOfMass=Vector3.zero;
		//get ComponentBody components from children
		ComponentBody[] componentBodies=GetComponentsInChildren<ComponentBody>(); 
		//iterate through all component bodies
		foreach(ComponentBody componentBody in componentBodies){
			//add mass to mass sum
			massSum+=componentBody.mass;
			//add mass * position to center of mass
			centerOfMass+=scalarMultiply(componentBody.transform.localPosition+componentBody.pos, componentBody.mass);
		}
		//divide center of mass by mass sum
		centerOfMass=scalarDivide(centerOfMass, massSum);
		//set composite center of mass
		compositeCenterOfMass=centerOfMass;


	}


	//TODO: time to get a centralized math library this will get clutered really fast
	public Vector3 scalarMultiply( Vector3 vector, double scalar){
		return new Vector3((float)(vector.x*scalar),(float)(vector.y*scalar),(float)(vector.z*scalar));
	}
	public Vector3 scalarDivide( Vector3 vector, double scalar){
		return new Vector3((float)(vector.x/scalar),(float)(vector.y/scalar),(float)(vector.z/scalar));
	}

}

