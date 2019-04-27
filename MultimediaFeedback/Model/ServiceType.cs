namespace MultimediaFeedback.Model
{
    public enum ServiceType
    {
        Morning1,
        Morning2,
        MICC,
        Evening,
        Special
    }

    public static class ServiceTypeExtensions
    {
        public static string ToFriendlyString(this ServiceType me)
        {
            switch (me)
            {
                case ServiceType.Morning1:
                    return "M-1-Godi";
                case ServiceType.Morning2:
                    return "M-2-Godi";
                case ServiceType.MICC:
                    return "MICC";
                case ServiceType.Evening:
                    return "A-Godi";
                case ServiceType.Special:
                    return "Sonderevent";
                default:
                    return "unbekannt";
            }
        }
    }
}
