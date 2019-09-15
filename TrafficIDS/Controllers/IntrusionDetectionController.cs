using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrafficIDS.Services;

namespace TrafficIDS.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class IntrusionDetectionController : ControllerBase
	{
		private readonly IntrusionDetectionService _intrusionDetection;

		public IntrusionDetectionController(IntrusionDetectionService intrusionDetection)
		{
			_intrusionDetection = intrusionDetection;
		}

		[HttpGet]
		public IActionResult Get()
		{
			var predictions = _intrusionDetection.GetSpikes();
			return Ok(predictions);
		}

		// GET: api/IntrusionDetection/5
		[HttpGet("{id}", Name = "Get")]
		public string Get(int id)
		{
			return "value";
		}

		// POST: api/IntrusionDetection
		[HttpPost]
		public void Post([FromBody] string value)
		{
		}

		// PUT: api/IntrusionDetection/5
		[HttpPut("{id}")]
		public void Put(int id, [FromBody] string value)
		{
		}

		// DELETE: api/ApiWithActions/5
		[HttpDelete("{id}")]
		public void Delete(int id)
		{
		}
	}
}
