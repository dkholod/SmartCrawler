module internal WebCrawlers.Utils

open System

let userAgentString = "Mozilla/5.0 (Windows NT 6.3; WOW64; Trident/7.0; rv:11.0) like Gecko"

let newLines n = 
    List.init n (fun e -> Environment.NewLine) |> List.reduce (fun acc e -> sprintf "%s%s" acc e)

let newLines2 = newLines 2

let newLines3 = newLines 3

let (|SeqEmpty|_|) (xs: 'a seq) = if Seq.isEmpty xs then Some() else None