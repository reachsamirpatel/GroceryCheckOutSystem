using System;
using System.Collections;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using GroceryCheckOut.Entity.Enums;


namespace GroceryCheckOut.Entity
{
    /// <summary>
    /// Login entity for login related functions
    /// </summary>
    [DataContract]
    [Serializable, XmlRoot("Logins")]
    public class Login
    {
        public Login()
        {

        }
        public Login(string loginName, string password, Guid userId)
        {
            LoginName = loginName;
            Password = password;
            UserId = userId;
        }
        [DataMember]
        public Guid UserId { get; set; }
        [DataMember]
        public string LoginName { get; set; }
        [DataMember]
        public string Password { get; set; }
    }

}
