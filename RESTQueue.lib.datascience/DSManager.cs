using System.Threading.Tasks;

using Microsoft.ML.Legacy;
using Microsoft.ML.Legacy.Data;
using Microsoft.ML.Legacy.Trainers;

using RESTQueue.lib.datascience.Models;

namespace RESTQueue.lib.datascience
{
    public class DSManager
    {
        public async Task TrainModel(string modelPath, string trainingData)
        {
            var pipeline = new LearningPipeline
            {
                new TextLoader(trainingData).CreateFrom<PEModelData>(useHeader: true, separator: ','),
                new FastTreeBinaryClassifier() {NumLeaves = 5, NumTrees = 5, MinDocumentsInLeafs = 2}
            };
            
            var model = pipeline.Train<PEModelData, PEPrediction>();
            
            await model.WriteAsync(modelPath);
        }

        public async Task<bool> IsMaliciousAsync(byte[] data)
        {
            var model = await PredictionModel.ReadAsync<PEModelData, PEPrediction>("pe.model");

            var predictorData = new PEModelData();

            var prediction = model.Predict(predictorData);
            
            return prediction.IsMalicious;
        }
    }
}