using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CCHMC.Core.Web.Scrambler.Demo.Models
{
    public class NestedObject
    {
        public NestedObject Child { get; set; }
        public string Name { get; set; }
        public int ID { get; set; }
        public DateTime Birthdate { get; set; }

        public NestedObject SpawnChild ()
        {
            if (Child == null)
            {
                this.Child = new NestedObject {
                    Name = String.Concat("Child of ", this.Name),//NameHelper.GenerateName(ScrambleNameAttribute.Gender.Random).FullName,
                    ID = this.ID + 1,
                    Birthdate = DateTime.Now
                };
                return this.Child;
            } else
            {
                return null;
            }
        }
    }
}