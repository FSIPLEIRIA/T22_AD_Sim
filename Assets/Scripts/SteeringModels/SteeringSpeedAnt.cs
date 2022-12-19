using System;

namespace T22_AD_Sim.Assets.Scripts{
    public class SteeringSpeedAnt: IMaxSteeringSpeedModel{

		private double maxSteeringAngleSpeed =(double)(2*Math.PI/9); //θ/s 
		public double getSafeMaxSteeringSpeed(){
			return maxSteeringAngleSpeed; 
		}
		public double getUnsafeMaxSteeringSpeed(){
			//above the safe specs without ruining the motor. its likely to return different values when called with seconds appart
			throw new NotImplementedException();
		}
		public double getEmergencyMaxSteeringSpeed(){
			//Just push the current more and more even  if it fries the motor, only used to protect car/whats inside
			throw new NotImplementedException();
		}
        
    }
}
