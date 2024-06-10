﻿namespace GRA.Domain.Model.Filters
{
    public class UserFilter : BaseFilter
    {
        public UserFilter(int? page = null) : base(page)
        {
            SortBy = SortUsersBy.LastName;
        }

        public bool CanAddToHousehold { get; set; }
        public bool? HasMultiplePrimaryVendorCodes { get; set; }
        public bool? IsSubscribed { get; set; }
        public bool OrderDescending { get; set; }
        public SortUsersBy SortBy { get; set; }
    }
}
