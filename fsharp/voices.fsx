open System
let SpeechSynthesizer = Type.GetType(@"
    Windows.Media.SpeechSynthesis.SpeechSynthesizer,
    Windows.Foundation.UniversalApiContract,
    ContentType=WindowsRuntime")
for voice in SpeechSynthesizer.GetProperty("AllVoices").GetValue(null) :?> seq<obj> do
    let t = voice.GetType()
    let lang = t.GetProperty("Language"   ).GetValue(voice) :?> String
    let desc = t.GetProperty("Description").GetValue(voice) :?> String
    printfn "%s: %s" lang desc
