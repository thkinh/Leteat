using Firebase.Firestore;

[FirestoreData]
public struct Player
{
    [FirestoreProperty]
    public int id { get; set; }

    [FirestoreProperty]
    public string email { get; set; }
    [FirestoreProperty]
    public string username { get; set; }
    [FirestoreProperty]
    public string password { get; set; }
}
