namespace Onion.SGV.API.Utils
{
    public static class BusinessDays
    {
        public static DateTime AddBusinessDays(DateTime initialDate, int businessDaysToAdd)
        {
            if (businessDaysToAdd == 0)
                return initialDate;

            DateTime newDate = initialDate;
            int addedDays = 0;

            while (addedDays < businessDaysToAdd)
            {
                newDate = newDate.AddDays(1);
                if (IsBusinessDay(newDate))
                    addedDays++;
            }
            return newDate;
        }

        private static bool IsBusinessDay(DateTime date)
        {
            if (date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday)
                return true;

            return false;
        }
    }
}
