// #r "\\Sources\\packages\\FSharp.Data.2.3.0-beta1\\lib\\net40\\FSharp.Data.dll"
// #load "\Sources\WebCrawlers\Utils.fs"
module WebCrawlers.DouUa

open FSharp.Data
open System
open Utils

type Vacancie = HtmlProvider<"http://jobs.dou.ua/companies/macphun-llc/vacancies/23515/?1448552426">
type Rss = XmlProvider<"http://jobs.dou.ua/vacancies/feeds/?category=.NET">

let getInnerText (el: HtmlNode) = el.InnerText()

let fetchByClass className (el: HtmlNode) = 
    try
        el.Descendants(fun e -> e.HasAttribute("class", className))
        |> Seq.map getInnerText
        |> Seq.reduce (fun acc elem -> sprintf "%s%s%s" acc newLines2 elem)
    with 
    | :? System.ArgumentException -> null

let fetchByElement (elementName: string) (el: HtmlNode) = 
    try
        el.Descendants(elementName)
        |> Seq.map getInnerText
        |> Seq.reduce (fun acc elem -> sprintf "%s%s%s" acc newLines2 elem)
    with 
    | :? System.ArgumentException -> null

let getPageItems (uri: string) =
    try
        let getDescription el = (el |> fetchByClass "g-h3", el |> fetchByElement "p")
        let data = 
            Http.RequestString(uri, headers=["user-agent", userAgentString])
            |> Vacancie.Parse
        data.Html.Body().Descendants(fun e -> e.HasAttribute("class","vacancy-section"))
        |> Seq.map getDescription
    with
        | _ -> Seq.empty

let getPageData (uri: string) = 
    getPageItems uri
    |> Seq.map (fun (title,description) -> sprintf "%s%s%s" title newLines2 description)
    |> Seq.reduce (fun a e -> sprintf "%s%s%s" a newLines3 e)

type RssItem = { Title: string; Link: string; PubDate: DateTime; Description: string }

type ItemsContainer = { Items: seq<RssItem>; RecentDate: DateTime }

let getCrawlerData (fromDate: DateTime) (uri:string) =
    let data = 
        Http.RequestString(uri, headers=["user-agent", userAgentString])
        |> Rss.Parse
    let items = 
        data.Channel.Items
          |> Seq.filter (fun i -> i.PubDate > fromDate)
          |> Seq.map (fun i -> { Title = i.Title; Link = i.Link; PubDate = i.PubDate; Description = getPageData i.Link })
      |> Seq.sortByDescending (fun e -> e.PubDate)
    let lastItemDate = match items with
                        | SeqEmpty -> fromDate
                        | _ -> (items |> Seq.head ).PubDate
    { Items = items; RecentDate = lastItemDate }