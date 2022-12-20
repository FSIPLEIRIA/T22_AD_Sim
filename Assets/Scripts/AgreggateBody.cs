using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using T22_AD_Sim.Assets.Scripts.Editor;


//custom editor for AgrerateBody
[CustomEditor(typeof(AgreggateBody))]
public class AgreggateBodyEditor : Editor{
	bool firstToggle=false;
	List<UnityEngine.Shader> shaders=new List<UnityEngine.Shader>();

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		AgreggateBody myScript = (AgreggateBody)target;
		
		if(GUILayout.Button("Recalculate Center Of Mass"))
		{
			myScript.calculateCOOM();
		}

		//the checkbox was already turned on skip everything TOOPTIMZE 
		if(!firstToggle){

			firstToggle = GUILayout.Toggle(firstToggle, "Color By Mass shader");
			if(firstToggle){

				//get gameobject associated with this
				GameObject gameObject=myScript.gameObject;
				// TO OPTIMIZE: it sucks but i dont make the rules...This should be an outwards function
				double maxWeight=0;
				//get all children
				ComponentBody[] componentBodies=gameObject.GetComponentsInChildren<ComponentBody>();
				//iterate through all children
				foreach(ComponentBody componentBody in componentBodies){
					//if componentBody.mass is greater than maxWeight
					if(componentBody.mass>maxWeight){
						//set maxWeight to componentBody.mass
						maxWeight=componentBody.mass;
					}
					shaders.Clear();
				}
				
				
				foreach(ComponentBody componentBody in componentBodies){
					
					shaders.Add(componentBody.GetComponent<Renderer>().material.shader);
					componentBody.GetComponent<Renderer>().material.shader=Shader.Find("T22_AD_Sim/AgregateMass");
					componentBody.GetComponent<Renderer>().material.SetFloat("_Weight", (float)(componentBody.mass/maxWeight));

				}
				
			}else{
				//get gameobject associated with this
				GameObject gameObject=myScript.gameObject;
				//get all children
				ComponentBody[] componentBodies=gameObject.GetComponentsInChildren<ComponentBody>();
				//iterate through all children
				int k= 0;
				foreach(ComponentBody componentBody in componentBodies){
					componentBody.GetComponent<Renderer>().material.shader=shaders[k++];
					
				}
				shaders.Clear();
			}
		}else{
			firstToggle = GUILayout.Toggle(firstToggle, "Color By Mass shader");
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

