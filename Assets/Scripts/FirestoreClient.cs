using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Firestore;
using System.Threading.Tasks;
using System.Linq;
using System;

public class FirestoreClient : MonoBehaviour
{
    public static FirestoreClient fc_instance;
    private static FirebaseFirestore db;
    public string thisPlayerID;
    private List<Relationship> friendlist   = new List<Relationship>();
    public Player thisPlayer;
    public bool playerisLoaded = false;
    public bool friendsisLoaded = false;
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
        db.Settings.PersistenceEnabled = false;
        thisPlayerID = "r18lDv36Rzaynt5jyyp8";
    }


    public async void Write(Match match)
    {
        DocumentReference matchRef = db.Collection("Matches").Document(match.lobbyId.ToString());
        await matchRef.SetAsync(match);
        Debug.Log("Match added with ID: " + matchRef.Id);
    }


    public async void Write(Lobby lobby)
    {
        DocumentReference lobbyRef = db.Collection("Lobbies").Document(lobby.ip.ToString());
        await lobbyRef.SetAsync(lobby);
        Debug.Log("Lobby added with ID: " + lobbyRef.Id);
    }

    public async void Write(Player player)
    {
        CollectionReference playerRef = db.Collection("Players");
        DocumentReference docRef = await playerRef.AddAsync(player);
        Debug.Log("Player added with ID: " + docRef.Id);
    }

    public async void AddMatch(Match match)
    {
        CollectionReference playerRef = db.Collection("Match");
        DocumentReference docRef = await playerRef.AddAsync(match);
        Debug.Log("Match added with ID: " + docRef.Id);
    }


    public async Task<int> GetDocumentCount(string collectionName)
    {
        QuerySnapshot snapshot = await db.Collection(collectionName).GetSnapshotAsync();
        return snapshot.Documents.Count();
    }

    public void Reload()
    {
        friendsisLoaded = false;
        FetchUserRelationShips();
    }


    //Ham lay list friends
    public async Task<List<Relationship>> FetchUserRelationShips(string userID)
    {
        if (string.IsNullOrEmpty(userID) || friendsisLoaded)
        {
            Debug.Log("Loaded friendlist");
            return friendlist;
        }

        List<Relationship> relationships = new List<Relationship>();
        DocumentReference docRef = FirebaseFirestore.DefaultInstance.Collection("Players").Document(userID);
        QuerySnapshot snapshot = await docRef.Collection("Relationships").GetSnapshotAsync();
        foreach (DocumentSnapshot document in snapshot.Documents)
        {
            Relationship relationship = document.ConvertTo<Relationship>();
            relationships.Add(relationship);
            //Debug.Log($"User: {relationship.playerID}, Relationship Type: {relationship.type}");
        }
        friendsisLoaded = true;
        Debug.Log("Friendlist updated");
        friendlist = relationships;
        return relationships;
    }

    //Ham update list friend
    public async void FetchUserRelationShips()
    {
        if (string.IsNullOrEmpty(thisPlayerID) || friendsisLoaded)
        {
            Debug.Log("Alo, bi khung khong?");
            return;
        }

        List<Relationship> relationships = new List<Relationship>();
        DocumentReference docRef = FirebaseFirestore.DefaultInstance.Collection("Players").Document(thisPlayerID);
        QuerySnapshot snapshot = await docRef.Collection("Relationships").GetSnapshotAsync();
        foreach (DocumentSnapshot document in snapshot.Documents)
        {
            Relationship relationship = document.ConvertTo<Relationship>();
            relationships.Add(relationship);
            //Debug.Log($"User: {relationship.playerID}, Relationship Type: {relationship.type}");
        }
        friendsisLoaded = true;
        Debug.Log("Friendlist updated");
        friendlist = relationships;
        return;
    }


    //ham ket ban

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
            username = thisPlayer.username,
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
            RegisPlayerID(document.Id, player);
        }
        else
        {
            // Handle case where no player with the specified email is found
            Debug.LogWarning("No player found with email: " + email);
        }

        return pass;
    }

    public async Task<string> ReadUsernameByID(string id)
    {
        string username = null;
        DocumentReference docRef = db.Collection("Players").Document(id);
        DocumentSnapshot doc = await docRef.GetSnapshotAsync();

        Player player = doc.ConvertTo<Player>();

        username = player.username;
        
        return username;
    }

    public async Task<bool> IsEmailExists(string email)
    {
        if (db == null)
        {
            Debug.LogError("Firestore is not initialized.");
            return false;

        }

        CollectionReference playersRef = db.Collection("Players");
        Query query = playersRef.WhereEqualTo("email", email);

        // Get the snapshot of the query result
        QuerySnapshot snapshot = await query.GetSnapshotAsync();

        // Check if there's a matching document
        return snapshot.Count > 0;
    }

    public async Task<bool> IsUsernameExists(string username)
    {
        CollectionReference playersRef = db.Collection("Players");
        Query query = playersRef.WhereEqualTo("username", username);

        // Get the snapshot of the query result
        QuerySnapshot snapshot = await query.GetSnapshotAsync();

        // Check if there's a matching document
        return snapshot.Count > 0;
    }
    public void RegisPlayerID(string id, Player player)
    {
        this.thisPlayerID = id;
        playerisLoaded = true;
        thisPlayer = player;
    }


    //Ham tim kiem ban be khong dieu kien
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

    public async Task<Player> GetPlayer(string playerID)
    {
        Player player = new Player();
        DocumentReference doc = db.Collection("Players").Document(playerID);
        DocumentSnapshot snapshot = await doc.GetSnapshotAsync();
        
        if(snapshot.Exists)
        {
            player = snapshot.ConvertTo<Player>();

        }

        return player;
    }

    public async Task<string> GetPlayerID(string username)
    {
        string playerId = null;
        
        Query query = db.Collection("Players").WhereEqualTo("username", username);
        QuerySnapshot snapshot = await query.GetSnapshotAsync();

        if (snapshot.Count > 0)
        {
            // Get the first document (assuming username is unique)
            DocumentSnapshot document = snapshot.FirstOrDefault();

            // Get the document ID
            playerId = document.Id;
        }
        else
        {
            Debug.LogWarning("No player found with username: " + username);
        }
        return playerId;
    }

    public async Task<string> GetPlayerID_byMail(string email)
    {
        string playerId = null;

        Query query = db.Collection("Players").WhereEqualTo("email", email);
        QuerySnapshot snapshot = await query.GetSnapshotAsync();

        if (snapshot.Count > 0)
        {
            // Get the first document (assuming username is unique)
            DocumentSnapshot document = snapshot.FirstOrDefault();

            // Get the document ID
            playerId = document.Id;
        }
        else
        {
            Debug.LogWarning("No player found with email: " + email);
        }
        return playerId;

    }


    //Ham xem ho so thang ban trong list friend
  
    public async Task<Player> FindPlayer_byName(string username)
    {
        Player player = new Player
        {
            username = null,
        };

        CollectionReference col = db.Collection("Players");

        Query query = col.WhereEqualTo("username", username);

        QuerySnapshot snapshots = await query.GetSnapshotAsync();

        DocumentSnapshot document = snapshots.FirstOrDefault();

        if (document != null)
        {
            player = document.ConvertTo<Player>();

        }

        return player;
    }

    public async void SendRequest(string username)
    {
        string toID = await GetPlayerID(username);

        CollectionReference colRef = db.Collection("Requests");

        Request request = new Request
        {
            from = FirestoreClient.fc_instance.thisPlayerID,
            to = toID,
            accepted = false,
        };

        await colRef.AddAsync(request);
    }

    //Lay ve list request, check xem accepted co true hay khong de tao object
    public async Task<List<Request>> RetrieveAllRequests()
    {
        List<Request> requests = new List<Request>();

        CollectionReference colRef = db.Collection("Requests"); 

        Query query = colRef.WhereEqualTo("to", FirestoreClient.fc_instance.thisPlayerID);

        QuerySnapshot snapshot = await query.GetSnapshotAsync();

        foreach (DocumentSnapshot doc in snapshot.Documents)
        {
            Request request = doc.ConvertTo<Request>();
            requests.Add(request);
        }

        return requests;
    }

    public async void Accept (string username)
    {
        string from = await  GetPlayerID(username);

        Relationship relationship = new Relationship
        {
            playerID = from,
            type = "friend",
            username = username
        };

        AddUserRelationship(thisPlayerID, relationship);

        Query query = db.Collection("Requests").WhereEqualTo("to", FirestoreClient.fc_instance.thisPlayerID).WhereEqualTo("from", from);

        QuerySnapshot snapshot = await query.GetSnapshotAsync();

        DocumentSnapshot doc = snapshot.FirstOrDefault();

        await doc.Reference.DeleteAsync();
        Debug.Log("Deleted document with ID: " + doc.Id);

    }

    public async void Reject (string username)
    {
        string from = await GetPlayerID(username);
        Query query = db.Collection("Requests").WhereEqualTo("to", FirestoreClient.fc_instance.thisPlayerID).WhereEqualTo("from", from);

        QuerySnapshot snapshot = await query.GetSnapshotAsync();

        DocumentSnapshot doc = snapshot.FirstOrDefault();

        await doc.Reference.DeleteAsync();
        Debug.Log("Deleted document with ID: " + doc.Id);
    }

    public bool IsFriended(string username)
    {
        foreach (Relationship relationship in friendlist)
        {
            if(relationship.username == username)
            {
                return true;
            }
        }
        return false;
    }

    public async Task<bool> Requested(string username)
    {
        string to = await GetPlayerID(username);
        Query query = db.Collection("Requests")
                        .WhereEqualTo("from", FirestoreClient.fc_instance.thisPlayerID)
                        .WhereEqualTo("to", to);

        QuerySnapshot snapshot = await query.GetSnapshotAsync();
        if (snapshot.Documents.Count() > 0)
        {
            return true;
        }

        return false;
    }

    public async void ChangePass(string userID, string newpass)
    {
        // Reference to the Firestore document for the specific user
        DocumentReference docRef = db.Collection("Players").Document(userID);

        // Create a dictionary with the field to update
        Dictionary<string, object> updates = new Dictionary<string, object>
        {
            { "password", newpass }
        };

        // Update the document
        await docRef.UpdateAsync(updates);
    }

    public async Task<string> GetLoobyIP(string foodid)
    {
        CollectionReference colRef = db.Collection("Lobbies");
        Query query = colRef.WhereEqualTo("foodid", foodid).WhereEqualTo("isactive", true);
        QuerySnapshot snapshot = await query.GetSnapshotAsync();

        if (snapshot.Documents.Count() > 0)
        {
            Lobby lobbyfound  = snapshot.Documents.FirstOrDefault().ConvertTo<Lobby>();
            return lobbyfound.ip;
        }
        return null;
    }

    public async void ChangeLobbyState(string serverip, bool state)
    {
        // Reference to the Firestore document for the specific lobby
        DocumentReference docRef = db.Collection("Lobbies").Document(serverip);

        // Create a dictionary with the field to update
        Dictionary<string, object> updates = new Dictionary<string, object>
        {
            { "isactive", state}
        };

        // Update the document
        await docRef.UpdateAsync(updates);
    }

    public async void UpdateDaySignIn()
    {
        // Reference to the Firestore document for the specific lobby
        DocumentReference docRef = db.Collection("Players").Document(thisPlayerID);

        // Create a dictionary with the field to update
        Dictionary<string, object> updates = new Dictionary<string, object>
        {
            { "LastSignIn", Timestamp.FromDateTime(DateTime.UtcNow)}
        };

        // Update the document
        await docRef.UpdateAsync(updates);
    }

    public async void UpdateExp(int exp)
    {
        // Reference to the Firestore document for the specific lobby
        DocumentReference docRef = db.Collection("Players").Document(thisPlayerID);

        // Create a dictionary with the field to update
        Dictionary<string, object> updates = new Dictionary<string, object>
        {
             { "exp", FieldValue.Increment(exp) }
        };

        // Update the document
        await docRef.UpdateAsync(updates);
    }

    public static void SortPlayersByExp(List<Player> players)
    {
        players.Sort((player1, player2) => player2.exp.CompareTo(player1.exp));
    }
    public static void SortPlayersByLastSignIn(List<Player> players)
    {
        players.Sort((player1, player2) => player2.LastSignIn.CompareTo(player1.LastSignIn));
    }

    public async Task<List<Player>> GetFriendsOrderByExp()
    {
        List<Player> friends = new List<Player>();
        foreach(Relationship relationship in friendlist)
        {
            Player player = await GetPlayer(relationship.playerID);
            friends.Add(player);
        }
        friends.Add(thisPlayer);
        SortPlayersByExp(friends);
        return friends;
    }
    public async Task<List<Player>> GetFriendsOrderByLastSignIn()
    {
        List<Player> friends = new List<Player>();
        foreach (Relationship relationship in friendlist)
        {
            Player player = await GetPlayer(relationship.playerID);
            friends.Add(player);
        }
        friends.Add(thisPlayer);
        SortPlayersByExp(friends);
        return friends;
    }
    public async void UpdateDayCreateLobby(string serverip)
    {
        // Reference to the Firestore document for the specific lobby
        DocumentReference docRef = db.Collection("Lobbies").Document(serverip);

        // Create a dictionary with the field to update
        Dictionary<string, object> updates = new Dictionary<string, object>
        {
            { "Date_Lobby", Timestamp.FromDateTime(DateTime.UtcNow)}
        };

        // Update the document
        await docRef.UpdateAsync(updates);
    }
}
