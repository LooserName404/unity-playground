namespace FSharp

open UnityEngine

type SimpleScript() =
    inherit MonoBehaviour()
    member this.Start() = Debug.Log("A Unity não manda em mim e eu uso a linguagem que eu quiser. Bom-dia")
