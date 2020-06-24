using System;
using Windows.Media.SpeechSynthesis;

class Program
{
    static void Main()
    {
        foreach (var voice in SpeechSynthesizer.AllVoices) {
            Console.WriteLine("{0}: {1}", voice.Language, voice.Description);
        }
    }
}
