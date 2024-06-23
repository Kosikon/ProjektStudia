using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User> Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return null;

            var user = await _context.Users.SingleOrDefaultAsync(x => x.Username == username);

            if (user == null)
                return null;

            if (!VerifyPasswordHash(password, user.PasswordHash))
                return null;

            return user;
        }

        public async Task<User> Create(User user, string password)
        {
            // Validation
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password is required");

            if (_context.Users.Any(x => x.Username == user.Username))
                throw new ArgumentException($"Username \"{user.Username}\" is already taken");

            byte[] passwordHash;
            CreatePasswordHash(password, out passwordHash);

            user.PasswordHash = Convert.ToBase64String(passwordHash);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task Delete(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetById(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task Update(User userParam, string password = null)
        {
            var user = await _context.Users.FindAsync(userParam.Id);

            if (user == null)
                throw new ArgumentException("User not found");

            if (!string.IsNullOrWhiteSpace(userParam.Username) && userParam.Username != user.Username)
            {
                if (_context.Users.Any(x => x.Username == userParam.Username))
                    throw new ArgumentException($"Username \"{userParam.Username}\" is already taken");

                user.Username = userParam.Username;
            }

            if (!string.IsNullOrWhiteSpace(userParam.Role))
                user.Role = userParam.Role;

            if (!string.IsNullOrWhiteSpace(password))
            {
                byte[] passwordHash;
                CreatePasswordHash(password, out passwordHash);

                user.PasswordHash = Convert.ToBase64String(passwordHash);
            }

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new HMACSHA512())
            {
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        private static bool VerifyPasswordHash(string password, string storedHash)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (string.IsNullOrWhiteSpace(storedHash)) throw new ArgumentException("Invalid stored hash.", "storedHash");

            var passwordHash = Convert.FromBase64String(storedHash);
            using (var hmac = new HMACSHA512())
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
    }
}
