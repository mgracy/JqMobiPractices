// Playground - noun: a place where people can play

import UIKit

var str = "Hello, playground"

enum Rank: Int {
    case Ace = 1
    case Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten
    case Jack, Queen, King
    func simpleDescription() -> String {
        switch self {
        case .Ace:
            return "ace"
        case .Jack:
            return "jack"
        case .Queen:
            return "queen"
        case .King:
            return "king"
        default:
            return String(self.toRaw())
        }
    }
}
let ace = Rank.Ace
let aceRawValue = ace.toRaw()

if let convertedRank = Rank.fromRaw(3) {
    let threeDescription = convertedRank.simpleDescription()
}

enum Suit {
    case Spades, Hearts, Diamonds, Clubs
    func simpleDescription() -> String {
        switch self {
        case .Spades:
            return "spades"
        case .Hearts:
            return "hearts"
        case .Diamonds:
            return "diamonds"
        case .Clubs:
            return "clubs"
            
        }
    }
    func color() -> String {
        switch self{
        case .Spades,.Clubs:
            return "black"
        case .Hearts,.Diamonds:
            return "red"
        }
    }
}

let hearts = Suit.Hearts
let heartsDescription = hearts.simpleDescription()
let hearsColor = hearts.color()

struct Card {
    var rank: Rank
    var suit: Suit
    func simpleDescription() -> String {
        return "The \(rank.simpleDescription()) of \(suit.simpleDescription())"
    }
}
let threeOfSpades = Card(rank: .King, suit: .Spades)
let threeOfSpadesDescription = threeOfSpades.simpleDescription()

enum ServerResponse {
    case Result(String, String)
    case Error(String)
}
let success = ServerResponse.Result("6:00 am", "8:09 pm")
let failure = ServerResponse.Error("Out of cheese.")

switch success {
case let .Result(sunrise, sunset):
    let serverResponses = "Sunrise is at \(sunrise) and sunset is \(sunset)"
case let .Error(error):
    let serverResponse = "Failure...  \(error)"
}

protocol ExampleProtocol {
    var simpleDescription: String {get}
    mutating func adjust()
}
class SimpleClass {
    var simpleDescription: String = "A very simple class."
    var anotherProperty: Int =  69105
    func adjust() {
        simpleDescription += " Now 100% adjusted."
    }
}
var sc = SimpleClass()
sc.adjust()
let scDescription = sc.simpleDescription

struct SimpleStruct {
    var simpleDescription: String = "A simple struct"
    mutating func adjust() {
        simpleDescription += " (adjusted)"
    }
}
var ss = SimpleStruct()
ss.adjust()
let ssDescription = ss.simpleDescription

enum SimpleEnum {
   case simpleDescription
    func adjust() -> String {
        return "Enum"
    }
}
let se = SimpleEnum.adjust(.simpleDescription)

extension Int: ExampleProtocol {
    var simpleDescription: String {
        return "The number \(self)"
    }
   mutating func adjust() {
        self += 42
    }
}
7.simpleDescription

//let protocolValue: ExampleProtocol = sc
//protocolValue.simpleDescription

//泛型
/*
func repeat<ItemType>(item: ItemType, times: Int) -> [ItemType] {
    //var result = [ItemType]()
    var result = [ItemType]()
    //var result: Int
    for i in 0..<times {
        result += item
    }
    return result
}
*/

let http404Error = (404, "Not Found")

let (statusCode, statusMessage) = http404Error

println("The status code is \(statusCode)")




