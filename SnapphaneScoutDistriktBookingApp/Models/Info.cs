using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapphaneScoutDistriktBookingApp.Models
{
    public class Info
    {
        [BsonId]
        public string Id { get; set; }
        public string InfoString { get; set; }
    }
}
