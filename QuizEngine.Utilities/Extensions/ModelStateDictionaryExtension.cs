using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ModelBinding;

namespace QuizEngine.Utilities.Extensions
{
    public static class ModelStateDictionaryExtension
    {
        public static string GenerateErrorMessage(this ModelStateDictionary modelStateDictionary)
        {
            var errorMessageBuilder = new StringBuilder("");

            var allErrors = modelStateDictionary.Values.SelectMany(v => v.Errors.Where(e => e != null)
                                .Where(e => !string.IsNullOrEmpty(e.ErrorMessage)));

            if (allErrors.Count() > 0)
            {
                var totalNumOfErrors = allErrors.Count();

                errorMessageBuilder.Append("Please resolve the following error(s): ");
                var messageFormat = string.Empty;
                for (var i = 0; i < totalNumOfErrors; i++)
                {
                    var modelError = allErrors.ElementAtOrDefault(i);
                    var message = modelError.ErrorMessage;

                    var number = i + 1;
                    if (totalNumOfErrors == 1)
                    {
                        messageFormat = "{0}) {1}."; // if only 1 error message, just display it
                    }
                    else
                    {
                        if (number < totalNumOfErrors)
                        {
                            if (totalNumOfErrors == 2)
                            {
                                messageFormat = "{0}) {1} "; // if there are two messages, and this is the first one, don't include comma at the end
                            }
                            else
                            {
                                messageFormat = "{0}) {1}, "; // if there are more than two messages, and this is not the last include comma at the end
                            }
                        }
                        else
                        {
                            messageFormat = "and {0}) {1}."; // this is the last message, prepend "and" and include a period at the end
                        }
                    }
                    errorMessageBuilder.AppendFormat(messageFormat, number, message);
                }
            }

            return errorMessageBuilder.ToString().ToLowerInvariant();            
        }
    }
}
