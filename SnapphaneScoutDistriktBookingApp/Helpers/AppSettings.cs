using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapphaneScoutDistriktBookingApp.Helpers
{
    public class AppSettings
    {
        public string CLERK_API_KEY { get; set; }
        public string SENDGRID_API_KEY { get; set; }
        public string SENDGRID_EMAIL { get; set; }
        public string SUPABASE_KEY { get; set; }
        public string SUPABASE_URL { get; set; }
    }
}
