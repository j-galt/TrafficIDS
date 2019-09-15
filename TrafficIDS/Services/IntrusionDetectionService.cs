using Microsoft.ML;
using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrafficIDS.Domain;

namespace TrafficIDS.Services
{
	public class IntrusionDetectionService
	{
		static private MLContext _mlContext;

		private static string _path = @"F:\Studies\Projects\TrafficIDS\TrafficIDS\TrafficIDS\Data\meterData.csv";
		private static string ModelPath = @"F:\Studies\Projects\TrafficIDS\TrafficIDS\TrafficIDS\MlModels\PowerAnomalyDetectionModel.zip";
		private static int _fileSize = 36;

		public IntrusionDetectionService()
		{
			_mlContext = new MLContext();
		}

		public IEnumerable<MeterPrediction> GetSpikes()
		{
			var dataView = LoadData();
			BuildTrainModel(dataView);
			var predictions = DetectAnomalies(dataView);

			return predictions;
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

		public void BuildTrainModel(IDataView dataView)
		{
			// Configure the Estimator
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

			ITransformer trainedModel = trainigPipeLine.Fit(dataView);

			// STEP 6: Save/persist the trained model to a .ZIP file
			_mlContext.Model.Save(trainedModel, dataView.Schema, ModelPath);
		}

		public IEnumerable<MeterPrediction> DetectAnomalies(IDataView dataView)
		{
			ITransformer trainedModel = _mlContext.Model.Load(ModelPath, out var modelInputSchema);

			var transformedData = trainedModel.Transform(dataView);

			// Getting the data of the newly created column as an IEnumerable
			IEnumerable<MeterPrediction> predictions =
				 _mlContext.Data.CreateEnumerable<MeterPrediction>(transformedData, false);

			return predictions;

			//var colCDN = dataView.GetColumn<float>("Value").ToArray();
			//var colTime = dataView.GetColumn<DateTime>("Time").ToArray();

			//int i = 0;
			//foreach (var p in predictions)
			//{
			//	if (p.Prediction[0] == 1)
			//	{
			//		Console.BackgroundColor = ConsoleColor.DarkYellow;
			//		Console.ForegroundColor = ConsoleColor.Black;
			//	}
			//	Console.WriteLine("{0}\t{1:0.0000}\t{2:0.00}\t{3:0.00}\t{4:0.00}",
			//		 colTime[i], colCDN[i],
			//		 p.Prediction[0], p.Prediction[1], p.Prediction[2]);
			//	Console.ResetColor();
			//	i++;
			//}
		}
	}
}
