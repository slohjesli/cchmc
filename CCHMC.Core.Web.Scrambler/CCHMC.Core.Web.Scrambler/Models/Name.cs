using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCHMC.Core.Web.Scrambler.Models
{
    /// <summary>
    /// Stores the names generated for the ScrambleName attribute.
    /// </summary>
    internal class Name
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public Name () { }

        /// <summary>
        /// Creates a new name from the first, middle, and last names given.
        /// </summary>
        /// <param name="fname">The first name to be used for the name.</param>
        /// <param name="mname">The middle name to be used for the name.</param>
        /// <param name="lname">The last name to be used for the name.</param>
        public Name (string fname, string mname, string lname)
        {
            FirstName = fname;
            MiddleName = mname;
            LastName = lname;
        }

        /// <summary>
        /// The first name associated with the Name.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// The middle name associated with the Name.
        /// </summary>
        public string MiddleName { get; set; }

        /// <summary>
        /// The last name associated with the Name.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// The full name, in the format "First Middle Last"
        /// </summary>
        public string FullName
        {
            get
            {
                return String.Concat(FirstName, " ", MiddleInitial, " ", LastName);
            }
        }

        /// <summary>
        /// The initial of the first name.
        /// </summary>
        public string FirstInitial
        {
            get
            {
                if (String.IsNullOrEmpty(FirstName))
                    return "";
                else
                    return FirstName.Substring(0, 1);
            }
        }

        /// <summary>
        /// The initial of the middle name.
        /// </summary>
        public string MiddleInitial
        {
            get
            {
                if (String.IsNullOrEmpty(MiddleName))
                    return "";
                else
                    return MiddleName.Substring(0, 1);
            }
        }

        /// <summary>
        /// The initial of the last name.
        /// </summary>
        public string LastInitial
        {
            get
            {
                if (String.IsNullOrEmpty(LastName))
                    return "";
                else
                    return LastName.Substring(0, 1);
            }
        }
    }
}
