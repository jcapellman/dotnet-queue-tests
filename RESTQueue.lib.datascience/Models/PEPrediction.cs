using Microsoft.ML.Runtime.Api;

namespace RESTQueue.lib.datascience.Models
{
    public class PEPrediction
    {
        [ColumnName("PredictedLabel")]
        public bool IsMalicious;
    }
}