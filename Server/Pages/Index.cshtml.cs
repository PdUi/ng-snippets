namespace NgSnippets.Pages
{
    using Bogus;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Models;
    using System;

    public class IndexModel : PageModel
    {
        public User DisplayUser { get; set; }

        public void OnGet()
        {
            DisplayUser = new Faker<User>()
                            .RuleFor(u => u.Id, (f) => f.Random.Number(1, Int32.MaxValue))
                            .RuleFor(u => u.FirstName, (f) => f.Name.FirstName())
                            .RuleFor(u => u.LastName, (f) => f.Name.LastName());
        }
    }
}
