using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine;
using Firebase.Firestore;
using Firebase.Extensions;

public class CheckMail : MonoBehaviour
{
    private FirebaseFirestore db;

    private void Awake()
    {
        InitializeFirebase();
    }

    private void InitializeFirebase()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                // Firebase is ready to use
                db = FirebaseFirestore.DefaultInstance;
            }
            else
            {
                Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyStatus}");
            }
        });

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



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
