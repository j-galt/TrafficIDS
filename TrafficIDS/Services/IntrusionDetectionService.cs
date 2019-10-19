using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.ML;
using System.Collections.Generic;
using System.IO;
using TrafficIDS.Domain;
using TrafficIDS.Models;

namespace TrafficIDS.Services
{
	public class IntrusionDetectionService
	{
		private static MLContext _mlContext = new MLContext();

		private readonly IConfiguration _config;
		private readonly IHostingEnvironment _hostingEnvironment;

		public IntrusionDetectionService(IConfiguration config, IHostingEnvironment hostingEnvironment)
		{
			_config = config;
			_hostingEnvironment = hostingEnvironment;
		}

		public IEnumerable<EvaluatedTrafficData> GetSpikes(string fileName)
		{
			var dataView = LoadData(
				Path.Combine(_hostingEnvironment.WebRootPath, _config["StoredFilesPath"], fileName));

			var trainedModel = BuildTrainModel(dataView);
			SaveTrainedModel(transformer: trainedModel, dataView);

			var evaluatedData = GetEvaluatedData(dataView);

			return evaluatedData;
		}

		public IDataView LoadData(string path)
		{
			try
			{
				return _mlContext.Data
					.LoadFromTextFile<MeterData>(path: path, hasHeader: true, separatorChar: ',');
			}
			catch
			{
				throw;
			}
		}

		public ITransformer BuildTrainModel(IDataView dataView)
		{
			const int PValueSize = 30;
			const int SeasonalitySize = 30;
			const int TrainingSize = 90;
			const int ConfidenceInterval = 98;

			string outputColumnName = nameof(MeterPrediction.Prediction);
			string inputColumnName = nameof(MeterData.Value);

			var trainigPipeLine = _mlContext.Transforms.DetectSpikeBySsa(
				 outputColumnName,
				 inputColumnName,
				 confidence: ConfidenceInterval,
				 pvalueHistoryLength: PValueSize,
				 trainingWindowSize: TrainingSize,
				 seasonalityWindowSize: SeasonalitySize);

			return trainigPipeLine.Fit(dataView);
		}

		public void SaveTrainedModel(ITransformer transformer, IDataView dataView)
		{
			try
			{
				var modelPath = Path.Combine(_hostingEnvironment.WebRootPath, _config["ModelPath"]);
				_mlContext.Model.Save(transformer, dataView.Schema, modelPath);
			}
			catch
			{
				throw;
			}
		}

		public IEnumerable<EvaluatedTrafficData> GetEvaluatedData(IDataView dataView)
		{
			var modelPath = Path.Combine(_hostingEnvironment.WebRootPath, _config["ModelPath"]);
			ITransformer trainedModel = _mlContext.Model.Load(modelPath, out var modelInputSchema);

			var transformedData = trainedModel.Transform(dataView);

			var evaluatedData = _mlContext.Data
				.CreateEnumerable<EvaluatedTrafficData>(transformedData, false);

			return evaluatedData;
		}
	}
}
