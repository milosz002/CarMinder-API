using CarMinder.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Car
{
    [Key]
    public int Id { get; set; }
    public string UserId { get; set; }



    public int car_local_id { get; set; }
    public string model_id { get; set; }
    public string title { get; set; }
    public string model_year { get; set; }
    public string model_name { get; set; }
    public string make_display { get; set; }
    public string model_body { get; set; }
    public string model_engine_position { get; set; }
    public string model_engine_cc { get; set; }
    public string model_engine_type { get; set; }
    public string model_engine_power_ps { get; set; }
    public string model_top_speed_kph { get; set; }
    public string model_drive { get; set; }
    public string model_transmission_type { get; set; }
    public string model_seats { get; set; }
    public string model_weight_kg { get; set; }
    public string make_country { get; set; }
    public string model_lkm_hwy { get; set; }
    public string model_lkm_mixed { get; set; }
    public string model_lkm_city { get; set; }
    public string model_fuel_cap_l { get; set; }
    public string car_image_url { get; set; }
    public string vehicle_technical_inspection_deadline { get; set; }
    public string vehicle_technical_inspection_deadline_notification_id { get; set; }
}
