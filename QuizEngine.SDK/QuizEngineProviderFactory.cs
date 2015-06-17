using QuizEngine.SDK.Interfaces;
using QuizEngine.SDK.Providers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizEngine.SDK
{
    public class QuizEngineProviderFactory
    {
        public const string ApiBaseUrlSettingsName = "QuizEngineApiBaseUrl";

        private static IQuizEngineProvider _quizEngineProvider;

        private QuizEngineProviderFactory() {}

        public static IQuizEngineProvider GetInstance()
        {
            if(_quizEngineProvider == null)
            {
                InitializeProvider();
            }

            return _quizEngineProvider;
        }

        private static void InitializeProvider()
        {
            if(_quizEngineProvider == null)
            {
                var baseUrl = ConfigurationManager.AppSettings[ApiBaseUrlSettingsName];
                if(string.IsNullOrEmpty(baseUrl))
                {
                    throw new ConfigurationErrorsException(string.Format("Error initializing provider: {0} is missing or not set in the configuration file.", ApiBaseUrlSettingsName));
                }

                _quizEngineProvider = new QuizEngineApiProvider(baseUrl);
            }
        }
    }
}
