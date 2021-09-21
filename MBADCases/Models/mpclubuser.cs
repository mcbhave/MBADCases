using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MBADCases.Models
{
    public class mpclubuser
    {
        public mpclubuserResponse response { get; set; }
    }
    public class mpclubuserBirthLocationGeographicAddress
    {
        public string address { get; set; }
        public double lat { get; set; }
        public double lng { get; set; }
    }

    public class mpclubuserResult
    {
        public bool active_boolean { get; set; }
        public string astro_sign_text { get; set; }
        public mpclubuserBirthLocationGeographicAddress birth_location_geographic_address { get; set; }
        public DateTime birthday_date { get; set; }
        public string delivery_method_text { get; set; }
        public string email_text { get; set; }
        public string enneagram_number_text { get; set; }
        public int life_path_number_number { get; set; }
        public string myers_briggs_type_text { get; set; }
        public string phone_number_text { get; set; }
        public string subscription_text { get; set; }

        [JsonProperty("Created Date")]
        public DateTime CreatedDate { get; set; }

        [JsonProperty("Created By")]
        public string CreatedBy { get; set; }

        [JsonProperty("Modified Date")]
        public DateTime ModifiedDate { get; set; }
        public string current_location_text { get; set; }
        public string timezone_text { get; set; }
        public string usertype_text { get; set; }
        public string first_text { get; set; }
        public string last_text { get; set; }
        public string _id { get; set; }
    }

    public class mpclubuserResponse
    {
        public int cursor { get; set; }
        public List<mpclubuserResult> results { get; set; }
        public int remaining { get; set; }
        public int count { get; set; }
    }

 
}
