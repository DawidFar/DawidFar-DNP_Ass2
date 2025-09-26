using RepositoryContracts;
using Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FileRepositories
{
    public class UserFileRepository : IUserRepository
    {
        private readonly string filePath;

        public UserFileRepository()
        {
            filePath = Path.Combine(AppContext.BaseDirectory, "data", "users.json");
            if (!File.Exists(filePath))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
                File.WriteAllText(filePath, "[]");
            }
        }

        public async Task<User> AddAsync(User user)
        {
            var users = await FileStorageHelper.LoadListAsync<User>(filePath);
            int maxId = users.Count > 0 ? users.Max(u => u.Id) : 0;
            user.Id = maxId + 1;
            users.Add(user);
            await FileStorageHelper.SaveListAsync(filePath, users);
            return user;
        }

        public IQueryable<User> GetMany()
        {
            var users = FileStorageHelper.LoadListAsync<User>(filePath).Result;
            return users.AsQueryable();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            var users = await FileStorageHelper.LoadListAsync<User>(filePath);
            return users.FirstOrDefault(u => u.Id == id);
        }

        public async Task<User?> DeleteAsync(int id)
        {
            var users = await FileStorageHelper.LoadListAsync<User>(filePath);
            var existing = users.FirstOrDefault(u => u.Id == id);
            if (existing == null) return null;
            users.Remove(existing);
            await FileStorageHelper.SaveListAsync(filePath, users);
            return existing;
        }

        public async Task<User?> UpdateAsync(int id, User updated)
        {
            var users = await FileStorageHelper.LoadListAsync<User>(filePath);
            var existing = users.FirstOrDefault(u => u.Id == id);
            if (existing == null) return null;

            existing.Username = updated.Username;
            existing.Password = updated.Password;

            await FileStorageHelper.SaveListAsync(filePath, users);
            return existing;
        }
    }
}
