using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrafficIDS.Domain
{
	public class MeterPrediction
	{
		[VectorType(3)]
		public double[] Prediction { get; set; }
	}
}
