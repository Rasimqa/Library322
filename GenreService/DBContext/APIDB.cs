using Library.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library.DBContext
{
    public class APIDB : DbContext
    {
        public APIDB(DbContextOptions options) : base(options) 
        {
        
       
        }
        public DbSet<Genre> Genre { get; set; }


    }
}
