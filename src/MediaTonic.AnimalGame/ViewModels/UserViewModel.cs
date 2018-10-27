using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaTonic.AnimalGame.API.ViewModels
{
    public class UserViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string UserName { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public int AnimalCount { get; set; }

        public IEnumerable<AnimalViewModel> Animals { get; set; }
    }
}
