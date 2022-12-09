using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Ackermann;
using System.IO;
using System;

public class CarController : MonoBehaviour{
    ROSConnection ros;
    public string controlTopicName = "/ackermann_control";
    public double wheelbase = 1550; // in cm
    public double cogToRear = 0.775; // center of gravity to rear wheel axis
	
	const double trackDivDoublelenght= 0.3871d; //track divided by double length 
    public double mass = 270; // in kg
    public double maxSpeed = 10; // m/s
    private double steeringAngle = 0; //radian based
	public double motorTorque=80; //in nm

    public double maxSteeringAngle =(double)(2*Math.PI/9); // +/-
	//private double ackermannRatio=1.1; 

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
		var k = new AckermannDriveMsg((float)Math.PI/4,(float)Math.PI/4,0,0,0);
		Control(k);
	}
    void Control(AckermannDriveMsg msg){
		//convert steeringAngle to radians and compare to msg.steering_angle
		if(msg.steering_angle !=steeringAngle){
			//time to fly
			StartCoroutine(TimeToFlySteering(msg.steering_angle, msg.steering_angle_velocity,1));
		}

		
	}
	void FixedUpdate(){
		Steer(steeringAngle); 
		
	}
    void Update(){
        
    }
	//simplification of it but im sleepy and it werks for now
	IEnumerator TimeToFlySteering(double objective_steer, double steering_angle_velocity, int interpolation=100){
		//while objective_steer is not reached lerp add steering_angle_velocity to steeringAngle
		
		while(objective_steer!=steeringAngle){

			steeringAngle = Mathf.Lerp((float)steeringAngle, (float)objective_steer, (float)steering_angle_velocity/interpolation);
			yield return new WaitForSeconds(1f/interpolation);
		}
		yield return null;
	}
	void Steer(double angle=0){
		var inner_angle =(float) Math.Atan(1 / (1 / Math.Tan(angle) - trackDivDoublelenght));
	    var outer_angle =(float) Math.Atan(1 / (1 / Math.Tan(angle) + trackDivDoublelenght));
		if(angle>0){
			frontWheels[0].steerAngle=outer_angle*Mathf.Rad2Deg; 
			frontWheels[1].steerAngle=inner_angle*Mathf.Rad2Deg;
		}else{
			frontWheels[0].steerAngle=inner_angle*Mathf.Rad2Deg;
			frontWheels[1].steerAngle=outer_angle*Mathf.Rad2Deg;
		}
	}


}