using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace SnapphaneScoutDistriktBookingApp.Models
{
    public class Admin
    {
        public ObjectId Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHashed { get; set; }
    }
}
