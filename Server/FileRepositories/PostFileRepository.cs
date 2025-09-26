using RepositoryContracts;
using Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FileRepositories
{
    public class PostFileRepository : IPostRepository
    {
        private readonly string filePath;

        public PostFileRepository()
        {
            filePath = Path.Combine(AppContext.BaseDirectory, "data", "posts.json");
            if (!File.Exists(filePath))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
                File.WriteAllText(filePath, "[]");
            }
        }

        public async Task<Post> AddAsync(Post post)
        {
            var posts = await FileStorageHelper.LoadListAsync<Post>(filePath);
            int maxId = posts.Count > 0 ? posts.Max(p => p.Id) : 0;
            post.Id = maxId + 1;
            posts.Add(post);
            await FileStorageHelper.SaveListAsync(filePath, posts);
            return post;
        }

        public IQueryable<Post> GetMany()
        {
            var posts = FileStorageHelper.LoadListAsync<Post>(filePath).Result;
            return posts.AsQueryable();
        }

        public async Task<Post?> GetByIdAsync(int id)
        {
            var posts = await FileStorageHelper.LoadListAsync<Post>(filePath);
            return posts.FirstOrDefault(p => p.Id == id);
        }

        public async Task<Post?> DeleteAsync(int id)
        {
            var posts = await FileStorageHelper.LoadListAsync<Post>(filePath);
            var existing = posts.FirstOrDefault(p => p.Id == id);
            if (existing == null) return null;
            posts.Remove(existing);
            await FileStorageHelper.SaveListAsync(filePath, posts);
            return existing;
        }

        public async Task<Post?> UpdateAsync(int id, Post updated)
        {
            var posts = await FileStorageHelper.LoadListAsync<Post>(filePath);
            var existing = posts.FirstOrDefault(p => p.Id == id);
            if (existing == null) return null;

            existing.Title = updated.Title;
            existing.Body = updated.Body;
            existing.Author = updated.Author;

            await FileStorageHelper.SaveListAsync(filePath, posts);
            return existing;
        }
    }
}
