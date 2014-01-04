using System.Net;
using System.Threading.Tasks;

namespace QDFeedParser.Extensions
{
    public static class HttpExtensions
    {
        public static Task<HttpWebResponse> GetResponseAsync(this HttpWebRequest request)
        {
            var taskComplete = new TaskCompletionSource<HttpWebResponse>();

            request.BeginGetResponse(asyncResponse =>
                                              {
                                                  try
                                                  {
                                                      HttpWebRequest responseRequest = (HttpWebRequest) asyncResponse.AsyncState;
                                                      HttpWebResponse response = (HttpWebResponse) responseRequest.EndGetResponse(asyncResponse);

                                                      taskComplete.TrySetResult(response);
                                                  }
                                                  catch (WebException ex)
                                                  {
                                                      HttpWebResponse failedResponse = (HttpWebResponse) ex.Response;
                                                      taskComplete.TrySetResult(failedResponse);
                                                      
                                                  }

                                              }, request);

            return taskComplete.Task;
        }
    }
}
