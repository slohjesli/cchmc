using ExternalLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScramblerPresentation.Models
{
    public class InternalClass
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public DateTime TimeAdded { get; set; }
    }

    public static class InternalHelper
    {
        public static List<InternalClass> AllThePeople = new List<InternalClass>()
        {
            new InternalClass() {
                Id = 1,
                Name = "Barack Obama",
                TimeAdded = new DateTime(2009, 1, 20)
            },
            new InternalClass() {
                Id = 2,
                Name = "George W. Bush",
                TimeAdded = new DateTime(2001, 1, 20)
            },
            new InternalClass() {
                Id = 3,
                Name = "Bill Clinton",
                TimeAdded = new DateTime(1993, 1, 20)
            },
            new InternalClass() {
                Id = 4,
                Name = "George H. W. Bush",
                TimeAdded = new DateTime(1989, 1, 20)
            },
            new InternalClass () {
                Id = 5,
                Name = "Ronald Reagan",
                TimeAdded = new DateTime(1981, 1, 20)
            }
        };

        public static List<InternalClass> AllTheOtherPeople = new List<InternalClass>()
        {
            new InternalClass() {
                Id = 11,
                Name = "Joe Biden",
                TimeAdded = new DateTime(2009, 1, 20)
            },
            new InternalClass() {
                Id = 12,
                Name = "Dick Cheney",
                TimeAdded = new DateTime(2001, 1, 20)
            },
            new InternalClass() {
                Id = 13,
                Name = "Al Gore",
                TimeAdded = new DateTime(1993, 1, 20)
            },
            new InternalClass() {
                Id = 14,
                Name = "Dan Quayle",
                TimeAdded = new DateTime(1989, 1, 20)
            },
            new InternalClass () {
                Id = 15,
                Name = "George H. W. Bush",
                TimeAdded = new DateTime(1981, 1, 20)
            }
        };

        public static List<ExternalClass> Externals = new List<ExternalClass>()
        {
            new ExternalClass() {
                Initial  ="Alpha",
                Secondary="Beta"
            },
            new ExternalClass() {
                Initial  ="Airplane",
                Secondary="Baggage"
            },
            new ExternalClass() {
                Initial  ="Achilles",
                Secondary="Bilbo"
            },
            new ExternalClass() {
                Initial  ="Armadillo",
                Secondary="Bamboo"
            },
            new ExternalClass() {
                Initial  ="After",
                Secondary="Before"
            },
            new ExternalClass() {
                Initial  ="Avacado",
                Secondary="Bacon"
            },
            new ExternalClass() {
                Initial  ="Asiago",
                Secondary="Brie"
            },
            new ExternalClass() {
                Initial  ="Achey",
                Secondary="Breakey"
            },
            new ExternalClass() {
                Initial  ="Armistice",
                Secondary="Battle"
            },
            new ExternalClass() {
                Initial  ="A",
                Secondary="B"
            }
        };

        public static IndexClass Index = new IndexClass()
        {
            Name = "Information",
            Internals = AllThePeople,
            Alts = AllTheOtherPeople,
            Externals = Externals
        };

    }
}