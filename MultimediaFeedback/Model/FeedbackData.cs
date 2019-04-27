using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultimediaFeedback.Model
{
    public class FeedbackData
    {
        public DateTime         Date;
        public ServiceType      TypeOfService;
        public FeedbackType     TypeOfFeedback;

        public string           Selection;
        public string           Name;
        public string           Mail = "";
        public string           Comment;
        public string           Attachment = "";

        public FeedbackData()
        {
            Date = DateTime.Now;

            // Set type of Service according to Date and time
            if (Date.DayOfWeek != DayOfWeek.Sunday)
            {
                TypeOfService = ServiceType.Special;
            }
            else
            {
                if (Date.Hour <= 11) { TypeOfService = ServiceType.Morning1; }
                else if (Date.Hour <= 13) { TypeOfService = ServiceType.Morning2; }
                else if (Date.Hour <= 16) { TypeOfService = ServiceType.MICC; }
                else { TypeOfService = ServiceType.Evening; }
            }
        }

        /// <param name="sep">Separation character</param>
        public string HeaderString(string sep = ";")
        {
            return $"{nameof(Date)}{sep}{nameof(TypeOfService)}{sep}{nameof(TypeOfService)}_String{sep}{nameof(TypeOfFeedback)}{sep}{nameof(TypeOfFeedback)}_String{sep}{nameof(Selection)}_String{sep}{nameof(Name)}{sep}{nameof(Mail)}{sep}{nameof(Comment)}";
        }

        public override string ToString()
        {
            string sep = ";";
            return $"{Date.ToString()}{sep}{(int)TypeOfService}{sep}{TypeOfService.ToFriendlyString()}{sep}{(int)TypeOfFeedback}{sep}{TypeOfFeedback.ToFriendlyString()}{sep}{Selection}{sep}{Name}{sep}{Mail}{sep}{Comment}";
        }
    }
}
