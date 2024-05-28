using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Firestore;
using Firebase.Extensions;
using System.Threading.Tasks;
using System.Linq;
using Unity.VisualScripting;

public class FirestoreClient : MonoBehaviour
{
    public static FirestoreClient fc_instance;
    private static FirebaseFirestore db;
    public string thisPlayerID;
    void Awake()
    {
        if (fc_instance == null)
        {
            fc_instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (fc_instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        db = FirebaseFirestore.DefaultInstance;
        thisPlayerID = "XxbEXW5ZIN2P18EmunDF";
    }

    public async void Write(Match match)
    {
        DocumentReference matchRef = db.Collection("Matches").Document(match.id.ToString());
        await matchRef.SetAsync(match);
        Debug.Log("Match added with ID: " + matchRef.Id);
    }

    public async void Write(Lobby lobby)
    {
        DocumentReference lobbyRef = db.Collection("Matches").Document(lobby.id.ToString());
        await lobbyRef.SetAsync(lobby);
        Debug.Log("Match added with ID: " + lobbyRef.Id);
    }

    public async void Write(Player player)
    {
        CollectionReference playerRef = db.Collection("Players");
        DocumentReference docRef = await playerRef.AddAsync(player);
        Debug.Log("Player added with ID: " + docRef.Id);
    }



    public async Task<int> GetDocumentCount(string collectionName)
    {
        QuerySnapshot snapshot = await db.Collection(collectionName).GetSnapshotAsync();
        return snapshot.Documents.Count();
    }



    //Ham lay list friends
    public async Task<List<Relationship>> FetchUserRelationShips(string userID)
    {
        if (string.IsNullOrEmpty(userID))
        {
            Debug.Log("Alo, bi khung khong?");
            return null;
        }

        List<Relationship> relationships = new List<Relationship>();
        DocumentReference docRef = FirebaseFirestore.DefaultInstance.Collection("Players").Document(userID);
        QuerySnapshot snapshot = await docRef.Collection("Relationship").GetSnapshotAsync();
        foreach (DocumentSnapshot document in snapshot.Documents)
        {
            Relationship relationship = document.ConvertTo<Relationship>();
            relationships.Add(relationship);
            Debug.Log($"User: {relationship.playerID}, Relationship Type: {relationship.type}");
        }

        return relationships;
    }



    //Ham ket ban
    //public void AddUserRelationship(string userID, string otherUserID, string relationshipType)
    //{
    //    DocumentReference userRef = db.Collection("Users").Document(userID).Collection("Relationships").Document(otherUserID);
    //    Dictionary<string, object> relationship = new Dictionary<string, object>
    //    {
    //        { "Type", relationshipType }
    //    };
    //    userRef.SetAsync(relationship).ContinueWithOnMainThread(task =>
    //    {
    //        if (task.IsCompleted)
    //        {
    //            Debug.Log($"Relationship added successfully for {userID} -> {otherUserID}");
    //        }
    //        else
    //        {
    //            Debug.LogError("Failed to add relationship: " + task.Exception);
    //        }
    //    });

    //    DocumentReference otherUserRef = db.Collection("Users").Document(otherUserID).Collection("Relationships").Document(userID);
    //    Dictionary<string, object> reciprocalRelationship = new Dictionary<string, object>
    //    {
    //        { "Type", relationshipType }
    //    };
    //    otherUserRef.SetAsync(reciprocalRelationship).ContinueWithOnMainThread(task =>
    //    {
    //        if (task.IsCompleted)
    //        {
    //            Debug.Log($"Reciprocal relationship added successfully for {otherUserID} -> {userID}");
    //        }
    //        else
    //        {
    //            Debug.LogError("Failed to add reciprocal relationship: " + task.Exception);
    //        }
    //    });
    //}

    public async void AddUserRelationship(string userID, Relationship relationship)
    {
        CollectionReference coref = db.Collection("Players").Document(userID).Collection("Relationships");
        QuerySnapshot snapshot = await coref.GetSnapshotAsync();
        

        //add ben this user
        DocumentReference userRef = db.Collection("Players").Document(userID).Collection("Relationships").Document($"R{snapshot.Count.ToString()}");
        Task addUserTask = userRef.SetAsync(relationship);


        coref = db.Collection("Players").Document(relationship.playerID).Collection("Relationships");
        QuerySnapshot napshot = await coref.GetSnapshotAsync();
        //add ben the other user
        DocumentReference otherUserRef = db.Collection("Players").Document(relationship.playerID).Collection("Relationships").Document($"R{napshot.Count.ToString()}");
        Relationship reverse_relationship = new Relationship()
        {
            playerID = userID,
            type = relationship.type,
        };
        Task addOtherUserTask = otherUserRef.SetAsync(reverse_relationship);
    }

    //Ham check sign in
    public async Task<string> ReadPassword_ByEmail(string email)
    {
        string pass = string.Empty;
        CollectionReference playersRef = db.Collection("Players");
        Query query = playersRef.WhereEqualTo("email", email);

        // Get the snapshot of the query result
        QuerySnapshot snapshot = await query.GetSnapshotAsync();

        // Check if there's a matching document
        if (snapshot.Count > 0)
        {
            // Get the first document (assuming email is unique)
            DocumentSnapshot document = snapshot.Documents.FirstOrDefault();

            // Deserialize the document data into a Player struct
            Player player = document.ConvertTo<Player>();

            // Get the password from the player struct
            pass = player.password;
            RegisPlayerID(document.Id);
        }
        else
        {
            // Handle case where no player with the specified email is found
            Debug.LogWarning("No player found with email: " + email);
        }

        return pass;
    }

    public void RegisPlayerID(string id)
    {
        this.thisPlayerID = id;
    }

    public async Task<List<Player>> QueryForAllPlayers()
    {
        List<Player> list = new List<Player>();

        CollectionReference collection = db.Collection("Players");
        
        QuerySnapshot snapshot = await collection.GetSnapshotAsync();

        foreach (var item in snapshot.Documents)
        {
            Player player = item.ConvertTo<Player>();
            list.Add(player);
        }
        return list;
    }

}
