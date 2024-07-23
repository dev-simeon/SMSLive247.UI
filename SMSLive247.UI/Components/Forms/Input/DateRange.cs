using System.ComponentModel.DataAnnotations;

namespace SMSLive247.UI.Components.Forms.Input
{
    public enum DateRangeFilter
    {
        TODAY,
        YESTERDAY,
        LAST_3_DAYS,
        LAST_7_DAYS,
        LAST_14_DAYS,
        LAST_30_DAYS,
        LAST_90_DAYS,
        THIS_MONTH,
        LAST_MONTH,
        CUSTOM
    }

    public class DateRange : IValidatableObject
    {
        private DateRangeFilter _filter = DateRangeFilter.CUSTOM;
        private DateTimeOffset? _startDate;
        private DateTimeOffset? _endDate;

        public DateTimeOffset? StartDate
        {
            get
            {
                SetDateRange();
                return _startDate;
            }
            set
            {
                _startDate = value;
            }
        }

        public DateTimeOffset? EndDate
        {
            get
            {
                SetDateRange();
                return _endDate;
            }
            set
            {
                _endDate = value;
            }
        }

        public DateRangeFilter Filter
        {
            get
            {
                return _filter;
            }
            set
            {
                _filter = value;
                SetDateRange();
            }
        }

        private void SetDateRange()
        {
            switch (_filter)
            {
                case DateRangeFilter.TODAY:
                    _startDate = DateTimeOffset.UtcNow.Date;
                    _endDate = _startDate.Value.AddDays(1).AddTicks(-1);
                    break;
                case DateRangeFilter.YESTERDAY:
                    _startDate = DateTimeOffset.UtcNow.Date.AddDays(-1);
                    _endDate = _startDate.Value.AddDays(1).AddTicks(-1);
                    break;
                case DateRangeFilter.LAST_3_DAYS:
                    _startDate = DateTimeOffset.UtcNow.Date.AddDays(-3);
                    _endDate = DateTimeOffset.UtcNow.Date.AddDays(1).AddTicks(-1);
                    break;
                case DateRangeFilter.LAST_7_DAYS:
                    _startDate = DateTimeOffset.UtcNow.Date.AddDays(-7);
                    _endDate = DateTimeOffset.UtcNow.Date.AddDays(1).AddTicks(-1);
                    break;
                case DateRangeFilter.LAST_14_DAYS:
                    _startDate = DateTimeOffset.UtcNow.Date.AddDays(-14);
                    _endDate = DateTimeOffset.UtcNow.Date.AddDays(1).AddTicks(-1);
                    break;
                case DateRangeFilter.LAST_30_DAYS:
                    _startDate = DateTimeOffset.UtcNow.Date.AddDays(-30);
                    _endDate = DateTimeOffset.UtcNow.Date.AddDays(1).AddTicks(-1);
                    break;
                case DateRangeFilter.LAST_90_DAYS:
                    _startDate = DateTimeOffset.UtcNow.Date.AddDays(-90);
                    _endDate = DateTimeOffset.UtcNow.Date.AddDays(1).AddTicks(-1);
                    break;
                case DateRangeFilter.THIS_MONTH:
                    _startDate = new DateTimeOffset(DateTime.UtcNow.Year,
                        DateTime.UtcNow.Month, 1, 0, 0, 0, TimeSpan.Zero);
                    _endDate = _startDate.Value.AddMonths(1).AddTicks(-1);
                    break;
                case DateRangeFilter.LAST_MONTH:
                    _startDate = new DateTimeOffset(DateTime.UtcNow.Year,
                        DateTime.UtcNow.Month, 1, 0, 0, 0, TimeSpan.Zero).AddMonths(-1);
                    _endDate = _startDate.Value.AddMonths(1).AddTicks(-1);
                    break;
                case DateRangeFilter.CUSTOM:
                    //if (_startDate.HasValue && _endDate.HasValue)
                    //{
                    //    // Ensure end of day for _endDate
                    //    _endDate = _endDate.Value.Date.AddDays(1).AddTicks(-1);
                    //}
                    break;
                default:
                    //_startDate = null;
                    //_endDate = null;
                    break;
            }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (_startDate.HasValue && _endDate.HasValue)
            {
                if (_startDate > _endDate)
                {
                    yield return new ValidationResult("End Date must be later than or equal to Start Date",
                        [nameof(StartDate), nameof(EndDate)]);
                }
            }
        }

    }
}
