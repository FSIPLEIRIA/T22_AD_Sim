using System;

namespace T22_AD_Sim.Assets.Scripts{
    public interface IMaxSteeringSpeedModel {
        double getSafeMaxSteeringSpeed();
		double getUnsafeMaxSteeringSpeed();
		double getEmergencyMaxSteeringSpeed();
    }
}
