using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace SnapphaneScoutDistriktBookingApp.Models
{
    [Table("contacts")]
    public class Contact : BaseModel
    {
        [PrimaryKey("id", false)]
        [Column("id")]
        public int Id { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("email")]
        public string Email { get; set; }
        [Column("phone")]
        public string PhoneNumber { get; set; }
    }
}
