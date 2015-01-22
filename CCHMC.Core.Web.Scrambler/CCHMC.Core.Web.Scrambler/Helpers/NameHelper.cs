using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CCHMC.Core.Web.Scrambler.Attributes;
using CCHMC.Core.Web.Scrambler.Models;

namespace CCHMC.Core.Web.Scrambler.Helpers
{
    /// <summary>
    /// A helper to create names.
    /// </summary>
    internal static class NameHelper
    {
        //Female Names
        /// <summary>
        /// A list of female first names.
        /// </summary>
        public static readonly List<string> FemaleFNames = new List<string> { "Abby", "Brooke", "Caddy", "Dawn", "Emily", "Fran", "Greta", "Hannah", "Ina", 
                                                               "Jane", "Karen", "Lucy", "Mary", "Nancy", "Olivia", "Paige", "Quintin", "Rose", 
                                                               "Steph", "Tricia", "Ursula", "Vera", "Willow", "Xena", "Yvette", "Zelda", };
        /// <summary>
        /// A list of female middle names.
        /// </summary>
        public static readonly List<string> FemaleMNames = new List<string> { "Anne", "Beth", "Cassandra", "Destiny", "Elizabeth", "Fanny", "Grace", "Hera", "Ivy", 
                                                               "Jasmine", "Karissa", "Lynn", "Marissa", "Naomi", "Ophelia", "Palla", "Quail", "Rachel", 
                                                               "Samantha", "Tabatha", "Ume", "Valentine", "Wanda", "Xue", "Yin", "Zoey", };
        //Male Names
        /// <summary>
        /// A list of male first names.
        /// </summary>
        public static readonly List<string> MaleFNames = new List<string> { "Alec", "Barack", "Carlton", "David", "Edgar", "Fred", "George", "Hamlet", "Ishmael", 
                                                             "Jaime", "Kyle", "Linus", "Marshall", "Ned", "Oscar", "Paul", "Quincy", "Robert", 
                                                             "Steve", "Tom", "Ulysses", "Vincent", "Will", "Xander", "Yoshi", "Zachary" };
        /// <summary>
        /// A list of male middle names.
        /// </summary>
        public static readonly List<string> MaleMNames = new List<string> { "Allen", "Basil", "Cameron", "Daniel", "Ellis", "Franklin", "Gregory", "Horatio", "Imhotep", 
                                                             "Jacob", "Kane", "Lloyd", "Manfred", "Nathaniel", "Orson", "Paris", "Quinto", "Ronald", 
                                                             "Stuart", "Timothy", "Umbra", "Victor", "Waldo", "Xavier", "Yeheshua", "Zeus" };


        /// <summary>
        /// A list of last names.
        /// </summary>
        public static readonly List<string> LNames = new List<string> { "Austen", "Atreides", "Baratheon", "Baggins", "Capulet", "Compton", "Darwin", "DaVinci",
                                                        "Eisenhower", "Evans", "Flanders", "Faulkner", "Goethe", "Giles", "Herbert", "House", "Ireland", 
                                                        "Irving", "Jackson", "Johnson", "Kierkegaard", "Kennedy", "Lannister", "Luo", "Montague", 
                                                        "Mozart", "Nixon", "Nietzsche", "Obama", "Orwell", "Picard", "Poe", 
                                                        "Quigley", "Quixote", "Riker", "Rosenberg", "Shakespeare", "Stark", "Targaryen", "Troi", 
                                                        "Underwood", "Urkel", "Virgil", "Voltaire", "Wilde", "Wilson", "Xu", "Xiong", 
                                                        "Yi", "York", "Ziegler", "Zoidberg" };

        /// <summary>
        /// Inserts a name into a string with formatting indicating the positions of the first, middle, and/or last names or initials.
        /// </summary>
        /// <param name="format">The format to follow and into which to insert the name.</param>
        /// <param name="name">The name to be inserted into the format.</param>
        /// <returns>A String matching the format but with {F}, {M}, {L}, {FI}, {MI}, and {LI} replaced by first name, middle name, last name, first initial, middle initial, and last initial respectively.</returns>
        public static string Format (string format, Name name)
        {
            IDictionary<string, string> map = new Dictionary<string, string>(){
                { "{F}", name.FirstName },
                { "{M}", name.MiddleName },
                { "{L}", name.LastName },
                { "{FI}", name.FirstInitial },
                { "{MI}", name.MiddleInitial },
                { "{LI}", name.LastInitial }
            };
            var regex = new Regex(String.Join("|", map.Keys));
            format = regex.Replace(format, m => map[m.Value]);//, RegexOptions.IgnoreCase);

            return format;
        }

        /// <summary>
        /// Creates a random name of either gender, chosen at random.
        /// </summary>
        /// <returns>A Name object of either gender.</returns>
        public static Name GenerateName()
        {
            return GenerateName(ScrambleNameAttribute.Gender.Random);
        }

        /// <summary>
        /// Creates a name at random with the specified gender.
        /// </summary>
        /// <param name="gender">Male, Female, or Random, indicating which gender should be used for the name.</param>
        /// <returns>A Name with first and middle names matching the gender and a last name.</returns>
        public static Name GenerateName (ScrambleNameAttribute.Gender gender)
        {
            Name name = new Name();

            if (gender == ScrambleNameAttribute.Gender.Female || (gender == ScrambleNameAttribute.Gender.Random && RandomHelper.Random.Next(0, 2) == 0))
                name = GenerateRandomName(FemaleFNames, FemaleMNames);
            else
                name = GenerateRandomName(MaleFNames, MaleMNames);

            return name;
        }

        /// <summary>
        /// Helper to create a random name using the given lists of first and last names.
        /// </summary>
        /// <param name="fnames">The list of names to use to select a first name.</param>
        /// <param name="mnames">The list of names to use to select a middle name.</param>
        /// <returns>A Name with first and middle names from the given lists and a last name from the LNames list.</returns>
        private static Name GenerateRandomName(List<string> fnames, List<string> mnames)
        {
            return new Name()
            {
                FirstName = fnames[RandomHelper.Random.Next(fnames.Count)],
                MiddleName = mnames[RandomHelper.Random.Next(mnames.Count())],
                LastName = LNames[RandomHelper.Random.Next(LNames.Count())]
            };
        }
    }
}
