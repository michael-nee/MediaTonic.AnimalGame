using Microsoft.AspNetCore.Mvc.ActionConstraints;

namespace MediaTonic.AnimalGame.API.Attributes
{
    internal class RequiredFromQueryActionConstraint : IActionConstraintMetadata
    {
        private string v;

        public RequiredFromQueryActionConstraint(string v)
        {
            this.v = v;
        }
    }
}