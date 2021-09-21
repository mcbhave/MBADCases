using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MBADCases.Models
{
    public class mpclubmessage
    {
        public MpclubmessageResponse response { get; set; } 
    }

        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
        public class mpclubmessageResult
        {
            [JsonProperty("Created Date")]
            public DateTime CreatedDate { get; set; }

            [JsonProperty("Created By")]
            public string CreatedBy { get; set; }

            [JsonProperty("Modified Date")]
            public DateTime ModifiedDate { get; set; }
            public DateTime publish_date { get; set; }
            public string message_text { get; set; }
            public string send_to_all_paid_text { get; set; }
            public string msg_type_text { get; set; }
        public string mb_type_text { get; set; }
        
            public string _id { get; set; }
            public string subscription_type_text { get; set; }
        public string enneagram_text { get; set; }
        
        public string yardillo_location { get; set; }
    }

        public class MpclubmessageResponse
    {
            public int cursor { get; set; }
            public List<mpclubmessageResult> results { get; set; }
            public int remaining { get; set; }
            public int count { get; set; }
        }

      


     
}
