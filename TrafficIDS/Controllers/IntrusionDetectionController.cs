using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using TrafficIDS.Services;

namespace TrafficIDS.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class IntrusionDetectionController : ControllerBase
	{
		private readonly IntrusionDetectionService _intrusionDetection;
		private readonly IConfiguration _config;
		private readonly IHostingEnvironment _hostingEnvironment;

		public IntrusionDetectionController(IntrusionDetectionService intrusionDetection,
			IConfiguration config,
			IHostingEnvironment hostingEnvironment)
		{
			_intrusionDetection = intrusionDetection;
			_config = config;
			_hostingEnvironment = hostingEnvironment;
		}

		[HttpGet]
		public IActionResult Get(string fileName)
		{
			var predictions = _intrusionDetection.GetSpikes(Path.Combine(fileName));
			return Ok(predictions);
		}

		[HttpPost]
		public async Task<IActionResult> OnPostUploadAsync()
		{
			string fileName = null;

			try
			{
				var file = Request.Form.Files[0];

				if (file.Length > 0)
				{
					fileName = Guid.NewGuid().ToString() + ".csv";

					var filePath = Path.Combine(_hostingEnvironment.WebRootPath,
						_config["StoredFilesPath"], fileName);

					using (var stream = System.IO.File.Create(filePath))
					{
						await file.CopyToAsync(stream);
					}
				}
			}
			catch (Exception e)
			{
				return StatusCode(500);
			}

			return Ok(new { fileName });
		}
	}
}
