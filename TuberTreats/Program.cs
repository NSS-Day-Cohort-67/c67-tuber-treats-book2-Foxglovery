using TuberTreats.Models;
using TuberTreats.Models.DTOs;

List<Customer> customers = new List<Customer>()
{
    new Customer()
    {
        Id = 1,
        Name = "Euphegenia Doubtfire",
        Address = "6404 Madaline Station",
        
    },
    new Customer()
    {
        Id = 2,
        Name = "Groogery Adlerburb",
        Address = "286 Reta Summit"
    },
    new Customer()
    {
        Id = 3,
        Name = "Jaspin Fortchock",
        Address = "7930 Elmo Drive"
    },
    new Customer()
    {
        Id = 4,
        Name = "Cinnabar Calabrast",
        Address = "2973 Walsh Wall"
    },
    new Customer()
    {
        Id = 5,
        Name = "Speve Doorblat",
        Address = "5583 Soledad Circle"
    },
};

List<TuberDriver> tuberDrivers = new List<TuberDriver>()
{
    new TuberDriver()
    {
        Id = 1,
        Name = "Jeef Dagsroll"
    },
    new TuberDriver()
    {
        Id = 2,
        Name = "Marmoseth Camphor"
    },
    new TuberDriver()
    {
        Id = 3,
        Name = "Leticious Juniper"
    },
};

List<Topping> toppings = new List<Topping>()
{
    new Topping()
    {
        Id = 1,
        Name = "Crawfish Boudin"
    },
    new Topping()
    {
        Id = 2,
        Name = "Hunty Nuggets"
    },
    new Topping()
    {
        Id = 3,
        Name = "Salmon Chicharons"
    },
    new Topping()
    {
        Id = 4,
        Name = "Dollar General Tso Tofurkey"
    },
    new Topping()
    {
        Id = 5,
        Name = "Hatch Pepper Oreo Crumbles"
    }
};

List<TuberOrder> tuberOrders = new List<TuberOrder>()
{
    new TuberOrder()
    {
        Id = 1,
        OrderPlacedOnDate = new DateTime(2023, 12, 10, 14, 02, 30),
        CustomerId = 2,
        TuberDriverId = 1,
        DeliveredOnDate = new DateTime(2023, 12, 10, 14, 43, 48)
    },
    new TuberOrder()
    {
        Id = 2,
        OrderPlacedOnDate = new DateTime(2023, 12, 11, 12, 00, 36),
        CustomerId = 1,
        TuberDriverId = 2,
        DeliveredOnDate = new DateTime(2023, 12, 11, 12, 23, 22)
    },
    new TuberOrder()
    {
        Id = 3,
        OrderPlacedOnDate = new DateTime(2023, 12, 11, 17, 02, 19),
        CustomerId = 4,
        TuberDriverId = 3,
        DeliveredOnDate = new DateTime(2023, 12, 11, 17, 57, 43)
    },
};

List<TuberTopping> tuberToppings = new List<TuberTopping>()
{
    new TuberTopping()
    {
        Id = 1,
        TuberOrderId = 1,
        ToppingId = 2
    },
    new TuberTopping()
    {
        Id = 2,
        TuberOrderId = 1,
        ToppingId = 5
    },
    new TuberTopping()
    {
        Id = 3,
        TuberOrderId = 2,
        ToppingId = 3
    },
    new TuberTopping()
    {
        Id = 4,
        TuberOrderId = 2,
        ToppingId = 1
    },
    new TuberTopping()
    {
        Id = 5,
        TuberOrderId = 3,
        ToppingId = 4
    },
    new TuberTopping()
    {
        Id = 6,
        TuberOrderId = 3,
        ToppingId = 2
    }
};


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapGet("/api/toppings", () =>
{
    return toppings.Select(t => new ToppingDTO
    {
        Id = t.Id,
        Name = t.Name
    });
});

app.MapGet("/api/toppings/{toppingId}", (int toppingId) =>
{
    Topping topping = toppings.FirstOrDefault(t => t.Id == toppingId);
    if (topping == null)
    {
        return Results.BadRequest();
    }
    return Results.Ok(topping);
});

app.MapGet("/api/tuberOrders", () =>
{
    
    return tuberOrders.Select(to => new TuberOrderDTO
    {
        Id = to.Id,
        OrderPlacedOnDate = to.OrderPlacedOnDate,
        CustomerId = to.CustomerId,
        TuberDriverId = to.TuberDriverId,
        DeliveredOnDate = to.DeliveredOnDate,
        Toppings = to.Toppings
    });
});

app.MapGet("/api/tuberOrders/{id}", (int id) =>
{   //How to fill toppings property
    TuberOrder tuberOrder = tuberOrders.FirstOrDefault(to => to.Id == id);
    
    
    return Results.Ok(tuberOrder);
});

app.Run();
//don't touch or move this!
public partial class Program { }