//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Project.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Friend
    {
        public int Friend_Id { get; set; }
        public string FriendName { get; set; }
        public Nullable<int> UserFriend_Id { get; set; }
        public Nullable<int> User_Id { get; set; }
    
        public virtual User User { get; set; }
        public virtual User User1 { get; set; }
    }
}
