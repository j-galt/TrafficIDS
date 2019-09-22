using System;

namespace TrafficIDS.Models
{
	public class EvaluatedTrafficData
	{
		public string Name { get; set; }
		public DateTime Time { get; set; }
		public float Value { get; set; }
		public double[] Prediction { get; set; }
	}
}
