using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Sources;

namespace CSConsoleApp
{
    internal class ResultPrinter
    {
        private string currentDirectory;
        private string filePath;
        private IReadOnlyList<MovieCredit> movieCredits;

        public ResultPrinter()
        {
            currentDirectory = System.IO.Directory.GetCurrentDirectory();
            filePath = System.IO.Directory.GetFiles(currentDirectory, "*.csv").First();
            try
            {
                movieCredits = new MovieCreditsParser(filePath).Parse();
            } catch (Exception ex) {
                Console.WriteLine("Не удалось распарсить csv");
                Environment.Exit(1);
            }
        }

        public IEnumerable<string> GetSpielbergMovies()
        {
            var StevenSpielbergMovies = from movie in movieCredits
                                        from CrewMember in movie.Crew
                                        where CrewMember.Name.Equals("Steven Spielberg")
                                        select movie;
            return StevenSpielbergMovies.Distinct().Select(m => m.Title);
        }

        public IEnumerable<string> GetTomHanksCharacters()
        {
            var TomHanksCharacters = from movie in movieCredits
                                     from CastMember in movie.Cast
                                     where CastMember.Name.Equals("Tom Hanks")
                                     select CastMember.Character;
            return TomHanksCharacters;
        }

        public IEnumerable<string> GetMostEnormousCast()
        {
            var TopFiveCast = (from movie in movieCredits
                              orderby movie.Cast.Count descending
                              select movie)
                              .Take(5);
            return TopFiveCast.Select(m => $"{m.Title} - {m.Cast.Count}");
        }

        public IEnumerable<string> GetTopTenActors()
        {
            var top10Actors = movieCredits
                                .SelectMany(movie => movie.Cast) 
                                .GroupBy(castMember => castMember.Name) 
                                .Select(group => new
                                {
                                    ActorName = group.Key,
                                    MovieCount = group.Count()
                                })
                                .OrderByDescending(actor => actor.MovieCount) 
                                .Take(10);
            return top10Actors.Select(a => $"{a.ActorName} - {a.MovieCount}");

        }

        public IEnumerable<string> getAllDepartments()
        {
            var AllDepartmants = from movie in movieCredits
                                  from CrewMember in movie.Crew
                                  select CrewMember.Department;
            return AllDepartmants.Distinct();
        }

        public IEnumerable<string> getZimmerComposer()
        {
            var AllMovies = from movie in movieCredits
                            from CrewMember in movie.Crew
                            where CrewMember.Job.Equals("Original Music Composer")
                            where CrewMember.Name.Equals("Hans Zimmer")
                            select movie;
            return AllMovies.Select(m => m.Title);
        }

        public Dictionary<int, string> getDictionary()
        {
            var Directors = movieCredits.ToDictionary(movie => movie.MovieId,
                    movie => movie.Crew.FirstOrDefault(c => c.Job.Equals("Director"))
                        ?.Name ?? "Неизвестен"
                );
            return Directors;
        }

        public IEnumerable<string> getBradAndClooney()
        {
            var AllMovies = from movie in movieCredits
                            let actorNames = movie.Cast.Select(c => c.Name).ToList()
                            where actorNames.Contains("Brad Pitt") && actorNames.Contains("George Clooney")
                            select movie;
            return AllMovies.Select(m => m.Title);
        }

        public int getCameraDepartmentCount()
        {
            var CameraDepartmentCount = (from movie in movieCredits
             from crewMember in movie.Crew
             where crewMember.Department.Equals("Camera")
             select crewMember.Id)
                .Distinct()
                .Count();
            return CameraDepartmentCount;
        }

        public IEnumerable<string> getActorCrew()
        {
            var titanicMembers =
                (from movie in movieCredits
                 where movie.Title.Equals("Titanic")
                 from actor in movie.Cast
                 from crew in movie.Crew
                 where actor.Name.Equals(crew.Name)
                 select actor.Name)
                .Distinct();
            return titanicMembers;
        }

        public IEnumerable<string> getTarantinoFiveCrew()
        {
            var tarantinoCrew =
                (from movie in movieCredits
                 where movie.Crew.Any(c => c.Job.Equals("Director")
                 && c.Name.Equals("Quentin Tarantino"))
                 from CrewMember in movie.Crew
                 where !CrewMember.Name.Equals("Quentin Tarantino")
                 group CrewMember by CrewMember.Name into CrewGroup
                 orderby CrewGroup.Count() descending
                 select new
                 {
                     name = CrewGroup.Key,
                     movieCount = CrewGroup.Count()
                 }).Take(5);
            return tarantinoCrew.Select(m => $"{m.name} - {m.movieCount}").Distinct();
        }

        public IEnumerable<string> getPopularPairs()
        {
            var popularPairs =
                (from movie in movieCredits
                 from a1 in movie.Cast
                 from a2 in movie.Cast
                 where string.Compare(a1.Name, a2.Name) < 0
                 group movie by new { actor1 = a1.Name, actor2 = a2.Name } into pairGroup
                 orderby pairGroup.Count() descending
                 select new
                 {
                     Pair = $"{pairGroup.Key.actor1} & {pairGroup.Key.actor2}",
                     MoviesCount = pairGroup.Count()
                 }).Take(5);
            return popularPairs.Select(m => $"{m.Pair} - {m.MoviesCount}");
        }

