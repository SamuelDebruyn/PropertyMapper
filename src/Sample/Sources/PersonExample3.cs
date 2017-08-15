using System.Collections.Generic;

namespace PropertyMapper.Sample.Sources
{
    public class PersonExample3
    {
        public string Gender { get; set; }
        public Name Name { get; set; }
        public List<Location> Locations { get; set; }
        public string Email { get; set; }
        public Login Login { get; set; }
        public string Dob { get; set; }
        public string Registered { get; set; }
        public string Phone { get; set; }
        public string Cell { get; set; }
        public Id Id { get; set; }
        public Picture Picture { get; set; }
        public string Nat { get; set; }
    }
}