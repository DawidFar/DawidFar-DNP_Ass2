using RepositoryContracts;
using Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FileRepositories
{
    public class CommentFileRepository : ICommentRepository
    {
        private readonly string filePath;

        public CommentFileRepository()
        {
            filePath = Path.Combine(AppContext.BaseDirectory, "data", "comments.json");
            if (!File.Exists(filePath))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
                File.WriteAllText(filePath, "[]");
            }
        }

        public async Task<Comment> AddAsync(Comment comment)
        {
            var comments = await FileStorageHelper.LoadListAsync<Comment>(filePath);
            int maxId = comments.Count > 0 ? comments.Max(c => c.Id) : 0;
            comment.Id = maxId + 1;
            comments.Add(comment);
            await FileStorageHelper.SaveListAsync(filePath, comments);
            return comment;
        }

        public IQueryable<Comment> GetMany()
        {
            var comments = FileStorageHelper.LoadListAsync<Comment>(filePath).Result;
            return comments.AsQueryable();
        }

        public async Task<Comment?> GetByIdAsync(int id)
        {
            var comments = await FileStorageHelper.LoadListAsync<Comment>(filePath);
            return comments.FirstOrDefault(c => c.Id == id);
        }

        public async Task<Comment?> DeleteAsync(int id)
        {
            var comments = await FileStorageHelper.LoadListAsync<Comment>(filePath);
            var existing = comments.FirstOrDefault(c => c.Id == id);
            if (existing == null) return null;
            comments.Remove(existing);
            await FileStorageHelper.SaveListAsync(filePath, comments);
            return existing;
        }

        public async Task<Comment?> UpdateAsync(int id, Comment updated)
        {
            var comments = await FileStorageHelper.LoadListAsync<Comment>(filePath);
            var existing = comments.FirstOrDefault(c => c.Id == id);
            if (existing == null) return null;

            existing.Author = updated.Author;
            existing.Content = updated.Content;
            existing.PostId = updated.PostId;

            await FileStorageHelper.SaveListAsync(filePath, comments);
            return existing;
        }
    }
}
