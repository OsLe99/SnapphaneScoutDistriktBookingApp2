using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapphaneScoutDistriktBookingApp.Models
{
    [Table("infos")]
    public class Info : BaseModel
    {
        [PrimaryKey("id", false)]
        [Column("id")]
        public Guid Id { get; set; }
        [Column("info_string")]
        public string? InfoString { get; set; }
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
