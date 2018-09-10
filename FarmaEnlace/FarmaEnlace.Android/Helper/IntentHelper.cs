using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Speech;
using Android.Util;
using System.Collections.Generic;


namespace FarmaEnlace.Android.Helper
{
    public class IntentHelper
    {
        static SpeechRecognizer Recognizer { get; set; }
        static Intent SpeechIntent { get; set; }
        static Context context = global::Android.App.Application.Context;

        public static bool IsVoiceIntent(int code)
        {
            return code == (int)RequestCodes.VoiceCode;
        }
        static Action<string> _callback;
        struct RequestCodes
        {
            public const int VoiceCode = 1010;
        }
        static Activity CurrentActivity
        {
            get
            {
                return (Xamarin.Forms.Forms.Context as MainActivity);
            }
        }
        public static void ActivityResult(int requestCode, Intent data)
        {
            if (_callback == null)
                return;
            if (requestCode == RequestCodes.VoiceCode)
            {
                Recognizer.StopListening();
                if (data != null)
                {
                    IList<string> res = data.GetStringArrayListExtra(RecognizerIntent.ExtraResults);
                    if (res.Count > 0)
                    {
                        _callback(res[0]);
                    }
                    else
                    {
                        _callback(string.Empty);
                    }
                }
                else
                {
                    _callback(string.Empty);
                }
            }
        }

        public static void GetRecognition(Action<string> callback)
        {
            _callback = callback;

            ListenerHelper listener = new ListenerHelper();
            listener.BeginSpeech += Listener_BeginSpeech;
            listener.EndSpeech += Listener_EndSpeech;
            listener.Error += Listener_Error;
            listener.Ready += Listener_Ready;

            Recognizer = SpeechRecognizer.CreateSpeechRecognizer(context);
            Recognizer.SetRecognitionListener(listener);

            SpeechIntent = new Intent(RecognizerIntent.ActionRecognizeSpeech);
            SpeechIntent.PutExtra(RecognizerIntent.ExtraLanguageModel, RecognizerIntent.LanguageModelFreeForm);
            SpeechIntent.PutExtra(RecognizerIntent.ExtraCallingPackage, context.PackageName);

            Recognizer.StartListening(SpeechIntent);

            CurrentActivity.StartActivityForResult(SpeechIntent, RequestCodes.VoiceCode);
        }

        private static void Listener_Ready(object sender, Bundle e) => Log.Debug(nameof(MainActivity), nameof(Listener_Ready));

        private static void Listener_BeginSpeech() => Log.Debug(nameof(MainActivity), nameof(Listener_BeginSpeech));

        private static void Listener_EndSpeech() => Log.Debug(nameof(MainActivity), nameof(Listener_EndSpeech));

        private static void Listener_Error(object sender, SpeechRecognizerError e) => Log.Debug(nameof(MainActivity), $"{nameof(Listener_Error)}={e.ToString()}");
    }
}