using System;
using Internal;

namespace PerformanceTests{
	public class Math1{
		public static volatile float inner_angle;
		public static volatile float outer_angle;
		static void Main(string[] args){
			double length = 3.6d;
			double track = 1.5d;
			double angle = Math.PI / 2;
			for (long i = 0; i < 100000000; i++){
				Math1.inner_angle = (float)Math.Atan(length * Math.Sin(angle) / (length * Math.Cos(angle) - track * Math.Sin(angle)));
				Math1.outer_angle = (float)Math.Atan(length * Math.Sin(angle) / (length * Math.Cos(angle) + track * Math.Sin(angle)));
			}
		}
	}
	
}