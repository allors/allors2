namespace Allors.Domain.TestPopulation
{
    using Allors.Domain;
 
    public static partial class PartyExtensions
    {
        public static string DisplayName(this Party @this)
        {
            if (@this.GetType().Name == typeof(Person).Name)
            {
                var person = (Person)@this;
                if (person.ExistFirstName || person.ExistLastName)
                {
                    string name = null;
                    if (person.ExistFirstName)
                    {
                        name = person.FirstName;
                    }

                    if (person.ExistMiddleName)
                    {
                        if (name != null)
                        {
                            name += ' ' + person.MiddleName;
                        }
                        else
                        {
                            name = person.MiddleName;
                        }
                    }

                    if (person.ExistLastName)
                    {
                        if (name != null)
                        {
                            name += ' ' + person.LastName;
                        }
                        else
                        {
                            name = person.LastName;
                        }
                    }

                    return name;
                }

                if (person.ExistUserName)
                {
                    return person.UserName;
                }

                return "N/A";
            }

            if (@this.GetType().Name == typeof(Organisation).Name)
            {
                var organisation = (Organisation)@this;
                return organisation.Name;
            }

            return "N/A";
        }
    }
}
