using System;
using System.Speech.Synthesis;

class Program
{
    static void Main()
    {
        var synth = new SpeechSynthesizer();
        foreach (var voice in synth.GetInstalledVoices()) {
            var vi = voice.VoiceInfo;
            Console.WriteLine("{0}: {1}", vi.Culture, vi.Description);
        }
    }
}
