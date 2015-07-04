namespace IdentityTutorial.Web.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNet.Mvc.ModelBinding;

    public class ErrorsViewModel
    {
        public IList<string> Errors { get; set; }

        public bool HasErrors => Errors.Count > 0;

        public void Add(string error)
        {
            Errors.Add(error);
        }

        public void AddModelStateErrors(ModelStateDictionary modelState)
        {
            foreach (var modelStateErrors in modelState.Select(x => x.Value.Errors))
            {
                foreach (var error in modelStateErrors)
                {
                    Errors.Add(error.ErrorMessage);
                }
            }
        }

        public ErrorsViewModel()
        {
            Errors = new List<string>();
        }
    }
}