using FAQsApp.Models;

namespace FAQsApp.Data;

public static class AppDbSeeder
{
    public static void Seed(AppDbContext db)
    {
        if (db.Topics.Any() || db.Categories.Any() || db.Faqs.Any()) return;

        var topics = new[]
        {
            new Topic { TopicId = "js",   Name = "JavaScript" },
            new Topic { TopicId = "mvc",  Name = "ASP.NET MVC" },
            new Topic { TopicId = "sql",  Name = "SQL" },
        };

        var categories = new[]
        {
            new Category { CategoryId = "hist", Name = "History" },
            new Category { CategoryId = "how",  Name = "How-To" },
            new Category { CategoryId = "gen",  Name = "General" },
        };

        db.Topics.AddRange(topics);
        db.Categories.AddRange(categories);
        db.SaveChanges();

        var faqs = new[]
        {
            new Faq {
                Question = "What is JavaScript?",
                Answer   = "A versatile programming language primarily used in web development.",
                TopicId = "js", CategoryId = "gen"
            },
            new Faq {
                Question = "How do I add a click handler in JS?",
                Answer   = "Use addEventListener: element.addEventListener('click', fn).",
                TopicId = "js", CategoryId = "how"
            },
            new Faq {
                Question = "What is ASP.NET Core MVC?",
                Answer   = "A framework for building web apps using the Model-View-Controller pattern.",
                TopicId = "mvc", CategoryId = "gen"
            },
            new Faq {
                Question = "When did MVC become part of ASP.NET Core?",
                Answer   = "It’s been part of ASP.NET Core since its initial releases that unified MVC and Web API.",
                TopicId = "mvc", CategoryId = "hist"
            },
            new Faq {
                Question = "How do I write a SELECT query?",
                Answer   = "Basic form: SELECT column_list FROM table WHERE predicate;",
                TopicId = "sql", CategoryId = "how"
            },
        };

        db.Faqs.AddRange(faqs);
        db.SaveChanges();
    }
}