        public IEnumerable<string> getIndex()
        {
            var index =
                (from movie in movieCredits
                 from crewMember in movie.Crew
                 group crewMember by crewMember.Name into crewGroup
                 let departmentCount = crewGroup
                     .Select(c => c.Department)
                     .Distinct()
                     .Count()
                 orderby departmentCount descending
                 select new
                 {
                     Name = crewGroup.Key,
                     DepartmentCount = departmentCount
                 })
                .Take(5);
            return index.Select(m => $"{m.Name} - {m.DepartmentCount}");
        }

        public IEnumerable<string> getTrio()
        {
            var trio =
                (from movie in movieCredits
                from crewMember in movie.Crew
                let personJobs = movie.Crew
                    .Where(c => c.Name == crewMember.Name)
                    .Select(c => c.Job)
                    .ToList()
                where personJobs.Contains("Director")
                   && personJobs.Contains("Writer")
                   && personJobs.Contains("Producer")
                select new
                {
                    Movie = movie.Title,
                    Person = crewMember.Name
                }).Distinct();
            return trio.Select(m => $"{m.Movie} - {m.Person}");
        }

        public IEnumerable<string> getKevinTwoSteps()
        {
            var oneStep =
                (from movie in movieCredits
                 where movie.Cast.Any(a => a.Name.Equals("Kevin Bacon"))
                 from actor in movie.Cast
                 where !actor.Name.Equals("Kevin Bacon")
                 select actor.Name)
                .Distinct().ToList();

            var twoSteps =
                (from movie in movieCredits
                 from actor in movie.Cast
                 where movie.Cast.Any(a => oneStep.Contains(a.Name))
                 where !actor.Name.Equals("Kevin Bacon")
                 select actor)
                .Distinct();
            return twoSteps.Select(a => a.Name);
        }
        
        public IEnumerable<string> getTeamWork()
        {
            var teamWork =
                from movie in movieCredits
                from director in movie.Crew
                where director.Job.Equals("Director", StringComparison.OrdinalIgnoreCase)
                group movie by director.Name into directorGroup
                select new
                {
                    Director = directorGroup.Key,
                    AverageCastSize = directorGroup.Average(m => m.Cast.Count),
                    AverageCrewSize = directorGroup.Average(m => m.Crew.Count)
                };
            return teamWork.Select(m => $"Director: {m.Director}   -   AverageCastSize: {m.AverageCastSize}, " +
            $"AverageCrewSize: {m.AverageCrewSize}");
        }

        public IEnumerable<string> getUniversals()
        {
            var actorNames = (from movie in movieCredits
                              from cast in movie.Cast
                              select cast.Name)
                 .Distinct()
                 .ToList();
            var crewNames = (from movie in movieCredits
                             from crew in movie.Crew
                             select crew.Name)
                            .Distinct()
                            .ToList();
            var universals = actorNames.Intersect(crewNames).ToList();

            var universalsTopDepartments =
                from nameUniversal in universals
                let departments =
                    (from movie in movieCredits
                     from crew in movie.Crew
                     where crew.Name.Equals(nameUniversal)
                     select crew.Department)
                let topDepartment = departments
                    .GroupBy(d => d)
                    .OrderByDescending(g => g.Count())
                    .Select(g => g.Key)
                select new { Name = nameUniversal, TopDepartment = topDepartment };
            return universalsTopDepartments.Select(u => $"{u.Name} - {u.TopDepartment}");
        }

        public IEnumerable<string> getMartinAndNolan()
        {
            var MartinCrewCast = (
                 from movie in movieCredits
                 where movie.Crew.Any(c => c.Job.Equals("Director")
                                  && c.Name.Equals("Martin Scorsese"))
                 from cast in movie.Cast
                 select cast.Name).ToList()
                .Concat(
                 from movie in movieCredits
                 where movie.Crew.Any(c => c.Job.Equals("Director")
                                          && c.Name.Equals("Martin Scorsese"))
                 from crew in movie.Crew
                 select crew.Name)
                .Distinct()
                .ToList();

            var NolanCrewCast = (
                from movie in movieCredits
                where movie.Crew.Any(c => c.Job.Equals("Director")
                                 && c.Name.Equals("Christopher Nolan"))
                from cast in movie.Cast
                select cast.Name).ToList()
                .Concat(
                 from movie in movieCredits
                 where movie.Crew.Any(c => c.Job.Equals("Director")
                                          && c.Name.Equals("Christopher Nolan"))
                 from crew in movie.Crew
                 select crew.Name)
                .Distinct()
                .ToList();

            var intersection = MartinCrewCast.Intersect(NolanCrewCast).ToList();

            return intersection;
        }

        public IEnumerable<string> getHiddenImpact()
        {
            var range =
                from tmp in
                    (from movie in movieCredits
                     from crew in movie.Crew
                     select new
                     {
                         Department = crew.Department,
                         CastCount = movie.Cast.Count 
                     })
                group tmp by tmp.Department into deptGroup
                select new
                { 
                    Department = deptGroup.Key,
                    AverageCastSize = deptGroup.Average(x => x.CastCount)
                } 
                into result
                orderby result.AverageCastSize descending
                select result;
            return range.Select(m => $"{m.Department} - AverageCast: {m.AverageCastSize}");
        }

        public IEnumerable<string> getJohnnyArchetypes()
        {
            var JohnnyArchetypes =
                from movie in movieCredits
                from cast in movie.Cast
                where cast.Name.Equals("Johnny Depp")
                let firstWord = cast.Character
                    .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                    .First()
                group cast by firstWord into g
                orderby g.Count() descending
                select new
                {
                    Archetype = g.Key,
                    Count = g.Count()
                };

            return JohnnyArchetypes.Select(m => $"{m.Archetype} - {m.Count}");
        }
    }
}
