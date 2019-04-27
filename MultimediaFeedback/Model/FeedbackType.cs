namespace MultimediaFeedback.Model
{
    public enum FeedbackType
    {
        Happy,
        Sad,
        Idea
    }

    public static class FeedbackTypeExtensions
    {
        public static string ToFriendlyString(this FeedbackType me)
        {
            switch (me)
            {
                case FeedbackType.Happy:
                    return "Alles hat gepasst";
                case FeedbackType.Sad:
                    return "Problem-Meldung";
                case FeedbackType.Idea:
                    return "Ideen-Vorschlag";
                default:
                    return "Feedback-Typ unbekannt";
            }
        }
    }
}
