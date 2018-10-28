using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using MediaTonic.AnimalGame.API.Enums;

namespace MediaTonic.AnimalGame.API.Models
{
    public class Animal
    {
        Animal(AnimalType type, string name, User user, int petRate = 5, int hungryRate = 5)
        {
            Guard.Against.Null(type, nameof(type));
            Guard.Against.NullOrEmpty(name, nameof(type));
            Guard.Against.Null(user, nameof(user));

            Guard.Against.OutOfRange(petRate, nameof(petRate), 1, 10);
            Guard.Against.OutOfRange(hungryRate, nameof(hungryRate), 1, 10);

            Id = Guid.NewGuid();
            Name = name;
            AnimalType = type;
            CreatedAt = DateTimeOffset.Now;
            LastFed = DateTimeOffset.Now;
            LastPetted = DateTimeOffset.Now;
            HungryRate = hungryRate;
            PetRate = petRate;
            User = user;

            _currentHappiness = 50;
            _previousHungriness = 50;

            AnimalList.Add(this);

            user.AddAnimalToUser(this);
        }

        public Guid Id { get; private set; } = Guid.NewGuid();

        public string Name { get; private set; }

        public DateTimeOffset LastPetted { get; private set; }

        public DateTimeOffset LastFed { get; private set; }

        public DateTimeOffset CreatedAt { get; private set; } = DateTimeOffset.UtcNow;

        private int _minsBetweenFeed { get { return (DateTimeOffset.UtcNow - LastFed).Minutes; } }

        private int _minsBetweenPetting { get { return (DateTimeOffset.UtcNow - LastPetted).Minutes; } }

        public decimal CurrentHungriness()
        {
            var hungryAmount = (_minsBetweenFeed / 60m) * HungryRate;
            if (hungryAmount == 0) return _currentHungriness;

            _currentHungriness += hungryAmount;

            return decimal.Round(_currentHungriness, 2, MidpointRounding.AwayFromZero);
        }

        private decimal _previousHungriness;

        private decimal _previousHapiness;

        private decimal _currentHungriness
        {
            get
            {
                return _previousHungriness;
            }
            set
            {
                if (value > 100) _previousHungriness = 100;
                else if (value < 0) _previousHungriness = 0;
                else _previousHungriness = value;
            }
        }

        //How often the hungry rate increases for each feed
        private decimal HungryRate { get; set; }

        //Ensure Value stays between 0 and 100 (boxing)
        private decimal _currentHappiness
        {
            get
            {
                return _previousHapiness;
            }
            set
            {
                if (value > 100) _previousHapiness = 100;
                else if (value < 0) _previousHapiness = 0;
                else _previousHapiness = value;
            }
        }

        public decimal CurrentHappiness()
        {
            decimal happinessAmount = (_minsBetweenPetting / 60m) * PetRate;
            if (happinessAmount == 0) return _currentHappiness;

            //deduct happiness
            _currentHappiness-= happinessAmount;

            return decimal.Round(_currentHappiness, 2, MidpointRounding.AwayFromZero);
        }

        private decimal PetRate { get; set; }

        public bool Alive { get; private set; } = true;

        public decimal FeedAnimal()
        {
            if (User == null)
                throw new Exception("Can't feed animal until it has an owner");

            //Update Hungriness between last fed and now
            CurrentHungriness();

            //Make Animal Less Hungry
            _currentHungriness -= HungryRate;

            //Set LastFedTimestamp
            LastFed = DateTimeOffset.UtcNow;

            //return CurrentHungriness
            return CurrentHungriness();
        }

        public decimal PetAnimal()
        {
            if (User == null)
                throw new Exception("Can't pet animal until it has an owner");

            CurrentHappiness();
            //Could put logic in so animal is not feed
            _currentHappiness+= PetRate;

            LastPetted = DateTimeOffset.UtcNow;

            return CurrentHappiness();
        }

        public static Animal GetAnimal(Guid id)
        {
            Guard.Against.Null(id, nameof(id));

            if (AnimalList.Any(c => c.Id == id))
                return AnimalList.SingleOrDefault(c => c.Id == id);

            return null;
        }

        public static Animal GetAnimal(Guid id, Guid userId)
        {
            Guard.Against.Null(id, nameof(id));
            Guard.Against.Null(userId, nameof(userId));

            if (AnimalList.Any(c => c.Id == id && c.User.Id == userId))
                return AnimalList.SingleOrDefault(c => c.Id == id && c.User.Id == userId);

            return null;
        }

        public static Animal GetAnimal(Guid id, string userName)
        {
            Guard.Against.Null(id, nameof(id));
            Guard.Against.NullOrEmpty(userName, nameof(userName));

            if (AnimalList.Any(c => c.Id == id && c.User.UserName.ToLower() == userName.ToLower()))
                return AnimalList.SingleOrDefault(c => c.Id == id && c.User.UserName.ToLower() == userName.ToLower());

            return null;
        }

        public User User { get; private set; }

        public AnimalType AnimalType { get; private set; }

        private static List<Animal> AnimalList = new List<Animal>();

        public static IEnumerable<Animal> Animals { get { return AnimalList; } }

        public static Animal CreateAnimal(AnimalType type, string name, User user)
        {
            Guard.Against.Null(type, nameof(type));
            Guard.Against.NullOrEmpty(name, nameof(name));
            Guard.Against.Null(user, nameof(user));

            switch (type)
            {
                case AnimalType.Cat:
                    return new Animal(type, name, user, 2, 4);
                case AnimalType.Dog:
                    return new Animal(type, name, user, 4, 6);
                case AnimalType.GuineaPig:
                    return new Animal(type, name, user, 6, 8);
                case AnimalType.Rabbit:
                    return new Animal(type, name, user, 3, 10);
                case AnimalType.Tortoise:
                    return new Animal(type, name, user, 1, 1);
                default:
                    throw new InvalidOperationException("Must require an animal type");
            }
        }
    }
}