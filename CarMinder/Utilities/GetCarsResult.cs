using CarMinder.Mobile;

namespace CarMinder.Utilities
{
    public class GetCarsResult
    {
        public bool Success { get; set; }
        public List<MobileAppCarModel> CarsList { get; set; }

        public GetCarsResult (bool success, List<MobileAppCarModel> cars_list)
        {
            Success = success;
            CarsList = cars_list;
        }
    }
}
