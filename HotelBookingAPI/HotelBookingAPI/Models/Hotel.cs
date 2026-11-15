namespace HotelBookingAPI.Models
{
    public class Hotel {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public double Rating { get; set; }  

        public Hotel (int id, string name, string address, string description, double rating) {
            (Id, Name, Address, Description, Rating) = (id, name, address, description, rating);
        }

        public Hotel(CreateHotelRequest request)
        {
            (Name, Address, Description, Rating) = (request.Name, request.Address, request.Description, request.Rating);
        }
        public Hotel (int id, CreateHotelRequest request) {
            (Id, Name, Address, Description, Rating) = (id, request.Name, request.Address, request.Description, request.Rating);
        }
    }
}
