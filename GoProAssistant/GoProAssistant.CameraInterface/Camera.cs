using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace GoProAssistant.CameraInterface
{
    public class Camera
    {
        private const string START_RECORDING_URL = "http://10.5.5.9/gp/gpControl/command/shutter?p=1";
        private const string STOP_RECORDING_URL = "http://10.5.5.9/gp/gpControl/command/shutter?p=0";

        private const string POWER_ON_URL = "http://10.5.5.9/gp/gpControl/command/shutter?p=1";
        private const string POWER_OFF_URL = "http://10.5.5.9/gp/gpControl/command/system/sleep";

        private const string LOCATE_ON_URL = "http://10.5.5.9/gp/gpControl/command/system/locate?p=1";
        private const string LOCATE_OFF_URL = "http://10.5.5.9/gp/gpControl/command/system/locate?p=0";

        public async Task<bool> StartRecordingAsync()
        {
            return await SendGoProRequest(START_RECORDING_URL);
        }

        public async Task<bool> StopRecordingAsync()
        {
            return await SendGoProRequest(STOP_RECORDING_URL);
        }

        public async Task<bool> PowerOnAsync()
        {
            return await SendGoProRequest(POWER_ON_URL);
        }

        public async Task<bool> PowerOffAsync()
        {
            return await SendGoProRequest(POWER_OFF_URL);
        }

        public async Task<bool> LocateOnAsync()
        {
            return await SendGoProRequest(LOCATE_ON_URL);
        }

        public async Task<bool> LocateOffAsync()
        {
            return await SendGoProRequest(LOCATE_OFF_URL);
        }

        private async Task<bool> SendGoProRequest(string url)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromMilliseconds(2000);
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
            catch
            {
                return false;
            }
        }
    }
}
