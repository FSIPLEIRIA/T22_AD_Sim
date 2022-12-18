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
	
	[Header("car/parts dimensions")]
	const double trackDivDoublelenght = 0.3871d; //track divided by double length
	const double wheelMass = 7.4668; //in Kg
	private double rearWheelMass; //in Kg
    const double wheelRadius = 0.2475; // in m
	const double numWheels = 2; //number of driving wheels
    const double cogToRear = 0.775; // center of gravity to rear wheel axis
	const double wheelBase = 1.550; // in m
	private double centerOfMass_to_frontWheel = 0.5; //in m

	[Header("car attributes")]
    const double mass = 270; // in kg
	const double frontalArea = 0.59; // in m^2
	const double rollingResistanceCoef = 0.018; // tyre rolling resistence coefficient for medium asphalt (not actual tyre value)
	const double dragCoef = 0.2219416395; // drag coefficient already divided by 2, drag coefficient = 0.443883279
	private double gripCoef = 1; //grip coefficient
	static readonly double[] diffEffiency = {1,1,1,1,1,1,1}; //efficiency of differential
	private double resistanceForce;
	private double slopeForce;
	private double dragForce;

	[Header("Gear selection")]
	[Range(0,6)] public int selectedGear = 0; //change only when using multiple gear ratios, else leave it at 0
	static readonly double[] gearRatio = {1.66, 2.846, 2.125, 1.632, 1.300, 1.091, 0.964}; //a medium ratio [0] or all gear ratios [1 to 6]
	const double finalRatio = 5.51; //Final gear ratio

	[Header("Engine")]
	private double wheelTorque = 0; // in Nm
	public double motorTorque = 0; // in Nm
	public double motorSpeed = 0; // rpm

	[Header("Angle, Speed and Acceleration")]
	public double speed = 0; // in m/s
	private double acceleration = 0; // in m/s^2
	public double maxAcceleration; //in m/s^2
    public double maxSpeed = 10; // m/s
    private double steeringAngle = 0; //radian based
    public double maxSteeringAngle = (double)(2*Math.PI/9); // +/-
	
	[Header("External Forces")]
	const double gravity = 9.80665; //gravitational acceleration in m/s^2
	public double slope = 0; //angle of slope/gradient in radians
	private double airDensity = 1.2922; //at NTP in Kg/m^3
	public double timeNeeded; //in s
	public double time=0;

	//private double ackermannRatio=1.1;
    public WheelCollider[] backWheels = new WheelCollider[2];
	public WheelCollider[] frontWheels = new WheelCollider[2];
	Rigidbody rb;

	void Start(){
		//get rigidbody and set mass to this mass
		rb = GetComponent<Rigidbody>();
		rb.mass = (float)mass;
		var k = new AckermannDriveMsg((float)Math.PI*0,(float)Math.PI*0,28,1,0);//Math.PI/4,(float)Math.PI/8,0,0,0);
		Control(k);
	}
    void Control(AckermannDriveMsg msg){
		if((msg.speed) !=speed)
		{
			//time to speed
			motorTorque = torquePrediction(msg.speed);
			timeNeeded = timeToReachSpeed(msg.acceleration, msg.speed);
			StartCoroutine(TimeToSpeed(msg.speed, acceleration));
		}
		//convert steeringAngle to radians and compare to msg.steering_angle
		if(msg.steering_angle !=steeringAngle){
			//time to fly
			StartCoroutine(TimeToFlySteering(msg.steering_angle, msg.steering_angle_velocity));
		}
	}
	void FixedUpdate(){
		Steer(steeringAngle);
		Drive(speed);
	}
    void Update(){
    }
	//simplification of it but im sleepy and it werks for now
	IEnumerator TimeToFlySteering(double objective_steer, double steering_angle_velocity, int interpolation=1000){
		//while objective_steer is not reached lerp add steering_angle_velocity to steeringAngle
		
		while(objective_steer!=steeringAngle){

			steeringAngle = Mathf.Lerp((float)steeringAngle, (float)objective_steer, (float)steering_angle_velocity/interpolation);
			yield return new WaitForSeconds(1f/interpolation);
		}
		yield return null;
	}

	IEnumerator TimeToSpeed(double objective_speed, double desired_acceleration, int interpolation=1000){
		//while objective_speed is not reached lerp add acceleration to speed
		time=0;
		while(time<=timeNeeded){
			speed = Mathf.Lerp((float)speed, (float)objective_speed, (float)desired_acceleration/interpolation);
			time+=Time.deltaTime;
			yield return new WaitForSeconds(1f/interpolation);
		}
		yield return null;
	}

	double torquePrediction(double objective_speed)
	{
		double motorTorquePrediction=0;
		resistanceForce = mass*gravity*rollingResistanceCoef*Math.Cos(slope);
		slopeForce = mass*gravity*Math.Sin(slope);
		dragForce = dragCoef*frontalArea*airDensity*Math.Pow(objective_speed, 2); //3.6 is Km/h to m/s
		wheelTorque = (resistanceForce + slopeForce + dragForce)*wheelRadius;
		motorTorquePrediction = wheelTorque/(gearRatio[selectedGear]*finalRatio*diffEffiency[selectedGear]);
		return motorTorquePrediction;
	}

	double timeToReachSpeed(double ackermanAcceleration, double ackermanDiseredSpeed)
	{	
		double timeVariation=0; // in s
		rearWheelMass = ((mass-4*wheelMass)*centerOfMass_to_frontWheel)/wheelBase;
		maxAcceleration = gripCoef*gravity*rearWheelMass/mass;
		if(ackermanAcceleration > maxAcceleration)
			acceleration = maxAcceleration;
		else
			acceleration = ackermanAcceleration;
		timeVariation = ((ackermanDiseredSpeed)-speed)/acceleration;
		return timeVariation;
	}

	void Steer(double angle=0){
		var inner_angle =(float) Math.Atan(1 / (1 / Math.Tan(angle) - trackDivDoublelenght));
		var outer_angle =(float) Math.Atan(1 / (1 / Math.Tan(angle) + trackDivDoublelenght));
		//Apply to wheels
		if(angle>0){
			frontWheels[0].steerAngle=outer_angle*Mathf.Rad2Deg;
			frontWheels[1].steerAngle=inner_angle*Mathf.Rad2Deg;
		}else{
			frontWheels[0].steerAngle=inner_angle*Mathf.Rad2Deg;
			frontWheels[1].steerAngle=outer_angle*Mathf.Rad2Deg;
		}
	}

	void Drive(double speed=0){
		//Apply to wheels
		backWheels[0].motorTorque=(float)speed;
		backWheels[1].motorTorque=(float)speed;
	}
}