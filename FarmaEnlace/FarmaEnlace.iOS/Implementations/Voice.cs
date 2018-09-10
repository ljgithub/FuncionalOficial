using AVFoundation;
using FarmaEnlace.Interfaces;
using FarmaEnlace.iOS.Implementations;
using Foundation;
using Speech;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(Voice))]
namespace FarmaEnlace.iOS.Implementations
{
    public class Voice : IVoice
    {

        private AVAudioEngine AudioEngine = new AVAudioEngine();
        private SFSpeechRecognizer SpeechRecognizer = new SFSpeechRecognizer();
        private SFSpeechAudioBufferRecognitionRequest LiveSpeechRequest = new SFSpeechAudioBufferRecognitionRequest();
        private SFSpeechRecognitionTask RecognitionTask;

        public Task<string> GetVoice()
        {
            TaskCompletionSource<string> task = new TaskCompletionSource<string>();
            try
            {
                var obj = Start();

                if (obj == null)
                {
                    task.SetResult(null);
                }
                else
                {
                    task.SetResult(obj);
                }
            }
            catch (Exception ex)
            {
                task.SetException(ex);
            }
            return task.Task;
        }

        public void InitializeProperties()
        {
            AudioEngine = new AVAudioEngine();
            SpeechRecognizer = new SFSpeechRecognizer();
            LiveSpeechRequest = new SFSpeechAudioBufferRecognitionRequest();
        }

        public void Stop()
        {
            CancelRecording();
        }
        public string Start()
        {
            string result = string.Empty;
            // Request user authorization
            SFSpeechRecognizer.RequestAuthorization((SFSpeechRecognizerAuthorizationStatus status) => {
                // Take action based on status
                switch (status)
                {
                    case SFSpeechRecognizerAuthorizationStatus.Authorized:
                        InitializeProperties();
                        result = StartRecordingSession();
                        break;
                    case SFSpeechRecognizerAuthorizationStatus.Denied:
                        // User has declined speech recognition
                        break;
                    case SFSpeechRecognizerAuthorizationStatus.NotDetermined:
                        // Waiting on approval

                        break;
                    case SFSpeechRecognizerAuthorizationStatus.Restricted:
                        // The device is not permitted
                        break;
                }
            });

            return result;
        }

        public string StartRecordingSession()
        {
            string result = string.Empty;
            // Start recording
            AudioEngine.InputNode.InstallTapOnBus(bus: 0, bufferSize: 1024, format: AudioEngine.InputNode.GetBusOutputFormat(0), tapBlock: (buffer, when) => LiveSpeechRequest?.Append(buffer));
            AudioEngine.Prepare();
            NSError error;
            AudioEngine.StartAndReturnError(out error);

            // Did recording start?
            if (error != null)
            {
                // Handle error and retur
                return result;
            }

            try
            {
                return CheckAndStartReconition();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return string.Empty;
            }

        }
        public string CheckAndStartReconition()
        {
            if (RecognitionTask?.State == SFSpeechRecognitionTaskState.Running)
            {
                CancelRecording();
                return string.Empty;
            }
            return StartVoiceRecognition();
        }
        public string StartVoiceRecognition()
        {
            string text = string.Empty;
            try
            {

                RecognitionTask = SpeechRecognizer.GetRecognitionTask(LiveSpeechRequest, (SFSpeechRecognitionResult result, NSError err) => {
                    if (result == null)
                    {
                        CancelRecording();
                    }
                    // Was there an error?
                    if (err != null)
                    {
                        CancelRecording();
                    }
                    //	 Is this the final translation?
                    if (result != null && result.BestTranscription != null && result.BestTranscription.FormattedString != null)
                    {
                        text = result.BestTranscription.FormattedString;
                    }
                    if (result.Final)
                    {
                        CancelRecording();
                    }
                });
                return text;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return string.Empty;
            }
        }
        public void StopRecording()
        {
            try
            {
                AudioEngine?.Stop();
                LiveSpeechRequest?.EndAudio();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void CancelRecording()
        {
            try
            {
                AudioEngine?.Stop();
                RecognitionTask?.Cancel();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

    }
}
