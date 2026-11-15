using HotelBookingAPI.Models;
using HotelBookingAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<HotelDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            
            var app = builder.Build();

            app.MapGet("/", () => "Welcome to Bunny Hotel!");
            app.MapGet("/hotels", async (HotelDbContext db) => {
                var hotels = await db.Hotels.ToListAsync();
                return Results.Ok(hotels);
            });

            app.MapGet("/hotels/{id:int}", async (int id, HotelDbContext db) => {
                var hotel = await db.Hotels.FindAsync(id);
                if (hotel == null) return Results.NotFound();
                return Results.Ok(hotel);
            });

            app.MapPost("/hotels", async (CreateHotelRequest request, HotelDbContext db) =>
            {
                if (string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.Address) ||
                    string.IsNullOrWhiteSpace(request.Description))
                {
                    return Results.BadRequest();
                }

                if (request.Rating < 0 || request.Rating > 5)
                {
                    return Results.BadRequest("Rating must be in range 0 - 5 !");
                }

                Hotel newHotel = new Hotel(request);
                db.Hotels.Add(newHotel);
                await db.SaveChangesAsync();
                return Results.Created($"/hotels/{newHotel.Id}", newHotel);
            });

            app.MapPut("/hotels/{id:int}", async (int id, CreateHotelRequest request, HotelDbContext db) => {
                var hotel = await db.Hotels.FindAsync(id);

                if (hotel == null) {
                    return Results.NotFound();
                } 

                if (string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.Address) ||
                    string.IsNullOrWhiteSpace(request.Description))
                {
                    return Results.BadRequest();
                }
                else if (request.Rating < 0 || request.Rating > 5)
                {
                    return Results.BadRequest("Rating must be in range 0 - 5 !");
                }

                hotel.Name = request.Name;
                hotel.Description = request.Description;
                hotel.Address = request.Address;
                hotel.Rating = request.Rating;
                await db.SaveChangesAsync();
                return Results.Ok(hotel);
            });

            app.MapDelete("/hotels/{id:int}", async (int id, HotelDbContext db) => {
                var hotel = await db.Hotels.FindAsync(id);

                if (hotel == null) return Results.NotFound();

                db.Hotels.Remove(hotel);
                await db.SaveChangesAsync();
                return Results.NoContent();
            });
            app.Run();
        }
    }
}

/*
 $body = @{
    name = "Moonlight Hotel"
    address = "Pattaya, Thailand"
    description = "A relaxing beachfront hotel."
    rating = 4.6
} | ConvertTo-Json

Invoke-RestMethod `
    -Uri "https://localhost:7031/hotels" `
    -Method POST `
    -ContentType "application/json" `
    -Body $body
 */