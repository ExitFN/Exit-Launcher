using System;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Threading.Tasks;
using FortniteLauncher.Pages;
using FortniteLauncher.Services;
using MongoDB.Driver.Core.Authentication;
using FortniteLauncher.Core.Loops;
using Microsoft.UI.Xaml;
using CommunityToolkit.WinUI.Helpers;

namespace FortniteLauncher.MongoDB
{
    public static class MongoDBAuthenticator 
    {
        public static async Task<string> CheckLogin(string email, string password)
        {
            try
            {
                string cng = await API.MongoDBConnectionString();
                // MongoDB connection string
                if (cng == null)
                {
                    return null;
                } else
                {
                string connectionString = cng; 

                // MongoDB client
                var client = new MongoClient(connectionString);

                // Access the database
                var database = client.GetDatabase("Put your backend name here ig i removed mine so people dont hack it");

                // Access the collection
                var collection = database.GetCollection<User>("users");

                //DialogService.CurcleLoading("Hello");
                // Call VerifyLoginAsync to handle login verification
                return await VerifyLoginAsync(collection, email, password);
                }

            }
            catch (Exception ex)
            {
                // Properly handle the exception, don't ignore it.
                DialogService.ShowSimpleDialog("An Error occurred while Authenticating: " + ex.Message, "Error");
                return "Error";
            }
        }
        public static async Task<string> VerifyLoginAsync(IMongoCollection<User> collection, string email, string password)
        {
            try
            {
                // Query the collection to find the user with the given email
                var filter = Builders<User>.Filter.Eq(u => u.email, email);
                var user = await collection.Find(filter).SingleOrDefaultAsync();

                if (user != null)
                {
                    // Check if the user is banned
                    if (user.banned)
                    {
                        return "Banned";
                    }

                    if (user.Reports > 5)
                    {
                        return "Deny";
                    }
                    // Verify the provided password against the stored hashed password using bcrypt
                    bool isPasswordValid = VerifyPassword(password, user.password);

                    if (!isPasswordValid)
                    {
                        return "Invalid";
                    }
                    else
                    {
                        if (user.username == null)
                        {
                            return "Error";
                        }
                        else
                        {
                            Definitions.UserName = user.username;
                            Definitions.Email = email;
                            Definitions.Password = password;
                            MainShellPage.DisplayUsername = user.username.ToString();
                            AccountCheck.CheckAccount();
                            return "Success";
                        }

                    }
                }
                else
                {
                    return "Invalid";
                }
            }
            catch (Exception ex) 
            {
                DialogService.ShowSimpleDialog("An Error occurred while Authenticating: " + ex.Message, "Error");
                return "Error";
            }
        }

        // Function to verify the provided password against the stored hashed password using bcrypt
        public static bool VerifyPassword(string password, string storedHashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, storedHashedPassword);
        }
    }

    // Define a User class that represents the MongoDB document in the "users" collection
    public class User
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public DateTime created { get; set; }
        public bool banned { get; set; }
        public string discordId { get; set; }
        public string accountId { get; set; }
        public string username { get; set; }
        public string username_lower { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public bool mfa { get; set; }
        public string matchmakingId { get; set; }
        public bool canCreateCodes { get; set; }
        public bool isServer { get; set; }
        public bool GivenFullLocker { get; set; }
        public int Reports { get; set; }
        public int __v { get; set; }
    }
}
