using Firebase.Auth;
using Firebase.Auth.Providers;
using Firebase.Database;
using Firebase.Database.Query;
using odev3gorsel.Models;

namespace odev3gorsel.Services
{
    public class FirebaseService
    {
        // ✅ بيانات مشروع Firebase الجديد الخاص بك
        private const string WebApiKey = "AIzaSyBglt8JusbvLuvpw9qV7XdYM5FaTCbBwtM";
        private const string FirebaseUrl = "https://odev3gorsel-default-rtdb.europe-west1.firebasedatabase.app/";
        private const string AuthDomain = "odev3gorsel.firebaseapp.com";

        private readonly FirebaseAuthClient _authClient;
        public readonly FirebaseClient _dbClient;

        public FirebaseService()
        {
            var config = new FirebaseAuthConfig
            {
                ApiKey = WebApiKey,
                AuthDomain = AuthDomain,
                Providers = new FirebaseAuthProvider[]
                {
                    new EmailProvider()
                }
            };

            _authClient = new FirebaseAuthClient(config);
            _dbClient = new FirebaseClient(FirebaseUrl);
        }

        public async Task<UserCredential> RegisterUser(string email, string password)
        {
            return await _authClient.CreateUserWithEmailAndPasswordAsync(email, password);
        }

        public async Task<UserCredential> LoginUser(string email, string password)
        {
            return await _authClient.SignInWithEmailAndPasswordAsync(email, password);
        }

        // ============ TODO CRUD İŞLEMLERİ ============

        // Tüm görevleri getir
        public async Task<List<TodoItem>> GetAllTodos()
        {
            var items = await _dbClient
                .Child("todos")
                .OnceAsync<TodoItem>();

            return items.Select(item => 
            {
                var todo = item.Object;
                todo.Id = item.Key;
                return todo;
            }).ToList();
        }

        // Yeni görev ekle
        public async Task<string> AddTodo(TodoItem item)
        {
            var result = await _dbClient
                .Child("todos")
                .PostAsync(item);

            return result.Key;
        }

        // Görev güncelle
        public async Task UpdateTodo(TodoItem item)
        {
            await _dbClient
                .Child("todos")
                .Child(item.Id)
                .PutAsync(item);
        }

        // Görev sil
        public async Task DeleteTodo(string id)
        {
            await _dbClient
                .Child("todos")
                .Child(id)
                .DeleteAsync();
        }
    }
}