//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PracticeFiles
{
    using System;
    using System.Collections.Generic;
    
    public partial class File
    {
        public File()
        {
            this.Users = new HashSet<User>();
            this.Purchases = new HashSet<Purchase>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public double Price { get; set; }
    
        public virtual ContentType ContentType { get; set; }
        public virtual FileFormat FileFormat { get; set; }
        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<Purchase> Purchases { get; set; }
    }
}