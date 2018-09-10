using System;
using System.Collections.Generic;
using System.Text;
using FarmaEnlace.Helpers;

namespace FarmaEnlace.Interfaces
{
    public interface ISpeechToText
    {
        void Start();
        void Stop();
        event EventHandler<EventArgsVoiceRecognition> textChanged;
    }
}
