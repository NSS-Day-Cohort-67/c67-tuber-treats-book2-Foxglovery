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
        DeliveredOnDate = null
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
{   // grab specific order
    TuberOrder tuberOrder = tuberOrders.FirstOrDefault(to => to.Id == id);
    //make a list of toppings for that order
    List<TuberTopping> toppingEntriesForOrder = tuberToppings.Where(tt => tt.TuberOrderId == id).ToList();
    //make new list by "mapping" over the topping entries and then for each one finding the first matching topping
    List<Topping> toppingsForOrder = toppingEntriesForOrder.Select(te => toppings.First(top => top.Id == te.ToppingId)).ToList();
    //add toppings to order entry
    tuberOrder.Toppings = toppingsForOrder;
    if (tuberOrder == null)
    {
        return Results.BadRequest();
    }
    return Results.Ok(tuberOrder);
});

app.MapPost("/api/tuberOrders", (TuberOrder order) =>
{
    //set new id for item
    order.Id = tuberOrders.Max(to => to.Id) + 1;
    tuberOrders.Add(order);
    return Results.Created($"/api/tuberOrders/{order.Id}", new TuberOrderDTO
    {
        Id = order.Id,
        OrderPlacedOnDate = DateTime.Now,
        CustomerId = order.CustomerId,
        TuberDriverId = order.TuberDriverId,
        DeliveredOnDate = order.DeliveredOnDate,
        Toppings = order.Toppings

    });
});

app.MapPost("/api/tuberToppings", (int orderId, TuberTopping topping) =>
{
    topping.Id = tuberToppings.Max(tt => tt .Id) + 1;
    topping.TuberOrderId = orderId;
    tuberToppings.Add(topping);
    return Results.Created($"/api/tuberToppings/{topping.Id}", new TuberToppingDTO
    {
        Id = topping.Id,
        TuberOrderId = orderId,
        ToppingId = topping.ToppingId
    });
});

app.MapPut("/api/tuberOrders/{id}", (int id, TuberDriver driver) =>
{
    TuberOrder orderToAssign = tuberOrders.FirstOrDefault(to => to.Id == id);
    if (orderToAssign == null)
    {
        return Results.BadRequest();
    }
    orderToAssign.TuberDriverId = driver.Id ;
    return Results.NoContent();
});
//the /complete has nothing to do with what is happening, it only lets the api know when to run this code
app.MapPost("/api/tuberOrders/{id}/complete", (int id) => 
{
    TuberOrder orderToComplete = tuberOrders.FirstOrDefault(to => to.Id == id);
    if (orderToComplete == null)
    {
        return Results.BadRequest();
    }
    orderToComplete.DeliveredOnDate = DateTime.Now;
    return Results.NoContent();
    
});

app.MapGet("/api/tuberToppings", () => 
{
    return tuberToppings.Select(tt => new TuberToppingDTO
    {
        Id = tt.Id,
        TuberOrderId = tt.TuberOrderId,
        ToppingId = tt.ToppingId
    });
});

app.MapDelete("/api/tuberToppings/{orderId}", (int orderId, int toppingId) => 
{
    List<TuberTopping> toppingsForOrder = tuberToppings.Where(tt => tt.TuberOrderId == orderId).ToList();
    TuberTopping toppingToDelete = toppingsForOrder.FirstOrDefault(td => td.ToppingId == toppingId);
    if (toppingToDelete == null)
    {
        return Results.BadRequest();
    }
    tuberToppings.Remove(toppingToDelete);
    return Results.NoContent();
});

app.MapGet("/api/customers", () =>
{
    return customers.Select(c => new CustomerDTO
    {
        Id = c.Id,
        Name = c.Name,
        Address = c.Address,
        TuberOrder = c.TuberOrders
    });
});

app.MapGet("/api/customers/{id}", (int id) =>
{   //grab customer
    Customer customer = customers.FirstOrDefault(c => c.Id == id);
    //make list of customer's orders
    List<TuberOrder> orderEntries = tuberOrders.Where(to => to.CustomerId == id).ToList();
    //for every order by that customer,
    foreach (var order in orderEntries)
    {   //make a list of all tuber topping entries with matching OrderId
        List<TuberTopping> tuberToppingEntries = tuberToppings.Where(tt => tt.TuberOrderId == order.Id).ToList();
        //Initialize a list of toppings that maps through those entries and for each one, find first matching topping Item.
        List<Topping> toppingsForOrder = tuberToppingEntries.Select(tt => toppings.First(t => tt.ToppingId == t.Id)).ToList();
        //set order object's topping property
        order.Toppings = toppingsForOrder;
    }
    //set customer order info
    customer.TuberOrders = orderEntries;
    if (customer == null)
    {
        return Results.BadRequest();
    }
    return Results.Ok(customer);
});

app.MapPost("/api/customers", (Customer customer) => 
{
    customer.Id = customers.Max(c => c.Id) + 1;
    customers.Add(customer);
    return Results.Created($"/api/customers/{customer.Id}", new CustomerDTO
    {
        Id = customer.Id,
        Name = customer.Name,
        Address = customer.Address,
        
    });
});

app.MapDelete("/api/customers/{id}", (int id) => 
{
    Customer customerToDelete = customers.FirstOrDefault(c => c.Id == id);
    if (customerToDelete == null)
    {
        return Results.BadRequest();
    }
    List<TuberOrder> ordersToDelete = tuberOrders.Where(to => to.CustomerId == id).ToList();
    foreach (TuberOrder order in ordersToDelete)
    {
        tuberOrders.Remove(order);
    }
    customers.Remove(customerToDelete);
    return Results.NoContent();
});

app.MapGet("/api/tuberDrivers", () =>
{
    return tuberDrivers.Select(td => new TuberDriverDTO
    {
        Id = td.Id,
        Name = td.Name,
        TuberDeliveries = td.TuberDeliveries
    });
});

app.MapGet("/api/tuberDrivers/{id}", (int id) =>
{
    TuberDriver driver = tuberDrivers.FirstOrDefault(td => td.Id == id);
    if (driver == null)
    {
        return Results.BadRequest();
    }
    List<TuberOrder> driverOrders = tuberOrders.Where(to => to.TuberDriverId == id).ToList();
    foreach (var order in driverOrders)
    {
        List<TuberTopping> tuberToppingEntries = tuberToppings.Where(tt => tt.TuberOrderId == order.Id).ToList();
        List<Topping> toppingsForOrder = tuberToppingEntries.Select(tt => toppings.First(t => tt.ToppingId == t.Id)).ToList();
        order.Toppings = toppingsForOrder;
    }
    driver.TuberDeliveries = driverOrders;
    return Results.Ok(driver);

});

app.Run();
//don't touch or move this!
public partial class Program { }