using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace GoProAssistant.CameraInterface
{
    public class Camera
    {
        private const string START_RECORDING_URL = "http://10.5.5.9/gp/gpControl/command/shutter?p=1";
        private const string STOP_RECORDING_URL = "http://10.5.5.9/gp/gpControl/command/shutter?p=0";

        public async Task<bool> StartRecordingAsync()
        {
            return await SendGoProRequest(START_RECORDING_URL);
        }

        public async Task<bool> StopRecordingAsync()
        {
            return await SendGoProRequest(STOP_RECORDING_URL);
        }

        private async Task<bool> SendGoProRequest(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                Uri uri = new Uri(url);

                HttpResponseMessage response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();

                    return true;
                }
                else
                    return false;
            }
        }
    }
}
