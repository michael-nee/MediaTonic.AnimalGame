using Ardalis.GuardClauses;
using MediaTonic.AnimalGame.API.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaTonic.AnimalGame.API.Models
{
    public class User
    {
        internal User(string name, string userName)
        {
            Guard.Against.NullOrEmpty(name, nameof(name));
            Guard.Against.NullOrEmpty(userName, nameof(userName));

            Id = Guid.NewGuid();
            Name = name;
            UserName = userName;
            CreatedAt = DateTimeOffset.UtcNow;

            UsersList.Add(this);
        }

        public Guid Id { get; private set; }

        public string Name { get; private set; }

        public string UserName { get; private set; }

        public DateTimeOffset CreatedAt { get; private set; }

        private List<Animal> UserAnimalsList = new List<Animal>();

        public IEnumerable<Animal> UserAnimals { get { return UserAnimalsList; } }

        internal bool AddAnimalToUser(Animal animal)
        {
            Guard.Against.Null(animal, nameof(animal));

            if (animal.User == null)
                throw new Exception("animal has not got a user");

            if(animal.User != this)
                throw new Exception("User's don't match.");

            if (UserAnimalsList.Contains(animal)) return false;

            UserAnimalsList.Add(animal);

            return true;
        }

        public static User CreateUser(string name, string userName)
        {
            Guard.Against.NullOrEmpty(name, nameof(name));
            Guard.Against.NullOrEmpty(userName, nameof(userName));

            if (UsersList.Any(c => c.UserName.ToLower() == userName.ToLower()))
                return UsersList.Single(c => c.UserName.ToLower() == userName.ToLower());

            return new User(name, userName);
        }

        public int AnimalCount { get { return UserAnimalsList.Count; } }

        public static IEnumerable<User> Users { get { return UsersList; } }

        private static List<User> UsersList = new List<User>();

        public static int UsersCount { get { return UsersList.Count; } }

        public static User GetUserById(Guid id)
        {
            Guard.Against.Null(id, nameof(id));
            return UsersList.SingleOrDefault(c => c.Id == id);
        }

        public static User GetUserByUserName(string userName)
        {
            Guard.Against.NullOrEmpty(userName, nameof(userName));
            return UsersList.SingleOrDefault(c => c.UserName.ToLower() == userName.ToLower());
        }
    }
}
