using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Ackermann;
using System.IO;

public class CarController : MonoBehaviour{
    ROSConnection ros;
    public string controlTopicName = "/ackermann_control";
    public double wheelbase = 1550; // in cm
    public double cogToRear = 0.775; // center of gravity to rear wheel axis
    public double mass = 270; // in kg
    public double maxSpeed = 10; // m/s
    public double steeringAngle = 0;
	public double motorTorque=80; //in nm
    public double maxSteeringAngle = 40; // +/-
	private double ackermannRatio=1.1; 
    public WheelCollider[] backWheels = new WheelCollider[2];
	public WheelCollider[] frontWheels = new WheelCollider[2];
	
	void Start(){
		//get rigidbody and set mass to this mass
		Rigidbody rb = GetComponent<Rigidbody>();
		rb.mass = (float)mass;
		foreach (var item in backWheels){
			//give the wheels the motor torque
			item.motorTorque = (float)motorTorque;
	    }
	}
    void Control(AckermannDriveMsg msg){
		
	}
	void FixedUpdate(){
		Steer(steeringAngle); 
		
	}
    void Update(){
        
    }
	//simplification of it but im sleepy and it werks for now

	void Steer(double angle=0){
		if(angle>0){
			frontWheels[0].steerAngle=(float)(angle); 
			frontWheels[1].steerAngle=(float)(angle*ackermannRatio);
		}else{
			frontWheels[0].steerAngle=(float)(angle*ackermannRatio); 
			frontWheels[1].steerAngle=(float)(angle);
		}
	}

}