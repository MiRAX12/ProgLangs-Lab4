using System.Runtime.InteropServices;

namespace CSConsoleApp
{
    public static class Program
    {

        public static void Main()
        {
            ResultPrinter resultPrinter = new ResultPrinter();

            PrintResult("Все фильмы, снятые Стивеном Спилбергом:", resultPrinter.GetSpielbergMovies());
            PrintResult("Персонажи, которых сыграл актер Tom Hanks.", resultPrinter.GetTomHanksCharacters());
            PrintResult("5 фильмов с самым большим количеством актеров", resultPrinter.GetMostEnormousCast());
            PrintResult("Топ-10 самых востребованных актеров", resultPrinter.GetTopTenActors());
            PrintResult("Список всех уникальных департаментов", resultPrinter.getAllDepartments());
            PrintResult("Все фильмы, где Hans Zimmer был композитором", resultPrinter.getZimmerComposer());
            PrintDictionary("Создать словарь, где ключ — ID фильма, а значение — имя режиссера.", resultPrinter.getDictionary());
            PrintResult("Найти фильмы, где в актерском составе есть и \"Brad Pitt\", и \"George Clooney\".", resultPrinter.getBradAndClooney());
            resultPrinter.getCameraDepartmentCount();
            PrintResult("Найти всех людей, которые в фильме \"Titanic\" были одновременно и в съемочной группе, и в списке актеров.",
                resultPrinter.getActorCrew());
            PrintResult("Найти \"внутренний круг\" режиссера: Для режиссера \"Quentin Tarantino\" найти топ-5 членов съемочной группы (не актеров)," +
                " которые работали с ним над наибольшим количеством фильмов.\r\n",
                resultPrinter.getTarantinoFiveCrew());
            PrintResult("Определить экранные \"дуэты\": Найти 10 пар актеров, которые чаще всего снимались вместе в одних и тех же фильмах.",
                resultPrinter.getPopularPairs());
            PrintResult("Вычислить \"индекс разнообразия\" для карьеры: Найти 5 членов съемочной группы, " +
                "которые поработали в наибольшем количестве различных департаментов за свою карьеру.", resultPrinter.getIndex());
            PrintResult("Найти \"творческие трио\": Найти фильмы, где один и тот же человек выполнял роли режиссера" +
                " (Director), сценариста (Writer) и продюсера (Producer).", resultPrinter.getTrio());
            PrintResult("Два шага до Кевина Бейкона: Найти всех актеров, которые снимались в одном фильме с актером," +
                " который, в свою очередь, снимался в одном фильме с \"Kevin Bacon\".", resultPrinter.getKevinTwoSteps());
            PrintResult("Проанализировать \"командную работу\": Сгруппировать фильмы по режиссеру и для каждого" +
                " из них найти средний размер как актерского состава (Cast), так и съемочной группы (Crew).", resultPrinter.getTeamWork());
            PrintResult("Найти пересечение \"элитных клубов\": Найти людей, которые работали и" +
                " с режиссером \"Martin Scorsese\", и с режиссером \"Christopher Nolan\".", resultPrinter.getMartinAndNolan());
            PrintResult("Выявить \"скрытое влияние\": Ранжировать все департаменты по" +
                " среднему количеству актеров в тех фильмах, над которыми они работали" +
                " (чтобы проверить, коррелирует ли работа определенного департамента с масштабом актерского состава).",
                resultPrinter.getHiddenImpact());
            PrintResult("Проанализировать \"архетипы\" персонажей:" +
                " Для актера \"Johnny Depp\" сгруппировать его роли по первому слову" +
                " в имени персонажа (например, \"Captain\", \"Jack\", \"Willy\")" +
                " и посчитать частоту каждого такого \"архетипа\".", resultPrinter.getJohnnyArchetypes());
        }

        private static void PrintResult(string title, IEnumerable<string> result)
        {
            Console.Clear();
            Console.WriteLine(title);
            Console.WriteLine(string.Join(Environment.NewLine, result));
            Console.WriteLine("\nНажмите любую клавишу чтобы продолжить");
            Console.ReadKey();
        }

        private static void PrintDictionary(string title, Dictionary<int, string> dictionary)
        {
            Console.WriteLine("Словарь на " + dictionary.Count + " элементов создан. Желаете просмотреть его?\ny/n");
            var key = Console.ReadKey(false).Key;
            if (key == ConsoleKey.Y) {
                foreach (var director in dictionary)
                {
                    Console.WriteLine(director.Key + "Режисер: " + director.Value);
                }
            }
            Console.WriteLine("\nНажмите любую клавишу чтобы продолжить");
            Console.ReadKey();
            return;
            
        }
    }
}