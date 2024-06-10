using Firebase.Firestore;
using System;
using System.Collections.Generic;

[FirestoreData]
public struct Player
{
    [FirestoreProperty]
    public string email { get; set; }
    [FirestoreProperty]
    public string username { get; set; }
    [FirestoreProperty]
    public string password { get; set; }
}


[FirestoreData]
public struct Match
{
    [FirestoreProperty]
    public int id { get; set; }
    [FirestoreProperty]
    public string date { get; set; }
    
    [FirestoreProperty]
    public float time { get; set; }

    [FirestoreProperty]
    public bool result { get; set; }

    [FirestoreProperty]
    public int exp { get; set; }
}

[FirestoreData]
public struct Lobby
{
    [FirestoreProperty]
    public string foodid { get; set; }
    [FirestoreProperty]
    public string hostname { get; set; }
    [FirestoreProperty]
    public string ip { get; set; }

    [FirestoreProperty]
    public bool isactive { get; set; }
    
}


[FirestoreData]
public struct Relationship
{
    [FirestoreProperty]
    public string playerID { get; set; }
    [FirestoreProperty]
    public string username { get; set; }

    [FirestoreProperty]
    public string type { get; set; }
}


[FirestoreData]
public struct Request
{
    [FirestoreProperty]
    public string from { get; set; }

    [FirestoreProperty]
    public string to { get; set; }

    [FirestoreProperty]
    public bool accepted { get; set; }
}
