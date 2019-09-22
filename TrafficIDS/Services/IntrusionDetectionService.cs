using Microsoft.ML;
using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrafficIDS.Domain;
using TrafficIDS.Models;

namespace TrafficIDS.Services
{
	public class IntrusionDetectionService
	{
		static private MLContext _mlContext;

		private static string _path = @"F:\Studies\Projects\TrafficIDS\TrafficIDS\Data\meterData.csv";
		private static string ModelPath = @"F:\Studies\Projects\TrafficIDS\TrafficIDS\MlModels\PowerAnomalyDetectionModel.zip";
		private static int _fileSize = 36;

		public IntrusionDetectionService()
		{
			_mlContext = new MLContext();
		}

		public IEnumerable<EvaluatedTrafficData> GetSpikes()
		{
			var dataView = LoadData();

			var trainedModel = BuildTrainModel(dataView);
			SaveTrainedModel(transformer: trainedModel, dataView);

			var evaluatedData = GetEvaluatedData(dataView);

			return evaluatedData;
		}

		public IDataView LoadData()
		{
			try
			{
				return _mlContext.Data
					.LoadFromTextFile<MeterData>(path: _path, hasHeader: true, separatorChar: ',');
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
				_mlContext.Model.Save(transformer, dataView.Schema, ModelPath);
			}
			catch
			{
				throw;
			}
		}

		public IEnumerable<EvaluatedTrafficData> GetEvaluatedData(IDataView dataView)
		{
			ITransformer trainedModel = _mlContext.Model.Load(ModelPath, out var modelInputSchema);

			var transformedData = trainedModel.Transform(dataView);

			var evaluatedData = _mlContext.Data
				.CreateEnumerable<EvaluatedTrafficData>(transformedData, false);

			return evaluatedData;
		}
	}
}
