using System;
using Internal;
namespace PerformanceTests{
	public class Math2{
		public static volatile float inner_angle;
		public static volatile float outer_angle;
		static void Main(string[] args){
			double angle=0*Math.PI;
			double tra_len= 0.3871d;
			int a = 10d; 
			
			for (long i = 0; i < 100000000; i++){			
				Math2.inner_angle =(float) Math.Atan(1 / (1 / Math.Tan(angle) - tra_len));
			    Math2.outer_angle =(float) Math.Atan(1 / (1 / Math.Tan(angle) + tra_len));
			}
			
		}
	}
	
}