#r "System.Speech"
let synth = new System.Speech.Synthesis.SpeechSynthesizer()
for voice in synth.GetInstalledVoices() do
    let vi = voice.VoiceInfo
    printfn "%A: %s" vi.Culture vi.Description
