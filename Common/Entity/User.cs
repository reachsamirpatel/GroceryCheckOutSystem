using System;
using System.Collections;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using GroceryCheckOut.Entity.Enums;


namespace GroceryCheckOut.Entity
{
    [DataContract]
    [Serializable, XmlRoot("Users")]
    public class User
    {
        public User()
        {

        }

        public User(string firstName, string lastName, string email, UserTypeEnum userType)
        {
            UserId = Guid.NewGuid();
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            UserType = userType;
        }
        public User(Guid userId, string firstName, string lastName, string email, UserTypeEnum userType)
        {
            UserId = userId;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            UserType = userType;
        }
        [DataMember]
        public UserTypeEnum UserType { get; set; }

        [DataMember]
        public Guid UserId { get; set; }

        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public string LastName { get; set; }

        [DataMember]
        public string Email { get; set; }
    }
}
