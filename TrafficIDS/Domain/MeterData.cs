using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrafficIDS.Domain
{
	public class MeterData
	{
		[LoadColumn(0)]
		public string Name { get; set; }
		[LoadColumn(1)]
		public DateTime Time { get; set; }
		[LoadColumn(2)]
		public float Value { get; set; }
	}
}
