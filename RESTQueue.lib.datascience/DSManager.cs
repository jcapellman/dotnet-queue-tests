using System.IO;

using Microsoft.ML;
using Microsoft.ML.Core.Data;
using Microsoft.ML.Runtime.Data;

using RESTQueue.lib.Common;
using RESTQueue.lib.datascience.Models;

namespace RESTQueue.lib.datascience
{
    public class DSManager
    {
        private readonly string _modelName;

        private PredictionFunction<PEModelData, PEPrediction> _predictionFunction;

        public DSManager(string modelName = Constants.FILENAME_MODEL)
        {
            _modelName = modelName;
        }

        public bool TrainModel(string trainingDataPath)
        {
            using (var stream = File.Create(_modelName))
            {
                var env = new LocalEnvironment();

                var reader = TextLoader.CreateReader(env, ctx => (
                        Label: ctx.LoadDouble(0),
                        Is64: ctx.LoadBool(1)),
                    hasHeader: true);

                var data = reader.Read(new MultiFileSource(trainingDataPath));

                var learningPipeline = reader.MakeNewEstimator();
                
                var classification = new BinaryClassificationContext(env);
                
                var model = learningPipeline.Fit(data);

                model.AsDynamic.SaveTo(env, stream);

                return true;
            }
        }

        private bool LoadModel()
        {
            using (var stream = File.OpenRead(_modelName))
            {
                var env = new LocalEnvironment();

                ITransformer loadedModel = TransformerChain.LoadFrom(env, stream);

                _predictionFunction = loadedModel.MakePredictionFunction<PEModelData, PEPrediction>(env);

                return true;
            }
        }

        public bool IsMalicious(byte[] data)
        {
            if (_predictionFunction == null)
            {
                LoadModel();
            }
            
            var predictorData = new PEModelData();

            var prediction = _predictionFunction.Predict(predictorData);
            
            return prediction.IsMalicious;
        }
    }
}