# realtime-communication
PubNub Realtime Communication using .Net Core framework with C#

## Setup Instructions

1. **Install .NET SDK**: Make sure you have the .NET SDK installed. You can download it from the [official .NET website](https://dotnet.microsoft.com/download).

2. **Create a new project**:
   ```bash
   dotnet new web -n PizzaStore
   cd PizzaStore
   ```

3. **Add Minimal API**: Open the `Program.cs` file and set up a minimal API. Here’s a simple example:
   ```csharp
   using Microsoft.AspNetCore.Builder;
   using Microsoft.Extensions.DependencyInjection;

   var builder = WebApplication.CreateBuilder(args);
   var app = builder.Build();

   app.MapGet("/", () => "Welcome to the Pizza Store!");

   app.Run();
   ```

4. **Run the application**:
   ```bash
   dotnet run
   ```

5. **Access the API**: Open your browser and navigate to `http://localhost:5000` to see the welcome message.
