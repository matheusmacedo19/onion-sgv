namespace Onion.SGV.API.Utils
{
    public static class BusinessDays
    {
        public static DateTime AddBusinessDays(DateTime initialDate, int businessDaysToAdd)
        {
            if (businessDaysToAdd == 0)
                return initialDate;

            int direction = Math.Sign(businessDaysToAdd);

            int totalDaysToAdd = Math.Abs(businessDaysToAdd);
            int addedDays = 0;

            DateTime newDate = initialDate;

            while (addedDays < totalDaysToAdd)
            {
                newDate = newDate.AddDays(direction);

                if (IsBusinessDay(newDate))
                    addedDays++;
            }

            return newDate;
        }

        private static bool IsBusinessDay(DateTime date)
        {
            return date.DayOfWeek switch
            {
                DayOfWeek.Saturday => false,
                DayOfWeek.Sunday => false,
                _ => true,
            };
        }
    }
}
